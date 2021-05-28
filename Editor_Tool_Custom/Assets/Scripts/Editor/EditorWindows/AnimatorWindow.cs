using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AnimatorWindow : EditorWindow
{
    public static AnimatorWindow window;

    public Animator[] animators { get; private set; }
    private string[] objectsName;
    private int currAnim;

    private float _lastEditorTime = 0f;
    private bool _isSimulatingAnimation = false;
    private float animTime = 0;
    private float speed = 1;

    public List<AnimatorPlayer> currentPlayers = new List<AnimatorPlayer>();

    [MenuItem("Window/AnimatorCustom")]
    static void Init()
    {
        window = (AnimatorWindow) GetWindow(typeof(AnimatorWindow));
        EditorApplication.playModeStateChanged += window._OnPlayModeStateChange;
        window.Show();
    }

    private void Update()
    {
        Repaint();
    }

    void OnGUI()
    {
        if (window == null) Init();
        if (GUILayout.Button("Refresh Animator list"))
            RefreshAnimatorList();

        if (animators == null || animators.Length == 0)
        {
            EditorGUILayout.Space();
            DisplayAndCenterStrings(new[] {"/!\\  Animators list is empty  /!\\", "Refresh Animator List"}, true, true);
            return;
        }

        DisplayAndCenterStrings(new[] {"Nbr animators in scene : " + animators.Length}, false, true);
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal(); 
        GUILayout.Label("Animators : ");
        currAnim = EditorGUILayout.Popup(currAnim, objectsName, GUILayout.MaxWidth(200));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select"))
        {
            Selection.activeObject = animators[currAnim].gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
            EditorGUIUtility.PingObject(Selection.activeObject);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Label("Relative controller :  " + animators[currAnim].runtimeAnimatorController.name);

        EditorGUILayout.Space();
        
        DisplayClipPlayer();
    }

    void DisplayClipPlayer()
    {
        GUILayout.BeginHorizontal();
        if (!_isSimulatingAnimation)
        {
            if (GUILayout.Button("PLAY",GUILayout.MinHeight(38), GUILayout.MinWidth(60)))
                StartAnimSimulation();
        }
        else
        {
            if (GUILayout.Button("STOP",GUILayout.MinHeight(38), GUILayout.MinWidth(60)))
                StopAnimSimulation();
        }
        // Var names
        GUILayout.BeginVertical();
        GUILayout.Label("Speed");
        GUILayout.Label("Time");
        GUILayout.EndVertical();
        
        // Float fields
        float animDuration = animators[currAnim].runtimeAnimatorController.animationClips[0].length;
        GUILayout.BeginVertical();
        speed = EditorGUILayout.FloatField(Mathf.Clamp(speed,0,2), GUILayout.MaxWidth(30));
        animTime = EditorGUILayout.FloatField(Mathf.Clamp(animTime,0,animDuration), GUILayout.MaxWidth(30));
        GUILayout.EndVertical();
        
        // Min Sliders value
        GUILayout.BeginVertical();
        GUILayout.Label("0");
        GUILayout.Label("0");
        GUILayout.EndVertical();
        
        // Sliders
        GUILayout.BeginVertical();
        speed = GUILayout.HorizontalSlider(speed, 0, 2f,GUILayout.MinWidth(100), GUILayout.MaxWidth(200));
        EditorGUILayout.Space(13);
        animTime = GUILayout.HorizontalSlider(animTime, 0, animDuration, GUILayout.MinWidth(100), GUILayout.MaxWidth(200));
        GUILayout.EndVertical();

        // Max Sliders value
        GUILayout.BeginVertical();
        GUILayout.Label("2");
        GUILayout.Label(animDuration.ToString());
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    void RefreshAnimatorList()
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        List<Animator> allAnimators = new List<Animator>();
        foreach (GameObject rootObject in rootObjects)
        {
            GetAnimatorsInChildren(allAnimators, rootObject.transform);
        }

        animators = new Animator[allAnimators.Count];
        objectsName = new String[animators.Length];

        int i = 0;
        foreach (Animator animator in allAnimators)
        {
            animators[i] = animator;
            objectsName[i] = animators[i].name;
            i++;
        }
    }

    void GetAnimatorsInChildren(List<Animator> list, Transform parent)
    {
        foreach (Transform transform in parent)
        {
            if (transform.GetComponent<Animator>())
                list.Add(transform.GetComponent<Animator>());
            GetAnimatorsInChildren(list, transform);
        }
    }

    void DisplayAndCenterStrings(string[] text, bool bold, bool inARow)
    {
        if (inARow)
            GUILayout.BeginHorizontal();

        foreach (string str in text)
        {
            if (!inARow)
                GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.Label(str, bold ? EditorStyles.boldLabel : EditorStyles.label);
            GUILayout.FlexibleSpace();

            if (!inARow)
                GUILayout.EndHorizontal();
        }

        if (inARow)
            GUILayout.EndHorizontal();
    }

    // --- SimulatingAnimation ---
    #region SimulatingAnimation

    private void UpdateAnimation()
    {
        if (animators[currAnim] == null) return;

        animTime += (Time.realtimeSinceStartup - _lastEditorTime - animTime)*speed;

        AnimationClip animClip = animators[currAnim].runtimeAnimatorController.animationClips[0];

        if (animTime >= animClip.length)
        {
            StopAnimSimulation();
            animTime = 0;
        }
        else
        {
            if (AnimationMode.InAnimationMode())
            {
                AnimationMode.SampleAnimationClip(animators[currAnim].gameObject, animClip, animTime);
            }
        }
    }

    public void StartAnimSimulation()
    {
        AnimationMode.StartAnimationMode();
        EditorApplication.update -= UpdateAnimation;
        EditorApplication.update += UpdateAnimation;
        _lastEditorTime = Time.realtimeSinceStartup;
        _isSimulatingAnimation = true;
    }

    public void StopAnimSimulation()
    {
        AnimationMode.StopAnimationMode();
        EditorApplication.update -= UpdateAnimation;
        _isSimulatingAnimation = false;
    }

    public void _OnPlayModeStateChange(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            StopAnimSimulation();
        }
    }
    #endregion
}

[Serializable]
public class AnimatorPlayer
{
    public Animator Animator{ get; private set; }
    public AnimationClip[] Clips { get; private set; }
    public int AnimatorIndex { get; private set; }

    public int currClip = 0;
    public float speed = 1;

    AnimatorPlayer(int animatorIndex)
    {
        AnimatorIndex = animatorIndex;
        Animator = AnimatorWindow.window.animators[animatorIndex];
        Clips = Animator.runtimeAnimatorController.animationClips;
    }
}