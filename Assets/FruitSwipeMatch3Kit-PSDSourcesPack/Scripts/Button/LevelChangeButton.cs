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
    public Canvas clearCanvas;

    public override void ClickButton()
    {
        base.ClickButton();

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
        if (panelAnim != null)
        {
            AnimatorStateInfo stateInfo = panelAnim.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Panel Fade out"))
            {
                if (stateInfo.normalizedTime >= 1.0f)
                {
                    string sceneName = SceneManager.GetActiveScene().name;

                    if (sceneName == "InGameScene1")
                    {
                        SceneManager.LoadScene("Scene" + CurLevel);
                    }

                    else
                        SceneManager.LoadScene("InGameScene" + CurLevel);
                }
            }
        }
    }
}
