using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameData))]
public class GameDataInspector : Editor
{
    public const int MAX_PLAYERS = 4;

    private SerializedProperty nbPlayersProperty = null;
    private GUIContent nbPlayersGUIContent = null;

    private SerializedProperty playersSpeedProperty = null;
    private GUIContent playersSpeedGUIContent = null;
    private GUIContent[] playerSpeedGUIContentArr = null;

    private void OnEnable()
    {
        nbPlayersProperty = serializedObject.FindProperty("_nbPlayers");
        nbPlayersGUIContent = new GUIContent(nbPlayersProperty.displayName);

        playersSpeedProperty = serializedObject.FindProperty("playersSpeed");
        if (playersSpeedProperty.arraySize == 0) {
            serializedObject.Update();
            playersSpeedProperty.arraySize = MAX_PLAYERS;
            serializedObject.ApplyModifiedProperties();
        }

        playersSpeedGUIContent = new GUIContent(playersSpeedProperty.displayName);
        playerSpeedGUIContentArr = new GUIContent[MAX_PLAYERS];
        for (int i = 0; i < MAX_PLAYERS; ++i) {
            playerSpeedGUIContentArr[i] = new GUIContent("Player" + (i + 1) + " Speed");
        }
    }

    private void OnDisable()
    {
        nbPlayersProperty = null;
        nbPlayersGUIContent = null;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Nb Players
        //Default property GUI
        EditorGUILayout.PropertyField(nbPlayersProperty);
        //Slider
        //nbPlayersProperty.intValue = EditorGUILayout.IntSlider(nbPlayersGUIContent, nbPlayersProperty.intValue, 1, 4);

        //Players speed
        //Default property GUI
        //EditorGUILayout.PropertyField(playersSpeedProperty);
        //Int fields according to nb of players
        for (int i = 0; i < nbPlayersProperty.intValue; ++i) {
            SerializedProperty playerSpeedProperty = playersSpeedProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(playerSpeedProperty, playerSpeedGUIContentArr[i]);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("infos"));

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Edit GameManager")) {
            Selection.activeObject = _FindGameManagerInScene();
        }
    }

    private GameManager _FindGameManagerInScene()
    {
        return FindObjectOfType<GameManager>();
    }
}
