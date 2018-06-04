// ------------------------------------------------------------------
// Title        :片头控制器
// Author       :Leo
// Date         :2018.05.10
// Description  :控制管理片头内容,片头完成后开启进度条
// ------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 片头状态回调
/// </summary>
public delegate void FilmHeadEventStateCallback();

public class FilmHeadManage
{
    /// <summary>
    /// 是否开启片头
    /// </summary>
    public static bool isOpen = true;
    static FilmHeadManage sm_inst;
    public static FilmHeadManage Inst
    {
        get
        {
            if (sm_inst == null)
                sm_inst = new FilmHeadManage();
            return sm_inst;
        }
    }

    //播放视频对象
    GameObject AudioSourcePlay;
    /// <summary>
    /// 视频集合
    /// </summary>
    List<VideoClip> _VideoClips;


    //声音监听
    List<KeyValuePair<bool, AudioListener>> AudioListeners;
    //存在的声音监听数量
    int length = 0;
    //开启片头事件回调参数
    EventArg ea;
    //视频播放
    FilmHeadPlay FilmHeadPlay;
    
    /// <summary>
    /// 初始
    /// </summary>
    public void Open()
    {
        if (isOpen)
        {
            Debug.Log("初始化片头");
            EventMgr.Inst.Regist(FilmHeadEvent.Open, Start);
            AudioListeners = new List<KeyValuePair<bool, AudioListener>>();
            AudioListener[] audioListener = Object.FindObjectsOfType<AudioListener>();
            length = audioListener.Length;
            for (int i = 0; i < length; i++)
            {
                AudioListeners.Add(new KeyValuePair<bool, AudioListener>(audioListener[i].enabled, audioListener[i]));
                audioListener[i].enabled = false;
            }
            _VideoClips = ResourcesEx.LoadAll<VideoClip>(PathFileTool.FilmHead);
        }
    }

    /// <summary>
    /// 开始片头
    /// </summary>
    void Start(EventArg ea)
    {
        Debug.Log("开始片头");
        this.ea = ea;
        if (AudioSourcePlay == null)
        {
            AudioSourcePlay = new GameObject("AudioSourcePlay");
            FilmHeadPlay = AudioSourcePlay.AddComponent<FilmHeadPlay>();
            FilmHeadPlay.EndCallback += End;
        }
        FilmHeadPlay.Prepare(_VideoClips);
    }
    /// <summary>
    /// 片头结束
    /// </summary>
    void End()
    {
        Debug.Log("结束片头");
        Complete();
        TimerMgr.Add((args) =>
        {
            EventArg ea = (EventArg)args[0];
            if (ea != null)
            {
                ea.Callback();
            }
        }, ea);
    }

    /// <summary>
    /// 播放完成
    /// </summary>
    void Complete()
    {
        //恢复其他监听
        for (int i = 0; i < length; i++)
        {
            var audioListener = AudioListeners[i];
            audioListener.Value.enabled = audioListener.Key;
        }
        GameObject.DestroyImmediate(AudioSourcePlay);
        AudioSourcePlay = null;
    }
}

/// <summary>
/// 片头视频播放
/// </summary>
public class FilmHeadPlay : MonoBehaviour
{
    /// <summary>
    /// 视频集合
    /// </summary>
    List<VideoClip> _VideoClips;
    /// <summary>
    /// 视频集合
    /// </summary>
    List<VideoClip> VideoClips
    {
        set
        {
            _VideoClips = value;
            count = _VideoClips.Count;
        }
        get { return _VideoClips; }
    }
    /// <summary>
    /// 视频数量
    /// </summary>
    int count = 0;
    /// <summary>
    /// 临时视频数量
    /// </summary>
    int temporaryCount = 0;
    /// <summary>
    /// 结束回调
    /// </summary>
    public FilmHeadEventStateCallback EndCallback;

    /// <summary>
    /// 视频播放组件
    /// </summary>
    VideoPlayer videoPlayer;
    /// <summary>
    /// 音频组件
    /// </summary>
    AudioSource AudioSource;

    /// <summary>
    /// 临时视频播放组件
    /// </summary>
    VideoPlayer temporaryVideoPlayer;
    /// <summary>
    /// 临时音频组件
    /// </summary>
    AudioSource temporaryAudioSource;


