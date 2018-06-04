using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;

public class WriteJsonToTxt : MonoBehaviour
{
    string fileName;
    string filePath;

    public class UIData
    {
        //UI对象路径
        public string PrefabPath { get; set; }
        //UI正常图片
        public string NormalImgPath { get; set; }
        //UI按下图片
        public string PressedImgPath { get; set; }
        //UI序号
        public string ID { get; set; }
        //UI文本内容
        public string ObjText { get; set; }
        //UI间隔
        public Vector2 ObjSpacing { get; set; }
        //UI位置
        public Vector2 ObjPosition { get; set; }
        //按钮事件（创建UI/发送事件）
        public string OnClickStr { get; set; }
    }

    public class UIDataList
    {
        public Dictionary<string, string> dic = new Dictionary<string, string>();
    }
    UIDataList uidataList = new UIDataList();

    private void Start()
    {
        List<UIData> list = new List<global::WriteJsonToTxt.UIData>();

        list = SetNewData();
        ToSaveData(list[0]);

    }

    void ToSaveData(UIData data)
    {
        fileName = "UIJsonData1.json";
        filePath = Application.dataPath + "/Scripts/PXJ/";

        if (!File.Exists(filePath + fileName))
        {
            uidataList.dic.Add("PrefabPath", data.PrefabPath);
            uidataList.dic.Add("NormalImgPath", data.NormalImgPath);
            uidataList.dic.Add("PressedImgPath", data.PressedImgPath);
            uidataList.dic.Add("ID", data.ID);
            uidataList.dic.Add("ObjText", data.ObjText);
            uidataList.dic.Add("ObjSpacing", data.ObjSpacing.ToString());
            uidataList.dic.Add("ObjPosition", data.ObjPosition.ToString());
            uidataList.dic.Add("OnClickStr", data.OnClickStr);

        }
        else
        {
            uidataList.dic["PrefabPath"] = data.PrefabPath;
            uidataList.dic["NormalImgPath"] = data.NormalImgPath;
            uidataList.dic["PressedImgPath"] = data.PressedImgPath;
            uidataList.dic["ID"] = data.ID;
            uidataList.dic["ObjText"] = data.ObjText;
            uidataList.dic["ObjSpacing"] = data.ObjSpacing.ToString();
            uidataList.dic["ObjPosition"] = data.ObjPosition.ToString();
            uidataList.dic["OnClickStr"] = data.OnClickStr;

        }
        //找到当前路径
        FileInfo file = new FileInfo(filePath + fileName);
        //判断有没有文件，有则打开文件，，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //ToJson接口将你的列表类传进去，，并自动转换为string类型
        //  string json = JsonMapper.ToJson(personList.dictionary);
        string json =JsonMapper .ToJson(uidataList.dic);        //将转换好的字符串存进文件，
        sw.WriteLine(json);
        //注意释放资源
        sw.Close();
        sw.Dispose();

        AssetDatabase.Refresh();

    }
    List<UIData> SetNewData()
    {
        List<UIData> list = new List<UIData>();

        UIData bn1 = new UIData();
        bn1.PrefabPath = "UIRes/Prefab/MainPanel/Level1Bn";
        bn1.NormalImgPath = "UIRes/Sprite/MainPanel/level1Bn0";
        bn1.PressedImgPath = "UIRes/Sprite/MainPanel/level1Bn1";
        bn1.ID = "0";
        bn1.ObjText = "区位规划";
        bn1.ObjSpacing = Vector2.zero;
        bn1.ObjPosition = Vector2.zero;
        bn1.OnClickStr = "UIJsonData1.json";
        list.Add(bn1);

        UIData bn2 = new UIData();
        bn2.PrefabPath = "UIRes/Prefab/MainPanel/Level2Bn";
        bn2.NormalImgPath = "UIRes/Sprite/MainPanel/level2Bn0";
        bn2.PressedImgPath = "UIRes/Sprite/MainPanel/level2Bn1";
        bn2.ID = "1";
        bn2.ObjText = "区位沙盘";
        bn2.ObjSpacing = Vector2.zero;
        bn2.ObjPosition = Vector2.zero;
        bn2.OnClickStr = "UIJsonData1.json";
        list.Add(bn2);

        return list;
    }
}
