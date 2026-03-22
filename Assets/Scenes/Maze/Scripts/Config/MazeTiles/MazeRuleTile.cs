using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Tiles/MazeRuleTile")]
[System.Serializable]
public class MazeRuleTile : RuleTile {

    private static bool canRenderDefault()
    {
        bool isEditor = false;

        #if UNITY_EDITOR 
        isEditor = true; 
        #endif

        if (isEditor) return !EditorApplication.isPlaying;
        else return false;
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        return base.StartUp(position, tilemap, instantiatedGameObject);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        if (!canRenderDefault()) tileData.sprite = null;
    }
}