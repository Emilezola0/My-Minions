using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDSpawner : MonoBehaviour
{
    public string hudSceneName = "HUD"; // Change this to the actual name of your HUD scene

    void Start()
    {
        LoadHUDScene();
    }

    void LoadHUDScene()
    {
        // Check if the HUD scene is not already loaded
        if (!SceneManager.GetSceneByName(hudSceneName).isLoaded)
        {
            // Load the HUD scene additively
            SceneManager.LoadScene(hudSceneName, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogWarning("HUD scene is already loaded.");
        }
    }
}