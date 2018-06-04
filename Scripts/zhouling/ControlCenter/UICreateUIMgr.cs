//================================================
//描 述 ： 使用UI配置文件进行创建UI
//作 者 ： Leo
//创建时间 ：2018/05/28 18:00:33  
//版 本： 
// ================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 事件回调
/// </summary>
public delegate void EventCallback(UIEventData eventData);

public class UICreateUIMgr
{
    static UICreateUIMgr sm_inst;

    public static UICreateUIMgr Inst
    {
        get
        {
            if (sm_inst == null)
                sm_inst = new UICreateUIMgr();
            return sm_inst;
        }
    }

    /// <summary>
    /// UI原始配置数据
    /// </summary>
    Dictionary<string, ScreenUIConfigureData> UIConfigureDatas;


    /// <summary>
    /// 开启
    /// </summary>
    public void Open()
    {
        EventMgr.Inst.Regist(UIEventDataType.OpenUI, OpenUI);
        EventMgr.Inst.Regist(UIEventDataType.CloseUI, CloseUI);
        UIConfigureDatas = new Dictionary<string, ScreenUIConfigureData>();
        foreach (ScreenUIConfigureData SUICD in ResourcesEx.LoadAll<ScreenUIConfigureData>(PathFileTool.UIConfigureData))
        {
            if (SUICD != null)
            {
                if (UIConfigureDatas.ContainsKey(SUICD.UINodeData.Describe))
                {
                    Debug.LogError(string.Format("UI配置文件{0}和{1}有相同的描述:{2}", SUICD.name, UIConfigureDatas[SUICD.UINodeData.Describe].name, SUICD.UINodeData.Describe));
                }
                else
                {
                    UIConfigureDatas.Add(SUICD.UINodeData.Describe, SUICD);
                }
            }
        }
    }
    /// <summary>
    /// 更新
    /// </summary>
    public void FixedUpdate()
    {
    }

    private void CreateUI(string UINode)
    {
        if (UIConfigureDatas.ContainsKey(UINode))
        {
            ScreenUIConfigureData suicd = UIConfigureDatas[UINode];

        }
        else
        {
            Debug.LogError(string.Format("没有UI配置数据:{0},无法创建UI", UINode));
        }
    }


    /// <summary>
    /// 打开UI事件
    /// </summary>
    private void OpenUI(EventArg ea)
    {
        List<string> UIStructure = ea[0] as List<string>;
        if (UIStructure != null)
        {
            string node = string.Empty;

            for (int i = 0; i < UIStructure.Count; i++)
            {
                if (i == 0)
                {
                    node = UIStructure[0];
                    if (node != "" && node != string.Empty)
                    {

                    }
                }
            }
        }
        else
        {
            Debug.Log("打开UI事件数据为空");
        }
    }
    /// <summary>
    /// 关闭UI事件
    /// </summary>
    private void CloseUI(EventArg ea)
    {
        List<string> UIStructure = ea[0] as List<string>;
        if (UIStructure != null)
        {
            string node = string.Empty;

            for (int i = 0; i < UIStructure.Count; i++)
            {
                if (i == 0)
                {
                    node = UIStructure[0];
                    if (node != "" && node != string.Empty)
                    {

                    }
                }
            }
        }
        else
        {
            Debug.Log("打开UI事件数据为空");
        }
    }

    //=============================================================创建工具=====================================

    /// <summary>
    /// 创建按钮
    /// </summary>
    public void CreateButtonNode(UIButtonElement element, Transform parent)
    {
        GameObject basicsObj = CreateUIBasicsNode(element, parent);
        UIButton interactive = basicsObj.AddComponent<UIButton>();

        Button button = basicsObj.AddComponent<Button>();
        button.transition = Selectable.Transition.None;
        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
        button.onClick.AddListener(interactive.onClick);

        GameObject NormalState = CreateButtonStateElement(element.NormalState, basicsObj.transform);
        GameObject PressSelectState = CreateButtonStateElement(element.PressSelectState, basicsObj.transform);

        interactive.Inst(EventCallback, element, NormalState, PressSelectState);
    }

    /// <summary>
    /// 创建UI按钮状态元素
    /// </summary>
    public GameObject CreateButtonStateElement(ButtonStateElement element, Transform parent)
    {
        GameObject basicsObj = CreateUIBasicsNode(element, parent);
        foreach (UIRawImageElement e in element.UIRawImageElements)
        {
            CreateRawImageNode(e, basicsObj.transform);
        }
        foreach (UIImageElement e in element.UITextureElements)
        {
            CreateImageNode(e, basicsObj.transform);
        }
        foreach (UITextElement e in element.UITextElements)
        {
            CreateUITextNode(e, basicsObj.transform);
        }
        foreach (UIObjectElement e in element.UIObjectElements)
        {
            CreateObjectNode(e, basicsObj.transform);
        }
        return basicsObj;
    }


