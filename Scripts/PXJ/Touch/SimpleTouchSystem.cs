using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTouchSystem : MonoSingleInstance<SimpleTouchSystem>
{




    public struct MyFingerData
    {
        /// <summary>
        /// 手指的Touch属性(第一个/第二个)
        /// </summary>
        public Touch fingerTouch;
        /// <summary>
        /// 手指位置
        /// </summary>
        public Vector2 fingerpos;
        /// <summary>
        /// 手指状态
        /// </summary>
        public TouchPhase fingerphase;
        /// <summary>
        /// 手指上一帧的位置
        /// </summary>
        public Vector2 oldfingerpos;
        /// <summary>
        /// 手指最开始刚触摸到屏幕时候的位置
        /// </summary>
        public Vector2 beganfingerpos;
    }

    public struct TwoFinger
    {

        public float startDistance;
        public float fingerDistance;
        public float deltaDisrance;

    }

    /// <summary>
    /// 定义的一个手指类
    /// </summary>
    class MyFinger
    {
        public int id = -1;
        public Touch touch;

        static private List<MyFinger> fingers = new List<MyFinger>();
        /// <summary>
        /// 手指容器（暂定2个手指）
        /// </summary>
        static public List<MyFinger> Fingers
        {
            get
            {
                if (fingers.Count == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        MyFinger mf = new MyFinger();
                        mf.id = -1;
                        fingers.Add(mf);
                    }
                }
                return fingers;
            }
        }
    }

    // 小圈圈：用来实时显示手指触摸的位置
    GameObject[] marks = new GameObject[2];
    public GameObject markPerfab = null;

    // 粒子效果：来所显示手指手动的大概路径
    ParticleSystem[] particles = new ParticleSystem[2];
    public ParticleSystem particlePerfab = null;

    //触控系统开关
    public static bool TouchEnable { get; set; }

    bool isOneFingerBegan = false;
    bool isOneFingerEnded = false;
    bool isTwoFingerBegan = false;
    bool isTwoFingerEnded = false;
    int OneFingerStayNum = 0;
    int TwoFingerStayNum = 0;
    //第一个手指的Touch
    Touch OneFingerTouch { get { return MyFinger.Fingers[0].touch; } }
    //第二个手指的Touch
    Touch TwoFingerTouch { get { return MyFinger.Fingers[1].touch; } }

    Vector2 OneFingerBeganPos { get; set; }
    Vector2 TwoFingerBeganPos { get; set; }

    public static MyFingerData currentOneFingerData { get; private set; }
    public static MyFingerData currentTwoFingerData { get; private set; }

    static bool _isTwoTouch;
    public static bool isTwoTouch
    {
        get { return _isTwoTouch; }
        set
        {
            _isTwoTouch = value;
            if (value)
            {
                GetTouchSlideDeltaPos = Vector2.zero;
            }
            else
            {
                GetTouchZoomDeltaPos = 0;
            }
        }
    }

    static Vector2 _GetTouchSlideDeltaPos;
    /// <summary>
    /// 获取手指滑动时的坐标增量
    /// </summary>
    public static Vector2 GetTouchSlideDeltaPos
    {
        get
        {
            //是否开启触控系统
            if (!TouchEnable || isTwoTouch || isTouchUi)
            {
                _GetTouchSlideDeltaPos = Vector2.zero;
            }
            //是否触控到UI

            return _GetTouchSlideDeltaPos;
        }
        set { _GetTouchSlideDeltaPos = value; }
    }

    static float _GetTouchZoomDeltaPos;
    /// <summary>
    /// 获取手指缩放时的坐标增量
    /// </summary>
    public static float GetTouchZoomDeltaPos
    {
        get
        {
            //是否开启触控系统
            if (!TouchEnable || !isTwoTouch || isTouchUi)
            {
                _GetTouchZoomDeltaPos = 0;
            }
            //是否触控到UI

            return _GetTouchZoomDeltaPos;
        }
        set { _GetTouchZoomDeltaPos = value; }
    }

    void Start()
    {
        //初始化手指显示物体和粒子效果
        for (int i = 0; i < MyFinger.Fingers.Count; i++)
        {
            GameObject mark = Instantiate(markPerfab, Vector3.zero, Quaternion.identity) as GameObject;
            mark.transform.SetParent(transform);
            mark.SetActive(false);
            marks[i] = mark;

            ParticleSystem particle = Instantiate(particlePerfab, Vector3.zero, Quaternion.identity) as ParticleSystem;
            particle.transform.SetParent(transform);
            particle.Pause();
            particles[i] = particle;

            TouchEnable = true;
        }

        MyFinger.Fingers[0].touch.phase = TouchPhase.Canceled;
        MyFinger.Fingers[1].touch.phase = TouchPhase.Canceled;

        //法1：若遇到带返回值的方法，则将那个返回值设置成object类型的即可
        // EventManager.Instance.Regist(TheEventID.GetFingerData, new EventResult(GetFingerScreenPosition));
        //    //法2：若遇到带返回值的方法，则保持那个返回值的类型，不变，新设一个返回值是object类型的委托，把这个方法强转成这个委托类型即可
        //     EventManager.Instance.Regist(TheEventID.GetFingerData, (D_V3)GetFingerScreenPosition);
    }
    //   public delegate object D_V3(params object[] para);

    private void Update()
    {
        Touch[] touches = Input.touches;
        //遍历所有的已经记录的手指。剔除已经不存在的手指
        foreach (MyFinger mf in MyFinger.Fingers)
        {
            if (mf.id == -1)
            {
                continue;
            }
            bool stillExit = false;
            foreach (Touch t in touches)
            {
                if (mf.id == t.fingerId)
                {
                    stillExit = true;
                    break;
                }
            }
            //剔除
            if (!stillExit)
            {
                mf.id = -1;
            }
        }
        // 遍历当前的touches
        // --并检查它们在是否已经记录在AllFinger中
        // --是的话更新对应手指的状态，不是的放放加进去
        foreach (Touch t in touches)
        {
            bool stillExit = false;
            // 存在--更新对应的手指
            foreach (MyFinger mf in MyFinger.Fingers)
            {
                if (t.fingerId == mf.id)
                {
                    stillExit = true;
                    mf.touch = t;
                    break;
                }
            }
            // 不存在--添加新记录
            if (!stillExit)
            {
                foreach (MyFinger mf in MyFinger.Fingers)
                {
                    if (mf.id == -1)
                    {
                        mf.id = t.fingerId;
                        mf.touch = t;
                        break;
                    }
                }
            }
        }

        //时刻发送事件 传手指的信息（手指阶段+手指屏幕位置）
        GetFingerStateAndPosition(0);
        //有第二个手指的时候才执行
        if (MyFinger.Fingers[1].id != -1)
        {
            GetFingerStateAndPosition(1);
            GetTwoFinger();
        }

        // 记录完手指信息后，就是响应相应和状态记录了
        for (int i = 0; i < MyFinger.Fingers.Count; i++)
        {
            MyFinger mf = MyFinger.Fingers[i];



            if (mf.id != -1)
            {
                if (mf.touch.phase == TouchPhase.Began)
                {
                    marks[i].SetActive(true);
                    marks[i].transform.position = GetWorldPos(mf.touch.position);
                    particles[i].transform.position = GetWorldPos(mf.touch.position);
                }
                else if (mf.touch.phase == TouchPhase.Moved)
                {
                    marks[i].transform.position = GetWorldPos(mf.touch.position);

                    if (!particles[i].isPlaying)
                    {
                        ParticleSystem.MainModule pm = particles[i].main;
                        pm.loop = true;
                        particles[i].Play();
                    }
                    particles[i].transform.position = GetWorldPos(mf.touch.position);
                }
                else if (mf.touch.phase == TouchPhase.Ended)
                {
                    marks[i].SetActive(false);
                    marks[i].transform.position = GetWorldPos(mf.touch.position);
                    ParticleSystem.MainModule pm = particles[i].main;
                    pm.loop = false;
                    particles[i].Play();
                    particles[i].transform.position = GetWorldPos(mf.touch.position);
                }
                else if (mf.touch.phase == TouchPhase.Stationary)
                {
                    if (particles[i].isPlaying)
                    {
                        particles[i].Pause();
                    }
                    particles[i].transform.position = GetWorldPos(mf.touch.position);
                }
            }
            else
            {
                ;
            }
        }



    }



    public static bool isDoubleTap { get; set; }
    public static bool isSingleTap { get; set; }
    float timer = 0;
    bool isCheckDoubleTap = false;

    public UIOnClick uionclick;
    //执行点击事件
    private void FixedUpdate()
    {
        if (isCheckDoubleTap)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                On_SingleTapClick();

                timer = 0;
                isCheckDoubleTap = false;
            }
        }
    }

    void GetFingerStateAndPosition(int fingerIndex)
    {
        switch (MyFinger.Fingers[fingerIndex].touch.phase)
        {
            case TouchPhase.Began:
                {
                    isDoubleTap = false;
                    if (fingerIndex == 0 && !isOneFingerBegan)
                    {
                        isOneFingerBegan = true;
                        isOneFingerEnded = false;
                        OneFingerBeganPos = OneFingerTouch.position;
                        GetFingerData(0);
                        //       Debug.Log("手指" + fingerIndex + "的坐标是：" + OneFingerTouch.position + "状态是" + OneFingerTouch.phase.ToString());
                        //       Debug.Log(OneFingerTouch.tapCount + "***********************************************************************");

                        if (OneFingerTouch.tapCount == 2)
                        {
                            isDoubleTap = true;
                            isSingleTap = false;
                            //     Debug.Log("******************************即将双击");
                        }
                        else if (OneFingerTouch.tapCount == 1)
                        {
                            isSingleTap = true;
                            //     Debug.Log("******************************即将单击?????????????????????????");

                        }
                    }
                    else if (fingerIndex == 1 && !isTwoFingerBegan)
                    {
                        isTwoFingerBegan = true;
                        isTwoFingerEnded = false;

                        isTwoTouch = true;

                        TwoFingerBeganPos = TwoFingerTouch.position;
                        GetFingerData(1);
                        Debug.Log("手指" + fingerIndex + "的坐标是：" + TwoFingerTouch.position + "状态是" + TwoFingerTouch.phase.ToString());
                    }
                    break;
                }
            case TouchPhase.Stationary:
                {
                    if (fingerIndex == 0)
                    {
                        OneFingerStayNum++;
                        GetFingerData(0);

                    }
                    else if (fingerIndex == 1)
                    {
                        TwoFingerStayNum++;
                        GetFingerData(1);

                    }

                    //     Debug.Log("手指" + fingerIndex + "的坐标是：" + MyFinger.Fingers[fingerIndex].touch.position + "状态是" + MyFinger.Fingers[fingerIndex].touch.phase.ToString());
                    break;
                }
            case TouchPhase.Moved:
                {
                    //  Debug.Log("手指" + fingerIndex + "的坐标是：" + MyFinger.Fingers[fingerIndex].touch.position + "状态是" + MyFinger.Fingers[fingerIndex].touch.phase.ToString());
                    if (fingerIndex == 0)
                    {
                        if (OneFingerStayNum != 0)
                        {
                            OneFingerStayNum = 0;
                        }
                        GetFingerData(0);

                        //赋值：滑动时候的位置增量
                        GetTouchSlideDeltaPos = OneFingerTouch.deltaPosition;
                        //    Debug.Log(GetTouchSlideDeltaPos + "MOVE时候 的增量");
                        //  Debug.Log(OneFingerTouch.position + "MOVE时候 的位置");
                    }
                    else if (fingerIndex == 1)
                    {
                        if (TwoFingerStayNum != 0)
                        {
                            TwoFingerStayNum = 0;

                        }
                        GetFingerData(1);
                    }
                    break;
                }
            case TouchPhase.Ended:
                {
                    if (fingerIndex == 0 && !isOneFingerEnded)
                    {
                        isOneFingerBegan = false;
                        isOneFingerEnded = true;
                        //        Debug.Log(OneFingerStayNum + "************0手指停留个数");
                        OneFingerStayNum = 0;
                        if (MyFinger.Fingers[0].id != -1)
                        {
                            GetFingerData(0);


                            //*********************************发送Touch点击事件*******************************
                            PickObject(currentOneFingerData.fingerpos);
                            if (OneFingerTouch.tapCount <= 2)
                                isCheckDoubleTap = true;
                            //       Invoke("On_SingleTapClick", 0.2f);


                            _GetTouchSlideDeltaPos = Vector2.zero;
                            //      isDoubleTap = false;
                        }


                    }
                    else if (fingerIndex == 1 && !isTwoFingerEnded)
                    {
                        isTwoFingerBegan = false;
                        isTwoFingerEnded = true;

                        isTwoTouch = false;

                        //      Debug.Log(TwoFingerStayNum + "***********1手指停留个数*");
                        TwoFingerStayNum = 0;
                        if (MyFinger.Fingers[1].id != -1)
                        {
                            GetFingerData(1);
                            //        Debug.Log(currentTwoFingerData.fingerIndex + "______________________________");
                        }
                        //       Debug.Log("手指" + fingerIndex + "的坐标是：" + MyFinger.Fingers[fingerIndex].touch.position + "状态是" + MyFinger.Fingers[fingerIndex].touch.phase.ToString());
                    }

                    break;
                }
        }
    }


    void On_SingleTapClick()
    {

        if (TouchEnable && !isTwoTouch && !isDoubleTap && isSingleTap && GetTouchSlideDeltaPos == Vector2.zero && GetTouchZoomDeltaPos == 0)
        {
            float onclickSqrOffset = (OneFingerTouch.position - OneFingerBeganPos).sqrMagnitude;//12*12=144           
            if (onclickSqrOffset >= 0 && onclickSqrOffset <= 400)
            {
                Debug.Log("******************************单击"+"碰到UI："+ isTouchUi);
                //发送 是否碰到UI，手指碰到的位置（这里指Began的位置而不是此时此刻的手指位置，因为会有误差）
                EventMgr.Inst.Fire(TouchInputEvent.Click, new EventArg(isTouchUi, currentOneFingerData.beganfingerpos));
                isSingleTap = false;
                if (isTouchUi)
                {
                    if (uionclick != null)
                    {
                        uionclick(PickObject(OneFingerBeganPos));
                    }
                }
            }
        }
        else
        {
            Debug.Log("不会再执行单击事件了");
        }
    }

    void GetFingerData(int fingerIndex)
    {
        //上一帧 两个手指之间的距离
        twofinger.startDistance = Vector2.Distance(OneFingerTouch.position, TwoFingerTouch.position);

        if (fingerIndex == 0)
        {
            TouchPhase onefingerphase = OneFingerTouch.phase;
            currentOneFingerData = SetFingerData(OneFingerTouch, OneFingerTouch.position, onefingerphase, OneFingerTouch.position - OneFingerTouch.deltaPosition, OneFingerBeganPos);
        }
        else if (fingerIndex == 1)
        {
            TouchPhase twofingerphase = TwoFingerTouch.phase;
            currentTwoFingerData = SetFingerData(TwoFingerTouch, TwoFingerTouch.position, twofingerphase, TwoFingerTouch.position - TwoFingerTouch.deltaPosition, TwoFingerBeganPos);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mytouch">touch属性</param>
    /// <param name="pos">手指当前位置</param>
    /// <param name="ph">手指状态</param>
    /// <param name="oldPos">手指上一帧位置</param>
    ///   /// <param name="beganpos">手指触摸开始的位置</param>
    /// <returns></returns>
    MyFingerData SetFingerData(Touch mytouch, Vector2 pos, TouchPhase ph, Vector2 oldPos, Vector2 beganpos)
    {
        MyFingerData MyFingerData = new MyFingerData();

        MyFingerData.fingerTouch = mytouch;
        MyFingerData.fingerpos = pos;
        MyFingerData.fingerphase = ph;
        MyFingerData.oldfingerpos = oldPos;

        return MyFingerData;
    }

    public static TwoFinger twofinger = new TwoFinger();


    /// <summary>
    /// 2个手指情况下执行
    /// </summary>
    void GetTwoFinger()
    {
        Vector2 touchOnePrePos = currentOneFingerData.fingerpos - OneFingerTouch.deltaPosition;
        Vector2 touchTwoPrePos = currentTwoFingerData.fingerpos - TwoFingerTouch.deltaPosition;

        float prevTouchDeltaMagnitude = (touchTwoPrePos - touchOnePrePos).magnitude;
        float nowTouchDeltaMagnitude = (TwoFingerTouch.position - OneFingerTouch.position).magnitude;

        float deltaMagnitudeDiffset = prevTouchDeltaMagnitude - nowTouchDeltaMagnitude;
        twofinger.deltaDisrance = deltaMagnitudeDiffset;

        GetTouchZoomDeltaPos = deltaMagnitudeDiffset;
        //    EventMgr.Inst.Fire(TouchInputEvent.Zoom, new global::EventArg(twofinger));
    }

    public Vector3 GetWorldPos(Vector2 screenPos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane + 10));
    }


    //不加碰撞器，则也不会检测到是碰到UI
    public static bool isTouchUi = false;
    GameObject PickObject(Vector2 touchpos)
    {
        GameObject targetObj = null;
        Ray ray = Camera.main.ScreenPointToRay(touchpos);
        RaycastHit hit;
        if (TouchEnable)
        {
            if (Physics.Raycast(ray, out hit))
            {
                targetObj = hit.collider.gameObject;
                if (targetObj.layer == LayerMask.NameToLayer("UI"))
                {
                    //     Debug.Log(targetObj.name);
                    isTouchUi = !isTouchUi;
                }
                else
                {
                    isTouchUi = false;
                }

            }

        }
        return targetObj;
    }
}
