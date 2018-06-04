
using System.Collections.Generic;
using UnityEngine;

//转换条件
public enum UITransition
{
    NullTransition = 0,
    Free,
    Btn1,
    Btn2,
    Btn3
}

//每个状态有独一无二的ID
public enum UIStateID
{
    NullState=0,
    Free,
    No1,
    No2,
    No3
}

public abstract class IUIState
{
    protected Dictionary<UITransition, UIStateID> mMap = new Dictionary<UITransition, UIStateID>();
    protected UIStateID mStateID;
    public UIStateID stateID { get { return mStateID; } }


    //相当于Animator里的那个连接线：添加转换条件
    public void AddTransition(UITransition trans, UIStateID id)
    {
        if (trans == UITransition.NullTransition)
        {
            Debug.LogError("转换条件 不能为空"); return;
        }
        if (id == UIStateID.NullState)
        {
            Debug.LogError("状态ID 不能为空"); return;
        }
        if (mMap.ContainsKey(trans))
        {
            Debug.LogError("已经添加"); return;
        }
        mMap.Add(trans, id);
    }

    public void DeleteTransition(UITransition trans)
    {
        if (!mMap.ContainsKey(trans))
        {
            Debug.LogError("删除转换条件时候，转换条件:[" + trans + "]不存在"); return;
        }
        mMap.Remove(trans);
    }

    //根据转换条件，判断是否可以转换。能就返回一个ID
    public UIStateID GetOutPutState(UITransition trans)
    {
        if (!mMap.ContainsKey(trans))
        {
            return UIStateID.NullState;
        }
        else
        {
            return mMap[trans];
        }
    }

    //选择性重写
    public virtual void DoBeforeEntering() { }
    public virtual void DoBeforeLeaving() { }

    //必须重写

    /// <summary>
    /// 什么情况下要转换状态
    /// </summary>
    public abstract void Reason(GameObject uiObj);
    /// <summary>
    /// 处理在该状态下的一些行为
    /// </summary>
    public abstract void Act();
}
