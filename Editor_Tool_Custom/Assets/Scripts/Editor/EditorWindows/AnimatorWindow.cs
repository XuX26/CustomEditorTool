using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimatorWindow : EditorWindow
{
    private Animator[] animators;
    private string[] objectsName;
    private int objectIndex;

    [MenuItem("Window/CustomAnimator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        AnimatorWindow window = (AnimatorWindow) EditorWindow.GetWindow(typeof(AnimatorWindow));
        window.Show();
    }

    void OnGUI()
    {
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
        objectIndex = EditorGUILayout.Popup(objectIndex, objectsName);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Select"))
        {
            Selection.activeObject = animators[objectIndex].gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Relative controller :  " + animators[objectIndex].runtimeAnimatorController.name);

        EditorGUILayout.Space();

        if (GUILayout.Button("Play anim"))
        {
            AnimationMode.StartAnimationMode();
            foreach (Animator animator in animators)
            {
                animator.Play(animator.GetCurrentAnimatorClipInfo(0).ToString());
            }
        }
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
}