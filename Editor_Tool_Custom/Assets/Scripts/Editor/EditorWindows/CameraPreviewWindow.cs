using UnityEditor;
using UnityEngine;

public class CameraPreviewWindow : EditorWindow
{
    private Camera _camera = null;
    private RenderTexture _renderTexture = null;

    [MenuItem("Toolbox/Camera/CameraPreview")]
    static void InitWindow()
    {
        CameraPreviewWindow window = GetWindow<CameraPreviewWindow>();
        window.autoRepaintOnSceneChange = true;
        window.Show();
        window.titleContent = new GUIContent("Camera Preview");
    }

    private void Update()
    {
        if (null == _renderTexture) {
            _RefreshRenderTexture();
        }

        //Resize texture according to window size
        if (_renderTexture.width != position.width || _renderTexture.height != position.height) {
            _RefreshRenderTexture();
        }

        if (null == _camera) {
            _camera = Camera.main;
        }

        _camera.targetTexture = _renderTexture;
        _camera.Render();
        _camera.targetTexture = null;
    }

    private void OnGUI()
    {
        GUI.Button(new Rect(50, 50, 100, 20), "Hello");
        if (null != _renderTexture) {
            GUI.DrawTexture(new Rect(0, 0, position.width, position.height), _renderTexture);
        }
    }

    private void OnSelectionChange()
    {
        if (null == Selection.activeGameObject) return;
        _camera = Selection.activeGameObject.GetComponent<Camera>();
    }

    private void _RefreshRenderTexture()
    {
        _renderTexture = new RenderTexture((int)position.width, (int)position.height, 24, RenderTextureFormat.ARGB32);
    }
}
