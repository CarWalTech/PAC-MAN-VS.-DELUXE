using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Tiles/Pellet Tile")]
[System.Serializable]
public class PelletTile : RuleTile, IPelletTile {

    public PelletTheme.PelletType key;
    public PelletTheme theme = null;

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
        else if (theme) tileData.sprite = theme.GetSpriteSet(key).sprites[0];
    }


    public void ForceUpdate()
    {
        EditorUtility.SetDirty(this);
    }

    public AnimatedSpriteSet GetSpriteSet()
    {
        return theme.GetSpriteSet(key);
    }

    public PelletTheme GetTheme()
    {
        return theme;
    }

    public void SetTheme(PelletTheme _theme)
    {
        theme = _theme;
    }

    public void RefreshTheme()
    {
        EditorUtility.SetDirty(this);
    }
}