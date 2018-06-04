using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammeState : IUIState
{

    public ProgrammeState()
    {
        mStateID = UIStateID.No1;
    }

    //按钮控制
    BnCtr bnCtr;


    //满足条件切换状态
    //只发出转换条件，至于具体做啥不用管
    public override void Reason(GameObject uiObj)
    {

        if (uiObj.GetComponent<BnCtr>())
        {
            bnCtr = uiObj.GetComponent<BnCtr>();
            if (mMap.ContainsKey(bnCtr.trans))
            {
                UIStateID id = mMap[bnCtr.trans];
                if (UIControl.Instance.fsm.currentState.stateID.Equals(id))
                {
                    UIControl.Instance.SetTransition(UITransition.Free);
                    return;
                }
                UIControl.Instance.SetTransition(bnCtr.trans);
                bnCtr.BnDown();
            }
        }
    }



    public override void Act()
    {
        //在当前状态下 做
        if (bnCtr != null)
            bnCtr.BnDown();
        Debug.Log("打开这个界面的相关内容");
    }
}
