using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolboxWindow : EditorWindow
{

    [MenuItem("Toolbox/Toolbox Window")]
    [MenuItem("Window/Toolbox/Toolbox Window")]
    static void InitWindow()
    {
        Debug.Log("Init Window");
        ToolboxWindow window = GetWindow<ToolboxWindow>();
        window.Show();
        window.titleContent = new GUIContent("Toolbox");
    }

    private void OnGUI()
    {
        //Focus Object In Scene (without changing selection)
        if (GUILayout.Button("Focus GameManager")) {
            GameManager gameManager = EditorUtils.FindGameManagerInScene();
            if (null != gameManager) {
                Object lastSelection = Selection.activeObject;
                Selection.activeObject = gameManager;
                SceneView.lastActiveSceneView.FrameSelected();
                Selection.activeObject = lastSelection;
                EditorGUIUtility.PingObject(gameManager);
            }
        }

        //Select Object In Scene
        if (GUILayout.Button("Select GameManager")) {
            GameManager gameManager = EditorUtils.FindGameManagerInScene();
            if (null != gameManager) {
                Selection.activeObject = gameManager;
                SceneView.lastActiveSceneView.FrameSelected();
                EditorGUIUtility.PingObject(gameManager);
            }
        }

        //Select Object in Project
        if (GUILayout.Button("Select GameData")) {
            GameData gameData = EditorUtils.FindGameDataIntoProject();
            if (null != gameData) {
                Selection.activeObject = gameData;
                EditorGUIUtility.PingObject(gameData);
            }
        }

        //Multiple Selections
        if (GUILayout.Button("Select All GameObjects In Scene")) {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.IsValid()) {
                Selection.objects = scene.GetRootGameObjects();
            }
        }
    }
}
