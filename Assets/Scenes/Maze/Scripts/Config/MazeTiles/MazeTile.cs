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
public class MazeTile : RuleTile, IMazeTile 
{
    public string key;
    public int value;
    public MazeTheme theme = null;

    public bool coloredTile = false;
    public Material coloredMaterial = null;

    
    
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        var result = base.StartUp(position, tilemap, instantiatedGameObject);
        if (instantiatedGameObject && instantiatedGameObject.GetComponent<WallTile>()) instantiatedGameObject.GetComponent<WallTile>().Setup(this);
        return result;
    }

    public void ForceUpdate()
    {
        EditorUtility.SetDirty(this);
    }

    public Sprite GetSprite()
    {
        return theme.GetSprite(key, value);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        if (theme)
        {
            theme.AttachTile(this, position, tilemap, ref tileData);
            tileData.sprite = GetSprite();
        }
    }

    public MazeTheme GetTheme()
    {
        return theme;
    }

    public void SetTheme(MazeTheme _theme)
    {
        theme = _theme;
    }

    public void RefreshTheme()
    {
        EditorUtility.SetDirty(this);
    }
}