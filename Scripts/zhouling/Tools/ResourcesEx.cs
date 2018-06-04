// ------------------------------------------------------------------
// Title        :资源加载扩展工具
// Author       :Leo
// Date         :2018.05.04
// Description  :扩展系统的资源加载器，可以加载指定路径的各种类型数据
// ------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public delegate void ConductAssetCallback(UnityEngine.Object arg);

static class ResourcesEx
{
    private static Dictionary<string, UnityEngine.Object> sm_assetObjs;                  //已导入的资源对象
    private static Dictionary<string, ConductAsset> sm_ConductassetObjs;                 //正在导入的游戏资源

    static ResourcesEx()
    {
        sm_assetObjs = new Dictionary<string, UnityEngine.Object>();
        sm_ConductassetObjs = new Dictionary<string, ConductAsset>();
    }

    //==================================异步加载======================================
    //异步加载资源更新
    public static void FixedUpdate()
    {
        if (sm_ConductassetObjs.Count > 0)
        {
            foreach (ConductAsset ca in new List<ConductAsset>(sm_ConductassetObjs.Values))
            {
                if (ca.isDone)
                {
                    if (ca.Asset != null)
                    {
                        SetCache(ca.path, ca.Asset);
                    }
                    sm_ConductassetObjs.Remove(ca.path);
                    ca.Callback();
                }
            }
        }
    }

    //异步读取对象
    public static void LoadObjectAsync(ConductAsset ca)
    {
        if (sm_assetObjs.ContainsKey(ca.path))
        {
            ca.ConductAssetCallback(sm_assetObjs[ca.path]);
            return;
        }
        if (sm_ConductassetObjs.ContainsKey(ca.path))
        {
            Debug.Log(string.Format("有相同资源加载中{0}", ca.path));
            sm_ConductassetObjs[ca.path].Add(ca);
            return;
        }
        string newPath = PathFileTool.GetFileName(ca.path);
        string bundlePath = PathFileTool.EncryptionResources + PathFileTool.GetMD5FileName(newPath) + ".data";
#if UNITY_EDITOR
        bundlePath = PathFileTool.StreamingAssets + PathFileTool.GetMD5FileName(newPath) + ".data";
#endif
        if (System.IO.File.Exists(bundlePath))
        {
            StreamLoadAsync(bundlePath, ca);
        }
        else
        {
            LocalLoadAsync(ca);
        }
    }

    //异步读取流资源
    private static void StreamLoadAsync(string bundlePath, ConductAsset ca)
    {
        Debug.Log(string.Format("加载流资源：{0}\r\n{1}", bundlePath, ca.path));
        FileStream fs = File.OpenRead(bundlePath);
        FileStateObject fso = new FileStateObject();
        fso.content = new byte[fs.Length];
        fso.fs = fs;
        fso.length = Convert.ToInt32(fs.Length);
        fs.BeginRead(fso.content, 0, fso.length, new AsyncCallback(ca.ReadInFileCallback), fso);
        sm_ConductassetObjs.Add(ca.path, ca);
    }
    //异步加载本地资源
    private static void LocalLoadAsync(ConductAsset ca)
    {
        Debug.Log(string.Format("加载本地资源：{0}", ca.path));
        string newPath = PathFileTool.GetFileName(ca.path);
        try
        {
            ca.resourceRequest = Resources.LoadAsync(newPath);
            sm_ConductassetObjs.Add(ca.path, ca);
        }
        catch (Exception ex)
        {
            Debug.LogError(string.Format("异步加载本地资源错误：{0}\r\n{1}", ex, ca.path));
        }
    }


    //==================================同步加载======================================

    /// <summary>
    /// 同步加载目录下指定类型的所有数据
    /// </summary>
    public static List<T> LoadAll<T>(string path)
        where T : UnityEngine.Object
    {
        return Resources.LoadAll<T>(path).ToList();
    }


    private static UnityEngine.Object Load(string path)
    {
        if (sm_assetObjs.ContainsKey(path))
        {
            return sm_assetObjs[path];
        }
        string newPath = PathFileTool.GetFileName(path);
        try
        {
            if (newPath.Contains("Assets/Resources/"))
            {
                newPath = newPath.Replace("Assets/Resources/", "");
            }
            UnityEngine.Object obj = Resources.Load(newPath);
            if (obj)
            {
                SetCache(path, obj);
            }
            return obj;
        }
        catch (Exception ex)
        {
            Debug.LogError(string.Format("加载本地资源错误：{0}\r\n{1}", ex, path));
            throw new Exception(path, ex);
        }
    }

    //同步导入object
    //先AssetBundle.CreateFile导入未压缩的assetbundl
    //再Resources.Load
    //返回object
    public static UnityEngine.Object LoadObject(string path)
    {
        if (sm_assetObjs.ContainsKey(path))
        {
            return sm_assetObjs[path];
        }
        string newPath = PathFileTool.GetFileName(path);
        string bundlePath = PathFileTool.EncryptionResources + PathFileTool.GetMD5FileName(newPath) + ".data";
#if UNITY_EDITOR

        bundlePath = PathFileTool.StreamingAssets + PathFileTool.GetMD5FileName(newPath) + ".data";
#endif

        if (System.IO.File.Exists(bundlePath))
        {
            byte[] bytes = File.ReadAllBytes(bundlePath);
            if (bytes.Length > 88)
            {
                Array.Reverse(bytes, 0, 88);
            }
            AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
            if (bundle != null)
            {
                UnityEngine.Object obj = bundle.mainAsset;
                bundle.Unload(false);
                SetCache(path, obj);
                return obj;
            }
        }
        else
        {
            Debug.Log(string.Format("加载本地资源：{0}", newPath));
            return Load(newPath);
        }
        return null;
    }


