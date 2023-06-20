using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButton : ResetButton
{
    public Button button;
    public Canvas settingCanvas;
    public bool isOn;

    public override void ClickButton()
    {
        button.gameObject.SetActive(isOn);
        isOn = !isOn;

        if (settingCanvas != null)
        {
            foreach (Transform child in settingCanvas.transform)
            {
                child.gameObject.SetActive(isOn);
            }

            if (isOn == true)
            {
                musicToggleButton.isMute = musicButton.isMute;
                soundToggleButton.isMute = soundButton.isMute;

                musicToggleButton.gameObject.SetActive(!musicToggleButton.isMute);
                soundToggleButton.gameObject.SetActive(!soundToggleButton.isMute);
            }
            else
            {
                musicButton.isMute = musicToggleButton.isMute;
                soundButton.isMute = soundToggleButton.isMute;

                musicButton.gameObject.SetActive(!musicButton.isMute);
                soundButton.gameObject.SetActive(!soundButton.isMute);
            }

        }
    }
}
