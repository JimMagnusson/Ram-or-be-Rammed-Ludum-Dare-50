using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    UIManager uIManager;
    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadSceneWithBuildIndex(int buildindex)
    {
        SceneManager.LoadScene(buildindex);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(uIManager.PauseImageScreenIsActive()) 
            {
                Time.timeScale = 1;
            }
            ReloadScene();
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
        }
        //&& Input.GetKeyDown(KeyCode.Return
        if ((uIManager.RetryImageScreenIsActive() || uIManager.WinLevelScreenIsActive()) && Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextScene();
        }
    }
}
