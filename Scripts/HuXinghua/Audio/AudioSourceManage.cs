//================================================
//描 述 ： 单个声音管理
//作 者 ：Vincent
//创建时间 ：2018/06/02 09:06:25  
//版 本： 
// ================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManage
{
    private AudioSource _audioSource;//声音组件
    private ToAudioPool _toAudioPool;//播放完成的回调
    private bool IsToPool = false;//是否放进对象池 当且仅当没有播放 并且IsToPool为true 将对象放到对象池
    public AudioSourceManage(AudioSource audioScource, ToAudioPool toAudioPool)
    {
        this.AudioSource = audioScource;
        this._toAudioPool = toAudioPool;
    }
    /// <summary>
    /// 管理的audioSource组件
    /// </summary>
    public AudioSource AudioSource
    {
        get
        {
            return _audioSource;
        }
        set
        {
            _audioSource = value;
        }
    }
    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        AudioSource.Play();
        IsToPool = true;
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        AudioSource.Stop();
        IsToPool = true;
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        AudioSource.Pause();
        IsToPool = false;
    }
    /// <summary>
    /// 取消暂停
    /// </summary>
    public void UnPause()
    {
        AudioSource.UnPause();
        IsToPool = true;
    }

    /// <summary>
    /// 声音更新
    /// </summary>
    public void AudioManageUpdate()
    {
        if (IsToPool && !AudioSource.isPlaying)
        {
            _toAudioPool(this);
            IsToPool = false;//放入对象池之后不再执行
        }
    }
}
