#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils_MenuItems : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("AirportMadness/PlayFromStart", false, -1000)]
    private static void StartScene0()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(0);
            EditorSceneManager.OpenScene(scenePath);
            EditorApplication.isPlaying = true;
        }
    }
#endif
}

