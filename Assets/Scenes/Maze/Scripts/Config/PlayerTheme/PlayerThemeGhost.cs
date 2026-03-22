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
    public int spriteResolution = 24;
    [SerializedDictionary("Group", "Sprites")] public SerializedDictionary<string, List<Sprite>> sprites;
    public override SerializedDictionary<string, List<Sprite>> GetSprites() { return sprites; }
}
