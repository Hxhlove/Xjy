using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        EventMgr.Inst.Regist(TouchInputEvent.Click, OnClick);

    }



    public void OnClick(EventArg arg)
    {
        SimpleTouchSystem.MyFingerData data = SimpleTouchSystem.currentOneFingerData;
        Debug.Log("执行Touch点击事件：" + data.fingerpos + "是否点击到Ui" + SimpleTouchSystem.isTouchUi);

    }

    public void OnSlide()
    {
        if (SimpleTouchSystem.GetTouchSlideDeltaPos != Vector2.zero)
        {
            Debug.Log("*****************执行  滑动");
            if (SimpleTouchSystem.currentOneFingerData.fingerpos != SimpleTouchSystem.currentOneFingerData.oldfingerpos)
                transform.Translate(SimpleTouchSystem.GetTouchSlideDeltaPos * Time.deltaTime, Space.Self);
        }
    }

    Vector3 localAxis = new Vector3(0, 0, 1);
    public void OnZoom()
    {
        if (SimpleTouchSystem.GetTouchZoomDeltaPos != 0)
        {
            transform.Translate(localAxis * SimpleTouchSystem.GetTouchZoomDeltaPos * Time.deltaTime, Space.Self);
            Debug.Log("***********************执行  缩放");
        }
        //if (!isfirst)
        //{
        //    isOpen = !isOpen;
        //    Debug.Log(isOpen);
        //    SimpleTouchSystem.TouchEnable = isOpen;
        //    isfirst = true;
        //}
        // }

    }

    bool isOpen = true;


    private void Update()
    {
        OnZoom();
        OnSlide();
    }

    //public void GetTwoFinger(params object[] para)
    //{
    //    SimpleTouchSystem.MyFingerData data = (SimpleTouchSystem.MyFingerData)para[0];
    //    if (!SimpleTouchSystem.isTwoTouch)
    //    {
    //        transform.Translate(data.fingerTouch.deltaPosition * Time.deltaTime, Space.Self);
    //    }
    //    Debug.Log("在移动................" + data.fingerTouch.deltaPosition);
    //}



    //public void SetCameraView(params object[] para)
    //{
    //    SimpleTouchSystem.TwoFinger f = (SimpleTouchSystem.TwoFinger)para[0];
    //    //缩放摄像机：
    //    transform.Translate(localAxis * f.deltaDisrance * Time.deltaTime, Space.Self);

    //    ////限制摄像机的View视口：在min 和max之间
    //    //     Camera.main.fieldOfView += f.deltaDisrance;
    //    //Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 179.9f);

    //}

}
