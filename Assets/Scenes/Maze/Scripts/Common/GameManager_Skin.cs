using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using EditorAttributes;
using System.Linq;

[Serializable]
public class GameManager_Skin
{
    public class Fallbacks
    {
        public static int TileResolution = 24;
    }


    


    [SerializeField, ButtonField(nameof(RefreshSkin), "Refresh Skins"), HideProperty] private EditorAttributes.Void refreshSkinHolder;  
    [Space(25)]

    public ViewportTheme guiTheme = null;
    public MazeTheme mazeTheme = null;
    public PlayerThemePacman pacmanTheme = null;
    public PlayerThemeGhost ghostTheme = null;
    public PelletTheme pelletTheme = null;
    public FruitTheme fruitTheme = null;
    public PopupTheme popupTheme = null;
    

    public void RefreshSkin()
    {
        DevelopmentThemes.RefreshGM(this);
    }

    #region Getters

    public int GetTileResolution()
    {
        var ppus = new int[]
        {
            mazeTheme.pixelsPerUnit,
            popupTheme.pixelsPerUnit,
            pacmanTheme.pixelsPerUnit,
            ghostTheme.pixelsPerUnit,
            pelletTheme.pixelsPerUnit,
            fruitTheme.pixelsPerUnit
        };

        //Double Res to Allow for Smoother Visuals
        return ppus.Max() + ppus.Max();
    }

    #endregion

}
