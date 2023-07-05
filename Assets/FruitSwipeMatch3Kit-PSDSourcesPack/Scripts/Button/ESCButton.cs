using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCButton : Button
{
    public Canvas canvas;
    public SoundButton musicToggleButton;
    public SoundButton soundToggleButton;
    public SoundButton musicButton;
    public SoundButton soundButton;
    public SettingButton button;

    public override void ClickButton()
    {
        base.ClickButton();

        foreach (Transform child in canvas.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (musicToggleButton != null)
        {
            musicButton.isMute = musicToggleButton.isMute;
            soundButton.isMute = soundToggleButton.isMute;

            musicButton.gameObject.SetActive(!musicButton.isMute);
            soundButton.gameObject.SetActive(!soundButton.isMute);

            if (musicButton.isMute == true)
                SelectLevelSceneManager.Instance.audioSource.Pause();
            else
                SelectLevelSceneManager.Instance.audioSource.Play();

            button.gameObject.SetActive(button.isOn);
            button.isOn = !button.isOn;
        }
    }
}
