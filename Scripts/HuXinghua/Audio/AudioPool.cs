//================================================
//描 述 ： 声音对象池
//作 者 ：Vincent
//创建时间 ：2018/06/01 10:13:30  
//版 本： 
// ================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool
{
    #region 单例
    private static AudioPool _audioPool;
    private AudioPool()
    {
        audioDictionarg = new Dictionary<string, List<AudioSourceManage>>();
    }
    public static AudioPool Inst
    {
        get
        {
            if (_audioPool == null)
            {
                _audioPool = new AudioPool();
            }
            return _audioPool;
        }
    }
    #endregion
    private Dictionary<string, List<AudioSourceManage>> audioDictionarg;//声音剪辑的名字 和管理该AudioSource的类

    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public AudioSourceManage GetAudioFormPool(string audioName)
    {
        //结果对象
        AudioSourceManage result = null;
        //判断是否有该名字的对象池
        if (audioDictionarg.ContainsKey(audioName))
        {
            //对象池里有对象
            if (audioDictionarg[audioName].Count > 0)
            {
                //获取结果
                result = audioDictionarg[audioName][0];
                //从池中移除该对象
                audioDictionarg[audioName].Remove(result);
                //返回结果
                return result;
            }
        }
        return result;
    }
    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    /// <param name="objName"></param>
    public void RecycleObj(AudioSourceManage audioObject)
    {
        //设置为非激活
        //audioObject.SetActive(false);
        //判断是否有该对象的对象池
        if (audioDictionarg.ContainsKey(audioObject.AudioSource.clip.name))
        {
            //放置到该对象池
            audioDictionarg[audioObject.AudioSource.clip.name].Add(audioObject);
        }
        else
        {
            //创建该类型的池子，并将对象放入
            audioDictionarg.Add(audioObject.AudioSource.clip.name, new List<AudioSourceManage>() { audioObject });
        }

    }
}
