using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class Path2DGlobalEditor
{
    static Path2DGlobalEditor()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView obj)
    {
        //TODO: Do not Find all path2d directly in sceneGUI callback. Reload this when scenes changed or hierarchy changed
        Path2D[] paths2DArr = _FindPaths2DInScene();

        foreach (Path2D path2D in paths2DArr) {
            Handles.color = path2D.color;
            for (int i = 0; i < path2D.points.Length; ++i) {
                Vector2 point = path2D.points[i];
                Handles.DrawSolidDisc(point, Vector3.forward, 0.1f);
                Handles.Label(point + new Vector2(0f, -0.1f), (i + 1).ToString(), EditorStyles.boldLabel);
                if (i > 0) {
                    Vector2 previousPoint = path2D.points[i - 1];
                    Handles.DrawLine(previousPoint, point);
                }
            }
        }
    }

    public static Path2D[] _FindPaths2DInScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid()) return null;

        List<Path2D> path2DList = new List<Path2D>();

        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects) {
            path2DList.AddRange(rootGameObject.GetComponentsInChildren<Path2D>(true));
        }

        return path2DList.ToArray();
    }
}
