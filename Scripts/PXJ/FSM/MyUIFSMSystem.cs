using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理所有的State对象
public class MyUIFSMSystem
{
    private List<IUIState> mStates = new List<IUIState>();
    private IUIState mCurrentState;
    public IUIState currentState { get { return mCurrentState; } }



    public void AddState(IUIState state)
    {
        if (state == null)
        {
            Debug.LogError("要添加的状态为空"); return;
        }
        //若集合数量为0，则设置默认的状态。
        if (mStates.Count == 0)
        {
            mStates.Add(state);
            mCurrentState = state;
           // Debug.Log("添加好了" + state+"切是默认状态");
            return;
        }

        foreach (IUIState s in mStates)
        {
            if (s.stateID == state.stateID)
            {
                Debug.LogError("要添加的状态ID:[" + s.stateID + "]已经添加"); return;
            }
        }
        mStates.Add(state);
      //  Debug.Log("添加好了" + state);
    }

    public void DeleteState(UIStateID stateID)
    {
        if (stateID == UIStateID.NullState)
        {
            Debug.LogError("要删除的状态ID为空:[" + stateID + "]"); return;
        }

        foreach (IUIState s in mStates)
        {
            if (s.stateID == stateID)
            {
                mStates.Remove(s); return;
            }
        }
        Debug.LogError("要删除的StateID不存在集合中：" + stateID);
    }

    //执行转换
    public void PerformTransition(UITransition trans)
    {
        if (trans == UITransition.NullTransition)
        {
            Debug.LogError("要执行的转换条件为空：[" + trans + "]"); return;
        }
        UIStateID nextstateID = mCurrentState.GetOutPutState(trans);
        if (nextstateID == UIStateID.NullState)
        {
            Debug.LogError("在[" + trans + "]转换条件下,没有对应的转换状态"); return;
        }
        foreach (IUIState s in mStates)
        {
            if (s.stateID == nextstateID)//说明s是即将转换的状态
            {
                mCurrentState.DoBeforeLeaving();
                mCurrentState = s;
                mCurrentState.DoBeforeEntering();
                return;
            }
        }
    }
}
