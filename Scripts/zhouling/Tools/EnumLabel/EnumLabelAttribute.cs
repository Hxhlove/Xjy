// ------------------------------------------------------------------
// Title        :�Զ�����չö�������������ʾ����
// Author       :Leo
// Date         :2018.05.28
// Description  :ʹ�õĵ����ļ� EnumLabelDrawer | EnumLabelAttribute
/* ��������
using UnityEngine;

public class EnumTest : MonoBehaviour
{
    [EnumLabel("��������")]
    public EmAniType AniType;
}

public enum EmAniType
{
    [EnumLabel("����")]
    Idle,

    [EnumLabel("��")]
    Walk,

    [EnumLabel("��")]
    Run,

    [EnumLabel("����")]
    Atk,

    [EnumLabel("�ܻ�")]
    Hit,

    [EnumLabel("����")]
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