using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

[CreateAssetMenu]
[System.Serializable]
public class MazeTile : TileBase {
    public int TileIndex = -1;
    public TileTheme theme = null;
    
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        return base.StartUp(position, tilemap, instantiatedGameObject);
    }

    private void LoadTileSprite(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (!theme) return;
        var result = theme.GetIndexedTile(name, TileIndex);
        if (result != null) tileData.sprite = result;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        LoadTileSprite(position, tilemap, ref tileData);
    }
}