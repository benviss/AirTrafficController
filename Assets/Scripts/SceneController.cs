using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public event Action<Scene, LoadSceneMode> OnSceneLoaded;

    private void Start()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded?.Invoke(scene, mode);
    }

    public void LoadScene(string sceneToLoad, LoadSceneMode mode, string sceneToUnload = "")
    {
        if (!string.IsNullOrEmpty(sceneToUnload))
            SceneManager.UnloadSceneAsync(sceneToUnload);

        SceneManager.LoadSceneAsync(sceneToLoad, mode);
    }
}
