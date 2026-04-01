using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using EditorAttributes;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/PlayerTheme - PacMan")]
[System.Serializable]
public class PacmanTheme : IPlayerTheme
{
    public string themeName;
    public string themeUUID;
    public GameObject model;
    public int pixelsPerUnit = 24;
    [SerializedDictionary("Group", "Sprites")] public SerializedDictionary<string, AnimatedSpriteSet> sprites;

    public AnimatedSpriteSet GetSpriteSet(string group)
    {
        if (sprites == null) return null;    
        if (!sprites.ContainsKey(group)) return null;
        return sprites[group];
        
    }
}
