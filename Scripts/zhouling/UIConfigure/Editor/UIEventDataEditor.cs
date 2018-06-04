//================================================
//描 述 ：UI事件数据编辑器扩展
//作 者 ：Leo
//创建时间 ：2018/05/28 11:42:42  
//版 本： 
// ================================================
using UnityEditor;
using UnityEngine;


/*
[CustomEditor(typeof(UIEventData))]
public class UIEventDataEditor : Editor
{
//目标数据类
UIEventData myTarget;
/// <summary>
/// 序列化目标对象
/// </summary>
SerializedObject serializedObj;

//序列化属性(UI事件类型))
protected SerializedProperty _UIEventDataType;
//序列化属性(打开关闭UI结构(根节点->子节点))
protected SerializedProperty _UIStructure;
//序列化属性(摄像机动画路径对象集合))
protected SerializedProperty _CameraPaths;
//序列化属性(摄像机控制预制))
protected SerializedProperty _PreinstallControlType;
//序列化属性(场景事件类型))
protected SerializedProperty _SceneEventType;

void OnEnable()
{
    myTarget = (UIEventData)target;
    serializedObj = new SerializedObject(myTarget);
    //获取当前类中可序列话的属性
    _UIEventDataType = serializedObj.FindProperty("UIEventDataType");
    _UIStructure = serializedObj.FindProperty("UIStructure");
    _CameraPaths = serializedObj.FindProperty("CameraPaths");
    _PreinstallControlType = serializedObj.FindProperty("PreinstallControlType");
    _SceneEventType = serializedObj.FindProperty("SceneEventType");
}

public override void OnInspectorGUI()
{
    myTarget = (UIEventData)target;
#if UNITY_5_6_OR_NEWER
    serializedObj.UpdateIfRequiredOrScript();
#else
    serializedObj.UpdateIfDirtyOrScript ();
#endif

    //更新
    serializedObj.Update();
    //开始检查是否有修改
    EditorGUI.BeginChangeCheck();

    myTarget.Describe = EditorGUILayout.TextField("事件描述", myTarget.Describe);
    //显示序列化属性,第二个参数必须为true，否则无法显示子节点即List内容
    EditorGUILayout.PropertyField(_UIEventDataType);

    switch (myTarget.UIEventDataType)
    {
        case UIEventDataType.OpenUI:
            //显示序列化属性,第二个参数必须为true，否则无法显示子节点即List内容
            EditorGUILayout.PropertyField(_UIStructure, true);
            break;
        case UIEventDataType.CloseUI:
            //显示序列化属性,第二个参数必须为true，否则无法显示子节点即List内容
            EditorGUILayout.PropertyField(_UIStructure, true);
            break;
        case UIEventDataType.OpenAudio:
            myTarget.AudioClip = (AudioClip)EditorGUILayout.ObjectField("打开音频", myTarget.AudioClip, typeof(AudioClip), true);
            myTarget.isLoopAudio = EditorGUILayout.Toggle("是否循环", myTarget.isLoopAudio);
            myTarget.DelayTime = EditorGUILayout.FloatField("延迟打开时间", myTarget.DelayTime);                
            break;
        case UIEventDataType.SuspendAudio:
            myTarget.AudioClip = (AudioClip)EditorGUILayout.ObjectField("暂停音频", myTarget.AudioClip, typeof(AudioClip), true);
            break;
        case UIEventDataType.CloseAudio:
            myTarget.AudioClip = (AudioClip)EditorGUILayout.ObjectField("停止音频", myTarget.AudioClip, typeof(AudioClip), true);
            myTarget.DelayTime = EditorGUILayout.FloatField("延迟关闭时间", myTarget.DelayTime);
            break;
        case UIEventDataType.CameraAnimation:
            //显示序列化属性,第二个参数必须为true，否则无法显示子节点即List内容
            EditorGUILayout.PropertyField(_CameraPaths, true);
            break;
        case UIEventDataType.CameraControl:
            myTarget.DataObject = (UIEventData)EditorGUILayout.ObjectField("摄像机操作数据结构", myTarget.DataObject, typeof(UIEventData), true);
            break;
        case UIEventDataType.CameraPreinstallControl:
            EditorGUILayout.PropertyField(_PreinstallControlType);
            break;
        case UIEventDataType.RoomSelection:
            myTarget.RoomName = EditorGUILayout.TextField("选房名称", myTarget.RoomName);
            break;
        case UIEventDataType.SceneEvent:

            //显示序列化属性,第二个参数必须为true，否则无法显示子节点即List内容
            EditorGUILayout.PropertyField(_SceneEventType);
            break;
    }

    //===保存修改的数据======
    //结束检查是否有修改
    if (EditorGUI.EndChangeCheck())
    {//提交修改
        serializedObj.ApplyModifiedProperties();
    }
    EditorUtility.SetDirty(target);
}
}
*/
