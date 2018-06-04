//================================================
//描 述 ： UI交互基础类，其他要交互的UI继承该类
//作 者 ： Leo
//创建时间 ：2018/05/30 11:39:26  
//版 本： 
// ================================================
using UnityEngine;

public class UIInteractiveBase : MonoBehaviour
{
    public static bool Interactive = true;
    /// <summary>
    /// 事件回调
    /// </summary>
    public EventCallback EventCallback;

    UnityEngine.UI.Button.ButtonClickedEvent _ButtonClickedEvent;

    UnityEngine.UI.Button.ButtonClickedEvent ButtonClickedEvent
    {
        get
        {
            if (_ButtonClickedEvent == null)
            {
                _ButtonClickedEvent = new UnityEngine.UI.Button.ButtonClickedEvent();
            }
            return _ButtonClickedEvent;
        }
    }
    /// <summary>
    /// 关闭(初始化)
    /// </summary>
    public virtual void Close()
    {
    }

    /// <summary>
    /// 按钮点击事件
    /// </summary>
    public void onClick()
    {
        if (Interactive)
        {
            Debug.Log(string.Format("{0}被点击", name));
            if (ButtonClickedEvent != null)
            {
                ButtonClickedEvent.Invoke();
            }
        }
    }

    /// <summary>
    /// 添加按钮点击事件
    /// </summary>
    public void AddButtonClickedEvent(UnityEngine.Events.UnityAction call)
    {
        if (ButtonClickedEvent != null)
        {
            ButtonClickedEvent.AddListener(call);
        }
    }
    /// <summary>
    /// 移除按钮点击事件
    /// </summary>
    public void RemoveButtonClickedEvent(UnityEngine.Events.UnityAction call)
    {
        if (ButtonClickedEvent != null)
        {
            ButtonClickedEvent.RemoveListener(call);
        }
    }

    /// <summary>
    /// 执行事件
    /// </summary>
    public void OnEvent(UIEventData eventData)
    {
        if (EventCallback != null && eventData != null)
        {
            EventCallback(eventData);
        }
    }
}