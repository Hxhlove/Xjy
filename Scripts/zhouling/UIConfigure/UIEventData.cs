// ------------------------------------------------------------------
// Title        :UI事件数据结构
// Author       :Leo
// Date         :2018.05.24
// Description  :用于配置UI事件
// ------------------------------------------------------------------

using UnityEngine;

/// <summary>
/// UI配置事件数据结构
/// </summary>
[System.Serializable]
public class UIEventData
{
    [Header("事件描述")]
    /// <summary>
    /// 该UI描述
    /// </summary>
    public string Describe;
    /// <summary>
    /// 事件类型
    /// </summary>
    [EnumLabel("事件类型")]
    public UIEventDataType UIEventDataType;

    [Header("UI结构(根节点->子节点)")]
    /// <summary>
    /// UI结构,有子节点的一般为组合类型为覆盖类型的UI结构,开启和关闭都是根节点操作,选择具体的子节点进行显示,如果启动这个事件的对象时是在这个跟节点下,只操作子节点
    /// </summary>
    public string[] UIStructure;

    [Header("音频文件")]
    /// <summary>
    /// 打开继续/暂停/关闭声音事件时有的音频文件
    /// </summary>
    public AudioClip AudioClip;
    [Header("是否循环播放音频")]
    /// <summary>
    /// 是否循环播放音频
    /// </summary>
    public bool isLoopAudio = false;
    [Header("音频延迟开启/打开时间")]
    /// <summary>
    /// 音频延迟开启
    /// </summary>
    public float DelayTime;

    [Header("摄像机动画路径对象集合")]
    /// <summary>
    /// 摄像机动画路径对象集合
    /// </summary>
    public Transform[] CameraPaths;
    
    /// <summary>
    /// 摄像机控制预制枚举
    /// </summary>
    [EnumLabel("摄像机控制预制")]
    public PreinstallControlType PreinstallControlType;

    [Header("摄像机控制数据结构")]
    /// <summary>
    /// 摄像机控制数据结构
    /// </summary>
    public UIEventData DataObject;
    [Header("使用名称发送选房事件")]
    /// <summary>
    /// 选房
    /// </summary>
    public string RoomName;
    /// <summary>
    /// 场景事件类型
    /// </summary>
    [EnumLabel("场景事件类型")]
    public SceneEventType SceneEventType;
}