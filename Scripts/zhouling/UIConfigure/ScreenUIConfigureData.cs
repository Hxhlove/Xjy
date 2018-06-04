// ------------------------------------------------------------------
// Title        :屏幕UI配置数据
// Author       :Leo
// Date         :2018.05.22
// Description  :用于配置屏幕内UI管理，配置屏幕UI节点
// ------------------------------------------------------------------
using UnityEngine;


[System.Serializable]
public class ScreenUIConfigureData : ScriptableObject
{
    [Header("节点总配置数据")]
    /// <summary>
    /// UI节点数据
    /// </summary>
    public UINodeData UINodeData;

    [Header("单独元素节点集合")]
    /// <summary>
    /// 基础元素节点
    /// </summary>
    public ScreenUIElementNode[] ScreenUIElementNodes;

    [Header("组合元素节点集合")]
    /// <summary>
    /// 组合元素节点
    /// </summary>
    public ScreenUIGroupElement[] ScreenUIGroupElements;


    [Header("打开事件")]
    /// <summary>
    /// UI打开事件
    /// </summary>
    public UIEventData[] UIOpenEvent;
    [Header("关闭事件")]
    /// <summary>
    /// UI关闭事件
    /// </summary>
    public UIEventData[] UICloseEvent;
}

/// <summary>
/// UI节点总数据
/// </summary>
[System.Serializable]
public class UINodeData : UIElementBasics
{
    [Header("参考其他UI节点对象:设置描点,空为参考屏幕")]
    /// <summary>
    /// 参考其他UI节点对象名称,没有参考对象或找不到参考对象时使用屏幕参考对象设置描点
    /// </summary>
    public string ReferenceUI;

    /// <summary>
    /// 打开UI效果类型
    /// </summary>
    [EnumLabel("打开UI效果类型")]
    public UISwitchEffectType OpenEffectType = UISwitchEffectType.Hide;
    /// <summary>
    /// 关闭UI效果类型
    /// </summary>
    [EnumLabel("关闭UI效果类型")]
    public UISwitchEffectType CloseEffectType = UISwitchEffectType.Hide;
}