using UnityEditor;

public class RemoveMissingScriptsUtility : Editor {

    [MenuItem("Edit/Remove Missing Scripts")]
    private static void RemoveMissingScripts() {
        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(Selection.gameObjects[i]);
        }
    }
}