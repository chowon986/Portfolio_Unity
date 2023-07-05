using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : Button
{
    public Button button;
    public bool isMute;

    public override void ClickButton()
    {
        base.ClickButton();

        button.gameObject.SetActive(isMute);
        isMute = !isMute;

        if(button.gameObject.name == "Btn_MusicOn")
        {
            if(isMute == true)
                SelectLevelSceneManager.Instance.audioSource.Pause();
            else
                SelectLevelSceneManager.Instance.audioSource.Play();
        }
    }
}
