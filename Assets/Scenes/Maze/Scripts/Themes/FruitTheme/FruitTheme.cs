using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using EditorAttributes;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/FruitTheme")]
[System.Serializable]
public class FruitTheme : ScriptableObject
{

    public string themeName;
    public string themeUUID;
    public int pixelsPerUnit = 24;
    [SerializedDictionary("Type", "Model")] public SerializedDictionary<FruitType, GameObject> models;
    [SerializedDictionary("Type", "Sprites")] public SerializedDictionary<FruitType, AnimatedSpriteSet> sprites;

    public AnimatedSpriteSet GetSpriteSet(FruitType group)
    {
        if (sprites == null) return null;    
        if (!sprites.ContainsKey(group)) return null;
        return sprites[group];
        
    }
}
