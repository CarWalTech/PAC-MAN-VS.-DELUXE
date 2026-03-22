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
public class PlayerThemePacman : PlayerThemeBase
{
    public string themeName;
    public string themeUUID;
    public int spriteResolution = 24;
    [SerializedDictionary("Group", "Sprites")] public SerializedDictionary<string, List<Sprite>> sprites;
    public override SerializedDictionary<string, List<Sprite>> GetSprites() { return sprites; }
}
