// ------------------------------------------------------------------
// Title        :事件参数对象
// Author       :Leo
// Date         :2018.05.04
// Description  :用来为事件传递参数的对象
// ------------------------------------------------------------------
/*
// -------------------------------------------------------------- 
// 注册事件
    EventMgr.Inst.Regist(事件ID(枚举), EventDelegate1 edlg)
    EventMgr.Inst.Regist(事件ID(枚举), EventDelegate2 edlg)
    
    无参数事件
    void Test()
    {
    }
    有参数无回调注册的使用方法
    void Test(EventArg args)
    {
        //获取参数
        int i = (int)args[0];
    }
    有参数有回调注册的使用方法
    void Test(EventArg arg)
    {
        //获取参数
        int i = (int)args[0];
        //启动回调
        //回调无参数的
        arg.Callback();
        //回调有参数的
        arg.Callback(new EventArg(设置参数));
    }

// 执行事件时有方法：
    //无参数
    EventMgr.Inst.Fire(事件ID(枚举));
    //有参数
    EventMgr.Inst.Fire(事件ID(枚举), new EventArg(参数1....参数N));
    //有参数带回调(回调方法也包括有参和无参)
    EventMgr.Inst.Fire(事件ID(枚举), new EventArg(回调方法, 参数1....参数N));

    
// 注销事件
    EventMgr.Inst.UnRegist(事件ID(枚举), EventDelegate1 edlg)
    EventMgr.Inst.UnRegist(事件ID(枚举), EventDelegate2 edlg)
    
*/
// --------------------------------------------------------------

/// <summary>
/// 事件参数对象
/// </summary>
public class EventArg
{
    //事件回调方法1
    private EventDelegate1 _Callback1 = null;
    //事件回调方法2
    private EventDelegate2 _Callback2 = null;
    private object[] m_args;
    public object[] Args { get { return m_args; } }
    public object this[int index] { get { return this.m_args[index]; } }
    /// <summary>
    /// 参数非空
    /// </summary>
    public bool NonEmpty
    {
        get
        {
            if (Args == null || Args.Length == 0)
            {
                return false;
            }
            return true;
        }
    }
    /// <summary>
    /// 不带回调方法参数的事件数据
    /// </summary>
    /// <param name="args">数据集合</param>
    public EventArg(params object[] args)
    {
        this.m_args = args;
    }
    /// <summary>
    /// 带回调方法的事件数据（有参回调）
    /// </summary>
    /// <param name="callback">回调方法</param>
    /// <param name="args">数据集合</param>
    public EventArg(EventDelegate1 callback, params object[] args)
    {
        _Callback1 = callback;
        this.m_args = args;
    }
    /// <summary>
    /// 带回调方法的事件数据（有参回调）
    /// </summary>
    /// <param name="callback">回调方法</param>
    /// <param name="args">数据集合</param>
    public EventArg(EventDelegate2 callback, params object[] args)
    {
        _Callback2 = callback;
        this.m_args = args;
    }
    /// <summary>
    /// 启动回调
    /// </summary>
    public void Callback(EventArg arg = null)
    {
        if (arg == null)
        {
            if (_Callback1 != null)
            {
                _Callback1();
            }
        }
        else
        {
            if (_Callback2 != null)
            {
                _Callback2(arg);
            }
        }
    }
}