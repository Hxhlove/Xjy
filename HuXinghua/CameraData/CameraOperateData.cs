using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ------------------------------------------------------------------
// Title        :摄像机操作的数据
// Author       :Vincent
// Date         :2018.05.21
// Description  :
// ------------------------------------------------------------------
[System.Serializable]
public class CameraOperateData : ScriptableObject
{
    /// <summary>
    /// 数据名字
    /// </summary>
    public string dataName;

    /// <summary>
    /// 围绕旋转的目标点
    /// </summary>
    public GameObject aroundTarget;
    /// <summary>
    /// 移动的目标点
    /// </summary>
    public Transform moveDirection;
    /// <summary>
    /// 是否响应控制
    /// </summary>
    public bool isResponse;
    /// <summary>
    /// 是否改变距离,即拉远拉近操作
    /// </summary>
    public bool isChangeDistance;
    /// <summary>
    /// 是否是第一人称移动
    /// </summary>
    public bool isFirstView;
    /// <summary>
    /// 是否朝向目标点移动
    /// </summary>
    public bool isMoveToTarget;
    /// <summary>
    /// Y方向能移动到的最小角度
    /// </summary>
    public float yMinLimit;
    /// <summary>
    /// Y方向能旋转到的最大角度
    /// </summary>
    public float yMaxLimit;
    /// <summary>
    /// X方向能旋转的最小角度
    /// </summary>
    public float xMinLimit;
    /// <summary>
    /// X方向能旋转的最大角度
    /// </summary>
    public float xMaxLimit;
    /// <summary>
    /// 与目标点的最小距离
    /// </summary>
    public float minDistance;
    /// <summary>
    /// 与目标点的最大距离
    /// </summary>
    public float maxDistance;
    /// <summary>
    /// 旋转的速度
    /// </summary>
    public float rototeSpeed;
}
