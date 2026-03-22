using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/MazeTheme [DEV]", order = 2)]
[System.Serializable]
public class MazeThemeDev : MazeThemeBase
{
    public MazeTheme theme;
    private List<MazeTile> _knownTiles = new List<MazeTile>();


    void OnValidate()
    {
        ReloadAllTiles();
    }

    public void ReloadAllTiles()
    {
        foreach (var tile in _knownTiles)
            tile.ForceUpdate();
    }

    public override SerializedDictionary<string, List<Sprite>> GetTiles()
    {
        if (!theme) return null;
        else return theme.tileList;
    }

    public override Sprite GetTileSprite(MazeTile tile, ref TileData data)
    {
        if (!_knownTiles.Contains(tile)) _knownTiles.Add(tile);
        return base.GetTileSprite(tile, ref data);
    }


    
    [CustomEditor(typeof(MazeThemeDev))]
    public class RefreshButton : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MazeThemeDev myScript = (MazeThemeDev)target;
            if (GUILayout.Button("Force Refresh")) myScript.ReloadAllTiles();
        }

    }
}
