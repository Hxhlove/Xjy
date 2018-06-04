//================================================
//描 述 ： 给脚本添加抬头
//作 者 ：HML
//创建时间 ：2018/05/22 11:49:15  
//版 本： 1.0
// ================================================
using UnityEngine;
using System.IO;

public class AddHeaderToScript : UnityEditor.AssetModificationProcessor
{
    private static string annotationStr =
          "//================================================\r\n"
           + "//描 述 ： \r\n"
           + "//作 者 ：\r\n"
           + "//创建时间 ：#CreatTime#  \r\n"
           + "//版 本： \r\n"
           + "// ================================================\r\n";

    private static void OnWillCreateAsset(string path)
    {      //方法必须为static
           //排除“.meta”的文件
        path = path.Replace(".meta", "");
        //如果是cs脚本，则进行添加注释处理
        if (path.EndsWith(".cs"))
        {
            //读取cs脚本的内容并添加到annotationStr后面
            annotationStr += File.ReadAllText(path);
            //把#CreateTime#替换成具体创建的时间
            annotationStr = annotationStr.Replace("#CreatTime#",
                System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            //把内容重新写入脚本
            File.WriteAllText(path, annotationStr);
            Debug.Log("创建脚本:" + path);
        }
    }
}