    public void Awake()
    {
        //直接在摄像机对象上添加播放视频组件，或者手动设置对应的摄像机(如果是把播放对象设置在模型上指定对应的模型)
        gameObject.AddComponent<AudioListener>();
        var camera = gameObject.AddComponent<Camera>();
        camera.cullingMask = 0;
        camera.clearFlags = CameraClearFlags.Color;
        camera.backgroundColor = Color.black;
        camera.depth = 10;
    }

    void CreateCideoPlayer(VideoClip videoClip)
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        videoPlayer.playOnAwake = false;
        //设置播放视频内容
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        AudioSource = gameObject.AddComponent<AudioSource>();
        videoPlayer.SetTargetAudioSource(0, AudioSource);
        //使用平面画面
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;

        //透明度
        videoPlayer.targetCameraAlpha = 1F;

        //设置不循环
        videoPlayer.isLooping = false;

        //拉伸画面方式
        videoPlayer.aspectRatio = VideoAspectRatio.NoScaling;

        //准备完成
        videoPlayer.prepareCompleted += Prepared;
        //播放开始
        videoPlayer.started += Started;
        //出现错误
        videoPlayer.errorReceived += ErrorReceived;
        //播放完成或开始循环
        videoPlayer.loopPointReached += LoopPointReached;


        videoPlayer.clip = videoClip;
    }

    public void Prepare(List<VideoClip> VideoClips)
    {
        count = 0;
        temporaryCount = 0;
        this.VideoClips = VideoClips;
        PlayStart();
    }

    /// <summary>
    /// 播放开始
    /// </summary>
    private void PlayStart()
    {
        try
        {
            if (temporaryCount < count)
            {
                bool jinr = true;
                while (jinr)
                {
                    VideoClip VideoClip = VideoClips[temporaryCount];
                    temporaryCount++;
                    if (VideoClip != null)
                    {
                        CreateCideoPlayer(VideoClip);
                        PreparePlayStart();
                        break;
                    }
                    else
                    {
                        if (temporaryCount >= count)
                        {
                            jinr = false;
                            EndFilmHeadPlay();
                        }
                    }
                }
            }
            else
            {
                EndFilmHeadPlay();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("播放错误:\n错误信息:{0}\n堆栈信息{1}", e.Message, e.StackTrace));
            EndFilmHeadPlay();
        }
    }

    /// <summary>
    /// 准备开始播放
    /// </summary>
    public void PreparePlayStart()
    {
        Debug.Log("视频播放----开始准备");
        //准备开始播放
        videoPlayer.Prepare();
    }

    //准备完成后播放
    void Prepared(VideoPlayer VideoPlayer)
    {
        Debug.Log("视频播放----准备完成");
        videoPlayer.Play();
    }
    //开始播放视频(暂停后从新开始也会调用(切换程序出去后在回来也会调用))
    void Started(VideoPlayer VideoPlayer)
    {
        Debug.Log("视频播放----开始");
        if (temporaryVideoPlayer)
        {
            temporaryVideoPlayer.Pause();
            temporaryVideoPlayer.Stop();
            temporaryVideoPlayer.clip = null;
            Destroy(temporaryVideoPlayer);
        }
        if (temporaryAudioSource)
        {
            temporaryAudioSource.Pause();
            temporaryAudioSource.Stop();
            temporaryAudioSource.clip = null;
            Destroy(temporaryAudioSource);
        }
    }

    //视频播放错误,停止播放
    void ErrorReceived(VideoPlayer VideoPlayer, string message)
    {
        Debug.Log(string.Format("视频播放错误:{0}", message));
        temporaryVideoPlayer = videoPlayer;
        temporaryAudioSource = AudioSource;
        PlayStart();
    }

    //播放结束或播放到循环的点时
    void LoopPointReached(VideoPlayer VideoPlayer)
    {
        Debug.Log("视频播放----完成");
        temporaryVideoPlayer = videoPlayer;
        temporaryAudioSource = AudioSource;
        PlayStart();
    }

    /// <summary>
    /// 结束播放
    /// </summary>
    void EndFilmHeadPlay()
    {
        Debug.Log("视频播放----结束");
        if (EndCallback != null)
        {
            EndCallback();
        }
    }

    /// <summary>
    /// 销毁
    /// </summary>
    void OnDestroy()
    {
        Debug.Log("删除播放视频");
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= Prepared;
            videoPlayer.started -= Started;
            videoPlayer.errorReceived -= ErrorReceived;
            videoPlayer.loopPointReached -= LoopPointReached;
            EndCallback = null;
        }
    }
}