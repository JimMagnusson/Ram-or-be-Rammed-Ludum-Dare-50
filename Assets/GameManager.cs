using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private UIManager uIManager;

    private bool paused = false;
    private AudioListener audioListener;

    private void Start()
    {
        audioListener = FindObjectOfType<AudioListener>();
        uIManager = FindObjectOfType<UIManager>();
        if (PlayerPrefs.GetInt("HavePlayed", 0) == 0 && SceneManager.GetActiveScene().buildIndex == 0)
        {
            Time.timeScale = 0;
            uIManager.ToggleStartGameImage(true);
            PlayerPrefs.SetInt("HavePlayed", 1);
        }
    }

    private void Update()
    {
        if(uIManager.StartGameImageIsActive() && Input.anyKey)
        {
            uIManager.ToggleStartGameImage(false);
            Time.timeScale = 1;
        } 

        else if(Input.GetKeyDown(KeyCode.P) && !uIManager.StartGameImageIsActive())
        {
            if(!paused)
            {
                uIManager.TogglePauseImage(true);
                paused = true;
                // Pause game
                Time.timeScale = 0;
            }
            else
            {
                uIManager.TogglePauseImage(false);
                paused = false;
                Time.timeScale = 1;
            }
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            if(audioListener.enabled)
            {
                audioListener.enabled = false;
            }
            else { audioListener.enabled = false; }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("HavePlayed");
    }
}
