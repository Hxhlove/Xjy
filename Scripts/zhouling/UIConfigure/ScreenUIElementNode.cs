// ------------------------------------------------------------------
// Title        :屏幕UI元素数据
// Author       :Leo
// Date         :2018.05.22
// Description  :用于配置屏幕UI元素
// ------------------------------------------------------------------
using UnityEngine;

/// <summary>
/// 元素基础类
/// </summary>
[System.Serializable]
public class UIElementBasics
{
    [Header("元素描述")]
    /// <summary>
    /// 该UI描述
    /// </summary>
    public string Describe;

    [Header("元素节点是否激活")]
    /// <summary>
    /// 元素节点是否激活
    /// </summary>
    public bool isActivate = true;
    /// <summary>
    /// 屏幕描点类型,相对于父节点
    /// </summary>
    [EnumLabel("描点类型")]
    public UIDrawingPointType ScreenPointType = UIDrawingPointType.Middle_Middle;
    /// <summary>
    /// 方向类型
    /// </summary>
    [EnumLabel("自身中心点方向类型")]
    public UIDrawingPointType DirectionType = UIDrawingPointType.Middle_Middle;
    [Header("相对于描点位置的偏移")]
    /// <summary>
    /// 偏移,和描点位置的偏移量
    /// </summary>
    public Vector2 Deviation;
    [Header("元素显示宽高区域")]
    /// <summary>
    /// 元素显示宽高区域
    /// </summary>
    public Vector2 WideAndHigh;
    [Header("元素顺序")]
    /// <summary>
    /// 元素顺序,
    /// </summary>
    public int Order = -1;
}

/// <summary>
/// 文本元素
/// </summary>
[System.Serializable]
public class UITextElement : UIElementBasics
{
    /// <summary>
    /// 构造函数设置默认参数
    /// </summary>
    public UITextElement()
    {
        //激活
        isActivate = true;
        //默认描述
        Describe = "Text文本";
        //默认显示大小
        WideAndHigh = new Vector2(100, 100);
        //默认文本
        Text = "New Text";
        //默认字体样式
        FontType = FontType.Normal;
        //默认字体大小
        FontScale = 14;
        //默认行距
        RowSpacing = 1;
        //默认文本对齐方式
        AlignmentType = UIDrawingPointType.Left_Up;
        //默认水平溢出
        HorizontalOverflow = false;
        //默认垂直溢出
        VerticalOverflow = false;
        //默认颜色
        Color = new Color32(50, 50, 50, 255);
    }

    [Header("文本")]
    /// <summary>
    /// 图片
    /// </summary>
    public string Text = "New Text";
    [Header("字体")]
    /// <summary>
    /// 字体
    /// </summary>
    public Font Font;
    /// <summary>
    /// 字体样式(没有/加粗/斜体/加粗+斜体)
    /// </summary>
    [EnumLabel("字体样式(没有|加粗|斜体|加粗+斜体)")]
    public FontType FontType = FontType.Normal;
    [Header("字体大小")]
    /// <summary>
    /// 字体大小
    /// </summary>
    public int FontScale = 14;
    [Header("行距")]
    /// <summary>
    /// 行距
    /// </summary>
    public int RowSpacing = 1;
    /// <summary>
    /// 文本对齐方式
    /// </summary>
    [EnumLabel("文本对齐方式")]
    public UIDrawingPointType AlignmentType = UIDrawingPointType.Left_Up;
    [Header("是否水平溢出")]
    /// <summary>
    /// 是否水平溢出
    /// </summary>
    public bool HorizontalOverflow = false;
    [Header("是纵向平溢出")]
    /// <summary>
    /// 是纵向平溢出
    /// </summary>
    public bool VerticalOverflow = false;
    [Header("文本颜色")]
    /// <summary>
    /// 文本颜色
    /// </summary>
    public Color Color = new Color32(50, 50, 50, 255);
}


/// <summary>
/// Texture图片纹理元素
/// </summary>
[System.Serializable]
public class UIRawImageElement : UIElementBasics
{
    /// <summary>
    /// 构造函数设置默认参数
    /// </summary>
    public UIRawImageElement()
    {
        //激活
        isActivate = true;
        //默认描述
        Describe = "Texture图片";
        //默认对齐父对象
        ScreenPointType = UIDrawingPointType.Middle_Middle;
        //默认偏移方式
        DirectionType = UIDrawingPointType.Middle_Middle;
        //默认显示大小
        WideAndHigh = new Vector2(100, 100);
        //默认UI偏移
        UVShifting = new Vector2(0, 0);
        //默认UI缩放
        UVScale = new Vector2(1, 1);
        //默认不使用图片大小设置显示区域
        SetNativeSize = false;
    }

