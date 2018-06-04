// ------------------------------------------------------------------
// Title        :路径工具
// Author       :Leo
// Date         :2018.05.04
// Description  :获取各种数据路径,和路径处理方法
// ------------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class PathFileTool
{
    static PathFileTool()
    {
        CreatePath(Res);
    }    
    public static string Res = string.Format("{0}{1}", GetProjectPathe(), "/Res");                           //加密资源路径
    
    //常用资源路径
    public static string PlanScene = "02";                                                          //园区规划场景
    public static string Location = "Location";                                                     //区域场景漫游数据
    public static string SandTable = "SandTable";                                                   //沙盘场景漫游数据

    public static string UIConfigureData = "UIConfigureData";                                       //UI配置数据




    public static string FilmHead = "FilmHead";                         //片头路径

    /// <summary>
    /// 加密资源路径
    /// </summary>
    public static string EncryptionResources
    {
        get { return string.Format("{0}{1}", Res, "/"); }
    }

    /// <summary>
    /// 流资源路径
    /// </summary>
    public static string StreamingAssets
    {
        get { return string.Format("{0}{1}", UnityEngine.Application.streamingAssetsPath, "/"); }
    }
    

    //获取项目名称前的完整路径
    public static string GetProjectPathe()
    {
        string path = System.IO.Directory.GetCurrentDirectory().Trim();
        path = path.Replace("\\", "/");
        return path;
    }

    //MD5加密路径名称
    public static string GetMD5FileName(string file)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(file));
        return BitConverter.ToString(hash).Replace("-", "");
    }

    //去除文件后缀
    public static string GetFileName(string path)
    {
        int index = path.LastIndexOf(".");
        return index >= 0 ? path.Remove(index) : path;
    }

    /// <summary>
    /// 判断目录是否存在,进行创建目录
    /// </summary>
    public static void CreatePath(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log(string.Format("创建目录:{0}", path));
        }
    }
}