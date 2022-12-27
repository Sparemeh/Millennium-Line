using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsPanel;
    public static bool isPaused;
    public bool optionsOpened;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            } else if(!optionsOpened) {
                PauseGame();
            }
        }
    }

    public void PauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame(){
        pauseMenu.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        optionsOpened = false;
    }

    public void OpenOptions(){
        optionsOpened = true;
    }

    public void QuitGame(){
        Application.Quit();
    }
}
