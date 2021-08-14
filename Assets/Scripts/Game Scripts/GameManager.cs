using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public string currentScene;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject); // Saves Manager.

        SceneManager.activeSceneChanged += ChangedActiveScene; // Subscribes to scene manager.

        currentScene = SceneManager.GetActiveScene().name; // Saves current scene name.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        currentScene = next.name; // Saves new scene name.

        switch (currentScene)
        {
            case "Game Over":
                WaitForInput();
                break;
            case "Battle": // Settings for battle.
                Time.fixedDeltaTime = 0.02f;
                QualitySettings.antiAliasing = 4;
                break;
            case "Overworld": // Settings for Overworld.
                Time.fixedDeltaTime = 0.01f;
                QualitySettings.antiAliasing = 0;
                break;
            default:
                break;
        }
    }

    public void GameOver()  // In case of player loss.
    {
        SceneManager.LoadScene("Game Over");
    }

    public void WinBattle()  // In case of player loss.
    {
        SceneManager.LoadScene("Overworld");
    }

    public void Continue()
    {
        SceneManager.LoadScene("Overworld");
    }

    public async void WaitForInput() // Task which waits for an input.
    {
        while (!Input.anyKey)
        {
            await Task.Delay(25);
        }

        Continue();
    }

}
