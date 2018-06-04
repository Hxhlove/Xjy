// ------------------------------------------------------------------
// Title        :全局定时回调
// Author       :Leo
// Date         :2018.05.04
// Description  :全局定时回调器，用于方法的定时调用
// ------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public delegate void TimerCallback1();
public delegate void TimerCallback2(params object[] args);

static class TimerMgr
{
    private static float mtime;

    /// <summary>
    /// 游戏开始到现在的时间
    /// </summary>
    public static float time
    {
        get { return mtime; }
    }
    /// <summary>
    /// 时间比例
    /// 大于1为快镜头，小于1为慢镜头
    /// </summary>
    public static float timeScale
    {
        get { return Time.timeScale; }
        set { Time.timeScale = value; }
    }
    /// <summary>
    /// 间隔时间
    /// 在所有更新方法的和上次调用这个方法之间的间隔,
    /// 如:Update,LateUpdate,
    /// 其他自己自定义的方法调用这个显示的是，Update,LateUpdate谁调用的自定义方法就显示谁的
    /// </summary>
    public static float deltaTime
    {
        get { return Time.deltaTime; }
    }
    /// <summary>
    /// 固定更新间隔设置
    /// </summary>
    public static float fixedDeltaTime
    {
        get { return Time.fixedDeltaTime; }
        set { Time.fixedDeltaTime = value; }
    }


    // --------------------------------------------------------------------
    // Timer 回调封装
    // --------------------------------------------------------------------
    #region Timer 回调基类
    private abstract class Callback
    {
        private readonly float m_interval;      // 隔多长时间触发一次 
        private float m_nextTime;               // 临时记录下一次应该执行的时间
        private readonly int m_count;           // 总共要执行的次数
        private int m_currCount;                // 临时记录当前已经执行过的次数

        /// <summary>
        /// 创建回调
        /// </summary>
        /// <param name="start">延迟时间</param>
        /// <param name="interval">多次回调间隔时间</param>
        /// <param name="count">回调次数</param>
        public Callback(float start, int count, float interval)
        {
            this.m_nextTime = time + start;
            this.m_count = count;
            this.m_interval = interval < 0 ? 0 : interval;
            this.m_currCount = 0;
        }
        /// <summary>
        /// 下次执行的时间（游戏时间）
        /// </summary>
        public float NextTime
        {
            get { return this.m_nextTime; }
        }

        /// <summary>
        /// 真实执行回调内容
        /// </summary>
        protected abstract void Call();

        /// <summary>
        /// 主进程执行下次回调 如果执行完本次回调，将无效的返回 true
        /// </summary>
        public bool TryCall()
        {
            if (this.m_nextTime > time)
                return false;
            try
            {
                this.Call();
            }
            catch (Exception err)
            {
                Debug.LogError(string.Format("定时器回调中发生异常 \"{0}\":\n{1}", err.StackTrace, err.Message));
            }
            if (this.m_count > 0)                           // 执行重复次数
            {
                if (++this.m_currCount >= this.m_count)
                {
                    return true;                            // 执行次数已经达到上限
                }
                else
                {
                    this.m_nextTime = time + this.m_interval;
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }

    #endregion

    #region 不带参数的 Timer 回调
    private class Callback1 : Callback
    {
        private TimerCallback1 m_cb;

        public Callback1(float start, int count, float interval, TimerCallback1 cb)
            : base(start, count, interval)

        {
            this.m_cb = cb;
        }

        protected override void Call()
        {
            this.m_cb();
        }
    }
    #endregion

    #region 带不定额外参数的 Timer 回调
    private class Callback2 : Callback
    {
        private TimerCallback2 m_cb;
        private object[] m_args;

        public Callback2(float start, int count, float interval, TimerCallback2 cb, object[] args)
            : base(start, count, interval)
        {
            this.m_cb = cb;
            this.m_args = args;
        }

        protected override void Call()
        {
            this.m_cb(this.m_args);
        }
    }
    #endregion

    // --------------------------------------------------------------------
    // 实现 Timer
    // --------------------------------------------------------------------
    private static Dictionary<Int64, Callback> sm_cbs;  // 回调列表
    private static Int64 sm_handle;                     // 当前 TimerID 值
    private static Int64 sm_ID;                         // ID大于最大值时使用的ID值
    private static float sm_nextTime;                   // 下次回调时间

    static TimerMgr()
    {
        sm_cbs = new Dictionary<Int64, Callback>();
        sm_handle = 1;
        sm_ID = 1;
        sm_nextTime = time;
    }

    // ----------------------------------------------------------
    // private
    // ----------------------------------------------------------
    private static Int64 GenTimerID()
    {
        if (sm_handle < Int64.MaxValue)
        {
            return ++sm_handle;
        }
        while (++sm_ID < Int64.MaxValue)
        {
            if (!sm_cbs.ContainsKey(sm_ID))
            {
                return sm_ID;
            }
        }
        if (sm_ID >= Int64.MaxValue) { sm_ID = 1; }
        while (++sm_ID < Int64.MaxValue)
        {
            if (!sm_cbs.ContainsKey(sm_ID))
            {
                return sm_ID;
            }
        }
        Exception ex = new Exception(string.Format("回调ID大于最大值:{0}", Int64.MaxValue));
        Debug.LogError(ex.Message + ex.StackTrace);
        throw ex;
    }

    // ----------------------------------------------------------
    // 使用固定更新时间比较准确
    // ----------------------------------------------------------
    public static void FixedUpdate()
    {
        mtime = Time.time;
        float now = time;
        if (now < sm_nextTime) return;                          // 时间还没到最近一个 Timer（防止频繁检索执行列表）
        if (sm_cbs.Count == 0) return;                          // 没有回调列表直接返回

        sm_nextTime = now + 100.0f;                             // 随便给个大点的下次执行时间
        List<Int64> invalids = new List<Int64>();
        Callback cb = null;
        foreach (Int64 id in sm_cbs.Keys.ToArray<Int64>())
        {
            if (sm_cbs.TryGetValue(id, out cb))
            {
                if (cb.TryCall())
                {
                    invalids.Add(id);
                }
                else if (cb.NextTime < sm_nextTime)
                {
                    sm_nextTime = cb.NextTime;                   // 搜索最近的下一个 Timer 的执行时间（防止频繁检索执行列表）} 
                }
            }
        }
        int count = invalids.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                sm_cbs.Remove(invalids[i]);                         // 去除已经无效的 Timer
            }
        }
    }

