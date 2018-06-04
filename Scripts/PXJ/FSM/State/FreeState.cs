using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeState : IUIState
{

    public FreeState()
    {
        mStateID = UIStateID.Free;
    }

    //满足一定条件就切换状态
    public override void Reason(GameObject uiObj)
    {

        if (uiObj.GetComponent<BnCtr>())
        {
            BnCtr ctr = uiObj.GetComponent<BnCtr>();
            if (mMap.ContainsKey(ctr.trans))
            {
                //只发出转换条件，至于具体做啥不用管
                UIControl.Instance.SetTransition(ctr.trans);
            }
        }


    }
    public override void Act()
    {
        //在当前状态下 需要做什么
    }
}