    [Header("图片")]
    /// <summary>
    /// 图片
    /// </summary>
    public Texture Texture;
    [Header("UV偏移")]
    /// <summary>
    /// UV偏移
    /// </summary>
    public Vector2 UVShifting = new Vector2(0, 0);
    [Header("UV缩放")]
    /// <summary>
    /// UV缩放
    /// </summary>
    public Vector2 UVScale = new Vector2(1, 1);
    [Header("是否使用图片尺寸进行设置显示区域")]
    /// <summary>
    /// 是否使用图片尺寸进行设置显示区域
    /// </summary>
    public bool SetNativeSize = false;
}

/// <summary>
/// Sprite图片纹理元素
/// </summary>
[System.Serializable]
public class UIImageElement : UIElementBasics
{
    /// <summary>
    /// 构造函数设置默认参数
    /// </summary>
    public UIImageElement()
    {
        //激活
        isActivate = true;
        //默认描述
        Describe = "Sprite图片";
        //默认对齐父对象
        ScreenPointType = UIDrawingPointType.Middle_Middle;
        //默认偏移方式
        DirectionType = UIDrawingPointType.Middle_Middle;
        //默认显示大小
        WideAndHigh = new Vector2(100, 100);
        //默认图片显示方式
        InageType = InageType.Simple;
        //默认不使用图片大小设置显示区域
        SetNativeSize = false;
    }

    [Header("Sprite图片")]
    /// <summary>
    /// 图片
    /// </summary>
    public Sprite sprite;
    [EnumLabel("图片显示类型")]
    /// <summary>
    /// 图片显示类型
    /// </summary>
    public InageType InageType = InageType.Simple;

    [Header("是否使用图片尺寸进行设置显示区域")]
    /// <summary>
    /// 是否使用图片尺寸进行设置显示区域
    /// </summary>
    public bool SetNativeSize = false;
}

/// <summary>
/// 对象元素
/// </summary>
[System.Serializable]
public class UIObjectElement : UIElementBasics
{
    /// <summary>
    /// 构造函数设置默认参数
    /// </summary>
    public UIObjectElement()
    {
        //激活
        isActivate = true;
        //默认描述
        Describe = "UIObject对象";
        //默认对齐父对象
        ScreenPointType = UIDrawingPointType.Middle_Middle;
        //默认偏移方式
        DirectionType = UIDrawingPointType.Middle_Middle;
        //默认显示大小
        WideAndHigh = new Vector2(100, 100);
    }
    [Header("预制对象")]
    /// <summary>
    /// 预制对象
    /// </summary>
    public GameObject GameObject;
}

/// <summary>
/// 按钮状态元素
/// </summary>
[System.Serializable]
public class ButtonStateElement : UIElementBasics
{
    [Header("RawImage图片元素集合")]
    /// <summary>
    /// RawImage图片集合
    /// </summary>
    public UIRawImageElement[] UIRawImageElements;
    [Header("Image图片元素集合")]
    /// <summary>
    /// Image图片集合
    /// </summary>
    public UIImageElement[] UITextureElements;
    [Header("文本元素集合")]
    /// <summary>
    /// 文本元素集合
    /// </summary>
    public UITextElement[] UITextElements;
    [Header("对象元素集合")]
    /// <summary>
    /// 对象元素集合
    /// </summary>
    public UIObjectElement[] UIObjectElements;
}

/// <summary>
/// 按钮元素
/// </summary>
[System.Serializable]
public class UIButtonElement : UIElementBasics
{
    /// <summary>
    /// 按钮操作类型
    /// </summary>
    [EnumLabel("按钮操作类型")]
    public UIButtonOperationType OperationType;

    [Header("正常状态")]
    /// <summary>
    /// 正常状态
    /// </summary>
    public ButtonStateElement NormalState;
    [Header("按下选择状态")]
    /// <summary>
    /// 按下选择状态
    /// </summary>
    public ButtonStateElement PressSelectState;

    [Header("点击事件")]
    /// <summary>
    /// 点击事件
    /// </summary>
    public UIEventData[] ClickEvents;
    [Header("点击选择事件")]
    /// <summary>
    /// 点击选择状态事件
    /// </summary>
    public UIEventData[] ClickSelectionEvents;
    [Header("点击取消选择关闭事件")]
    /// <summary>
    /// 点击取消选择关闭事件
    /// </summary>
    public UIEventData[] ClickLeaveEvents;
}