    /// <summary>
    /// 创建UI基础节点,基础数据和父节点
    /// </summary>
    public GameObject CreateUIBasicsNode(UIElementBasics element, Transform parent)
    {
        GameObject basicsObj = new GameObject(element.Describe);
        basicsObj.transform.parent = parent;
        RectTransform rt = basicsObj.AddComponent<RectTransform>();
        SetAnchor(element.ScreenPointType, rt);
        SetPivot(element.DirectionType, rt);
        rt.anchoredPosition = element.Deviation;
        rt.sizeDelta = element.WideAndHigh;
        return basicsObj;
    }

    /// <summary>
    /// 创建UI文本节点
    /// </summary>
    public void CreateUITextNode(UITextElement element, Transform parent)
    {
        GameObject basicsObj = CreateUIBasicsNode(element, parent);
        basicsObj.AddComponent<CanvasRenderer>();
        Text Text = basicsObj.AddComponent<Text>();
        Text.text = element.Text;
        Text.font = element.Font;
        SetFontType(element.FontType, Text);
        Text.fontSize = element.FontScale;
        Text.lineSpacing = element.RowSpacing;
        SetTextAlignmentType(element.AlignmentType, Text);
        if (element.HorizontalOverflow)
        {
            Text.horizontalOverflow = HorizontalWrapMode.Overflow;
        }
        else
        {
            Text.horizontalOverflow = HorizontalWrapMode.Wrap;
        }
        if (element.VerticalOverflow)
        {
            Text.verticalOverflow = VerticalWrapMode.Overflow;
        }
        else
        {
            Text.verticalOverflow = VerticalWrapMode.Truncate;
        }
        Text.color = element.Color;
    }


    /// <summary>
    /// 创建UIRawImage图片节点
    /// </summary>
    public void CreateRawImageNode(UIRawImageElement element, Transform parent)
    {
        GameObject basicsObj = CreateUIBasicsNode(element, parent);
        basicsObj.AddComponent<CanvasRenderer>();
        RawImage RawImage = basicsObj.AddComponent<RawImage>();
        RawImage.texture = element.Texture;
        RawImage.uvRect = new Rect(element.UVShifting, element.UVScale);
        if (element.SetNativeSize)
        {
            RectTransform rt = basicsObj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(element.Texture.width, element.Texture.height);
        }
    }


    /// <summary>
    /// 创建UIRawImage图片节点
    /// </summary>
    public void CreateImageNode(UIImageElement element, Transform parent)
    {
        GameObject basicsObj = CreateUIBasicsNode(element, parent);
        basicsObj.AddComponent<CanvasRenderer>();
        Image Image = basicsObj.AddComponent<Image>();
        Image.sprite = element.sprite;
        if (element.SetNativeSize)
        {
            RectTransform rt = basicsObj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(element.sprite.texture.width, element.sprite.texture.height);
        }

        Image.Type type = Image.Type.Simple;
        switch (element.InageType)
        {
            case InageType.Simple:
                type = Image.Type.Simple;
                break;
            case InageType.Sliced:
                type = Image.Type.Sliced;
                break;
            case InageType.Tiled:
                type = Image.Type.Tiled;
                break;
            case InageType.Filled:
                type = Image.Type.Filled;
                break;
        }
        Image.type = type;
    }

    /// <summary>
    /// 创建UI对象节点
    /// </summary>
    public void CreateObjectNode(UIObjectElement element, Transform parent)
    {
        GameObject basicsObj = CreateUIBasicsNode(element, parent);
        GameObject Obj = Object.Instantiate(element.GameObject);
        Obj.transform.parent = basicsObj.transform;

        RectTransform rt = Obj.GetComponent<RectTransform>();
        if (rt == null)
        {
            rt = Obj.AddComponent<RectTransform>();
        }
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(0, 0);
    }


