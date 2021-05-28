using UnityEngine;

public class ActionMoveLeft : StateMachineBehaviour, IAction
{
    public float time = 0f;

    public string GetName()
    {
        return "MOVE LEFT";
    }
}
