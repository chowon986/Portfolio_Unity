using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeButton : Button
{
    public int CurLevel;

    public override void ClickButton()
    {
        if(tag == "InGame")
            SceneManager.LoadScene("InGameScene" + CurLevel);
        else
            SceneManager.LoadScene("Scene" + (CurLevel + 1));

    }
}
