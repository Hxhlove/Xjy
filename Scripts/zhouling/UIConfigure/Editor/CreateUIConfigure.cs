// ------------------------------------------------------------------
// Title        :创建UI相关配置菜单管理
// Author       :Leo
// Date         :2018.05.22
// Description  :用于编辑器创建UI数据菜单
// ------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

public class CreateUIConfigure
{
    [MenuItem("Assets/创建屏幕UI数据")]
    public static void CreateScreenUIConfigureData()
    {
        ScreenUIConfigureData data = ScriptableObject.CreateInstance<ScreenUIConfigureData>();
        //获取创建时的路径
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New屏幕UI数据" + ".asset");
        AssetDatabase.CreateAsset(data, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }
    /*
    [MenuItem("Assets/创建UI事件数据")]
    public static void CreateUIEventData()
    {
        UIEventData data = ScriptableObject.CreateInstance<UIEventData>();
        data.Describe = "NewUI事件数据";
        //获取创建时的路径
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/NewUI事件数据" + ".asset");
        AssetDatabase.CreateAsset(data, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
    }
    */
}