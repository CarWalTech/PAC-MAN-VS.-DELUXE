using System.Collections.Generic;
using System.Linq;
using EditorAttributes;
using SimpleSpritePacker;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze/Themes/ViewportTheme", order = 3)]
[System.Serializable]
public class ViewportTheme : ScriptableObject
{
    public string themeName;
    public string themeUUID;

    #region Pac-Man Viewport
    [SerializeField, FoldoutGroup("Pac-Man Viewport", false, nameof(sb_Frame), nameof(groupHolder1a), nameof(groupHolder1b), nameof(groupHolder1c))] private Void groupHolder1;

    #region General
    [SerializeField, HideProperty, Rename("Sidebar Background")] public Sprite sb_Frame;
    #endregion

    #region 3D Viewport
    [SerializeField, HideProperty, FoldoutGroup("3D Viewport", false, nameof(sb_ViewportPanel), nameof(sb_ViewportFrame), nameof(sb_ViewportMask))] private Void groupHolder1a;
    [SerializeField, HideProperty, Rename("Background")] public Sprite sb_ViewportPanel;
    [SerializeField, HideProperty, Rename("Frame")] public Sprite sb_ViewportFrame; 
    [SerializeField, HideProperty, Rename("Mask")] public Sprite sb_ViewportMask; 
    #endregion

    #region Player Scorecards
    [SerializeField, HideProperty, FoldoutGroup("Player Scorecards", false, nameof(groupHolder1ba), nameof(groupHolder1bb), nameof(groupHolder1bc))] private Void groupHolder1b;

    #region Containers
    [SerializeField, HideProperty, FoldoutGroup("Containers", false, nameof(ms_Container), nameof(ms_ContainerHue), nameof(ms_ContainerShadow))] private Void groupHolder1ba;
    [SerializeField, HideProperty, Rename("Background")] public Sprite ms_Container;
    [SerializeField, HideProperty, Rename("Background (Fill)")] public Sprite ms_ContainerHue;
    [SerializeField, HideProperty, Rename("Background (Shadows)")] public Sprite ms_ContainerShadow;
    #endregion

    #region Labels
    [SerializeField, HideProperty, FoldoutGroup("Labels", false, nameof(ms_PlayerHeader), nameof(ms_COMHeader), nameof(ms_HeaderNumbers), nameof(ms_ScoreNumbers))] private Void groupHolder1bb;
    [SerializeField, HideProperty, Rename("Player")] public Sprite ms_PlayerHeader;
    [SerializeField, HideProperty, Rename("Computer")] public Sprite ms_COMHeader;
    [SerializeField, HideProperty, Rename("Slots")] public Sprite[] ms_HeaderNumbers;
    [SerializeField, HideProperty, Rename("Symbols")] public Sprite[] ms_ScoreNumbers;
    #endregion

    #region Icons
    [SerializeField, HideProperty, FoldoutGroup("Icons", false, nameof(ms_PacMan), nameof(ms_GhostBase), nameof(ms_GhostEyes))] private Void groupHolder1bc;
    [SerializeField, HideProperty, Rename("Pac-Man")] public Sprite ms_PacMan;
    [SerializeField, HideProperty, Rename("Ghost")] public Sprite ms_GhostBase;
    [SerializeField, HideProperty, Rename("Ghost Eyes")] public Sprite ms_GhostEyes;
    #endregion

    #endregion

    #region Target Score

    [SerializeField, HideProperty, FoldoutGroup("Target Score", false, nameof(mts_Background), nameof(mts_Numbers), nameof(mts_symbolColor), nameof(mts_symbolShadow))] private Void groupHolder1c;
    [SerializeField, HideProperty, Rename("Background")] public Sprite mts_Background;
    [SerializeField, HideProperty, Rename("Symbols")]  public Sprite[] mts_Numbers;
    [SerializeField, HideProperty, Rename("Symbol Color")] public Color mts_symbolColor = Color.white;
    [SerializeField, HideProperty, Rename("Symbol Shadow")] public bool mts_symbolShadow = true;

    #endregion

    #endregion

    #region Ghost Viewport

    [SerializeField, FoldoutGroup("Ghost Viewport", false, nameof(groupHolder2a), nameof(groupHolder2b), nameof(groupHolder2c), nameof(groupHolder2d), nameof(groupHolder2e))] private Void groupHolder2;

    #region 2D Viewport
    [SerializeField, HideProperty, FoldoutGroup("2D Viewport", false, nameof(gvm_ViewportContainer), nameof(gvm_ViewportFrame), nameof(gvm_ViewportMask), nameof(groupHolder2aa))] private Void groupHolder2a;
    [SerializeField, HideProperty, Rename("Background")] public Sprite gvm_ViewportContainer;
    [SerializeField, HideProperty, Rename("Frame")] public Sprite gvm_ViewportFrame; 
    [SerializeField, HideProperty, Rename("Mask")] public Sprite gvm_ViewportMask;

