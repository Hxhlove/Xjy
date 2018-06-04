// ------------------------------------------------------------------
// Title        :全局事件管理中心
// Author       :Leo
// Date         :2018.05.04
// Description  :管理和执行全局事件
// ------------------------------------------------------------------

using System;
using System.Collections.Generic;

// --------------------------------------------------------------
// 事件代理
// --------------------------------------------------------------
#region GBEventDelegates

//无参数事件
public delegate void EventDelegate1();
//有参数事件
public delegate void EventDelegate2(EventArg arg);

/// <summary>
/// 一个类型对应的事件委托对象集合
/// </summary>
class EventDelegates
{
    private List<EventDelegate1> m_edlgs1;
    private List<EventDelegate1> m_current1;
    private List<EventDelegate2> m_edlgs2;
    private List<EventDelegate2> m_current2;

    public void Add(EventDelegate1 edlg)
    {
        if (m_edlgs1 == null)
        {
            this.m_edlgs1 = new List<EventDelegate1>();
            this.m_current1 = new List<EventDelegate1>();
        }
        if (!this.m_edlgs1.Contains(edlg))
        {
            this.m_edlgs1.Add(edlg);
        }
    }
    public void Add(EventDelegate2 edlg)
    {
        if (m_edlgs2 == null)
        {
            this.m_edlgs2 = new List<EventDelegate2>();
            this.m_current2 = new List<EventDelegate2>();
        }
        if (!this.m_edlgs2.Contains(edlg))
        {
            this.m_edlgs2.Add(edlg);
        }
    }
    public void Remove(EventDelegate1 edlg)
    {
        if (m_edlgs1 != null && this.m_edlgs1.Contains(edlg))
        {
            this.m_edlgs1.Remove(edlg);
        }
    }
    public void Remove(EventDelegate2 edlg)
    {
        if (m_edlgs2 != null && this.m_edlgs2.Contains(edlg))
        {
            this.m_edlgs2.Remove(edlg);
        }
    }
    public void Call(EventArg earg)
    {
        if (earg == null)
        {
            if (m_edlgs1 != null && m_current1 != null)
            {
                m_current1.Clear();
                m_current1.AddRange(m_edlgs1);
                int count = m_current1.Count;
                EventDelegate1 _Delegate = null;
                for (int i = 0; i < count; i++)
                {
                    _Delegate = m_current1[i];
                    if (_Delegate != null)
                    {
                        _Delegate();
                    }
                }
            }
        }
        else
        {
            if (m_edlgs2 != null && m_current2 != null)
            {
                m_current2.Clear();
                m_current2.AddRange(m_edlgs2);
                int count = m_current2.Count;
                EventDelegate2 _Delegate = null;
                for (int i = 0; i < count; i++)
                {
                    _Delegate = m_current2[i];
                    if (_Delegate != null)
                    {
                        _Delegate(earg);
                    }
                }
            }
        }
    }
}

#endregion

// --------------------------------------------------------------
// 全局事件
// --------------------------------------------------------------
class EventMgr
{
    static EventMgr sm_inst;
    private Dictionary<Type, Dictionary<int, EventDelegates>> m_events;

    private EventMgr()
    {
        this.m_events = new Dictionary<Type, Dictionary<int, EventDelegates>>();
    }
    public static EventMgr Inst
    {
        get
        {
            if (sm_inst == null)
                sm_inst = new EventMgr();
            return sm_inst;
        }
    }
    // -------------------------------------------------------------------
    // 注册事件,使用ID把对应的委托方法注册到事件管理上(eid 必须是枚举参数)
    // -------------------------------------------------------------------
    #region 注册事件
    public void Regist(object eid, EventDelegate1 edlg)
    {
        Type type = eid.GetType();
        if (type.IsEnum)
        {
            int id = (int)eid;
            Dictionary<int, EventDelegates> mEventDelegates = null;
            if (!this.m_events.TryGetValue(type, out mEventDelegates))
            {
                mEventDelegates = new Dictionary<int, EventDelegates>();
                this.m_events.Add(type, mEventDelegates);
            }
            EventDelegates mEventDelegate = null;
            if (!mEventDelegates.TryGetValue(id, out mEventDelegate))
            {
                mEventDelegate = new EventDelegates();
                mEventDelegates.Add(id, mEventDelegate);
            }
            mEventDelegate.Add(edlg);
        }
    }
    public void Regist(object eid, EventDelegate2 edlg)
    {
        Type type = eid.GetType();
        if (type.IsEnum)
        {
            int id = (int)eid;
            Dictionary<int, EventDelegates> mEventDelegates = null;
            if (!this.m_events.TryGetValue(type, out mEventDelegates))
            {
                mEventDelegates = new Dictionary<int, EventDelegates>();
                this.m_events.Add(type, mEventDelegates);
            }
            EventDelegates mEventDelegate = null;
            if (!mEventDelegates.TryGetValue(id, out mEventDelegate))
            {
                mEventDelegate = new EventDelegates();
                mEventDelegates.Add(id, mEventDelegate);
            }
            mEventDelegate.Add(edlg);
        }
    }
    #endregion

    // ----------------------------------------------------------
    // 注销事件
    // ----------------------------------------------------------
    #region 注销事件
    public void UnRegist(object eid, EventDelegate1 edlg)
    {
        Type type = eid.GetType();
        if (type.IsEnum)
        {
            Dictionary<int, EventDelegates> mEventDelegates = null;
            if (this.m_events.TryGetValue(type, out mEventDelegates))
            {
                int id = (int)eid;
                EventDelegates mEventDelegate = null;
                if (mEventDelegates.TryGetValue(id, out mEventDelegate))
                {
                    mEventDelegate.Remove(edlg);
                }
            }
        }
    }
    public void UnRegist(object eid, EventDelegate2 edlg)
    {
        Type type = eid.GetType();
        if (type.IsEnum)
        {
            Dictionary<int, EventDelegates> mEventDelegates = null;
            if (this.m_events.TryGetValue(type, out mEventDelegates))
            {
                int id = (int)eid;
                EventDelegates mEventDelegate = null;
                if (mEventDelegates.TryGetValue(id, out mEventDelegate))
                {
                    mEventDelegate.Remove(edlg);
                }
            }
        }
    }
    #endregion

    // ----------------------------------------------------------
    // 执行事件,使用事件ID和参数对象
    // ----------------------------------------------------------
    /// <summary>
    /// 执行事件
    /// </summary>
    /// <param name="eid">事件枚举ID</param>
    /// <param name="arg">事件参数对象</param>
    public void Fire(object eid, EventArg arg = null)
    {
        Type type = eid.GetType();
        if (this.m_events.ContainsKey(type))
        {
            EventDelegates edlgs;
            int id = (int)eid;
            if (this.m_events[type].TryGetValue(id, out edlgs))
            {
                edlgs.Call(arg);
            }
        }
    }
}