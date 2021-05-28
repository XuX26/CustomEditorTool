using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomSliderAttribute))]
public class CustomSliderPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CustomSliderAttribute sliderAttribute = attribute as CustomSliderAttribute;
        EditorGUI.IntSlider(position, property, sliderAttribute.Min, sliderAttribute.Max);
    }
}