    #region Labels
    [SerializeField, HideProperty, FoldoutGroup("Labels", false, nameof(gvm_PlayerNumbers), nameof(gvm_COMHeader))] private Void groupHolder2aa;
    [SerializeField, HideProperty, Rename("Player")] public Sprite[] gvm_PlayerNumbers;
    [SerializeField, HideProperty, Rename("Computer")] public Sprite gvm_COMHeader;
    #endregion

    #endregion

    #region 3D Viewport
    [SerializeField, HideProperty, FoldoutGroup("3D Viewport", false, nameof(gv_ViewportContainer), nameof(gv_ViewportFrame), nameof(gv_ViewportMask), nameof(groupHolder2ba))] private Void groupHolder2b;
    [SerializeField, HideProperty, Rename("Background")] public Sprite gv_ViewportContainer;
    [SerializeField, HideProperty, Rename("Frame")] public Sprite gv_ViewportFrame;
    [SerializeField, HideProperty, Rename("Mask")] public Sprite gv_ViewportMask; 

    #region Labels
    [SerializeField, HideProperty, FoldoutGroup("Labels", false, nameof(gv_PlayerNumbers), nameof(gv_COMHeader))] private Void groupHolder2ba;
    [SerializeField, HideProperty, Rename("Player")] public Sprite[] gv_PlayerNumbers;
    [SerializeField, HideProperty, Rename("Computer")] public Sprite gv_COMHeader;
    #endregion

    #endregion

    #region Target Score

    [SerializeField, HideProperty, FoldoutGroup("Target Score", false, nameof(gts_Background), nameof(gts_Numbers), nameof(gts_symbolColor), nameof(gts_symbolShadow))] private Void groupHolder2c;
    [SerializeField, HideProperty, Rename("Background")] public Sprite gts_Background;
    [SerializeField, HideProperty, Rename("Symbols")]  public Sprite[] gts_Numbers;
    [SerializeField, HideProperty, Rename("Symbol Color")] public Color gts_symbolColor = Color.white;
    [SerializeField, HideProperty, Rename("Symbol Shadow")] public bool gts_symbolShadow = true;

    #endregion

    #region Player Scorecards
    [SerializeField, HideProperty, FoldoutGroup("Player Scorecards", false, nameof(groupHolder2da), nameof(groupHolder2db), nameof(groupHolder2dc))] private Void groupHolder2d;

    #region Containers
    [SerializeField, HideProperty, FoldoutGroup("Containers", false, nameof(gs_Container))] private Void groupHolder2da;
    [SerializeField, HideProperty, Rename("Background")] public Sprite gs_Container;
    #endregion

    #region Labels
    [SerializeField, HideProperty, FoldoutGroup("Labels", false, nameof(gs_PlayerHeader), nameof(gs_COMHeader), nameof(gs_HeaderNumbers), nameof(gs_ScoreNumbers))] private Void groupHolder2db;
    [SerializeField, HideProperty, Rename("Player")] public Sprite gs_PlayerHeader;
    [SerializeField, HideProperty, Rename("Computer")] public Sprite gs_COMHeader;
    [SerializeField, HideProperty, Rename("Slots")] public Sprite[] gs_HeaderNumbers;
    [SerializeField, HideProperty, Rename("Symbols")] public Sprite[] gs_ScoreNumbers;
    #endregion

    #region Icons
    [SerializeField, HideProperty, FoldoutGroup("Icons", false, nameof(ms_PacMan), nameof(ms_GhostBase), nameof(ms_GhostEyes))] private Void groupHolder2dc;
    [SerializeField, HideProperty, Rename("Pac-Man")] public Sprite gs_PacMan;
    [SerializeField, HideProperty, Rename("Ghost")] public Sprite gs_GhostBase;
    [SerializeField, HideProperty, Rename("Ghost Eyes")] public Sprite gs_GhostEyes;
    #endregion

    #endregion

    #region Radar
    [SerializeField, HideProperty, FoldoutGroup("Radar", false, nameof(gr_Container), nameof(gr_PelletIcon), nameof(gr_ScoreNumbers))] private Void groupHolder2e;
    [SerializeField, HideProperty, Rename("Background")] public Sprite gr_Container;
    [SerializeField, HideProperty, Rename("Pellet Symbol")] public Sprite gr_PelletIcon;
    [SerializeField, HideProperty, Rename("Symbols")] public Sprite[] gr_ScoreNumbers;

    #endregion

    #endregion

}
