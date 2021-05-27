using UnityEditor;
using UnityEngine;

public static class CameraMenuItems
{
    [MenuItem("Toolbox/Camera/Select Main Camera", true)]
    [MenuItem("Window/Toolbox/Camera/Select Main Camera", true)]
    static bool CheckMainCameraExists()
    {
        return Camera.main != null;
    }

    [MenuItem("Toolbox/Camera/Select Main Camera")]
    [MenuItem("Window/Toolbox/Camera/Select Main Camera")]
    static void SelectMainCamera()
    {
        Selection.activeObject = Camera.main;
    }

}