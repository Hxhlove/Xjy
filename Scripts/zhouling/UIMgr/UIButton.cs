//================================================
//描 述 ： UI按钮交互类
//作 者 ： Leo
//创建时间 ：2018/05/30 11:50:27  
//版 本： 
// ================================================
using UnityEngine;

public class UIButton : UIInteractiveBase
{
    void Awake()
    {
        AddButtonClickedEvent(Click);
    }

    /// <summary>
    /// 固定更新
    /// </summary>
    void FixedUpdate()
    {
        if (Isclick)
        {
            temporaryTime += Time.deltaTime;
            if (temporaryTime >= intervalTime)
            {
                Isclick = false;
            }
        }
    }

    /// <summary>
    /// 初始化按钮数据
    /// </summary>
    public void Inst(EventCallback EventCallback ,UIButtonElement UIButtonElement, GameObject NormalState, GameObject PressSelectState)
    {
        base.EventCallback = EventCallback;
        this.UIButtonElement = UIButtonElement;
        this.NormalState = NormalState;
        this.PressSelectState = PressSelectState;
        Close();
    }

    /// <summary>
    /// 按钮元素
    /// </summary>
    public UIButtonElement UIButtonElement;
    /// <summary>
    /// 正常模块
    /// </summary>
    GameObject NormalState;
    /// <summary>
    /// 选择模块
    /// </summary>
    GameObject PressSelectState;
    /// <summary>
    /// 点击事件切换间隔
    /// </summary>
    float intervalTime = 0.1f;
    float temporaryTime = 0f;
    bool _Isclick = false;
    /// <summary>
    /// 设置点击
    /// </summary>
    bool Isclick
    {
        set
        {
            _Isclick = value;
            temporaryTime = 0f;
            if (_Isclick)
            {
                SelectionEffect();
            }
            else
            {
                LeaveEffect();
            }
        }
        get
        {
            return _Isclick;
        }
    }



    /// <summary>
    /// 设置正常模块显示
    /// </summary>
    bool isNormalState
    {
        set
        {
            if (NormalState != null)
            {
                NormalState.SetActive(value);
            }
        }
    }
    /// <summary>
    /// 设置选择模块显示
    /// </summary>
    bool isPressSelectState
    {
        set
        {
            if (PressSelectState != null)
            {
                PressSelectState.SetActive(value);
            }
        }
    }

    /// <summary>
    /// 点击或选择状态
    /// </summary>
    private bool isClick = false;


    /// <summary>
    /// 关闭(初始化)
    /// </summary>
    public override void Close()
    {
        isClick = false;
        LeaveEffect();
    }

    /// <summary>
    /// 点击
    /// </summary>
    private void Click()
    {
        isClick = !isClick;

        if (UIButtonElement.OperationType == UIButtonOperationType.OnlyClick)
        {
            ClickEvents();
        }
        else
        {
            if (isClick)
            {
                ClickSelectionEvents();
            }
            else
            {
                ClickLeaveEvents();
            }
        }
    }

    /// <summary>
    /// 点击效果
    /// </summary>
    private void ClickEffect()
    {
        Isclick = true;
    }

    /// <summary>
    /// 选择效果
    /// </summary>
    private void SelectionEffect()
    {
        isNormalState = false;
        isPressSelectState = true;
    }
    /// <summary>
    /// 取消选择效果
    /// </summary>
    private void LeaveEffect()
    {
        isNormalState = true;
        isPressSelectState = false;
    }


    /// <summary>
    /// 执行点击事件
    /// </summary>
    private void ClickEvents()
    {
        ClickEffect();
        if (UIButtonElement.ClickEvents != null)
        {
            int length = UIButtonElement.ClickEvents.Length;            
            for (int i =0;i< length;i++)
            {
                OnEvent(UIButtonElement.ClickEvents[i]);
            }
        }
    }

    /// <summary>
    /// 执行点击选择事件
    /// </summary>
    private void ClickSelectionEvents()
    {
        SelectionEffect();
        if (UIButtonElement.ClickSelectionEvents != null)
        {
            int length = UIButtonElement.ClickSelectionEvents.Length;
            for (int i = 0; i < length; i++)
            {
                OnEvent(UIButtonElement.ClickSelectionEvents[i]);
            }
        }
    }
    /// <summary>
    /// 执行点击取消选择事件
    /// </summary>
    private void ClickLeaveEvents()
    {
        LeaveEffect();
        if (UIButtonElement.ClickLeaveEvents != null)
        {
            int length = UIButtonElement.ClickLeaveEvents.Length;
            for (int i = 0; i < length; i++)
            {
                OnEvent(UIButtonElement.ClickLeaveEvents[i]);
            }
        }
    }
}