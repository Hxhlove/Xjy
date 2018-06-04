using UnityEngine;


/// <summary>
/// 普通单例（需要继承此父类）
/// </summary>
public abstract class SingleInstance<T> where T : SingleInstance<T> , new()
{
    private static T instance = default(T);

    /// <summary>
    /// 单例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}

/// <summary>
/// MONO单例（需要继承此父类）,对象必须启动不能隐藏
/// </summary>
public class MonoSingleInstance<T> : MonoBehaviour where T : MonoSingleInstance<T>
{
    private static T instance;
    /// <summary>
    /// 单例,可以获取未启动的管理脚本
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] tt = FindObjectsOfType<T>();
                int Length = tt.Length;
                if (Length == 0)
                {
                    Debug.Log(string.Format("{0}单例管理对象必须有一个,场景内有没有对象", typeof(T)));
                }
                else
                if (Length > 1)
                {
                    Debug.Log(string.Format("{0}单例管理对象只能有一个,场景内有多个对象", typeof(T)));
                }
                else
                {
                    instance = tt[0];
                }
            }
            return instance;
        }
    }
}