using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(ActionsSequencer))]
public class ActionsSequencerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Actions")) {
            _GenerateActions();
        }
    }

    private void _GenerateActions()
    {
        ActionsSequencer sequencer = target as ActionsSequencer;
        if (sequencer == null) return;

        AnimatorController controller = sequencer.controller as AnimatorController;
        if (controller == null) return;

        //Remove sequencer children
        while (sequencer.transform.childCount > 0) {
            DestroyImmediate(sequencer.transform.GetChild(0).gameObject);
        }

        AnimatorState state = controller.layers[0].stateMachine.entryTransitions[0].destinationState;
        IAction action = (state.behaviours[0] as IAction);
        GameObject stateParentGameObject = new GameObject(action.GetName());
        stateParentGameObject.AddComponent<HelloWorld>();

        stateParentGameObject.transform.SetParent(sequencer.transform);
        while (state.transitions.Length > 0) {
            state = state.transitions[0].destinationState;

            //Generate State GameObject
            GameObject stateGameObject = new GameObject(state.name);
            stateGameObject.AddComponent<HelloWorld>();
            stateGameObject.transform.SetParent(stateParentGameObject.transform);

            //Reassign parent
            stateParentGameObject = stateGameObject;
        }
    }
}
