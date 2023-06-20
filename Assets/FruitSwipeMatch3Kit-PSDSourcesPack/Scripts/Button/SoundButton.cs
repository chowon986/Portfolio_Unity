using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : Button
{
    public Button button;
    public bool isMute;

    public override void ClickButton()
    {
        button.gameObject.SetActive(isMute);
        isMute = !isMute;
    }
}
