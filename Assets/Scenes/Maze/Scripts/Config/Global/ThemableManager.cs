using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AYellowpaper.SerializedCollections;
using SimpleSpritePacker;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using EditorAttributes;
using System.Linq;
using UnityEditor.SceneManagement;
using System.Collections;


[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/ThemableManager", order = 0)]
[System.Serializable]
public class ThemableManager : ScriptableObject, IMazeTheme, IPelletTheme
{
    [System.Serializable]
    public class ThemableData
    {
        public MazeTheme mazeTheme = null;
        public PelletTheme pelletTheme = null;
        public ViewportTheme guiTheme = null;
        public PlayerThemePacman pacmanTheme = null;
        public PlayerThemeGhost ghostTheme = null;
        public FruitTheme fruitTheme = null;
        public PopupTheme popupTheme = null;

        [FoldoutGroup("Prefabs", nameof(tiles), nameof(ctmTiles), nameof(pelletTiles))]
        [SerializeField] private Void prefabGroupHolder;
        [SerializeField, HideProperty] public List<MazeTile> tiles = new List<MazeTile>();
        [SerializeField, HideProperty] public List<MazeTileCTM> ctmTiles = new List<MazeTileCTM>();
        [SerializeField, HideProperty] public List<PelletTile> pelletTiles = new List<PelletTile>();    
    }
    

    [SerializeField, ButtonField(nameof(RefreshThemePriv), "Refresh")] private EditorAttributes.Void refreshSkinHolder;
    public ThemableData themableData = new ThemableData();

    
    public void OnValidate()
    {

    }
    
    #region Get Themes

    public MazeTheme GetMazeTheme()
    {
        return themableData.mazeTheme;
    }
    public PelletTheme GetPelletTheme()
    {
        return themableData.pelletTheme;
    }
    
    #endregion

    #region Theme Overrides

    public MazeTheme.MazeRules GetMazeRules()
    {
        return themableData.mazeTheme.GetMazeRules();
    }
    
    #endregion

    #region Refresh Themes

    public static void RefreshTheme(GameManager_Skin skin) 
    {
        ThemableData instance = new ThemableData
        {
            pelletTiles = GameObject.FindObjectsByType<PelletTile>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList(),
            tiles = GameObject.FindObjectsByType<MazeTile>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList(),
            ctmTiles = GameObject.FindObjectsByType<MazeTileCTM>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList(),
            pacmanTheme = skin.pacmanTheme,
            ghostTheme = skin.ghostTheme,
            popupTheme = skin.popupTheme,
            fruitTheme = skin.fruitTheme,
            mazeTheme = skin.mazeTheme,
            pelletTheme = skin.pelletTheme,
            guiTheme = skin.guiTheme
        };
        RefreshTheme(instance);
    }
    public static void RefreshTheme(ThemableData themableData)
    {
        foreach (var tile in themableData.tiles)
            tile.SetSkin(themableData.mazeTheme);

        foreach (var tile in themableData.ctmTiles)
            tile.SetSkin(themableData.mazeTheme);

        foreach (var tile in themableData.pelletTiles)
            tile.SetSkin(themableData.pelletTheme);

        var currentPrefab = PrefabStageUtility.GetCurrentPrefabStage();
        if (currentPrefab)
        {
            foreach (var hook in currentPrefab.FindComponentsOfType<MazeThemeHolder>())
                hook.SetSkin(themableData.mazeTheme, themableData.pelletTheme);

            foreach (var hook in currentPrefab.FindComponentsOfType<PelletThemeHolder>())
                hook.SetSkin(themableData.pelletTheme);

            foreach (var hook in currentPrefab.FindComponentsOfType<ViewportThemeHolder>())
                hook.SetSkin(themableData.guiTheme);
        }
        else
        {
            foreach (var hook in Object.FindObjectsByType<MazeThemeHolder>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                hook.SetSkin(themableData.mazeTheme, themableData.pelletTheme);

            foreach (var hook in Object.FindObjectsByType<PelletThemeHolder>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                hook.SetSkin(themableData.pelletTheme);

            foreach (var hook in Object.FindObjectsByType<ViewportThemeHolder>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                hook.SetSkin(themableData.guiTheme);
        }
    }
    private void RefreshThemePriv()
    {
        RefreshTheme(themableData);
    }

    #endregion

}
