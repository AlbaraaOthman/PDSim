#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class PDSimSceneBaker
{
    [MenuItem("Tools/PDSim/Bake Runtime Objects (Safe)")]
    public static void Bake()
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        GameObject root = new GameObject("PDSim_Baked");
        int count = 0;

        foreach (GameObject go in allObjects)
        {
            // Skip editor-only scenes or objects not in the active scene
            if (!go.scene.IsValid() || !go.scene.isLoaded)
                continue;

            // Skip our own root object
            if (go.name == "PDSim_Baked")
                continue;

            // Skip cameras
            if (go.GetComponent<Camera>())
                continue;

            // Skip lights
            if (go.GetComponent<Light>())
                continue;

            // Skip Unity Visual Scripting internal singletons
            if (go.GetComponent<Unity.VisualScripting.SceneVariables>() != null)
                continue;

            if (go.GetComponent<Unity.VisualScripting.ScriptMachine>() != null)
                continue;

            if (go.GetComponent<Unity.VisualScripting.StateMachine>() != null)
                continue;

            // Skip any hidden/internals
            if (go.hideFlags != HideFlags.None)
                continue;

            // Now safely clone the object
            GameObject clone = Object.Instantiate(go, root.transform);
            clone.name = go.name;

            if (clone.name.EndsWith("(Clone)"))
                clone.name = clone.name.Replace("(Clone)", "");

            count++;
        }

        Debug.Log($"PDSim: Baked {count} objects safely into the scene.");
    }
}
#endif