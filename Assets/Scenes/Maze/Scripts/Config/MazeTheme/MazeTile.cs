using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Tiles/MazeTile")]
[System.Serializable]
public class MazeTile : IMazeTile 
{
    public string key;
    public int value;

    public bool coloredTile = false;
    public Material coloredMaterial = null;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        var result = base.StartUp(position, tilemap, instantiatedGameObject);
        return result;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        var theme = GetSkin();
        base.GetTileData(position, tilemap, ref tileData);
        if (theme)
        {
            theme.AttachTile(this, position, tilemap, ref tileData);
            tileData.sprite = theme.GetSprite(key, value);
        }
    }
}