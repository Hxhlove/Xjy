//================================================
//描 述 ：所有的声音管理
//作 者 ：Vincent
//创建时间 ：2018/06/01 09:27:30  
//版 本： 
// ================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ToAudioPool(AudioSourceManage audioSourceManage);//声音播放完成的回调
public class AudioManagement
{
    private Dictionary<string, AudioClip> _musicDictionary;//声音剪辑字典
    private bool _isPlayAudio = true;//全局控制是否响应声音事件
    private GameObject _musicManagement;//挂载AudioSource的对象
    private List<AudioSourceManage> _audioSourceList;//管理单个audiosource的集合
    #region//单例
    private static AudioManagement _audioManagement;
    private AudioManagement() { }
    public static AudioManagement Inst
    {
        get
        {
            if (_audioManagement == null)
            {
                _audioManagement = new AudioManagement();
            }
            return _audioManagement;
        }
    }
    #endregion
    /// <summary>
    /// 初始化
    /// </summary>
    public void Open()
    {
        _musicManagement = new GameObject("MusicManagement");//声音管理对象
        _audioSourceList = new List<AudioSourceManage>();//声音管理的list
        _musicDictionary = new Dictionary<string, AudioClip>();
        _musicDictionary.Add(Resources.Load<AudioClip>("Music/BGM").name, Resources.Load<AudioClip>("Music/BGM"));//加载声音放进字典
        _musicDictionary.Add(Resources.Load<AudioClip>("Music/bgMusic").name, Resources.Load<AudioClip>("Music/bgMusic"));
        _musicDictionary.Add(Resources.Load<AudioClip>("Music/uiClickMusic").name, Resources.Load<AudioClip>("Music/uiClickMusic"));
        EventMgr.Inst.Regist(AudioEvent.Play, Play);//播放声音事件
        EventMgr.Inst.Regist(AudioEvent.Stop, Stop);//播放停止事件
        EventMgr.Inst.Regist(AudioEvent.Pause, Pause);//暂停播放事件
        EventMgr.Inst.Regist(AudioEvent.UnPause, UnPause);//继续播放事件
        EventMgr.Inst.Regist(AudioEvent.StopAll, StopAll);//停止所有声音
        EventMgr.Inst.Regist(AudioEvent.OpenAudio, OpenAudio);//打开声音响应
    }
    #region//声音事件
    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="arg">1.声音的名字2.是否循环</param>
    private void Play(EventArg arg)
    {
        if (_isPlayAudio)
        {
            string audioClip = (string)arg[0];
            bool isLoop = (bool)arg[1];
            AudioSourceManage audioSourceManage = AudioPool.Inst.GetAudioFormPool(audioClip);
            if (audioSourceManage == null)
            {
                audioSourceManage = GreatAudioSource(audioClip, isLoop);
            }
            audioSourceManage.Play();
            _audioSourceList.Add(audioSourceManage);
        }
    }
    /// <summary>
    /// 停止播放
    /// </summary>
    /// <param name="arg">暂停声音的名字</param>
    private void Stop(EventArg arg)
    {
        if (_isPlayAudio)
        {
            string audioName = (string)arg[0];

            for (int i = 0; i < _audioSourceList.Count; i++)
            {
                if (_audioSourceList[i].AudioSource.clip.name == audioName)
                {
                    _audioSourceList[i].Stop();
                }
            }
        }
    }
    ///// <summary>
    ///// 暂停播放
    ///// </summary>
    private void Pause(EventArg arg)
    {
        if (_isPlayAudio)
        {
            string audioName = (string)arg[0];

            for (int i = 0; i < _audioSourceList.Count; i++)
            {
                if (_audioSourceList[i].AudioSource.clip.name == audioName)
                {
                    _audioSourceList[i].Pause();
                }
            }
        }
    }
    ///// <summary>
    ///// 关闭所有声音响应
    ///// </summary>
    ///// <param name="arg"></param>
    private void StopAll()
    {
        _isPlayAudio = false;
        for (int i = 0; i < _audioSourceList.Count; i++)
        {
            _audioSourceList[i].Pause();
        }
    }
    /// <summary>
    /// 打开所有声音响应
    /// </summary>
    /// <param name="arg"></param>
    private void OpenAudio()
    {
        _isPlayAudio = true;//打开响应
        for (int i = 0; i < _audioSourceList.Count; i++)
        {
            _audioSourceList[i].UnPause();
        }
    }
    /// <summary>
    /// 继续播放
    /// </summary>
    private void UnPause(EventArg arg)
    {
        string audioName = (string)arg[0];

        for (int i = 0; i < _audioSourceList.Count; i++)
        {
            ;
            if (_audioSourceList[i].AudioSource.clip.name == audioName)
            {
                _audioSourceList[i].UnPause();
            }
        }
    }
    #endregion
    /// <summary>
    /// 创建一个声音对象
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="isLoop"></param>
    /// <returns></returns>
    private AudioSourceManage GreatAudioSource(string audioClip, bool isLoop)
    {
        AudioSource audioSource = _musicManagement.AddComponent<AudioSource>();//添加一个AudioCource组件
        AudioClip Clip;
        _musicDictionary.TryGetValue(audioClip, out Clip);//从字典中获取Clip
        AudioSourceManage audioSourceManage = new AudioSourceManage(audioSource, ToAudioPool);
        audioSourceManage.AudioSource.clip = Clip;
        audioSourceManage.AudioSource.loop = isLoop;
        return audioSourceManage;
    }
    /// <summary>
    /// 放入对象池
    /// </summary>
    /// <param name="audioSourceManage"></param>
    private void ToAudioPool(AudioSourceManage audioSourceManage)
    {
        for (int i = 0; i < _audioSourceList.Count; i++)
        {
            if (audioSourceManage == _audioSourceList[i])
            {
                AudioPool.Inst.RecycleObj(_audioSourceList[i]);//加入对象池
                _audioSourceList.Remove(_audioSourceList[i]);//从list中移除
            }
        }
    }
    /// <summary>
    /// 声音更新
    /// </summary>
    public void AudioUpdate()
    {
        if (_isPlayAudio)
        {
            //有声音在播放的list里面
            if (_audioSourceList.Count > 0)
            {
                for (int i = 0; i < _audioSourceList.Count; i++)
                {
                    _audioSourceList[i].AudioManageUpdate();
                }
            }
        }
    }
}
/// <summary>
/// 声音的类型
/// </summary>
public enum AudioEvent
{
    Play,//播放
    Stop,//停止某个声音
    Pause,//暂停某个声音
    UnPause,//继续播放某个声音
    StopAll,//关闭所有声音
    OpenAudio,//打开声音响应
}
