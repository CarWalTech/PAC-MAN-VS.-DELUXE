using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MazeThemeBase : ScriptableObject
{

    public virtual SerializedDictionary<string, List<Sprite>> GetTiles()
    {
        return null;
    }



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

    public virtual bool NeedsUpdate()
    {
        return false;
    }

    public virtual Sprite GetTileSprite(MazeTile tile, ref TileData data)
    {
        return GetSprite(tile.key, tile.value);
    }

}
