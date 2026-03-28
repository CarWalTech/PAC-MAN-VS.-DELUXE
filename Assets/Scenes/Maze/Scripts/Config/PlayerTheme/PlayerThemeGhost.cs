using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using EditorAttributes;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/PlayerTheme - Ghost")]
[System.Serializable]
public class PlayerThemeGhost : PlayerThemeBase
{
    public string themeName;
    public string themeUUID;
    public GameObject model;
    public int pixelsPerUnit = 24;
    [SerializedDictionary("Character", "Material")] public SerializedDictionary<PlayerCharacter, Material> materials;
    [SerializedDictionary("Group", "Sprites")] public SerializedDictionary<string, AnimatedSpriteSet> sprites;

    public AnimatedSpriteSet GetSpriteSet(string group)
    {
        if (sprites == null) return null;    
        if (!sprites.ContainsKey(group)) return null;
        return sprites[group];
        
    }
}
