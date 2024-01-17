using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public GameObject PauseMenuUI;
    public Metalogic metalogic;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
            if (gameIsPaused){
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume(){
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause(){
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        //gameIsPaused = false;
        metalogic.triggerMainMenu();
    }

    public void QuitGame(){
        Time.timeScale = 1f;
        metalogic.triggerQuit();
    }
}
