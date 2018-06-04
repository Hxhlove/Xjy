// ------------------------------------------------------------------
// Title        :游戏驱动器
// Author       :Leo
// Date         :2018.05.04
// Description  :绑定在场景内的驱动游戏对象上，用以驱动游戏主逻辑的运行
// ------------------------------------------------------------------

using UnityEngine;

public class GameDrive : MonoBehaviour
{
    /// <summary>
    /// 启动加载
    /// </summary>
    void Awake()
    {
        ControlCenter.Inst.Awake();
    }
    /// <summary>
    /// 驱动开始执行
    /// </summary>
    void Start()
    {
        ControlCenter.Inst.Start();
    }
    /// <summary>
    /// 常规更新
    /// </summary>
    void Update()
    {
        ControlCenter.Inst.Update();
    }
    /// <summary>
    /// 固定更新
    /// </summary>
    void FixedUpdate()
    {
        ControlCenter.Inst.FixedUpdate();
    }
    /// <summary>
    /// 最后更新
    /// </summary>
    void LateUpdate()
    {
        ControlCenter.Inst.LateUpdate();
    }
    /// <summary>
    /// 应用失去焦点
    /// </summary>
    void OnApplicationFocus(bool hasFocus)
    {
        ControlCenter.Inst.OnApplicationFocus(hasFocus);
    }
    /// <summary>
    /// 驱动销毁
    /// </summary>
    void OnDestroy()
    {
        ControlCenter.Inst.OnDestroy();
    }
}