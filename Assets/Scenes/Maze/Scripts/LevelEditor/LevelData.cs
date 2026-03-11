using UnityEngine;


[CreateAssetMenu(menuName = "Level Configuration")]
[System.Serializable]
public class LevelData : ScriptableObject
{
    public string levelName;
    public GameObject levelTiles;
    public GameObject levelModel;
}
