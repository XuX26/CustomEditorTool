using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimSampler))]
public class AnimSamplerInspector : Editor
{
    private float _lastEditorTime = 0f;
    private bool _isSimulatingAnimation = false;

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += _OnPlayModeStateChange;
    }

    private void _OnPlayModeStateChange(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode) {
            StopAnimSimulation();
        }
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= _OnPlayModeStateChange;
        StopAnimSimulation();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AnimSampler sampler = target as AnimSampler;
        if (null == sampler) return;

        Animator animator = sampler.GetComponent<Animator>();
        if (null == animator) return;

        if (!_isSimulatingAnimation) {
            if (GUILayout.Button("Play")) {
                StartAnimSimulation();
            }
        } else {
            if (GUILayout.Button("Stop")) {
                StopAnimSimulation();
            }
        }
    }

    private void OnEditorUpdate()
    {
        AnimSampler sampler = target as AnimSampler;
        if (null == sampler) return;

        Animator animator = sampler.GetComponent<Animator>();
        if (null == animator) return;

        AnimationClip animClip = animator.runtimeAnimatorController.animationClips[0];

        float animTime = Time.realtimeSinceStartup - _lastEditorTime;
        if (animTime >= animClip.length) {
            StopAnimSimulation();
        } else {
            if (AnimationMode.InAnimationMode()) {
                AnimationMode.SampleAnimationClip(sampler.gameObject, animClip, animTime);
            }
        }
    }

    public void StartAnimSimulation()
    {
        AnimationMode.StartAnimationMode();
        EditorApplication.update -= OnEditorUpdate;
        EditorApplication.update += OnEditorUpdate;
        _lastEditorTime = Time.realtimeSinceStartup;
        _isSimulatingAnimation = true;
    }

    public void StopAnimSimulation()
    {
        AnimationMode.StopAnimationMode();
        EditorApplication.update -= OnEditorUpdate;
        _isSimulatingAnimation = false;
    }
}
