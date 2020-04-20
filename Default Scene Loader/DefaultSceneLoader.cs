using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Comp3Utils.DefaultSceneLoader
{
    [InitializeOnLoad]
    public static class DefaultSceneLoader
    {
        static DefaultSceneLoader()
        {
            EditorApplication.playModeStateChanged += ToggleScenes;
        }

        private static void ToggleScenes(PlayModeStateChange state)
        {
            if (EditorPrefs.GetBool(DefaultScenePrefs.isActive))
            {
                string defaultScene = SceneUtility.GetScenePathByBuildIndex(EditorPrefs.GetInt(DefaultScenePrefs.defaultSceneIndex));

                string currentScenePath = SceneManager.GetActiveScene().path;

                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    EditorPrefs.SetString(DefaultScenePrefs.returnPath, currentScenePath);

                    if (currentScenePath != defaultScene)
                    {
                        if (EditorPrefs.GetBool(DefaultScenePrefs.autoSave))
                            EditorSceneManager.SaveOpenScenes();
                        else
                            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

                        EditorSceneManager.OpenScene(defaultScene);
                    }
                }

                if (state == PlayModeStateChange.EnteredEditMode)
                {
                    string returnPath = EditorPrefs.GetString(DefaultScenePrefs.returnPath);

                    if (currentScenePath != returnPath)
                        EditorSceneManager.OpenScene(returnPath);
                }
            }
        }
    }
}