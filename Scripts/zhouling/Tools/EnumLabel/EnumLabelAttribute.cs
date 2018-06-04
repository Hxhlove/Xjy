// ------------------------------------------------------------------
// Title        :自定义扩展枚举在属性面板显示中文
// Author       :Leo
// Date         :2018.05.28
// Description  :使用的到两文件 EnumLabelDrawer | EnumLabelAttribute
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

using UnityEngine;

public class EnumLabelAttribute : HeaderAttribute
{
    public EnumLabelAttribute(string header) : base(header)
    {
    }
}