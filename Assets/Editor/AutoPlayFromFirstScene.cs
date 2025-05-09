using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

[InitializeOnLoad]
public static class AutoPlayFromFirstScene
{
    private const string LastScenePathKey = "AutoPlayFromFirstScene_LastScenePath";
    private const string FirstScenePath = "Assets/BeverageKingdom/Scenes/HomeScene.unity"; // 👉 Đổi path scene đầu tiên ở đây

    static AutoPlayFromFirstScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string currentScenePath = SceneManager.GetActiveScene().path;
            EditorPrefs.SetString(LastScenePathKey, currentScenePath);

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                if (File.Exists(FirstScenePath))
                {
                    EditorSceneManager.OpenScene(FirstScenePath);
                }
                else
                {
                    Debug.LogError("First scene path not found: " + FirstScenePath);
                }
            }
            else
            {
                // User canceled save
                EditorApplication.isPlaying = false;
            }
        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            string lastScenePath = EditorPrefs.GetString(LastScenePathKey, "");
            if (!string.IsNullOrEmpty(lastScenePath) && File.Exists(lastScenePath))
            {
                EditorSceneManager.OpenScene(lastScenePath);
            }
        }
    }
}