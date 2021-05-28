using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrefabSpawner))]
public class PrefabSpawnerInspector : Editor
{
    private void OnEnable()
    {
        PrefabSpawner spawner = target as PrefabSpawner;
        if (null == spawner) return;
        if (null == spawner.prefab) return;

        //Remove previous children (if exists)
        while (spawner.transform.childCount > 0) {
            DestroyImmediate(spawner.transform.GetChild(0).gameObject);
        }

        GameObject prefabInstance = Instantiate(spawner.prefab, spawner.transform);
        prefabInstance.hideFlags = HideFlags.HideAndDontSave;
    }

    private void OnDisable()
    {
        PrefabSpawner spawner = target as PrefabSpawner;
        if (null == spawner) return;

        //Remove previous children (if exists)
        while (spawner.transform.childCount > 0) {
            DestroyImmediate(spawner.transform.GetChild(0).gameObject);
        }
    }
}
