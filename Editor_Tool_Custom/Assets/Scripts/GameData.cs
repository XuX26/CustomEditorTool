using UnityEngine;

[System.Serializable]
public class GameDataInfos
{
    public int lastEditTime = 0;
    public string version = "";
}

[CreateAssetMenu(menuName = "Toolbox/Game/GameData")]
public class GameData : ScriptableObject
{
    [CustomSlider(1, 4)]
    [SerializeField] private int _nbPlayers = 4;
    public float[] playersSpeed;

    public GameDataInfos infos;
}
