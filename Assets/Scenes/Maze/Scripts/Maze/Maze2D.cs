using System.Collections;
using EditorAttributes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Maze2D : MonoBehaviour
{
    [SerializeField, ButtonField(nameof(RefreshTheme), "Refresh Theme")] private EditorAttributes.Void refreshHolder;

    [SerializeField] public float mazeWidth;
    [SerializeField] public float mazeHeight;
    [SerializeField] public Vector2 mazeOrigin = Vector2.zero;

    public GameObject pellets;
    public GameObject walls;
    public GameObject background = null;
    public GameObject spawns;
    public GameObject additionalSpawns = null;

    public GameObject colorLayer = null;
    public GameObject colorBackground = null;

    public GameObject redScatterTarget;
    public GameObject blueScatterTarget;
    public GameObject pinkScatterTarget;
    public GameObject orangeScatterTarget;

    public Transform[] enterHomeWaypoints;
    public Transform[] exitHomeWaypoints;


    [SerializeField] public Vector2 pacManInitalDirection = Vector2.zero;
    [SerializeField] public Vector2 ghostP1InitalDirection = Vector2.zero;
    [SerializeField] public Vector2 ghostP2InitalDirection = Vector2.zero;
    [SerializeField] public Vector2 ghostP3InitalDirection = Vector2.zero;
    [SerializeField] public Vector2 ghostP4InitalDirection = Vector2.zero;

    [SerializeField] private DevelopmentThemes editorTheme;
    [HideProperty] private MazeTheme _runtimeSkin;
    [HideProperty] private PelletTheme _runtimePelletSkin;
    
    private void Awake()
    {
        spawns.GetComponent<TilemapRenderer>().enabled = false;
        if (additionalSpawns) additionalSpawns.GetComponent<TilemapRenderer>().enabled = false;
    }
    private void Start()
    {

    }

    void OnValidate()
    {

    }

    private IMazeTheme GetMazeTheme()
    {
        if (_runtimeSkin != null) return _runtimeSkin;
        else return editorTheme;
    }

    private IPelletTheme GetPelletTheme()
    {
        if (_runtimePelletSkin != null) return _runtimePelletSkin;
        else return editorTheme;
    }

    private void ChangeTilemapTheme(Tilemap tilemap, MazeTheme theme, PelletTheme pelletTheme)
    {
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            for (int x = 0; x < bounds.size.x; x++) 
                for (int y = 0; y < bounds.size.y; y++) 
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null && tile is IMazeTile)
                    {
                        var themableTile = (IMazeTile)tile;
                        themableTile.SetTheme(theme); 
                    }   
                    else if (tile != null && tile is IPelletTile)
                    {
                        var themableTile = (IPelletTile)tile;
                        themableTile.SetTheme(pelletTheme); 
                    }
                }
            tilemap.RefreshAllTiles();
    }

    public void RefreshTheme()
    {
        UpdateStyles();
    }

    public void SetRuntimeSkin(MazeTheme skin, PelletTheme pelletSkin)
    {
        _runtimeSkin = skin;
        _runtimePelletSkin = pelletSkin;
    }

    public void UpdateStyles()
    {
        var theme = GetMazeTheme();
        var pelletTheme = GetPelletTheme();

        foreach (var tilemap in GetComponentsInChildren<Tilemap>(true))
            ChangeTilemapTheme(tilemap, theme.GetMazeTheme(), pelletTheme.GetPelletTheme());

        foreach (var colorFilters in GetComponentsInChildren<MazeTilemapColoring>(true))
            colorFilters.Refresh();

        if (colorLayer) 
            colorLayer.SetActive(theme.HasRecolorSupport());

        if (background) 
            background.SetActive(theme.HasBackgroundSupport());

        if (colorBackground) 
            colorBackground.SetActive(theme.HasBackgroundSupport() && theme.HasBackgroundRecolorSupport());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            new Vector3(mazeOrigin.x, mazeOrigin.y, 0),
            new Vector3(mazeWidth, mazeHeight, 0)
        );
    }
}
