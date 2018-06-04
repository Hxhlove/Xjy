// ------------------------------------------------------------------
// Title        :中心控制器
// Author       :Leo
// Date         :2018.05.10
// Description  :控制逻辑控制管理
// ------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

public class CenterController
{
    static CenterController sm_inst;

    public static CenterController Inst
    {
        get
        {
            if (sm_inst == null)
                sm_inst = new CenterController();
            return sm_inst;
        }
    }

    //片头是否完成
    bool isFilmHeadEnd = false;
    //场景是否加载完成
    bool isLoadSceneEnd = false;
    //加载进度完成
    bool LoadScheduleStart = false;

    /// <summary>
    /// 初始逻辑结构
    /// </summary>
    CenterController()
    {
        //注册事件
        //漫游结束
        EventMgr.Inst.Regist(AutomaticRoamEvent.RoamEnd, RoamEnd);
        //漫游中断
        EventMgr.Inst.Regist(AutomaticRoamEvent.InterruptRoam, RoamEnd);
    }
    /// <summary>
    /// 进入场景
    /// </summary>
    public void Open()
    {
        //开始运行主逻辑
        StartRunning();
    }
    
    /// <summary>
    /// 加载进度描述
    /// </summary>
    public string LoadSchedule
    {
        get
        {
            return LoadSceneController.Inst.AsyncLoadScene;
        }
    }


    /// <summary>
    /// 逻辑更新
    /// </summary>
    public void FixedUpdate()
    {
        if (LoadScheduleStart)
        {
            //更新加载进度描述
            EventMgr.Inst.Fire(LoadScheduleEvent.Schedule, new EventArg(LoadSchedule));
        }

        //测试输入状态和空闲状态
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("A");
            EventMgr.Inst.Fire(InputStateEvent.Input);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            Debug.Log("B");
            EventMgr.Inst.Fire(InputStateEvent.Free);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("C");
            EventMgr.Inst.Fire(AutomaticRoamEvent.RoamCameraPath, new EventArg("RoomRoaming"));
        }
    }


    /// <summary>
    /// 开始运行
    /// </summary>
    public void StartRunning()
    {
        //1.关闭输入
        EventMgr.Inst.Fire(ProhibitInputEvent.ProhibitInput);

        //2.开始片头
        EventMgr.Inst.Fire(FilmHeadEvent.Open, new EventArg(FilmHeadEnd));

        //3.异步加载主场景
        EventMgr.Inst.Fire(LoadSceneEvent.Load, new EventArg(LoadSceneEnd, PathFileTool.PlanScene));
    }

    /// <summary>
    /// 片头结束回调
    /// </summary>
    private void FilmHeadEnd()
    {
        Debug.Log("片头结束回调");
        isFilmHeadEnd = true;
        //场景是否加载完成
        if (isLoadSceneEnd)
        {
            AutomaticRoam();
        }
        else
        {
            //1.开启加载进度UI管理
            EventMgr.Inst.Fire(LoadScheduleEvent.OpenUI);
            LoadScheduleStart = true;
        }
    }

    /// <summary>
    /// 加载场景异步完成回调
    /// </summary>
    private void LoadSceneEnd()
    {
        Debug.Log("场景加载结束回调");
        isLoadSceneEnd = true;

        Debug.Log("初始化camera");
        //1.设置摄像机初始和控制参数
        EventMgr.Inst.Fire(CameraControlEvent.InitialPosition);
        EventMgr.Inst.Fire(CameraControlEvent.PreinstallControl, new EventArg(PreinstallControlType.NoOperation));
        
        //2.检测片头是否播放完成
        if (isFilmHeadEnd)
        {
            AutomaticRoam();
        }
    }

    /// <summary>
    /// 进入场景时自动漫游
    /// </summary>
    private void AutomaticRoam()
    {
        //1.关闭加载进度UI管理
        EventMgr.Inst.Fire(LoadScheduleEvent.CloseUI);
        LoadScheduleStart = false;        

        //2.开启漫游
        EventMgr.Inst.Fire(AutomaticRoamEvent.RoamStart, new EventArg(AutomaticRoamEnd, AutomaticRoamType.Location));

        //3.开启输入
        EventMgr.Inst.Fire(ProhibitInputEvent.PromiseInput);
    }

    /// <summary>
    /// 进入场景的自动漫游结束或退出回调
    /// </summary>
    private void AutomaticRoamEnd()
    {
        Debug.Log("进入场景的自动漫游结束回调");
        //1.设置摄像机进入区域规划视角动画
        EventMgr.Inst.Fire(CameraControlEvent.PreinstallAnimation, new EventArg(PreinstallAnimationType.ToRegion));

    }


    /// <summary>
    /// 漫游结束或中断
    /// </summary>
    private void RoamEnd()
    {
        Debug.Log("漫游完成返回主线程");
    }
}