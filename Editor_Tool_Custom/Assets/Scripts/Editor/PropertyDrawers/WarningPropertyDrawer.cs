using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WarningAttribute))]
public class WarningPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        WarningAttribute warningAttribute = attribute as WarningAttribute;

        Rect helpBoxPosition = position;
        helpBoxPosition.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.HelpBox(helpBoxPosition, warningAttribute.Message, MessageType.Warning);

        Rect propertyPosition = position;
        propertyPosition.y += EditorGUIUtility.singleLineHeight;
        propertyPosition.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(propertyPosition, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2f;
    }
}
