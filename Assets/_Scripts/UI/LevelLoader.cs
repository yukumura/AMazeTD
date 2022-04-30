using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private float delay = 2;
    private string NewLevel = "Game";
    private string MainMenu = "MainMenu";
    void Start()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneAfterDelay(float delay, string scene)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    public void StartNewGame()
    {
        StartCoroutine(LoadSceneAfterDelay(delay, NewLevel));
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 0;
        StartCoroutine(LoadSceneAfterDelay(0, MainMenu));
    }

    public void StartNewGameImmediatly()
    {        
        StartCoroutine(LoadSceneAfterDelay(0, NewLevel));
    }
}