// ------------------------------------------------------------------
// Title        :按钮组合元素数据
// Author       :Leo
// Date         :2018.05.23
// Description  :使用按钮组合的按钮一般使用基础配置的图片和音效，如果
//              集合按钮内有配置独立的图片或其他的使用独立配置的进行设置
// ------------------------------------------------------------------
/// <summary>
/// 组合按钮元素
/// </summary>
[System.Serializable]
public class UIGroupButtonElements : UIElementBasics
{
    /// <summary>
    /// 组合按钮方向类型
    /// </summary>
    [EnumLabel("按钮组合类型(横向|纵向|替换)")]
    public UIGroupType GroupButtonType;
    /// <summary>
    /// 组合按钮操作类型
    /// </summary>
    [EnumLabel("组合按钮操作类型(一个激活|多个激活)")]
    public UIButtonGroupOperationType UIButtonGroupOperationType;
    [Header("间隔距离")]
    /// <summary>
    /// 间隔距离
    /// </summary>
    public int Interval;
    /// <summary>
    /// 按钮操作类型
    /// </summary>
    [EnumLabel("按钮操作类型")]
    public UIButtonOperationType OperationType;
    [Header("按钮文本")]
    /// <summary>
    /// 按钮文本
    /// </summary>
    public string ButtonText;
    [Header("正常图片")]
    /// <summary>
    /// 正常图片
    /// </summary>
    public Texture NormalTexture;
    [Header("正常图片显示大小")]
    /// <summary>
    /// 正常图片显示大小
    /// </summary>
    public Vector2 NormalTextureScale;
    [Header("选择图片")]
    /// <summary>
    /// 选择图片
    /// </summary>
    public Texture ChoiceTexture;
    [Header("选择图片显示大小")]
    /// <summary>
    /// 选择图片显示大小
    /// </summary>
    public Vector2 ChoiceTextureScale;
    [Header("点击事件")]
    /// <summary>
    /// 点击事件
    /// </summary>
    public UIEventData[] ClickEvents;
    [Header("点击选择事件")]
    /// <summary>
    /// 点击选择状态事件
    /// </summary>
    public UIEventData[] ClickSelectionEvents;
    [Header("点击取消选择关闭事件")]
    /// <summary>
    /// 点击取消选择关闭事件
    /// </summary>
    public UIEventData[] ClickLeaveEvents;

    [Header("按钮集合")]
    /// <summary>
    /// 按钮集合
    /// </summary>
    public UIButtonElement[] UIButtonElement;
}

/// <summary>
/// 独立单元UI节点
/// </summary>
[System.Serializable]
public class ScreenUIElementNode : UIElementBasics
{
    [Header("场景位置参考对象")]
    /// <summary>
    /// 场景位置参考对象,该对象不为空时该元素节点为场景元素对象
    /// </summary>
    public GameObject SceneReferenceObject;
    [Header("RawImage图片元素集合")]
    /// <summary>
    /// RawImage图片集合
    /// </summary>
    public UIRawImageElement[] UIRawImageElements;
    [Header("Image图片元素集合")]
    /// <summary>
    /// Image图片集合
    /// </summary>
    public UIImageElement[] UITextureElements;
    [Header("文本元素集合")]
    /// <summary>
    /// 文本元素集合
    /// </summary>
    public UITextElement[] UITextElements;
    [Header("独立按钮元素集合")]
    /// <summary>
    /// 按钮元素集合
    /// </summary>
    public UIButtonElement[] UIButtonElements;
    [Header("组合按钮元素集合")]
    /// <summary>
    /// 组合按钮元素集合
    /// </summary>
    public UIGroupButtonElements[] UIGroupButtonElements;
    [Header("对象元素集合")]
    /// <summary>
    /// 对象元素集合
    /// </summary>
    public UIObjectElement[] UIObjectElements;
}

/// <summary>
/// UI组合元素
/// </summary>
[System.Serializable]
public class ScreenUIGroupElement : UIElementBasics
{
    /// <summary>
    /// 组合按钮方向类型
    /// </summary>
    [EnumLabel("元素组合类型(横向|纵向|替换)")]
    public UIGroupType ElementGroupType;
    [Header("组合计数,非覆盖时使用")]
    /// <summary>
    /// 组合计数,非覆盖时使用,表示可拖拽滑动
    /// </summary>
    public int GroupCount;
    [Header("间隔距离")]
    /// <summary>
    /// 间隔距离,元素对象之间的间隔
    /// </summary>
    public int Interval;
    [Header("组合节点集合")]
    /// <summary>
    /// 图片元素集合
    /// </summary>
    public ScreenUIElementNode[] ScreenUIElementNodes;
}