using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using EditorAttributes;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/Development Theme", order = 2)]
[System.Serializable]
public class DevelopmentThemes : ScriptableObject, IMazeTheme, IPelletTheme
{
    public MazeTheme mazeTheme = null;
    public PelletTheme pelletTheme = null;
    public ViewportTheme guiTheme = null;
    [SerializeField, ButtonField(nameof(RefreshTheme), "Refresh")] private EditorAttributes.Void refreshSkinHolder;


    [Header("Overrides")]
    [SerializeField] private bool enableOverrides = false;
    [SerializeField, ShowField(nameof(enableOverrides))] private bool supportsRecolors = false;
    [SerializeField, ShowField(nameof(enableOverrides))] private bool supportsBackground = false;
    [SerializeField, ShowField(nameof(enableOverrides))] private bool supportsBackgroundRecolors = false;


    [SerializeField] public List<MazeTile> tiles = new List<MazeTile>();
    [SerializeField] public List<MazeTileCTM> ctmTiles = new List<MazeTileCTM>();
    [SerializeField] public List<PelletTile> pelletTiles = new List<PelletTile>();
    

    public MazeTheme GetMazeTheme()
    {
        return mazeTheme;
    }

    public bool HasBackgroundRecolorSupport()
    {
        if (enableOverrides) return supportsBackgroundRecolors;
        else return mazeTheme.HasBackgroundRecolorSupport();
    }

    public bool HasBackgroundSupport()
    {
        if (enableOverrides) return supportsBackground;
        else return mazeTheme.HasBackgroundSupport();
    }

    public bool HasRecolorSupport()
    {
        if (enableOverrides) return supportsRecolors;
        else return mazeTheme.HasRecolorSupport();
    }

    public void OnValidate()
    {
        RefreshTheme(mazeTheme, pelletTheme, guiTheme);
    }






    public static void RefreshGM(GameManager_Skin skin) 
    {
        RefreshStatic(skin.mazeTheme, skin.pelletTheme, skin.guiTheme);
    }
    public static void RefreshStatic(MazeTheme _mazeTheme, PelletTheme _pelletTheme, ViewportTheme _guiTheme)
    {
        foreach (var hook in GameObject.FindObjectsByType<Pellet>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            hook.skin = _pelletTheme;
            hook.RefreshTheme();
        }

        foreach (var hook in GameObject.FindObjectsByType<SkinableBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            hook.SetSkin(_guiTheme);
        }
    }
    private void RefreshTheme(MazeTheme _mazeTheme, PelletTheme _pelletTheme, ViewportTheme _guiTheme)
    {
        foreach (var tile in tiles)
        {
            tile.SetTheme(_mazeTheme);
            tile.RefreshTheme();
        }

        foreach (var tile in ctmTiles)
        {
            tile.SetTheme(_mazeTheme);
            tile.RefreshTheme();
        }

        foreach (var tile in pelletTiles)
        {
            tile.SetTheme(_pelletTheme);
            tile.RefreshTheme();
        }
        
        RefreshStatic(_mazeTheme, _pelletTheme, _guiTheme);
    }
    public void RefreshTheme()
    {
        RefreshTheme(mazeTheme, pelletTheme, guiTheme);
    }


    public PelletTheme GetPelletTheme()
    {
        return pelletTheme;
    }
}
