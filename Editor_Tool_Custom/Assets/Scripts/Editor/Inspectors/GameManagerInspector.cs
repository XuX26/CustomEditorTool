using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor
{
    private static readonly string[] EXCLUDED_PROPERTIES = new string[]
    {
        "m_Script",
        "gameData"
    };

    private void OnEnable()
    {
        SerializedProperty gameDataProperty = serializedObject.FindProperty("gameData");
        if (null == gameDataProperty.objectReferenceValue) {
            serializedObject.Update();
            gameDataProperty.objectReferenceValue = EditorUtils.FindGameDataIntoProject();
            serializedObject.ApplyModifiedProperties();
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameData"));
        EditorGUI.EndDisabledGroup();

        //Standard Editor Space
        EditorGUILayout.Space();

        if (GUILayout.Button("Edit GameData")) {
            Selection.activeObject = serializedObject.FindProperty("gameData").objectReferenceValue;
        }

        //Custom Space
        GUILayout.Space(20);

        DrawPropertiesExcluding(serializedObject, EXCLUDED_PROPERTIES);
    }
}
