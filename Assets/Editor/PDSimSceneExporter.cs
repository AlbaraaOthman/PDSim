#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;

public static class PDSimSceneExporter
{
    private static Scene tempScene;
    private static string savePath;

    [MenuItem("Tools/PDSim/Export Runtime Scene As New Scene")]
    public static void Export()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("You must be in Play Mode to export the PDSim scene.");
            return;
        }

        savePath = EditorUtility.SaveFilePanelInProject(
            "Save Exported Scene",
            "PDSim_ExportedScene",
            "unity",
            "Choose where the baked scene will be saved."
        );

        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogWarning("PDSim Export cancelled.");
            return;
        }

        tempScene = SceneManager.CreateScene("PDSim_Exported_Temp");

        int count = 0;

        foreach (GameObject go in Object.FindObjectsOfType<GameObject>())
        {
            if (!go.scene.IsValid() || !go.scene.isLoaded)
                continue;

            if (go.GetComponent<Camera>() || go.GetComponent<Light>())
                continue;

            if (go.GetComponent<Unity.VisualScripting.SceneVariables>() ||
                go.GetComponent<Unity.VisualScripting.ScriptMachine>() ||
                go.GetComponent<Unity.VisualScripting.StateMachine>())
                continue;

            if (go.hideFlags != HideFlags.None)
                continue;

            GameObject clone = Object.Instantiate(go);
            SceneManager.MoveGameObjectToScene(clone, tempScene);
            RemovePDSimComponents(clone);

            count++;
        }

        Debug.Log($"PDSim Exported Scene staged. Objects baked: {count}");
        Debug.Log("Exit Play Mode to save the scene.");

        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            bool saved = EditorSceneManager.SaveScene(tempScene, savePath);

            if (!saved)
            {
                Debug.LogError("❌ Failed to save PDSim exported scene!");
            }
            else
            {
                string fullPath = Path.GetFullPath(savePath);
                Debug.Log("✅ PDSim Exported Scene saved at (absolute path):\n" + fullPath);

                // Ping the asset in the Project window
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(savePath);
                if (asset != null)
                {
                    EditorGUIUtility.PingObject(asset);
                    Selection.activeObject = asset;
                }
            }

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }
    }

    private static void RemovePDSimComponents(GameObject obj)
    {
        var components = obj.GetComponentsInChildren<Component>(true);

        foreach (var comp in components)
        {
            if (comp == null) continue;

            if (comp.GetType().Namespace != null &&
                comp.GetType().Namespace.StartsWith("PDSim"))
            {
                Object.DestroyImmediate(comp);
            }
        }
    }
}
#endif