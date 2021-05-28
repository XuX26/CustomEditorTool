using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Path2D))]
public class Path2DInspector : Editor
{
    private ReorderableList _pointsReorderableList = null;

    private void OnEnable()
    {
        _pointsReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("points"));
        _pointsReorderableList.drawHeaderCallback += _OnListDrawHeader;
        _pointsReorderableList.drawElementCallback += _OnListDrawElement;
        _pointsReorderableList.onRemoveCallback += _OnListRemoveElement;
    }

    private void _OnListRemoveElement(ReorderableList list)
    {
        if (EditorUtility.DisplayDialog("Remove Point", "Do you really want to remove this point?", "Yes", "No")) {
            serializedObject.FindProperty("points").DeleteArrayElementAtIndex(list.index);
        }
    }

    private void _OnListDrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Points List");
    }

    private void _OnListDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty pointProperty = serializedObject.FindProperty("points").GetArrayElementAtIndex(index);
        pointProperty.vector2Value = EditorGUI.Vector2Field(rect, "Point" + index, pointProperty.vector2Value);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, new string[] { "points" });

        _pointsReorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        Path2D path2D = target as Path2D;
        if (null == path2D) return;

        //Move points
        for (int i = 0; i < path2D.points.Length; ++i) {
            EditorGUI.BeginChangeCheck();
            Vector2 point = path2D.points[i];
            point = Handles.PositionHandle(point, Quaternion.identity);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(path2D, "Edit point");
                path2D.points[i] = point;
            }
        }

        //Draw points
        //Handles.color = Color.yellow;
        //for (int i = 0; i < path2D.points.Length; ++i) {
        //    Vector2 point = path2D.points[i];
        //    Handles.DrawSolidDisc(point, Vector3.forward, 0.1f);
        //    Handles.Label(point + new Vector2(0f, -0.1f), (i + 1).ToString(), EditorStyles.boldLabel);
        //    if (i > 0) {
        //        Vector2 previousPoint = path2D.points[i - 1];
        //        Handles.DrawLine(previousPoint, point);
        //    }
        //}

        //Disable Scene Interactions
        if (Event.current.type == EventType.Layout) {
            HandleUtility.AddDefaultControl(0);
        }
    }
}
