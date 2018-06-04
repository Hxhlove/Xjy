using UnityEngine;
using System.Collections;

public class FrameAniByNewTest : MonoBehaviour
{
    public GameObject go;
    FrameAnimationUseByNewTool tool1;//  没有音频的动画
    // Use this for initialization
    void Start()
    {
        tool1 = new FrameAnimationUseByNewTool("UIRes/An_Bni", go);
    }

    bool isShow = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isShow = true;
            tool1.FrameAniPlay(-1, true);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            isShow = false;
        }
    }
    void FixedUpdate()
    {
        if (isShow)
            tool1.FixedUpdate();
    }
}
