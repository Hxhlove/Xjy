using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ------------------------------------------------------------------
// Title        :摄像机控制
// Author       :Vincent
// Date         :2018.05.24
// Description  :
// ------------------------------------------------------------------
public delegate void CameraAnimationC();//动画结束的回调委托
public class CameraStateControl
{
    private Camera _animationCamera;//动画摄像机
    private CameraOperateData _cameraStateData;//摄像机控制的数据
    private CameraStateLoadSave _cameraStateLoadSave;//保存的控制数据
    private bool isEndAnimation = true;//动画是否结束
    private CameraAnimation _cameraAnimation;//摄像机动画类
    private CameraStateLogic _cameraStateLogic;//摄像机操作类
    private List<CameraPath> _cameraPathList;//预设的摄像机路径动画
    #region //单例对象
    private static CameraStateControl _cameraStateControl;
    private CameraStateControl() { }
    /// <summary>
    /// 取得CameraStateControl唯一对象
    /// </summary>
    /// <returns></returns>
    public static CameraStateControl Init
    {
        get
        {
            if (_cameraStateControl == null)
            {
                _cameraStateControl = new CameraStateControl();
            }
            return _cameraStateControl;
        }
    }
    #endregion
    /// <summary>
    /// 摄像机控制数据初始化
    /// </summary>
    public void Open()
    {
        _animationCamera = new GameObject("AnimationCamera").AddComponent<Camera>();
        
        _cameraAnimation = new CameraAnimation(AnimationEndCallBack);//摄像机动画类
        _cameraStateLogic = CameraStateLogic.Init;//摄像机控制
        _cameraStateLoadSave = CameraStateLoadSave.GetInstance();//控制数据
        EventMgr.Inst.Regist(CameraControlEvent.OnePointsAnimation, OnePointsAnimation);//一个点的过度动画
        EventMgr.Inst.Regist(CameraControlEvent.InitialPosition, InitialPosition);//瞬间移动
        EventMgr.Inst.Regist(CameraControlEvent.PreinstallControl, PreinstallControl);//设置操作数据
        EventMgr.Inst.Regist(CameraControlEvent.PreinstallAnimation, PreinstallAnimation);//播放路径动画
        EventMgr.Inst.Regist(CameraControlEvent.ControlParameters, ControlParameters);//内部配套控制数据
        _cameraStateLoadSave.CameraStateDataSave(Resources.Load<CameraOperateData>("NoOperation").dataName, Resources.Load<CameraOperateData>("NoOperation"));//不操作
        _cameraStateLoadSave.CameraStateDataSave(Resources.Load<CameraOperateData>("Scenery").dataName, Resources.Load<CameraOperateData>("Scenery"));//阳台景观
        _cameraStateLoadSave.CameraStateDataSave(Resources.Load<CameraOperateData>("SandTable").dataName, Resources.Load<CameraOperateData>("SandTable"));//沙盘操作
        _cameraStateLoadSave.CameraStateDataSave(Resources.Load<CameraOperateData>("FirstPerson").dataName, Resources.Load<CameraOperateData>("FirstPerson"));//第一人称
    }
    #region//事件
    /// <summary>
    /// 一点进行过度动画(摄像机当前位置到指定位置(位置和方向))
    /// </summary>
    /// <param name="arg">目标位置transform</param>
    private void OnePointsAnimation(EventArg arg)
    {
        isEndAnimation = false;
        Transform cameraTarget = (Transform)arg[0];//目标位置
        List<Transform> pathTransform = new List<Transform>();
        pathTransform.Add(_animationCamera.transform);
        pathTransform.Add(cameraTarget);
        _cameraAnimation.SetAnimationData(pathTransform, CameraControlEvent.OnePointsAnimation);
    }
    /// <summary>
    /// 瞬间传送到初始位置
    /// </summary>
    /// <param name="arg">目标位置transform</param>
    private void InitialPosition(EventArg arg)
    {
        isEndAnimation = false;
        Transform cameraTarget = (Transform)arg[0];//目标位置
        List<Transform> pathTransform = new List<Transform>();
        pathTransform.Add(_animationCamera.transform);
        pathTransform.Add(cameraTarget);
        _cameraAnimation.SetAnimationData(pathTransform,CameraControlEvent.InitialPosition);
    }
    /// <summary>
    /// 启动摄像机预设控制(发送枚举类型)
    /// </summary>
    /// <param name="arg">操作类型</param>
    private void PreinstallControl(EventArg arg)
    {
        PreinstallControlType preinstallControlType = (PreinstallControlType)arg[0];
        GetOperateData(preinstallControlType);//根据枚举获取数据
        _cameraStateLogic.SetCameraStateLogicData(_animationCamera.transform, _cameraStateData);
    }
    /// <summary>
    /// 启动预设动画
    /// </summary>
    /// <param name="arg">预设动画序号</param>
    private void PreinstallAnimation(EventArg arg)
    {
        isEndAnimation = false;
        int pathIndex = (int)arg[0];
        CameraPath cameraPath = _cameraPathList[pathIndex];
        _cameraAnimation.SetAnimationData(cameraPath, CameraControlEvent.PreinstallAnimation);
    }
    /// <summary>
    /// 设置摄像机的操作参数 发送参数 内部配套
    /// </summary>
    /// <param name="arg"></param>
    private void ControlParameters(EventArg arg)
    {
        _cameraStateData = (CameraOperateData)arg[0];
        _cameraStateLogic.SetCameraStateLogicData(_animationCamera.transform, _cameraStateData);
    }
    #endregion
    /// <summary>
    /// 根据操作枚举获取操作数据
    /// </summary>
    /// <param name="preinstallControlType"></param>
    private void GetOperateData(PreinstallControlType preinstallControlType)
    {
        switch (preinstallControlType)
        {
            case PreinstallControlType.NoOperation:
                _cameraStateData = _cameraStateLoadSave.TraverseCameraStateLoadData("NoOperation");
                break;
            case PreinstallControlType.SandTable:
                _cameraStateData = _cameraStateLoadSave.TraverseCameraStateLoadData("SandTable");
                break;
            case PreinstallControlType.Scenery:
                _cameraStateData = _cameraStateLoadSave.TraverseCameraStateLoadData("Scenery");
                break;
            case PreinstallControlType.FirstPerson:
                _cameraStateData = _cameraStateLoadSave.TraverseCameraStateLoadData("FirstPerson");
                break;
        }
        //return _cameraStateData;
    }
    /// <summary>
    /// 状态更新
    /// </summary>
    public void StateUpdate()
    {
        Debug.Log(isEndAnimation);
        if (isEndAnimation)
        {
            //调用控制更新
            _cameraStateLogic.RotateStateUpdate();
        }
        else
        {
            //调动动画更新
            _cameraAnimation.AnimationUpdate();
        }
        if (_cameraStateData.isFirstView == false && _animationCamera.gameObject.GetComponent<Rigidbody>() != null)
        {
            UnityEngine.Object.Destroy(_animationCamera.gameObject.GetComponent<Rigidbody>());
        }
    }
    #region//动画结束回调方法
    /// <summary>
    /// 摄像机动画结束之后的回调
    /// </summary>
    public void AnimationEndCallBack()
    {
        //Debug.Log("回调");
        isEndAnimation = true;//进入控制分支
        _cameraStateLogic.ReverseIncrement();//如果目标点不为空 重新计算角度
    }
    #endregion
}