using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeButton : Button
{
    int CurLevel;

    public override void ClickButton() 
    {
        SceneManager.LoadScene("Scene" + (CurLevel + 1));
    }
}
