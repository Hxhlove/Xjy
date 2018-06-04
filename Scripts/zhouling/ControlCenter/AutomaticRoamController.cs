// ------------------------------------------------------------------
// Title        :漫游控制器
// Author       :Leo
// Date         :2018.05.16
// Description  :控制漫游管理
// ------------------------------------------------------------------


using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutomaticRoamController
{

    static AutomaticRoamController sm_inst;

    public static AutomaticRoamController Inst
    {
        get
        {
            if (sm_inst == null)
                sm_inst = new AutomaticRoamController();
            return sm_inst;
        }
    }
    /// <summary>
    /// 漫游摄像机对象
    /// </summary>
    GameObject CameraObj;

    CameraPathAnimator _CameraPathAnimator;
    /// <summary>
    /// 动画漫游对象
    /// </summary>
    CameraPathAnimator RoamController
    {
        get
        {
            if (_CameraPathAnimator == null)
            {
                GameObject roamController = new GameObject("RoamController");
                _CameraPathAnimator = roamController.AddComponent<CameraPathAnimator>();

                _CameraPathAnimator.AnimationStartedEvent += OnAnimationStarted;
                _CameraPathAnimator.AnimationPausedEvent += OnAnimationPaused;
                _CameraPathAnimator.AnimationStoppedEvent += OnAnimationStopped;
                _CameraPathAnimator.AnimationFinishedEvent += OnAnimationFinished;
                _CameraPathAnimator.AnimationCustomEvent += OnCustomEvent;

                CameraObj = new GameObject("RoamCamera");
                var camera = CameraObj.AddComponent<Camera>();
                camera.cullingMask = ~(1 << 5);
                camera.depth = 10;
                _CameraPathAnimator.SetAnimationObject = CameraObj.transform;                
            }
            return _CameraPathAnimator;
        }
        set
        {
            _CameraPathAnimator = value;
        }
    }

    List<CameraPath> _Location;
    /// <summary>
    /// 区域场景漫游集合
    /// </summary>
    List<CameraPath> Location
    {
        get
        {
            if (_Location == null || _Location.Count <= 0)
            {
                _Location = new List<CameraPath>();
                List<GameObject> gameObjects = ResourcesEx.LoadAll<GameObject>(PathFileTool.Location);
                foreach (GameObject gameObject in gameObjects)
                {
                    if (gameObject)
                    {
                        var cps = gameObject.GetComponentsInChildren<CameraPath>();
                        if (cps != null && cps.Length > 0)
                        {
                            _Location.AddRange(cps.ToList());
                        }
                    }
                }
            }
            return _Location;
        }
    }
    List<CameraPath> _SandTable;
    /// <summary>
    /// 沙盘场景漫游集合
    /// </summary>
    List<CameraPath> SandTable
    {
        get
        {
            if (_SandTable == null)
            {
                _SandTable = new List<CameraPath>();
                List<GameObject> gameObjects = ResourcesEx.LoadAll<GameObject>(PathFileTool.SandTable);
                foreach (GameObject gameObject in gameObjects)
                {
                    if (gameObject)
                    {
                        var cps = gameObject.GetComponentsInChildren<CameraPath>();
                        if (cps != null && cps.Length > 0)
                        {
                            _SandTable.AddRange(cps.ToList());
                        }
                    }
                }
            }
            return _SandTable;
        }
    }
    /// <summary>
    /// 指定名称路径集合
    /// </summary>
    Dictionary<string, List<CameraPath>> CameraPaths;
    /// <summary>
    /// 当前获取的路径名称对应的集合
    /// </summary>
    List<CameraPath> CameraPath;

    /// <summary>
    /// 是否循环漫游
    /// </summary>
    bool isLoopRoam = false;


    /// <summary>
    /// 当前漫游数据集合
    /// </summary>
    Queue<CameraPath> CurrentCameraPath;

    /// <summary>
    /// 当前外景漫游状态
    /// </summary>
    AutomaticRoamType type;
    /// <summary>
    /// 外景漫游状态
    /// </summary>
    AutomaticRoamType Type
    {
        set
        {
            if (type != value)
            {
                type = value;
                CurrentCameraPath.Clear();
                switch (type)
                {
                    case AutomaticRoamType.Automatic:
                        if (Location != null && Location.Count>0)
                        {
                            foreach (CameraPath cp in Location)
                            {
                                CurrentCameraPath.Enqueue(cp);
                            }
                        }
                        if (SandTable != null && SandTable.Count > 0)
                        {
                            foreach (CameraPath cp in SandTable)
                            {
                                CurrentCameraPath.Enqueue(cp);
                            }
                        }
                        break;
                    case AutomaticRoamType.Location:
                        if (Location != null && Location.Count > 0)
                        {
                            foreach (CameraPath cp in Location)
                            {
                                CurrentCameraPath.Enqueue(cp);
                            }
                        }
                        break;
                    case AutomaticRoamType.SandTable:
                        if (SandTable != null && SandTable.Count > 0)
                        {
                            foreach (CameraPath cp in SandTable)
                            {
                                CurrentCameraPath.Enqueue(cp);
                            }
                        }
                        break;
                }
            }
        }
    }
    /// <summary>
    /// 是否开启自动漫游
    /// </summary>
    bool isStartRoam = false;

    /// <summary>
    /// 漫游回调
    /// </summary>
    EventArg RoamCallback;

    /// <summary>
    /// 进入场景
    /// </summary>
    public void Open()
    {
        type = AutomaticRoamType.Undefined;
        CurrentCameraPath = new Queue<CameraPath>();
        CameraPaths = new Dictionary<string, List<CameraPath>>();
        CameraPath = new List<global::CameraPath>();
        //注册事件
        //漫游开始
        EventMgr.Inst.Regist(AutomaticRoamEvent.RoamStart, RoamStart);
        //指定漫游路径进行漫游
        EventMgr.Inst.Regist(AutomaticRoamEvent.RoamCameraPath, RoamCameraPath);
        //输入
        EventMgr.Inst.Regist(InputStateEvent.Input, Input);
        //空闲
        EventMgr.Inst.Regist(InputStateEvent.Free, Free);
    }
    /// <summary>
    /// 开始自动漫游
    /// </summary>
    private void StartAnimationRoam()
    {
        Debug.Log("自动漫游----开始");
        StartRoam();
    }

    /// <summary>
    /// 开始漫游
    /// </summary>
    private void StartRoam()
    {
        isStartRoam = true;
        if (RoamController && CurrentCameraPath.Count > 0)
        {
            RoamController.Stop();
            RoamController.cameraPath = CurrentCameraPath.Dequeue();
            if (isLoopRoam)
            {
                CurrentCameraPath.Enqueue(RoamController.cameraPath);
            }
            RoamController.Play();
        }
        else
        {
            RoamDestroy();
        }
    }

    /// <summary>
    /// 销毁漫游
    /// </summary>
    private void RoamDestroy()
    {
        if (isStartRoam)
        {
            isStartRoam = false;
            isLoopRoam = false;
            type = AutomaticRoamType.Undefined;
            CurrentCameraPath.Clear();
            if (RoamController)
            {
                RoamController.Stop();
                RoamController.AnimationStartedEvent -= OnAnimationStarted;
                RoamController.AnimationPausedEvent -= OnAnimationPaused;
                RoamController.AnimationStoppedEvent -= OnAnimationStopped;
                RoamController.AnimationFinishedEvent -= OnAnimationFinished;
                RoamController.AnimationCustomEvent -= OnCustomEvent;
                GameObject.Destroy(RoamController.gameObject);
                GameObject.Destroy(RoamController);
                RoamController = null;
            }
            GameObject.Destroy(CameraObj);            
            TimerMgr.Add((callback) =>
            {
                EventArg eventArg = (EventArg)callback[0];
                if (eventArg != null)
                {
                    eventArg.Callback();
                }
                EventMgr.Inst.Fire(AutomaticRoamEvent.RoamEnd);
            }, RoamCallback);
            RoamCallback = null;
            Debug.Log("自动漫游----结束");
        }
    }


    /// <summary>
    /// 外景漫游事件
    /// </summary>
    private void RoamStart(EventArg ea)
    {
        isLoopRoam = false;
        RoamCallback = ea;
        Type = (AutomaticRoamType)ea[0];
        StartAnimationRoam();
    }

    /// <summary>
    /// 指定漫游路径进行漫游
    /// </summary>
    public void RoamCameraPath(EventArg ea)
    {
        RoamCallback = ea;
        string cameraPath = (string)ea[0];
        CurrentCameraPath.Clear();
        CameraPath = null;
        if (!CameraPaths.TryGetValue(cameraPath,out CameraPath))
        {
            CameraPath = new List<CameraPath>();
            List<GameObject> gameObjects = ResourcesEx.LoadAll<GameObject>(cameraPath);
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject)
                {
                    var cps = gameObject.GetComponentsInChildren<CameraPath>();
                    if (cps != null && cps.Length > 0)
                    {
                        CameraPath.AddRange(cps.ToList());
                    }
                }
            }
            CameraPaths.Add(cameraPath, CameraPath);
        }
        if (CameraPath != null && CameraPath.Count > 0)
        {
            foreach (CameraPath cp in CameraPath)
            {
                CurrentCameraPath.Enqueue(cp);
            }
            StartAnimationRoam();
        }
        else
        {
            RoamDestroy();
        }
    }
    /// <summary>
    /// 输入
    /// </summary>
    private void Input()
    {
        RoamDestroy();
    }
    /// <summary>
    /// 空闲
    /// </summary>
    private void Free()
    {
        isLoopRoam = true;
        Type = AutomaticRoamType.Automatic;
        StartAnimationRoam();
    }    


    /// <summary>
    /// 漫游线路开始
    /// </summary>
    private void OnAnimationStarted()
    {
        Debug.Log("漫游线路开始");
    }
    /// <summary>
    /// 漫游线路暂停
    /// </summary>
    private void OnAnimationPaused()
    {
        Debug.Log("漫游线路暂停");
    }
    /// <summary>
    /// 漫游线路停止
    /// </summary>
    private void OnAnimationStopped()
    {
        Debug.Log("漫游线路停止");
    }

    /// <summary>
    /// 漫游线路完成
    /// </summary>
    private void OnAnimationFinished()
    {
        Debug.Log("漫游线路完成");
        //线路漫游完成,执行下一个路线
        StartRoam();
    }
    /// <summary>
    /// 当用户定义的事件被触发时，将事件名称作为字符串发送
    /// </summary>
    private void OnCustomEvent(string eventname)
    {
        Debug.Log("事件被触发: " + eventname);
    }
}