using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : Button
{
    public Canvas canvas;
    public int CurLevel;

    public override void ClickButton()
    {
        base.ClickButton();
        foreach (Transform child in canvas.transform)
        {
            if (child.name == "TextPink_CurLevel")
            {
                Text CurLevelText = child.GetComponent<Text>();
                CurLevelText.text = CurLevel.ToString();
            }
            else if(child.name == "Btn_Start")
            {
                LevelChangeButton Button = child.GetComponent<LevelChangeButton>();
                Button.CurLevel = CurLevel;
            }

            child.gameObject.SetActive(true);
        }

        if(tag == "BlueButton")
        {

        }

        else if (tag == "PinkButton" ||
                tag == "BlueButton")
        {
            // 레벨 이동 창 
        }
    }
}
