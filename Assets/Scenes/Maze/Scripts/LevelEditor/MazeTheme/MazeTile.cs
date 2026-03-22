using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Tiles/MazeTile")]
[System.Serializable]
public class MazeTile : TileBase {
    public int TileIndex = -1;
    public MazeThemeBase theme = null;
    public string key;
    public int value;
    
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        return base.StartUp(position, tilemap, instantiatedGameObject);
    }

    public void ForceUpdate()
    {
        EditorUtility.SetDirty(this);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        if (theme)
        {
            var result = theme.GetTileSprite(this, ref tileData);
            if (result != null) tileData.sprite = result;    
        }
        
    }
}