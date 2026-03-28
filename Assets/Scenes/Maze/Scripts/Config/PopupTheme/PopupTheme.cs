using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using EditorAttributes;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/PopupTheme")]
[System.Serializable]
public class PopupTheme : ScriptableObject
{
    public string themeName;
    public string themeUUID;
    public int pixelsPerUnit = 8;
    public int numberWidth = 4;
    public int numberHeight = 7;
    public List<Sprite> numbers = new List<Sprite>();
}
