using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void UIOnClick(GameObject obj);
public class UIControl : MonoSingleInstance<UIControl>
{

    public MyUIFSMSystem fsm;

    public void SetTransition(UITransition t)
    {
        fsm.PerformTransition(t);
        Debug.Log("状态发生改变，为：" + fsm.currentState);
    }

    public void Start()
    {
        MakeFSM();
        SimpleTouchSystem.Instance.uionclick = UIOnClick;
        Debug.Log(fsm.currentState + "*********************");
    }

    public void UIOnClick(GameObject obj)
    {
       // Debug.Log(obj + "执行了委托。并点击到了UI");

        fsm.currentState.Reason(obj);
        fsm.currentState.Act();

    }
    //构建状态机
    public void MakeFSM()
    {
        //空闲状态机：
        FreeState freeState = new FreeState();
        //区位规划状态机:
        ProgrammeState programmeState = new ProgrammeState();
        //区位沙盘状态机：
        SandTableState sandtableState = new SandTableState();

        //添加转换条件：当在Free条件下，要切换状态为free
        freeState.AddTransition(UITransition.Free, UIStateID.Free);
        freeState.AddTransition(UITransition.Btn1, UIStateID.No1);
        freeState.AddTransition(UITransition.Btn2, UIStateID.No2);

        programmeState.AddTransition(UITransition.Free, UIStateID.Free);
        programmeState.AddTransition(UITransition.Btn1, UIStateID.No1);
        programmeState.AddTransition(UITransition.Btn2, UIStateID.No2);

        sandtableState.AddTransition(UITransition.Free, UIStateID.Free);
        sandtableState.AddTransition(UITransition.Btn1, UIStateID.No1);
        sandtableState.AddTransition(UITransition.Btn2, UIStateID.No2);


        fsm = new MyUIFSMSystem();

        fsm.AddState(freeState);
        fsm.AddState(programmeState);
        fsm.AddState(sandtableState);
    }


}
