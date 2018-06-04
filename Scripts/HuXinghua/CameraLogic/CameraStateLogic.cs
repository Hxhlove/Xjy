using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ------------------------------------------------------------------
// Title        :摄像机操作
// Author       :Vincent
// Date         :2018.05.21
// Description  :
// ------------------------------------------------------------------

    //摄像机动画完成回调


public class CameraStateLogic
{
    private float _distance;//摄像机与目标点的距离
    private float _xIncrement;//X方向的增量
    private float _yIncrement;//y方向的增量
    private Transform _cameraMain;//操作的摄像机
    private CameraOperateData _cameraStateData;//操作摄像机需要的数据
//    private bool isResponse;//是否响应控制
//    private Transform moveDirection;//控制角度旋转时移动的方向 >>>>>>>>>>>>>
    #region//单例对象
    /// <summary>
    /// 单例
    /// </summary>
    private static CameraStateLogic cameraStateLogic;
    private CameraStateLogic() { }
    public static CameraStateLogic Init
    {
        get
        {
            if (cameraStateLogic == null)
            {
                cameraStateLogic = new CameraStateLogic();
            }
            return cameraStateLogic;
        }
    }
    #endregion
    #region//字段封装
    /// <summary>
    /// X方向增量
    /// </summary>
    public float XIncrement
    {
        get
        {
            return _xIncrement;
        }

        set
        {
            _xIncrement = value;
        }
    }
    /// <summary>
    /// Y方向增量
    /// </summary>
    public float YIncrement
    {
        get
        {
            return _yIncrement;
        }

        set
        {
            _yIncrement = value;
        }
    }
    #endregion
    /// <summary>
    /// 设置控制数据
    /// </summary>
    /// <param name="cameraMain"></param>
    /// <param name="cameraStateData"></param>
    public void SetCameraStateLogicData(Transform cameraMain, CameraOperateData cameraStateData)
    {
        _cameraMain = cameraMain;
        _cameraStateData = cameraStateData;
    }
    #region//控制状态更新
    /// <summary>
    /// 摄像机旋转
    /// </summary>
    /// <param name="_cameraMain">默认为摄像机</param>
    /// <param name="_cameraStateData">旋转所需要的数据类</param>
    public void RotateStateUpdate()
    {
        if (_cameraStateData.isResponse && _cameraStateData != null)
        {
            //目标点为空 旋转或者第一人称 
            if (_cameraStateData.aroundTarget == null)
            {
                //目标为空 需要改变位置 执行第一人称控制方式
                if (_cameraStateData.isFirstView)
                {
                    //第一人称控制
                    ControlMove(_cameraMain, _cameraStateData);
                }
                //围绕自身旋转
                else
                {
                    RotateAroundSelf();
                }
            }
            //目标点不为空 围绕目标旋转
            else
            {
                RotateAroundTarget();
            }
        }
    }
    /// <summary>
    /// 围绕自身旋转
    /// </summary>
    private void RotateAroundSelf()
    {
        _cameraMain.Rotate(new Vector3(Input.GetAxis("Mouse Y") * 5f, Input.GetAxis("Mouse X") * 5f, 0), Space.World);
        XIncrement = ClampAngles(_cameraMain.localEulerAngles.y, _cameraStateData.xMinLimit, _cameraStateData.xMaxLimit);//y角度限制
        YIncrement = ClampAngles(_cameraMain.localEulerAngles.x, _cameraStateData.yMinLimit, _cameraStateData.yMaxLimit);//x方向角度限制
        Quaternion quaternionEuler = Quaternion.Euler(YIncrement, XIncrement, 0);
        _cameraMain.rotation = quaternionEuler;
        //在目标点方向上移动
        if (_cameraStateData.isMoveToTarget)
        {
            Debug.Log(Vector3.Distance(_cameraMain.position, _cameraStateData.moveDirection.position));
            LimitDistance(_cameraMain, _cameraStateData, _cameraStateData.moveDirection);
        }
    }
    /// <summary>
    /// 围绕目标点旋转
    /// </summary>
    private void RotateAroundTarget()
    {
        //Debug.Log(_distance);
        _distance = Vector3.Distance(_cameraMain.position, _cameraStateData.aroundTarget.transform.position);//与目标位置的距离
        _distance -= Input.GetAxis("Mouse ScrollWheel");//此处数据从控制中调，暂为测试
        _distance = Mathf.Clamp(_distance, _cameraStateData.minDistance, _cameraStateData.maxDistance);
        XIncrement += Input.GetAxis("Mouse X");//此处数据从控制中调，暂为测试 
        YIncrement -= Input.GetAxis("Mouse Y");//此处数据从控制中调，暂为测试
        YIncrement = ClamAngle(YIncrement, _cameraStateData.yMinLimit, _cameraStateData.yMaxLimit);//y角度限制
        XIncrement = ClamAngle(XIncrement, _cameraStateData.xMinLimit, _cameraStateData.xMaxLimit);//x方向角度限制
        Quaternion quaternionEuler = Quaternion.Euler(YIncrement, XIncrement, 0);
        Vector3 direction = quaternionEuler * Vector3.forward;
        _cameraMain.position = _cameraStateData.aroundTarget.transform.position - direction * _distance;
        _cameraMain.rotation = quaternionEuler;
        _cameraMain.LookAt(_cameraStateData.aroundTarget.transform.position);
    }
    #endregion
    /// <summary>
    /// 对角度的限制,角度超过最大或最小的时候返回最大或最小角度的值
    /// </summary>
    /// <param name="angle">当前角度</param>
    /// <param name="min">最小角度</param>
    /// <param name="max">最大角度</param>
    /// <returns></returns>
    private float ClamAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
        {
            angle += 360.0f;
        }
        if (angle > 360.0f)
        {
            angle -= 360.0f;
        }
        return Mathf.Clamp(angle, min, max);
    }


    /// <summary>
    /// 反算增量
    /// </summary>
    public void ReverseIncrement()
    {
        if (_cameraStateData.aroundTarget != null)
        {
            _distance = Vector3.Distance(_cameraMain.position, _cameraStateData.aroundTarget.transform.position);//与目标位置的距离
            Vector3 direction1 = (_cameraStateData.aroundTarget.transform.position - _cameraMain.position) / _distance;
            Quaternion quaternionFromToRotation = Quaternion.FromToRotation(Vector3.forward, direction1);//反算当前角度
            XIncrement = quaternionFromToRotation.eulerAngles.y;//当前x的值
            YIncrement = quaternionFromToRotation.eulerAngles.x;//当前y的值
            Debug.Log(XIncrement + "xy" + YIncrement);
        }
    }

    /// <summary>
    /// 限制移动的距离
    /// </summary>
    private void LimitDistance(Transform cameraMain, CameraOperateData cameraStateData, Transform moveDirection)
    {
        //超出最大 只允许减小
        if (Vector3.Distance(cameraMain.position, moveDirection.position) > cameraStateData.maxDistance)
        {
            Debug.Log("最大");
            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Debug.Log("最大并且移动");
                MoveToDirection();
            }
        }
        //超出最小值 只允许增加
        else if (Vector3.Distance(cameraMain.position, moveDirection.position) < cameraStateData.minDistance)
        {
            Debug.Log("最小");
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Debug.Log("最小并且移动");
                MoveToDirection();
            }
        }
        else
        {
            MoveToDirection();
        }
    }
    /// <summary>
    /// 在某个方向上进行移动
    /// </summary>
    private void MoveToDirection()
    {
        _cameraMain.Translate(Vector3.forward * -Input.GetAxis("Mouse ScrollWheel"), _cameraStateData.moveDirection);//向该Transform的Z方向进行移动
    }
    /// <summary>
    /// 摄像机第一人称控制
    /// </summary>
    /// <param name="cameraMain"></param>
    /// <param name="cameraRigidbody"></param>
    public void ControlMove(Transform cameraMain, CameraOperateData cameraStateData)
    {
        //添加刚体和碰撞
        if (cameraMain.gameObject.GetComponent<Rigidbody>() == null)
        {
            cameraMain.gameObject.AddComponent<Rigidbody>();
            cameraMain.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            cameraMain.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //Debug.Log(cameraMain.gameObject.GetComponent<Rigidbody>().freezeRotation);
            cameraMain.gameObject.AddComponent<CapsuleCollider>();
            cameraMain.gameObject.GetComponent<CapsuleCollider>().height = 1.7f;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 LocalPos = cameraMain.position;/*物体所处的世界坐标向量*/
            Vector3 LocalForward = cameraMain.TransformPoint(Vector3.forward * Input.GetAxis("Mouse Y"));/*物体前方距离为speed的位置的世界坐标向量*/
            cameraMain.Rotate(cameraMain.up, Input.GetAxis("Mouse X"), Space.World);
            Vector3 VecSpeed = LocalForward - LocalPos;/*物体自身Vector3.forward * speed的世界坐标向量*/
            cameraMain.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(VecSpeed.x * cameraStateData.rototeSpeed, VecSpeed.y * cameraStateData.rototeSpeed, VecSpeed.z * cameraStateData.rototeSpeed);
        }
        else
        {
            cameraMain.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
    /// <summary>
    /// 绕自身旋转的角度限制
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    protected float ClampAngles(float angle, float min, float max)
    {

        angle = NormalizeAngle(angle);
        if (angle > 180)
        {
            angle -= 360;
        }
        else if (angle < -180)
        {
            angle += 360;
        }

        min = NormalizeAngle(min);
        if (min > 180)
        {
            min -= 360;
        }
        else if (min < -180)
        {
            min += 360;
        }

        max = NormalizeAngle(max);
        if (max > 180)
        {
            max -= 360;
        }
        else if (max < -180)
        {
            max += 360;
        }
        // Aim is, convert angles to -180 until 180.
        return Mathf.Clamp(angle, min, max);
    }
    protected float NormalizeAngle(float angle)
    {
        while (angle > 360)
            angle -= 360;
        while (angle < 0)
            angle += 360;
        return angle;
    }
}
