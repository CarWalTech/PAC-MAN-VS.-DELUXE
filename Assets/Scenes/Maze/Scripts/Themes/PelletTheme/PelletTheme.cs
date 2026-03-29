using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using EditorAttributes;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/PelletTheme")]
[System.Serializable]
public class PelletTheme : ScriptableObject, IPelletTheme
{
    public enum PelletType
    {
        Normal,
        Powered
    }
    public string themeName;
    public string themeUUID;
    public int pixelsPerUnit = 24;
    [SerializedDictionary("Type", "Model")] public SerializedDictionary<PelletType, GameObject> models;
    [SerializedDictionary("Group", "Sprites")] public SerializedDictionary<PelletType, AnimatedSpriteSet> sprites;

    public AnimatedSpriteSet GetSpriteSet(PelletType group)
    {
        if (sprites == null) return null;    
        if (!sprites.ContainsKey(group)) return null;
        return sprites[group];
        
    }

    public PelletTheme GetPelletTheme()
    {
        return this;
    }
}
