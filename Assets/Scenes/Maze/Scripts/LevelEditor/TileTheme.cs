using System;
using System.Collections.Generic;
using System.Linq;
using SimpleSpritePacker;
using UnityEngine;

[Serializable] public class TileList : SerializableDictionary<string, bool> { }

[CreateAssetMenu(menuName = "Pac-Man VS/Maze Configuration/Maze Skin")]
[System.Serializable]
public class TileTheme : ScriptableObject
{
    public string themeName;
    public string themeUUID;
    public bool supportsRecolors = false;
    public int tileResolution = 24;
    public SPInstance themeTiles = null;
    [SerializeField] private TileList tileList = new TileList();
    public bool useLegacyMethod = true;
    
    

    public void OnValidate()
    {

    }

    public Sprite GetIndexedTile(string name, int index)
    {
        //if (themeTiles.sprites.Count <= index || index < 0) return null;
        //return sprites[index];
        if (useLegacyMethod)
        {
            if (themeTiles == null) return null;    
            var sprites = themeTiles.sprites.ConvertAll<Sprite>(x => (Sprite)x.source).OrderBy(x => x.name).ToList();
            var found_items = sprites.Where(x => x.name == name).ToList();
            if (found_items.Count >= 1) return found_items[0];
            else return null;
        }
        else
        {
            return null;
            //if (tiles == null) return null;    
            //if (tiles.Count <= index || index < 0) return null;
            //return tiles[index];     
        }
        
        
        
        
        
        
    }
}
