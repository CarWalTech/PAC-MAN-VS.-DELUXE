using System.Collections.Generic;
using System.Linq;
using SimpleSpritePacker;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(menuName = "Pac-Man VS/Maze Configuration/Viewport Skin")]
[System.Serializable]
public class InterfaceTheme : ScriptableObject
{
    public string themeName;
    public string themeUUID;

    [Header("Maze Sidebars")]
    public Sprite sb_Frame;
    public Sprite sb_ViewportFrame; 
    public Sprite sb_ViewportMask; 
    public Sprite sb_ViewportPanel;

    [Header("Maze Scorecard")]
    public Sprite ms_Container;
    public Sprite ms_ContainerHue;
    public Sprite ms_ContainerShadow;
    public Sprite ms_PlayerHeader;
    public Sprite ms_COMHeader;
    public Sprite[] ms_HeaderNumbers;
    public Sprite[] ms_ScoreNumbers;
    public Sprite ms_PacMan;
    public Sprite ms_GhostBase;
    public Sprite ms_GhostEyes;

    [Header("Maze Target")]
    public Sprite mts_Background;
    public Sprite[] mts_Numbers;

    [Header("Ghost Viewport (Maze)")]
    public Sprite gvm_ViewportContainer;
    public Sprite gvm_ViewportFrame; 
    public Sprite gvm_ViewportMask; 
    public Sprite[] gvm_PlayerNumbers;
    public Sprite gvm_COMHeader;

    [Header("Ghost Viewport (Normal)")]
    public Sprite gv_ViewportFrame;
    public Sprite gv_ViewportMask; 
    public Sprite[] gv_PlayerNumbers;
    public Sprite gv_COMHeader;

    [Header("Ghost Target")]
    public Sprite gts_Background;
    public Sprite[] gts_Numbers;

    [Header("Ghost Scorecard")]
    public Sprite gs_Container;
    public Sprite gs_PlayerHeader;
    public Sprite gs_COMHeader;
    public Sprite[] gs_HeaderNumbers;
    public Sprite[] gs_ScoreNumbers;
    public Sprite gs_PacMan;
    public Sprite gs_GhostBase;
    public Sprite gs_GhostEyes;

    [Header("Ghost Radar")]
    public Sprite gr_Container;
    public Sprite gr_PelletIcon;
    public Sprite[] gr_ScoreNumbers;
}
