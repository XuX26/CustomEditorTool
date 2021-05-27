using UnityEngine;

[CreateAssetMenu(menuName = "Toolbox/Game/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private int _nbPlayers = 4;
    public float[] playersSpeed;
}
