using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Maze2D : MonoBehaviour
{
    [SerializeField] public float mazeWidth;
    [SerializeField] public float mazeHeight;
    [SerializeField] public Vector2 mazeOrigin = Vector2.zero;

    public GameObject pellets;
    public GameObject walls;
    public GameObject spawns;
    public GameObject additionalSpawns = null;
    public GameObject colorLayer = null;

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
    
    private void Awake()
    {
        spawns.GetComponent<TilemapRenderer>().enabled = false;
        if (additionalSpawns) additionalSpawns.GetComponent<TilemapRenderer>().enabled = false;
    }
    private void Start()
    {

    }

    public void UpdateStyles()
    {
        foreach (var tilemap in GetComponentsInChildren<Tilemap>(true))
        {
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            for (int x = 0; x < bounds.size.x; x++) 
            {
                for (int y = 0; y < bounds.size.y; y++) 
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null && tile is MazeTile)
                    {
                        var themableTile = (MazeTile)tile;
                        themableTile.theme = GameManager.Instance.GetMazeManager().theme; 
                    }   
                }
            }  
            tilemap.RefreshAllTiles();
        }
        
        if (colorLayer)
            colorLayer.SetActive(GameManager.Instance.GetMazeManager().GetRecolorSupportState());
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
