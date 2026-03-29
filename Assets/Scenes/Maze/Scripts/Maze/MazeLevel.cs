using UnityEngine;


[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Levels/MazeLevel")]
[System.Serializable]
public class MazeLevel : ScriptableObject
{
    public string levelName;
    public string levelUUID;
    public GameObject levelTiles;
    public GameObject levelModel;
}
