// ------------------------------------------------------------------
// Title        :游戏驱动中心
// Author       :Leo
// Date         :2018.05.04
// Description  :游戏控制中心通过驱动器驱动，管理和调用其他管理对象
//               推进游戏的进程.所有管理对象都是单利对象.
// ------------------------------------------------------------------

using UnityEngine;

class ControlCenter
{
    static ControlCenter sm_inst;

    public static ControlCenter Inst
    {
        get
        {
            if (sm_inst == null)
                sm_inst = new ControlCenter();
            return sm_inst;
        }
    }
    private ControlCenter()
    {
        DebugLog.Open(); ;                         //写入日志管理初始
        LoadSceneController.Inst.Open();           //场景管理加载初始化
        FilmHeadManage.Inst.Open();                //片头管理初始化
        AutomaticRoamController.Inst.Open();       //自动漫游管理初始化
        CenterController.Inst.Open();              //逻辑管理初始化
        UICreateUIMgr.Inst.Open();                 //UI创建控制初始化
    }
    /// <summary>
    /// 进入场景
    /// </summary>
    internal void Awake()
    {
        CameraStateControl.Inst.Open();//初始化摄像机控制
        AudioManagement.Inst.Open();//初始化声音控制
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    internal void Start()
    {
    }
    /// <summary>
    /// 帧更新
    /// </summary>
    internal void Update()
    {
    }
    /// <summary>
    /// 固定更新
    /// </summary>
    internal void FixedUpdate()
    {
        TimerMgr.FixedUpdate();                             //全局定时回调,固定更新时间比较准确
        ResourcesEx.FixedUpdate();                          //资源加载更新
        CenterController.Inst.FixedUpdate();                //逻辑更新
        UICreateUIMgr.Inst.FixedUpdate();                   //UI控制更新
        AudioManagement.Inst.AudioUpdate();                 //声音播放更新
    }
    /// <summary>
    /// 最后更新,一般用以摄像机和移动操作
    /// </summary>
    internal void LateUpdate()
    {
        CameraStateControl.Inst.StateUpdate();
    }
    /// <summary>
    /// 应用失去焦点
    /// </summary>
    internal void OnApplicationFocus(bool hasFocus)
    {
    }
    /// <summary>
    /// 驱动器销毁
    /// </summary>
    internal void OnDestroy()
    {
    }
    /// <summary>
    /// 编辑器退出游戏
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}