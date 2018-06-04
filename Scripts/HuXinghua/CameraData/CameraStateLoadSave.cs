using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ------------------------------------------------------------------
// Title        :摄像机操作数据加载保存
// Author       :Vincent
// Date         :2018.05.21
// Description  :程序一开始将数据加载保存到字典_cameraStateDictionary
// ------------------------------------------------------------------
public class CameraStateLoadSave
{
    private Dictionary<string, CameraOperateData> _cameraStateDictionary = new Dictionary<string, CameraOperateData>();
    private static CameraStateLoadSave _cameraStateLoadSave;

    /// <summary>
    /// 保存的旋转数据
    /// </summary>
    public Dictionary<string, CameraOperateData> CameraStateDictionary
    {
        get
        {
            return _cameraStateDictionary;
        }
    }
    /// <summary>
    /// 获取该类的实例
    /// </summary>
    /// <returns></returns>
    public static CameraStateLoadSave GetInstance()
    {
        if (_cameraStateLoadSave == null)
        {
            _cameraStateLoadSave = new CameraStateLoadSave();
        }
        return _cameraStateLoadSave;
    }
    /// <summary>
    /// 加载的数据保存到字典当中
    /// </summary>
    /// <param name="dataname"></param>
    /// <param name="cameraStateData"></param>
    public void CameraStateDataSave(string dataname, CameraOperateData cameraStateData)
    {
        _cameraStateDictionary.Add(dataname, cameraStateData);
    }
    /// <summary>
    /// 根据数据的名字取出该条数据
    /// </summary>
    /// <param name="dataname">数据的名字</param>
    /// <returns></returns>
    public CameraOperateData TraverseCameraStateLoadData(string dataname)
    {
        CameraOperateData cameraStateData;
        _cameraStateDictionary.TryGetValue(dataname, out cameraStateData);
        return cameraStateData;
    }
}
