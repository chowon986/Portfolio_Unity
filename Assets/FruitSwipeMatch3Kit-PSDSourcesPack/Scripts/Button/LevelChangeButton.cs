using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChangeButton : Button
{
    public int CurLevel;
    public Animator panelAnim;

    public override void ClickButton()
    {
        if (tag == "InGame")
        {
            if(panelAnim != null)
            {
                panelAnim.SetBool("isFadeOut", true);
            }
        }
        else
            SceneManager.LoadScene("Scene" + (CurLevel + 1));

    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = panelAnim.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Panel Fade out"))
        {
            if(stateInfo.normalizedTime >= 1.0f)
            {
                SceneManager.LoadScene("InGameScene" + CurLevel);
            }
        }
    }
}
