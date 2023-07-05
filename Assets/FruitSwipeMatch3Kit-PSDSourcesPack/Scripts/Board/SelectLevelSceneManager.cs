using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelSceneManager : MonoBehaviour
{
    private static SelectLevelSceneManager instance;
    private string prevSceneName;
    public AudioSource audioSource;
    public AudioClip clickClip;
    public AudioClip crashClip;
    public GameObject explosionParticlePrefab;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            if (prevSceneName == null)
            {
                prevSceneName = SceneManager.GetActiveScene().name;
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static SelectLevelSceneManager Instance
    {
        get { return instance; }
    }

    private void Update()
    {
        string curSceneName = SceneManager.GetActiveScene().name;

        if(curSceneName != prevSceneName)
        {
            if(prevSceneName == "InGameScene1")
            {
                if (curSceneName == "Scene1")
                    StartCoroutine(SetButtonActive());
            }

            prevSceneName = curSceneName;
        }


    }

    private IEnumerator SetButtonActive()
    {
        yield return new WaitForSeconds(1.0f);

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        yield return new WaitForSeconds(0.2f);

        Instantiate(explosionParticlePrefab, transform.position, explosionParticlePrefab.gameObject.transform.rotation);
        audioSource.PlayOneShot(crashClip, 1.0f);

        foreach (Transform transform in canvas.transform)
        {
            if (transform.gameObject.name == "LevelMap")
            {
                foreach (Transform levelMapTransform in transform)
                {
                    if (levelMapTransform.gameObject.name == "LevelMap0")
                    {
                        foreach (Transform levelMap0Transform in levelMapTransform)
                        {
                            if (levelMap0Transform.gameObject.name == "Btn_Pink1")
                            {
                                levelMap0Transform.gameObject.SetActive(false);
                            }
                            else if(levelMap0Transform.gameObject.name == "Btn_Blue1")
                            {
                                levelMap0Transform.gameObject.SetActive(true);
                            }
                            else if (levelMap0Transform.gameObject.name == "Btn_Grey2")
                            {
                                levelMap0Transform.gameObject.SetActive(false);
                            }
                            else if (levelMap0Transform.gameObject.name == "Btn_Pink2")
                            {
                                levelMap0Transform.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }

}
