using UnityEngine;


[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Levels/LevelConfiguration")]
[System.Serializable]
public class LevelConfiguration : ScriptableObject
{
    public string levelName;
    public string levelUUID;
    public GameObject levelTiles;
    public GameObject levelModel;
}
