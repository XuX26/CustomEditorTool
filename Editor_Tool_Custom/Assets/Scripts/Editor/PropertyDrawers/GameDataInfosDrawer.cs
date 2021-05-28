using System.Xml;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameDataInfos))]
public class GameDataInfosDrawer : PropertyDrawer
{
    public const int MARGINS = 5;
    public static readonly Color BACKGROUND_COLOR = new Color(0.5f, 0f, 0f);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Background Rect
        Rect backgroundRect = position;
        EditorGUI.DrawRect(backgroundRect, BACKGROUND_COLOR);

        //Label
        Rect labelPosition = position;
        labelPosition.height = EditorGUIUtility.singleLineHeight;
        labelPosition.y += MARGINS;
        EditorGUI.LabelField(labelPosition, "Infos", EditorStyles.boldLabel);

        //Last Edit Time
        Rect lastEditTimePosition = position;
        lastEditTimePosition.height = EditorGUIUtility.singleLineHeight;
        lastEditTimePosition.y += MARGINS + EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(lastEditTimePosition, property.FindPropertyRelative("lastEditTime"));

        //Version
        Rect versionPosition = position;
        versionPosition.height = EditorGUIUtility.singleLineHeight;
        versionPosition.y += MARGINS + EditorGUIUtility.singleLineHeight * 2;
        EditorGUI.PropertyField(versionPosition, property.FindPropertyRelative("version"));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (MARGINS * 2) + EditorGUIUtility.singleLineHeight * 3;
    }
}