    /// <summary>
    /// 设置UI节点父类对齐方式
    /// </summary>
    public void SetAnchor(UIDrawingPointType type, RectTransform rt)
    {
        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;
        switch (type)
        {
            case UIDrawingPointType.Left_Up:
                min = new Vector2(0, 1);
                max = new Vector2(0, 1);
                break;
            case UIDrawingPointType.Left_Middle:
                min = new Vector2(0, 0.5f);
                max = new Vector2(0, 0.5f);
                break;
            case UIDrawingPointType.Left_Down:
                min = new Vector2(0, 0);
                max = new Vector2(0, 0);
                break;
            case UIDrawingPointType.Middle_Up:
                min = new Vector2(0.5f, 1);
                max = new Vector2(0.5f, 1);
                break;
            case UIDrawingPointType.Middle_Middle:
                min = new Vector2(0.5f, 0.5f);
                max = new Vector2(0.5f, 0.5f);
                break;
            case UIDrawingPointType.Middle_Down:
                min = new Vector2(0.5f, 0);
                max = new Vector2(0.5f, 0);
                break;
            case UIDrawingPointType.Right_Up:
                min = new Vector2(1, 1);
                max = new Vector2(1, 1);
                break;
            case UIDrawingPointType.Right_Middle:
                min = new Vector2(1, 0.5f);
                max = new Vector2(1, 0.5f);
                break;
            case UIDrawingPointType.Right_Down:
                min = new Vector2(1, 0);
                max = new Vector2(1, 0);
                break;
        }
        rt.anchorMin = min;
        rt.anchorMax = max;
    }

    /// <summary>
    /// 设置UI节点轴对齐方式
    /// </summary>
    public void SetPivot(UIDrawingPointType type, RectTransform rt)
    {
        Vector2 pivot = Vector2.zero;
        switch (type)
        {
            case UIDrawingPointType.Left_Up:
                pivot = new Vector2(0, 1);
                break;
            case UIDrawingPointType.Left_Middle:
                pivot = new Vector2(0, 0.5f);
                break;
            case UIDrawingPointType.Left_Down:
                pivot = new Vector2(0, 0);
                break;
            case UIDrawingPointType.Middle_Up:
                pivot = new Vector2(0.5f, 1);
                break;
            case UIDrawingPointType.Middle_Middle:
                pivot = new Vector2(0.5f, 0.5f);
                break;
            case UIDrawingPointType.Middle_Down:
                pivot = new Vector2(0.5f, 0);
                break;
            case UIDrawingPointType.Right_Up:
                pivot = new Vector2(1, 1);
                break;
            case UIDrawingPointType.Right_Middle:
                pivot = new Vector2(1, 0.5f);
                break;
            case UIDrawingPointType.Right_Down:
                pivot = new Vector2(1, 0);
                break;
        }
        rt.pivot = pivot;
    }

    /// <summary>
    /// 设置UI节点轴对齐方式
    /// </summary>
    public void SetFontType(FontType type, Text text)
    {
        FontStyle fontStyle = FontStyle.Normal;
        switch (type)
        {
            case FontType.Normal:
                fontStyle = FontStyle.Normal;
                break;
            case FontType.Bold:
                fontStyle = FontStyle.Bold;
                break;
            case FontType.Italic:
                fontStyle = FontStyle.Italic;
                break;
            case FontType.BoldAndItalic:
                fontStyle = FontStyle.BoldAndItalic;
                break;
        }
        text.fontStyle = fontStyle;
    }
    /// <summary>
    /// 设置文本自身对齐方式
    /// </summary>
    public void SetTextAlignmentType(UIDrawingPointType alignmentType, Text text)
    {
        TextAnchor alignment = TextAnchor.UpperLeft;
        switch (alignmentType)
        {
            case UIDrawingPointType.Left_Up:
                alignment = TextAnchor.UpperLeft;
                break;
            case UIDrawingPointType.Left_Middle:
                alignment = TextAnchor.MiddleLeft;
                break;
            case UIDrawingPointType.Left_Down:
                alignment = TextAnchor.LowerLeft;
                break;
            case UIDrawingPointType.Middle_Up:
                alignment = TextAnchor.UpperCenter;
                break;
            case UIDrawingPointType.Middle_Middle:
                alignment = TextAnchor.MiddleCenter;
                break;
            case UIDrawingPointType.Middle_Down:
                alignment = TextAnchor.LowerCenter;
                break;
            case UIDrawingPointType.Right_Up:
                alignment = TextAnchor.UpperRight;
                break;
            case UIDrawingPointType.Right_Middle:
                alignment = TextAnchor.MiddleRight;
                break;
            case UIDrawingPointType.Right_Down:
                alignment = TextAnchor.LowerRight;
                break;
        }
        text.alignment = alignment;
    }


    //=============================================================事件工具=====================================

    /// <summary>
    /// 事件回调
    /// </summary>
    public void EventCallback(UIEventData eventData)
    {

    }


}