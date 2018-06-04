using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandTableState : IUIState
{
    public SandTableState()
    {
        mStateID = UIStateID.No2;
    }
    //只发出转换条件，至于具体做啥不用管
    public override void Reason(GameObject uiObj)
    {
        if (uiObj.GetComponent<BnCtr>())
        {
            BnCtr ctr = uiObj.GetComponent<BnCtr>();
     //       Debug.Log(mMap.Count);
            if (mMap.ContainsKey(ctr.trans))
            {
                UIStateID id = mMap[ctr.trans];
                if (UIControl.Instance.fsm.currentState.stateID.Equals(id))
                {
                    UIControl.Instance.SetTransition(UITransition.Free);
                    return;
                }
                UIControl.Instance.SetTransition(ctr.trans);
            }
        }
    }

    public override void Act()
    {
        Debug.Log("打开这个界面的相关内容");
 }
}
