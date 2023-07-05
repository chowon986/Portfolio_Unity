using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : Button
{
    public SoundButton musicToggleButton;
    public SoundButton soundToggleButton;
    public SoundButton musicButton;
    public SoundButton soundButton;

    public override void ClickButton()
    {
        base.ClickButton();

        if (musicToggleButton != null &&
            soundToggleButton != null &&
            musicButton != null &&
            soundButton != null)
        {
            musicToggleButton.isMute = musicButton.isMute;
            soundToggleButton.isMute = soundButton.isMute;

            musicToggleButton.gameObject.SetActive(!musicToggleButton.isMute);
            soundToggleButton.gameObject.SetActive(!soundToggleButton.isMute);
        }
    }
}