    // ----------------------------------------------------------
    // public
    // ----------------------------------------------------------
    // ----------------------------------------------------------
    // 添加一个 Timer
    // 参数：
    //     delay   : 延迟多长时间后开启回调，如果为 0，则在下一帧时立刻调用
    //     count   : 回调次数
    //     interval: 间隔多长时间回调一次，和回调次数配合使用,如果为0表示每帧都用一次到回调次数结束为止
    //     cb      : 回调方法
    //     args    : 为额外参数
    // 返回：
    //     返回一个长整形数值，表示 TimerID，通过调用 Timer.Cancel(TimerID) 能取消整个 Timer
    // ----------------------------------------------------------

    /// <summary>
    /// 下一帧直接回调
    /// </summary>
    public static Int64 Add(TimerCallback1 cb)
    {
        return Add(0, cb);
    }
    /// <summary>
    /// 延迟时间后回调
    /// </summary>
    public static Int64 Add(float delay, TimerCallback1 cb)
    {
        return Add(delay, 0, cb);
    }
    /// <summary>
    /// 延迟回调后,再每帧调用到指定次数
    /// </summary>
    public static Int64 Add(float delay, int count, TimerCallback1 cb)
    {
        return Add(delay, count, 0, cb);
    }
    /// <summary>
    /// 延迟回调后,按照指定间隔回调指定次数
    /// </summary>
    public static Int64 Add(float delay, int count, float interval, TimerCallback1 cb)
    {
        Int64 timerID = TimerMgr.GenTimerID();
        sm_nextTime = Math.Min(sm_nextTime, time + delay);
        sm_cbs[timerID] = new Callback1(delay, count, interval, cb);
        return timerID;
    }
    /// <summary>
    /// 下一帧直接回调
    /// </summary>
    public static Int64 Add(TimerCallback2 cb, params object[] args)
    {
        return Add(0, cb, args);
    }
    /// <summary>
    /// 延迟时间后回调
    /// </summary>
    public static Int64 Add(float delay, TimerCallback2 cb, params object[] args)
    {
        return Add(delay, 0, cb, args);
    }
    /// <summary>
    /// 延迟回调后,再每帧调用到指定次数
    /// </summary>
    public static Int64 Add(float delay, int count, TimerCallback2 cb, params object[] args)
    {
        return Add(delay, count, 0, cb, args);
    }
    /// <summary>
    /// 延迟回调后,按照指定间隔回调指定次数
    /// </summary>
    public static Int64 Add(float delay, int count, float interval, TimerCallback2 cb, params object[] args)
    {
        Int64 timerID = TimerMgr.GenTimerID();
        sm_nextTime = Math.Min(sm_nextTime, time + delay);
        sm_cbs[timerID] = new Callback2(delay, count, interval, cb, args);
        return timerID;
    }
    // ----------------------------------------------------------
    // 取消一个 Timer
    // 参数：timerID: 要取消的 Timer
    // ----------------------------------------------------------
    public static void Cancel(Int64 timerID)
    {
        Callback cb;
        if (sm_cbs.TryGetValue(timerID, out cb))
        {
            sm_cbs.Remove(timerID);
        }
    }
}