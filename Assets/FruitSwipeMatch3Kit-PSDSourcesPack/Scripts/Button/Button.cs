using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public SoundButton settingSoundButton;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Scene0")
        {
            Canvas canvas = GameObject.Find("Canvas_Mine").GetComponent<Canvas>();

            if (canvas != null)
            {
                foreach (Transform transform in canvas.transform)
                {
                    if (transform.gameObject.name == "Btn_SoundOn")
                        settingSoundButton = transform.gameObject.GetComponent<SoundButton>();
                }
            }
        }
    }

    public virtual void ClickButton()
    {
        if(settingSoundButton != null && SceneManager.GetActiveScene().name == "Scene0")
        {
            if (settingSoundButton.isMute == false)
                SelectLevelSceneManager.Instance.audioSource.PlayOneShot(SelectLevelSceneManager.Instance.clickClip);
        }
        else if (SceneManager.GetActiveScene().name == "Scene1")
                SelectLevelSceneManager.Instance.audioSource.PlayOneShot(SelectLevelSceneManager.Instance.clickClip);
    }
}
