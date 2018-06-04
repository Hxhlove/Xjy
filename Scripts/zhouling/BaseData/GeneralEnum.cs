// ------------------------------------------------------------------
// Title        :通用枚举数据
// Author       :Leo
// Date         :2018.05.04
// Description  :通用枚举数据
// ------------------------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#endif

/// <summary>
/// 自动漫游类型
/// </summary>
public enum AutomaticRoamType
{
    [EnumLabel("未定义")]
    Undefined,          // 未定义
    [EnumLabel("自动区位和沙盘漫游")]
    Automatic,          // 自动区位和沙盘漫游
    [EnumLabel("区位漫游")]
    Location,           // 区位漫游
    [EnumLabel("沙盘漫游")]
    SandTable,          // 沙盘漫游
}


/// <summary>
/// 专题状态类型
/// </summary>
public enum ThematicStateType
{
    [EnumLabel("区位")]
    Location,           // 区位
    [EnumLabel("沙盘")]
    SandTable,          // 沙盘
    [EnumLabel("户型")]
    Huxing,             // 户型
    [EnumLabel("精装")]
    Hardcover,          // 精装
    [EnumLabel("品牌")]
    Brand,              // 品牌
    [EnumLabel("工具")]
    Tools,              // 工具
}
/// <summary>
/// 摄像机预制操作参数类型
/// </summary>
public enum PreinstallControlType
{
    [EnumLabel("无操作")]
    NoOperation,           // 无操作
    [EnumLabel("沙盘操作")]
    SandTable,             // 沙盘操作
    [EnumLabel("阳台景观操作")]
    Scenery,               // 阳台景观操作
    [EnumLabel("第一人称操作")]
    FirstPerson,           // 第一人称操作
}

/// <summary>
/// 摄像机预制动画类型
/// </summary>
public enum PreinstallAnimationType
{
    [EnumLabel("到区域")]
    ToRegion,               // 到区域
    [EnumLabel("到沙盘")]
    ToSandTable,            // 到沙盘
    [EnumLabel("到手动漫游")]
    ToManualRoam,           // 到手动漫游
}


/// <summary>
/// UI描点类型
/// </summary>
public enum UIDrawingPointType
{
    [EnumLabel("左上")]
    Left_Up,               // 左上
    [EnumLabel("左中")]
    Left_Middle,           // 左中
    [EnumLabel("左下")]
    Left_Down,             // 左下
    [EnumLabel("中上")]
    Middle_Up,             // 中上
    [EnumLabel("中中")]
    Middle_Middle,         // 中中
    [EnumLabel("中下")]
    Middle_Down,           // 中下
    [EnumLabel("右上")]
    Right_Up,              // 右上
    [EnumLabel("右中")]
    Right_Middle,          // 右中
    [EnumLabel("右下")]
    Right_Down,            // 右下
}

/// <summary>
/// 图片显示类型
/// </summary>
public enum InageType
{
    [EnumLabel("图片像素大小显示图片")]
    Simple,                 // 简单方式(图片像素大小显示图片)
    [EnumLabel("切片(九宫格)")]
    Sliced,                 // 切片(九宫格)
    [EnumLabel("平铺(使用图片像素大小进行重复铺设)")]
    Tiled,                  // 平铺(使用图片像素大小进行重复铺设)
    [EnumLabel("拉伸(拉伸铺满所设置的大小)")]
    Filled,                 // 拉伸(拉伸铺满所设置的大小),可以设置裁剪
}

/// <summary>
/// UI打开关闭效果类型
/// </summary>
public enum UISwitchEffectType
{
    [EnumLabel("直接隐藏")]
    Hide,                   // 直接隐藏
    [EnumLabel("左向右飞进飞出")]
    Left_Right,             // 左向右飞进飞出
    [EnumLabel("右向左飞进飞出")]
    Right_Left,             // 右向左飞进飞出
    [EnumLabel("上下飞出")]
    Up_Down,                // 上下飞出
    [EnumLabel("下上飞出")]
    Down_Up,                // 下上飞出
    [EnumLabel("大到小")]
    Max_Min,                // 大到小
    [EnumLabel("小到大")]
    Min_Max,                // 小到大
}

/// <summary>
/// UI字体样式类型
/// </summary>
public enum FontType
{
    [EnumLabel("没有")]
    Normal,                 // 没有
    [EnumLabel("加粗")]
    Bold,                   // 加粗
    [EnumLabel("斜体")]
    Italic,                 // 斜体
    [EnumLabel("加粗+斜体")]
    BoldAndItalic,          // 加粗+斜体
}


/// <summary>
/// UI元素组合方式
/// </summary>
public enum UIGroupType
{
    [EnumLabel("横向")]
    Horizontal,                 // 横向
    [EnumLabel("纵向")]
    Vertical,                   // 纵向
    [EnumLabel("替换")]
    Replace,                    // 替换
}



/// <summary>
/// UI按钮操作类型
/// </summary>
public enum UIButtonOperationType
{
    [EnumLabel("只点击")]
    OnlyClick,                    // 只点击
    [EnumLabel("可选择")]
    Selectable,                   // 可选择
}

/// <summary>
/// UI组合按钮操作类型
/// </summary>
public enum UIButtonGroupOperationType
{
    [EnumLabel("是激活一个")]
    One,                 // 一个激活
    [EnumLabel("多个可重复激活")]
    Multiple,            // 多个可重复激活
}

/// <summary>
/// UI事件数据结构类型
/// </summary>
public enum UIEventDataType
{
    [EnumLabel("打开UI")]
    OpenUI,                    // 打开UI
    [EnumLabel("关闭UI")]
    CloseUI,                   // 关闭UI
    [EnumLabel("打开(继续播放)音频")]
    OpenAudio,                 // 打开(继续播放)音频
    [EnumLabel("暂停音频")]
    SuspendAudio,              // 暂停音频
    [EnumLabel("关闭音频")]
    CloseAudio,                // 关闭音频
    [EnumLabel("摄像机动画(Transform序列集合)")]
    CameraAnimation,           // 摄像机动画(Transform序列集合)
    [EnumLabel("摄像机控制数据")]
    CameraControl,             // 摄像机控制数据
    [EnumLabel("摄像机预设控制枚举")]
    CameraPreinstallControl,   // 摄像机预设控制枚举
    [EnumLabel("选房事件")]
    RoomSelection,             // 选房事件
    [EnumLabel("场景事件(内部配套|主干道|地铁|公交)")]
    SceneEvent,                // 场景事件(内部配套/主干道/地铁/公交)
}

/// <summary>
/// UI事件场景事件类型
/// </summary>
public enum SceneEventType
{
    [EnumLabel("主干道")]
    MainRoad,                  // 主干道
    [EnumLabel("地铁")]
    Metro,                     // 地铁
    [EnumLabel("公交")]
    Transit,                   // 公交
    [EnumLabel("内部配套")]
    InternalMatching,          // 内部配套
}