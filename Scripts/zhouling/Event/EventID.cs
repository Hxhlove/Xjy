// ------------------------------------------------------------------
// Title        :系统事件ID
// Author       :Leo
// Date         :2018.05.04
// Description  :系统事件ID
// ------------------------------------------------------------------


/// <summary>
/// 禁止/应许输入事件
/// </summary>
public enum ProhibitInputEvent
{
    ProhibitInput,         //禁止输入
    PromiseInput,          //应许输入
}

/// <summary>
/// 触控输入事件
/// </summary>
public enum TouchInputEvent
{
    Click,          //点击
    Slide,          //滑动
    Zoom,           //缩放
}

/// <summary>
/// 输入空闲状态(在指定时间内未输入)
/// </summary>
public enum InputStateEvent
{
    Input,         //输入状态
    Free,          //空闲状态
}

/// <summary>
/// 自动漫游事件
/// </summary>
public enum AutomaticRoamEvent
{
    RoamCameraPath,     //指定漫游路径进行漫游
    RoamStart,          //漫游开始(需要输入漫游类型)
    RoamEnd,            //漫游结束
    InterruptRoam,      //中断漫游
}

/// <summary>
/// 开启片头
/// </summary>
public enum FilmHeadEvent
{
    Open,         //打开
}

/// <summary>
/// 加载进度UI
/// </summary>
public enum LoadScheduleEvent
{
    OpenUI,         //打开加载进度UI
    CloseUI,        //关闭加载进度UI
    Schedule,       //加载进度
}



/// <summary>
/// 加载场景
/// </summary>
public enum LoadSceneEvent
{
    Load,         //加载
}

/// <summary>
/// 摄像机控制相关事件
/// </summary>
public enum CameraControlEvent
{
    InitialPosition,            //瞬间传送到初始位置
    PreinstallAnimation,        //启动摄像机预设动画(发送序列号)
    PreinstallControl,          //启动摄像机预设控制(发送序列号)
    Delivery,                   //瞬间传送到指定位置(一个坐标(位置和方向)参数)
    OnePointsAnimation,         //一点进行过度动画(摄像机当前位置到指定位置(位置和方向))
    TwoPointsAnimation,         //两点进行过度动画(开始点(位置和方向)，结束点(位置和方向))
    MultipointPointsAnimation,  //多点点进行过度动画(根据指定的多个位置(位置和方向)进行顺序的过度动画)
    ControlParameters,          //设置摄像机的操作参数(发送摄像机操作数据对象)
}
