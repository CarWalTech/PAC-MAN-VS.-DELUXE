using System.Collections.Generic;
using System.Linq;
using SimpleSpritePacker;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze Configuration/Maze Skin")]
[System.Serializable]
public class TileTheme : ScriptableObject
{
    public string themeName;
    public string themeUUID;
    public bool supportsRecolors = false;
    public SPInstance themeTiles = null;
    public Sprite GetIndexedTile(string name, int index)
    {
        //if (themeTiles.sprites.Count <= index || index < 0) return null;
        //return sprites[index];

        if (themeTiles == null) return null;
        var sprites = themeTiles.sprites.ConvertAll<Sprite>(x => (Sprite)x.source).OrderBy(x => x.name).ToList();
        var found_items = sprites.Where(x => x.name == name).ToList();
        if (found_items.Count >= 1) return found_items[0];
        else return null;
        
    }
}
