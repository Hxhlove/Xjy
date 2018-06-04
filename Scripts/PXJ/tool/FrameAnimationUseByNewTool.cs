using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class FrameAnimationUseByNewTool
{
    /// <summary>
    /// 图片资源集合
    /// </summary>
    List<Sprite> spriteRes;
    string AudioResPath;
    /// <summary>
    ///  图片显示预制体
    /// </summary>
    GameObject PlayScreen;

    AudioClip MyClip;
    /// <summary>
    /// 音频组件
    /// </summary>
    AudioSource MyAudioSource;

    float playDuratin { get { if (MyClip == null) return 1f; else return MyClip.length; } }
    /// <summary>
    /// 获取图片显示的预制体的Image组件
    /// </summary>
    Image myImg { get { if (PlayScreen.GetComponent<Image>() == null) { PlayScreen.AddComponent<Image>(); } return PlayScreen.GetComponent<Image>(); } }

    //UiAnimationTool MyBodyTool { get { return PlayScreen.GetComponent<UiAnimationTool>(); } }

    /// <summary>
    /// 语音的
    /// </summary>
    int index = 0;
    float onceTime; //一张图片需要的时间
    int totalCount;//资源图片的总数
    float timer;

    [Header("开始播放")]
    public bool Play;


    [Header("重新播放")]
    public bool Replay;

    [Header("暂停播放")]
    public bool Pause;

    [Header("继续播放")]
    public bool Continue;

    bool _play;
    bool isPlaying;
    Action doneCallback;//回调
    bool isLoop;
    Action loopCallback;//循环播放的回调

    bool _frameAniFirstCenter;
    /// <summary>
    /// 第一次播放
    /// </summary>
    public bool FrameAniFirstCenter
    {
        get { return _frameAniFirstCenter; }
        set { _frameAniFirstCenter = value; }
    }

    //bool _myPanelState;
    ///// <summary>
    ///// 帧动画的面板的进入与离开的状态控制
    ///// </summary>
    //public bool MyPanelState
    //{
    //    get { return _myPanelState; }
    //    set
    //    {
    //        _myPanelState = value;
    //    }
    //}
    /// <summary>
    /// 构造函数     参数：图片资源路径，音频资源路径 音频播放组件
    /// </summary>
    /// <param SpriteResPath= "动画图片资源的Resource路径"></param>
    /// <param AudioResPath="动画音频资源的Resource路径"> </param>
    public FrameAnimationUseByNewTool(string _SpriteResPath, string _AudioResPath, GameObject _PlayScreen, AudioSource _MyAudioSource)
    {
        Sprite[] spriteResArr = Resources.LoadAll<Sprite>(_SpriteResPath);
        AudioResPath = _AudioResPath;
        PlayScreen = _PlayScreen;
        MyAudioSource = _MyAudioSource;


        List<Sprite> spriteResList = new List<Sprite>();//将所有图片的数组转换成集合new List<Sprite>(spriteResArr)
        spriteResList.AddRange(spriteResArr); //添加多个元素 将所有图片的数组转换成集合
        int spritesTotalCount = spriteResList.Count;
        int currentListCount = spritesTotalCount;

        if (spriteResList.Count > 0)
        {
            spriteRes = new List<Sprite>(spriteResList);
            for (int i = 0; i < spriteResList.Count; i++)
            {
                //获取图片名字
                string spriteName = spriteResList[i].name;
                //如果第一张图片是0，继续下一个操作
                if (spriteName == "0") { continue; }
                //获取名字的有效部分，即去掉名字前面的干扰
                string spriteNameTemp = DeleteFirstChar(spriteName, '0');
                //  Debug.Log(spriteNameTemp);
                //将名字的有效部分转化成Int类型
                int spriteIndex = int.Parse(spriteNameTemp);
                //
                Sprite spriteTemp = spriteResList[spriteIndex];
                spriteRes[spriteIndex] = spriteResList[i];

            }
          }
        else Debug.Log("失败？");

        totalCount = spriteRes.Count;//资源图片的总数
        onceTime =1f / totalCount;  //每1张图片 需要的时间 6s总时间/150张图片
        PlayScreen.SetActive(false);

    }
    /// <summary>
    /// 构造函数  参数：图片资源路径
    /// </summary>
    /// <param SpriteResPath="动画图片资源的Resource路径"></param>
    public FrameAnimationUseByNewTool(string _SpriteResPath, GameObject _PlayScreen)
    {
        PlayScreen = _PlayScreen;
        Sprite[] spriteResArr = Resources.LoadAll<Sprite>(_SpriteResPath);
        List<Sprite> spriteResList = new List<Sprite>();//将所有图片的数组转换成集合new List<Sprite>(spriteResArr)
        spriteResList.AddRange(spriteResArr); //添加多个元素 将所有图片的数组转换成集合
        int spritesTotalCount = spriteResList.Count;
        int currentListCount = spritesTotalCount;

        if (spriteResList.Count > 0)
        {
            spriteRes = new List<Sprite>(spriteResList);
            for (int i = 0; i < spriteResList.Count; i++)
            {
                //获取图片名字
                string spriteName = spriteResList[i].name;

                //如果第一张图片是0，继续下一个操作
                if (spriteName == "0") { continue; }
                //获取名字的有效部分，即去掉名字前面的干扰
                string spriteNameTemp = DeleteFirstChar(spriteName, '0');

                //  Debug.Log(spriteNameTemp);
                //将名字的有效部分转化成Int类型
                int spriteIndex = int.Parse(spriteNameTemp);
                //
                Sprite spriteTemp = spriteResList[spriteIndex];
                spriteRes[spriteIndex] = spriteResList[i];

            }
       
        }
        else Debug.Log("失败？");

        totalCount = spriteRes.Count;//资源图片的总数
        onceTime =2f / totalCount;  //每1张图片 需要的时间 6s总时间/150张图片
        PlayScreen.SetActive(false);
    }



    ///// <summary>
    ///// 帧动画的Transform
    ///// </summary>
    //public Transform MyFrameAniObjTrs
    //{
    //    get { return transform.parent; }
    //    set { transform.parent = value; }
    //}

    /// <summary>
    /// 去除字符串首出现的某个字符
    /// </summary>
    /// <param name="sourceStr"></param>
    /// <param name="deleteChar"></param>
    /// <returns></returns>
    String DeleteFirstChar(String sourceStr, char deleteChar)
    {
        bool beginIndexFlag = true;
        do
        {
            int beginIndex = sourceStr.IndexOf('0') == 0 ? 1 : 0;
            sourceStr = sourceStr.Substring(beginIndex, sourceStr.Length - beginIndex);
            beginIndexFlag = (sourceStr.IndexOf('0') == 0);
        }
        while (beginIndexFlag);
        return sourceStr;
    }

    void ChangeSprite(int index)
    {
        if (index < totalCount)  //如果index 小于播放的总时间6秒
        {
            myImg.sprite = spriteRes[index];
            if (index == totalCount - 1)
            {
                _play = false;
                Play = false;
                isPlaying = false;
                Replay = false;
                if (doneCallback != null)
                { doneCallback(); }
                else if (isLoop)
                {
                    Debug.Log("动画要求循环播放");
                    FrameAniPlay(-1, true);
                }
                else if (!isLoop)
                { Debug.Log("帧动画完成 回调为空"); }
            }
        }
    }


    public void FixedUpdate()
    {
        #region 播放方法
        if (Play && !isPlaying)  //没播放过，开始播放
        {
            _play = true;
            if (index != 0)
            {
                index = 0;
            }
            timer = onceTime;
            isPlaying = true;
            //       MyAudioSource.Play();
        }
        if (_play)
        {
            PlayMyAnim();
        }

        if (Replay)//，重置播放
        {
            _play = true;
            Pause = false;
            Continue = false;
            Replay = false;
            index = 0;
            timer = onceTime;
            isPlaying = true;
        }

        if (Pause && isPlaying)  //暂停
        {
            _play = false;
            Continue = false;
        }

        if (!Pause && Continue && isPlaying)  //继续
        {
            if (_play)
                return;
            if (!_play)
            {
                _play = true;
            }
        }

        #endregion

    }

    /// <summary>
    ///播放 方法
    /// </summary>
    void PlayMyAnim()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //     print(Time.time);         
            //   ChangeSprite(index);
            ChangeSprite(index);
            index++;
            timer = onceTime;
        }
    }


    /// <summary>
    /// 视频第一次播放（如果正在播放中，再调用，会无效）
    /// </summary>
    public void FrameAniPlay(int num, bool _isLoop, Action callback = null)
    {
        PlayScreen.SetActive(true);
        if (num >= 0)
        {
            string str = AudioResPath + num.ToString(); // 如  AudioRes/0  
            this.MyClip = Resources.Load<AudioClip>(str); //找到Assets里的路径下的所有音频源;
            if (MyClip)
            {
                MyAudioSource.clip = MyClip;
            }
            onceTime = playDuratin / totalCount;  //每1张图片 需要的时间 6s总时间/150张图片
        }
        if (!Play)
        {
           // myImg.sprite = spriteRes[0];
            //          MyBodyTool.FadeCome(delegate ()
            //          {
          //  myImg.sprite = spriteRes[0]; //不隐藏的话循环会导致卡一下
            if (MyAudioSource != null)
            {
                MyAudioSource.Play();
            }

            index = 0;
            timer = onceTime;
            Play = true;
            if (!_isLoop)  //不循环播放就执行播放完成后的回调
            {
                if (callback != null)
                {
                    callback();
                }
            }
            else if (_isLoop) //循环播放
            {
                isLoop = _isLoop; //  FrameAniResetPlay(num);
            }
            //        });
        }
    }


    /// <summary>
    /// 动画显示打开
    /// </summary>
    void FrameAniActiveIsOpen()
    {
        PlayScreen.SetActive(true);
    }

    /// <summary>
    /// 视频重置播放（必要条件：任何时候可以重置,从头开始播放） 0.5秒后显示打开
    /// </summary>
    public void FrameAniResetPlay(int num, Action callback = null)
    {
        PlayScreen.SetActive(false);
        //Invoke("FrameAniActiveIsOpen", 0.5f);
        PlayScreen.SetActive(true);
        MyAudioSource.Stop();
        if (num >= 0)
        {
            string str = AudioResPath + num.ToString();
            this.MyClip = Resources.Load<AudioClip>(str); //找到Assets里的路径下的所有音频源;
            if (MyClip)
            { MyAudioSource.clip = MyClip; }
            onceTime = playDuratin / totalCount;  //每1张图片 需要的时间 6s总时间/150张图片
        }
        _play = false;
        Play = false;
        Pause = false;
        Continue = false;
        isPlaying = false;
        index = 0;

        timer = onceTime;
        myImg.sprite = spriteRes[0];
        //   MyBodyTool.FadeCome(delegate ()
        //   {
        Replay = true;
        MyAudioSource.Play();
        if (callback != null)
        {
            callback();
        }
        //   });
    }

    /// <summary>
    /// 视频停止播放并关闭显示
    /// </summary>
    public void FrameAniStop(Action callback = null)
    {
        PlayScreen.SetActive(false);
        if (callback != null)
        {
            callback();
        }
        //      MyBodyTool.FadeGo(delegate ()
        //     {
        myImg.sprite = spriteRes[0];
        //     });
        _play = false;
        MyAudioSource.Stop();

        Play = false;

        isPlaying = false;

        Replay = false;

        Pause = false;
        Continue = false;
        index = 0;
        timer = onceTime;


        //    MyPanelState = false;

        MyClip = null;
        // MyClip.UnloadAudioData();
        //       PlayScreen.SetActive(false);
    }

    /// <summary>
    /// 视频暂停（必要条件：正在播放中）
    /// </summary>
    public void FrameAniPause(Action callback = null)
    {
        Pause = true;
        MyAudioSource.Pause();
        if (callback != null)
        {
            callback();
        }
    }

    /// <summary>
    /// 视频继续（必要条件：正在播放中）
    /// </summary>
    public void FrameAniContinue(Action callback = null)
    {
        Pause = false;
        Continue = true;
        MyAudioSource.UnPause();
        if (callback != null)
        {
            callback();
        }
    }


    ///// <summary>
    ///// 视频播放完成
    ///// </summary>
    ///// <param name="callback"></param>
    public void FrameAniOnComplete(Action callback = null)
    {
        if (callback != null)
        {
            doneCallback = callback;
        }

        //     MyPanelState = true;
        MyClip = null;
        // MyClip.UnloadAudioData();
    }

}
