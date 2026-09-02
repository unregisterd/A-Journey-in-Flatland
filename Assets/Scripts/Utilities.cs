using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class Utilities
{
    public static int PlayerDeaths = 0;

    public static string UpdateDeathCount(ref int curDeathTime)
    {
        PlayerDeaths += curDeathTime + 1;
        return "Death Times: " + PlayerDeaths;
    }

    public static string ShowDeathCount()
    {
        return "Death Times: " + PlayerDeaths;
    }
    
    public static bool RestartLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1.0f;
        return true;
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public static void InitialGame()
    {
        SceneManager.LoadScene(1);
    }

    public static void BackToMenu()
    {
        SceneManager.LoadScene(0);
        PlayerDeaths = 0;
    }
}
