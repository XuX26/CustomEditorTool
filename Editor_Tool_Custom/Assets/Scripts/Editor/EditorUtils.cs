using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EditorUtils
{
    public static GameManager FindGameManagerInScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid()) return null;

        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects) {
            if (!rootGameObject.activeInHierarchy) continue;
            GameManager gameManager = rootGameObject.GetComponent<GameManager>();
            if (null != gameManager) {
                return gameManager;
            }
        }

        return null;
    }

    public static GameData FindGameDataIntoProject()
    {
        string[] fileGuidsArr = AssetDatabase.FindAssets("t:" + typeof(GameData));
        if (fileGuidsArr.Length > 0) {
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidsArr[0]);
            return AssetDatabase.LoadAssetAtPath<GameData>(assetPath);
        } else {
            GameData gameData = ScriptableObject.CreateInstance<GameData>();
            AssetDatabase.CreateAsset(gameData, "Assets/GameData.asset");
            AssetDatabase.SaveAssets();
            return gameData;
        }
    }
}
