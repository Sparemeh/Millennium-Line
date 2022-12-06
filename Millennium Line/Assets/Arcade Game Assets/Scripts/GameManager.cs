using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Sprite img;
    private int lives;
    private int score;

    public GameObject player;
    public GameObject spawner;

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        // load level
    }

    public void LevelComplete()
    {
        score += 1000;
        
        // load level
    }

    public void LevelFailed()
    {
        lives--;

        if (lives <= 0) {
            NewGame();
        } else {
            
            // reload current level
        }
    }
}
