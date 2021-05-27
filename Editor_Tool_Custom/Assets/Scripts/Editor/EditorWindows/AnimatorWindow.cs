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

    // bool groupEnabled;
    // bool myBool = true;
    // float myFloat = 1.23f;

    // Add menu named "CustomAnimator" to the Window menu

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
        {
            RefreshAnimatorList();
        }
        EditorGUILayout.Space();

        if (animators == null || animators.Length == 0)
        {
            GUILayout.Label("/!\\  Animators list is empty  /!\\\n- Refresh Animator List", EditorStyles.boldLabel);
            return;
        }
 
        GUILayout.Label("Nbr animators : " + animators.Length);
        objectIndex = EditorGUILayout.Popup(objectIndex, objectsName);

        EditorGUILayout.Space();

        if (GUILayout.Button("Play anim"))
        {
            AnimationMode.StartAnimationMode();
            foreach (Animator animator in animators)
            {
                animator.Play(animator.GetCurrentAnimatorClipInfo(0).ToString());
            }
        }

        // GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        // myString = EditorGUILayout.TextField("Text Field", myString);
        //
        // groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        // myBool = EditorGUILayout.Toggle("Toggle", myBool);
        // myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        // EditorGUILayout.EndToggleGroup();
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

    void GetAnimatorsInChildren(List<Animator> list,Transform parent)
    {
        foreach (Transform transform in parent)
        {
            if (transform.GetComponent<Animator>())
                list.Add(transform.GetComponent<Animator>());
            GetAnimatorsInChildren(list, transform);
        }
    }
}