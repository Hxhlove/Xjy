// ------------------------------------------------------------------
// Title        :场景加载控制器
// Author       :Leo
// Date         :2018.05.15
// Description  :异步加载场景管理
// ------------------------------------------------------------------


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneController
{
    static LoadSceneController sm_inst;

    public static LoadSceneController Inst
    {
        get
        {
            if (sm_inst == null)
                sm_inst = new LoadSceneController();
            return sm_inst;
        }
    }

    /// <summary>
    /// 初始
    /// </summary>
    public void Open()
    {
        LoadSceneList = new Queue<EventArg>();
        EventMgr.Inst.Regist(LoadSceneEvent.Load, LoadScene);
    }

    /// <summary>
    /// 加载场景数据地址集合
    /// </summary>
    Queue<EventArg> LoadSceneList;
    /// <summary>
    /// 当前加载中的场景
    /// </summary>
    EventArg CurrentLoadScene;
    /// <summary>
    /// 场景异步加载进度
    /// </summary>
    AsyncOperation AsyncOperation;
    /// <summary>
    /// 加载进度描述
    /// </summary>
    public string AsyncLoadScene
    {
        get
        {
            if (AsyncOperation == null)
            {
                return "完成";
            }
            else
            {
                return string.Format("{0}%(1/{1})", (int)(AsyncOperation.progress *100), LoadSceneList.Count+1);
            }
        }
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    public void LoadScene(EventArg ea)
    {
        LoadSceneList.Enqueue(ea);
        LoadSceneStart();
    }

    /// <summary>
    /// 开始异步加载
    /// </summary>
    private void LoadSceneStart()
    {
        if (CurrentLoadScene == null && LoadSceneList.Count > 0)
        {
            CurrentLoadScene = LoadSceneList.Dequeue();
            Debug.Log(string.Format("场景{0}异步加载----开始", (string)CurrentLoadScene[0]));
            AsyncOperation = SceneManager.LoadSceneAsync((string)CurrentLoadScene[0], LoadSceneMode.Additive);
            if (AsyncOperation != null)
            {
                CurrentLoadScene.Callback(new EventArg(AsyncOperation));
                AsyncOperation.completed += AsyncOperationEnd;
            }
            else
            {
                CurrentLoadScene = null;
                AsyncOperation = null;
                LoadSceneStart();
            }
        }
    }
    /// <summary>
    /// 异步加载完成回调
    /// </summary>
    private void AsyncOperationEnd(AsyncOperation ao)
    {
        Debug.Log(string.Format("场景{0}异步加载----完成", (string)CurrentLoadScene[0]));
        CurrentLoadScene.Callback();
        CurrentLoadScene = null;
        AsyncOperation = null;
        LoadSceneStart();
    }    
}