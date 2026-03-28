using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Tiles/MazeTile (CTM)")]
[System.Serializable]
public class MazeTileCTM : RuleTile, IMazeTile
{
    public MazeTheme theme = null;
    public string key;
    public string connectionGroup;
    private GameObject go = null;
    private int _currentTile = 0;


    private int[] _spriteMappings = new int[]
        {
            0, 
            1, 
            2, 
            3, 
            12,
            24,
            36,
            4, 
            5, 
            16,
            17,
            13,
            14,
            15,
            25,
            26,
            27,
            37,
            38,
            39,
            6, 
            7, 
            18,
            19,
            28,
            29,
            40,
            41,
            30,
            31,
            42,
            43,
            8, 
            9, 
            20,
            21,
            10,
            11,
            22,
            23,
            32,
            33,
            44,
            45,
            34,
            35,
            46,
        };


    private Color currentColor = Color.white;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        for (int i = 0; i < 47; i++)
        {
            m_TilingRules[i].m_GameObject = m_DefaultGameObject;
            m_TilingRules[i].m_ColliderType = Tile.ColliderType.Sprite;
            m_TilingRules[i].m_Output = TilingRuleOutput.OutputSprite.Single;
            m_TilingRules[i].m_Sprites = new Sprite[1] { theme.GetSprite(key, _spriteMappings[i]) };
        }

        var result = base.StartUp(position, tilemap, instantiatedGameObject);
        if (instantiatedGameObject && instantiatedGameObject.GetComponent<WallTile>()) instantiatedGameObject.GetComponent<WallTile>().Setup(this);
        return result;
    }
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);  
        Matrix4x4 transform = Matrix4x4.identity;
        foreach (TilingRule tilingRule in m_TilingRules)
        {
            if (!RuleMatches(tilingRule, position, tilemap, ref transform)) continue;
            _currentTile = _spriteMappings[m_TilingRules.IndexOf(tilingRule)];
        }
    }
    public override bool RuleMatch(int neighbor, TileBase other)
    {
        var group = string.Empty;
        if (other != null)
        {

            if (other.GetType() == typeof(MazeTileCTM))
            {
                var res = (MazeTileCTM)other;
                group = res.connectionGroup;
            }
        }


        if (neighbor == TilingRule.Neighbor.This)
        {
            return other != null;
        }
        else if (neighbor == TilingRule.Neighbor.NotThis)
        {
            return other == null;
        }
        else
        {
            return false;
        }
    }

    public Sprite GetSprite()
    {
        return theme.GetSprite(key, _currentTile);
    }
    public MazeTheme GetTheme()
    {
        return theme;
    }
    public void SetTheme(MazeTheme _theme)
    {
        theme = _theme;
    }

    public void ForceUpdate()
    {
        if (go) EditorUtility.SetDirty(go);
        EditorUtility.SetDirty(this);
    }
    public void RefreshTheme()
    {
        EditorUtility.SetDirty(this);
    }
}