// ------------------------------------------------------------------
// Title        :自定义扩展枚举在属性面板显示中文
// Author       :Leo
// Date         :2018.05.28
// Description  :使用的到两文件 EnumLabelDrawer | EnumLabelAttribute
// 全部数据结构不能使用List、字典、先进先出等集合,只能使用数组
/* 测试用例
using UnityEngine;

public class EnumTest : MonoBehaviour
{
    [EnumLabel("动画类型")]
    public EmAniType AniType;
}

public enum EmAniType
{
    [EnumLabel("待机")]
    Idle,

    [EnumLabel("走")]
    Walk,

    [EnumLabel("跑")]
    Run,

    [EnumLabel("攻击")]
    Atk,

    [EnumLabel("受击")]
    Hit,

    [EnumLabel("死亡")]
    Die
}
*/
// ------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumLabelAttribute))]
public class EnumLabelDrawer : PropertyDrawer
{
    private readonly List<string> m_displayNames = new List<string>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var att = (EnumLabelAttribute)attribute;
        Type type = null;
        if (property.propertyPath.Length > property.name.Length)
        {
            string[] _names = property.propertyPath.Split('.');
            type = GetType(0, _names, property.serializedObject.targetObject.GetType());
        }
        else
        {
            type = GetType(0, new string[] { property.name}, property.serializedObject.targetObject.GetType());
        }
        if (type != null)
        {
            foreach (var enumName in property.enumNames)
            {
                var enumfield = type.GetField(enumName);
                var hds = enumfield.GetCustomAttributes(typeof(HeaderAttribute), false);
                m_displayNames.Add(hds.Length <= 0 ? enumName : ((HeaderAttribute)hds[0]).header);
            }
            EditorGUI.BeginChangeCheck();
            var value = EditorGUI.Popup(position, att.header, property.enumValueIndex, m_displayNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                property.enumValueIndex = value;
            }
        }
        else
        {
            Debug.Log(string.Format("{0}对象EnumLabel枚举标签设置有错误", property.name));
        }
    }
    private Type GetType(int i, string[] names, Type type)
    {
        Type _type = null;
        FieldInfo fi = type.GetField(names[i]);
        if (fi != null)
        {
            _type = fi.FieldType;
            if (_type.BaseType.Name == "Array")
            {
                i = i + 3;
                string name = _type.Name.Substring(0, _type.Name.Length - 2);
                if (name == "UITextElement")
                {
                    _type = typeof(UITextElement);
                }
                else if (name == "UIRawImageElement")
                {
                    _type = typeof(UIRawImageElement);
                }
                else if (name == "UIImageElement")
                {
                    _type = typeof(UIImageElement);
                }
                else if (name == "UIObjectElement")
                {
                    _type = typeof(UIObjectElement);
                }
                else if (name == "ButtonStateElement")
                {
                    _type = typeof(ButtonStateElement);
                }
                else if (name == "UIButtonElement")
                {
                    _type = typeof(UIButtonElement);
                }
                else if (name == "UIGroupButtonElements")
                {
                    _type = typeof(UIGroupButtonElements);
                }
                else if (name == "ScreenUIElementNode")
                {
                    _type = typeof(ScreenUIElementNode);
                }
                else if (name == "ScreenUIGroupElement")
                {
                    _type = typeof(ScreenUIGroupElement);
                }
                else if (name == "UIEventData")
                {
                    _type = typeof(UIEventData);
                }
                else if (name == "UINodeData")
                {
                    _type = typeof(UINodeData);
                }
            }
            else
            {
                i++;
            }
            if (i < names.Length)
            {
                return GetType(i, names, _type);
            }
        }
        return _type;
    }
}