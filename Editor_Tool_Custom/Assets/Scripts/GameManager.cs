using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const string GAMEOBJECT_NAME = "GameManager";

    public GameData gameData = null;

    public float timeScale = 0f;

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (gameObject.name != GAMEOBJECT_NAME) {
            gameObject.name = GAMEOBJECT_NAME;
        }
    }

#endif
}
