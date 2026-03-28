using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using EditorAttributes;
using SimpleSpritePacker;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/MazeTheme", order = 1)]
[Serializable]
public class MazeTheme : ScriptableObject, IMazeTheme
{
    public struct CacheObject
    {
        public ITilemap tilemap;
        public IMazeTile tile;
        public TileData data;
        public Vector3Int position;
    }
    private List<CacheObject> _instanceCache = new List<CacheObject>();


    public string themeName;
    public string themeUUID;
    [FormerlySerializedAs("tileResolution")] public int pixelsPerUnit = 24;
    [SerializedDictionary("Tile Group", "Sprites")] public SerializedDictionary<string, List<Sprite>> tileList;


    [Serializable]
    public class MazeRules
    {
        [SerializeField] public bool supportsRecolors = false;
        [SerializeField] public bool supportsBackground = false;
        [SerializeField] public bool supportsBackgroundRecolors = false;
    }

    [SerializeField] private MazeRules rules;


    private void OnValidate()
    {
        
    }

    public SerializedDictionary<string, List<Sprite>> GetTiles() { return tileList; }
    public Sprite GetSprite(string group, int index)
    {
        var tileList = GetTiles();
        if (tileList == null) return null;    
        if (!tileList.ContainsKey(group)) return null;

        var group_list = tileList[group];
        if (group_list == null) return null;    
        if (group_list.Count <= index || index < 0) return null;
        return group_list[index];     
    }
    public void AttachTile(IMazeTile tile, Vector3Int position, ITilemap tilemap, ref TileData data)
    {
        var result = new CacheObject
        {
            tile = tile,
            tilemap = tilemap,
            data = data,
            position = position
        };

        if (!_instanceCache.Contains(result)) _instanceCache.Add(result);
    }

    public MazeTheme GetMazeTheme()
    {
        return this;
    }

    public MazeRules GetMazeRules()
    {
        return rules;
    }


}