    public static T LoadObject<T>(string path)
        where T : UnityEngine.Object
    {
        return LoadObject(path) as T;
    }

    //读取图片
    public static Texture2D LoadTexture(string path)
    {
        string newPath = PathFileTool.GetFileName(path);
        string bundlePath = PathFileTool.EncryptionResources + PathFileTool.GetMD5FileName(newPath) + ".data";
#if UNITY_EDITOR

        bundlePath = PathFileTool.StreamingAssets + PathFileTool.GetMD5FileName(newPath) + ".data";
#endif

        if (System.IO.File.Exists(bundlePath))
        {
            //创建文件读取流
            FileStream fileStream = new FileStream(bundlePath, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];
            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取流
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;

            //创建Texture
            int width = 512;
            int height = 512;
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            texture.Apply();
            return texture;
        }
        return LoadObject<Texture2D>(path) as Texture2D;
    }

    //读文本文件
    public static string LoadText(string file)
    {
        TextAsset textasset = LoadObject(file) as TextAsset;
        if (textasset)
            return textasset.text;
        return string.Empty;
    }
    //加载材质球
    public static Material LoadMaterial(string file)
    {
        return LoadObject(file) as Material;
    }


    //==================================常用方法======================================

    //设置缓存
    private static void SetCache(string path, UnityEngine.Object obj)
    {
        if (!sm_assetObjs.ContainsKey(path))
        {
            sm_assetObjs.Add(path, obj);
        }
    }

    //销毁对象
    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj == null) return;
        GameObject.Destroy(obj);
        UnloadUnusedAssets();
    }

    //跳转场景时,卸载掉所有的资源,GC掉没引用的类
    public static void Clear()
    {
        sm_assetObjs.Clear();
        Unload();
    }
    //资源加载完成后调用一次
    public static void UnloadUnusedAssets()
    {
        GC.Collect();
    }
    public static void Unload()
    {
        Resources.UnloadUnusedAssets();
        Caching.ClearCache(); 
        UnloadUnusedAssets();
    }
}
/// <summary>
/// 定义回调函数中传递的对象
/// </summary>
public class FileStateObject
{
    public byte[] content;          //读取的内容        
    public FileStream fs;           //操作时的FileStream对象        
    public int length;              //读取的内容长度
}
/// <summary>
/// 定义异步加载资源对象
/// </summary>
class ConductAsset
{
    public ConductAsset(string mpath, ConductAssetCallback callback)
    {
        this.path = mpath;
        m_Callback = callback;
    }
    public string path;                          //数据名称或路径
    public ConductAssetCallback m_Callback;      //加载完成回调
    public ResourceRequest resourceRequest;      //异步加载过程
    List<ConductAsset> list;                     //相同路径下的其他异步加载对象
    private bool m_isDone = false;               //是否加载完成
    private UnityEngine.Object mainAsset;        //加载完成的数据

    public bool isDone
    {
        get
        {
            if (resourceRequest != null && resourceRequest.isDone)
            {
                m_isDone = true;
                mainAsset = resourceRequest.asset;
            }
            return m_isDone;
        }
    }
    public UnityEngine.Object Asset { get { return mainAsset; } }

    public void ConductAssetCallback(UnityEngine.Object asset)
    {
        m_Callback(asset);
        if (list != null)
        {
            foreach (ConductAsset ca in list)
            {
                ca.ConductAssetCallback(asset);
            }
            list = null;
        }
    }
    public void Add(ConductAsset ca)
    {
        if (list == null)
        {
            list = new List<ConductAsset>();
        }
        list.Add(ca);
    }
    public void Callback()
    {
        ConductAssetCallback(Asset);
    }
    /// <summary>
    /// 异步读取流文件完成
    /// </summary>
    public void ReadInFileCallback(IAsyncResult asyncResult)
    {
        FileStateObject fso = (FileStateObject)asyncResult.AsyncState;  //从参数中取出传来的对象
        FileStream fs = fso.fs;
        try
        {
            int byteread = fs.EndRead(asyncResult);                         //结束异步操作,返回读取字符数
            if (byteread == fso.length)
            {
                byte[] bytes = fso.content;
                if (bytes.Length > 88)
                {
                    Array.Reverse(bytes, 0, 88);
                }
                AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
                if (bundle != null)
                {
                    mainAsset = bundle.mainAsset;
                    if (!bundle.isStreamedSceneAssetBundle)
                    {
                        bundle.Unload(false);
                    }
                    else
                    {
                        TimerMgr.Add(5,() =>
                        {
                            bundle.Unload(false);
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(string.Format("异步读取流文件错误:{0}\r\n{1}", e.Message, e.StackTrace));
        }
        finally
        {
            fs.Close();
            m_isDone = true;
        }
    }
}