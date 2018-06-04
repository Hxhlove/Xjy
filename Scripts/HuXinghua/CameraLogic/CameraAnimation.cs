//================================================
//描 述 ： 摄像机动画测试
//作 者 ：Vincent
//创建时间 ：2018/05/30 15:09:53  
//版 本： 
// ================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation
{

    private Transform _moveCamera;//动画摄像机
    private Transform _targetTransform;//目标位置
    private CameraAnimationC CameraAnimationC;//回调委托
    private CameraControlEvent _cameraControlEvent;//摄像机事件类型
    private CameraPath _cameraPath;//摄像机预设路径动画
                                   // private CameraPathAnimator _cameraPathAnimator = new CameraPathAnimator();//预设动画播放控制类
                                   //private CameraAnimator _cameraAnimator = new CameraAnimator();


    public CameraAnimation(CameraAnimationC cameraAnimationC)
    {
        this.CameraAnimationC = cameraAnimationC;
        //_cameraAnimator.
    }

    /// <summary>
    /// 动画数据和类型赋值
    /// </summary>
    /// <param name="pathTransform"></param>
    /// <param name="cameraControlEvent"></param>
    public void SetAnimationData(List<Transform> pathTransform, CameraControlEvent cameraControlEvent)
    {
        _moveCamera = pathTransform[0];
        _targetTransform = pathTransform[1];
        this._cameraControlEvent = cameraControlEvent;
    }
    public void SetAnimationData(CameraPath cameraPath, CameraControlEvent cameraControlEvent)
    {
        this._cameraPath = cameraPath;
        this._cameraControlEvent = cameraControlEvent;
    }

    public void AnimationUpdate()
    {
        Debug.Log("执行动画");
        switch (_cameraControlEvent)
        {
            case CameraControlEvent.InitialPosition://瞬间移动
                InitialPosition(_moveCamera, _targetTransform);
                break;
            case CameraControlEvent.OnePointsAnimation://一个点的动画
                Debug.Log("执行一个点动画");
                AnimationPlay(_moveCamera, _targetTransform);
                break;
            case CameraControlEvent.PreinstallControl://启动预设路径动画
                                                      // _cameraPathAnimator.Play();
                break;

        }
    }
    private Vector3 oldPosition = Vector3.zero;//上一帧的位置
    private Vector3 newPosition = Vector3.zero;//当前帧的位置
    private Quaternion oldRotation = Quaternion.identity;
    private Quaternion newRotation = Quaternion.identity;
    private void AnimationPlay(Transform moveCamera, Transform targetTransform)
    {
        moveCamera.position = Vector3.Slerp(moveCamera.position, targetTransform.position, 0.2f);
        moveCamera.rotation = Quaternion.Lerp(moveCamera.transform.rotation, targetTransform.rotation, 0.2f);
        newPosition = moveCamera.position;
        newRotation = moveCamera.rotation;
        if (newPosition == oldPosition && newRotation == oldRotation)
        {
            Debug.Log("动画结束");
            if (CameraAnimationC != null)
            {
                CameraAnimationC();
            }
        }
        oldPosition = newPosition;
        oldRotation = newRotation;
    }
    private void InitialPosition(Transform moveCamera, Transform targetTransform)
    {
        moveCamera = targetTransform;
        if (moveCamera == targetTransform && CameraAnimationC != null)
        {
            CameraAnimationC();
        }
    }
    /// <summary>
    /// 播放预制动画
    /// </summary>
    //private void PreinstallControl()
    //{
    //    _cameraPathAnimator.Play();
    //    if (!_cameraPathAnimator.isPlaying && CameraAnimationC != null)
    //    {
    //        CameraAnimationC();
    //    }
    //}
}
