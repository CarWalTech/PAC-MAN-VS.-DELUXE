using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class GameManager_Maze
{
    public enum MazeScatterTarget
    {
        Red,
        Blue,
        Orange,
        Pink,
        Player
    }
    public enum MazeWaypoints
    {
        HomeEnter,
        HomeExit
    }


    public LevelConfiguration level;
    
    [Header("Maze Configuration")]    
    //[SerializeField] private int m_mazeAssetsPPU = 100;
    //[SerializeField] private int m_mazeRefResolutionX = 320;
    //[SerializeField] private int m_mazeRefResolutionY = 180;
    [SerializeField] public Vector2Int mazeTileViewportSize = new Vector2Int(21, 21);

    [Header("World Configuration")]
    [SerializeField] private float originScale = 10;


    #region Properties: State Accessors

    public Vector2 mazeOrigin => _mazeData.mazeOrigin;
    public float mazeWidth => _mazeData.mazeWidth;
    public float mazeHeight => _mazeData.mazeHeight;
    public bool isLoaded => _isLoaded;

    #endregion

    #region Properties: Private Fields

    private GameObject _mazeObject = null;
    private GameObject _worldObject = null;
    private Maze2D _mazeData => _mazeObject.GetComponent<Maze2D>();
    private Maze3D _worldData => _worldObject.GetComponent<Maze3D>();
    private bool _isLoaded = false;

    #endregion

    #region Unity Messages

    public void Setup(MazeTheme mazeTheme, PelletTheme pelletTheme)
    {
        var mazeContainer = GameManager.Instance.GetEntityManager().GetMazeTiles();
        var worldContainer = GameManager.Instance.GetEntityManager().GetWorldTiles();
        _mazeObject = GameObject.Instantiate(level.levelTiles, mazeContainer.transform.position, mazeContainer.transform.rotation, mazeContainer.transform);
        _worldObject = GameObject.Instantiate(level.levelModel, worldContainer.transform.position, worldContainer.transform.rotation, worldContainer.transform);
        _mazeData.SetRuntimeSkin(mazeTheme, pelletTheme);
        _mazeData.UpdateStyles();
        _worldData.Adjust();
        _isLoaded = true;
    }

    private void Reset()
    {

    }

    private void Awake()
    {

    }
    
    private void Start()
    {

    }

    #endregion

    #region Getters

    public GameObject GetMazeScatterTarget(MazeScatterTarget target)
    {
        switch (target)
        {
            case MazeScatterTarget.Red:
                return _mazeData.redScatterTarget;
            case MazeScatterTarget.Blue:
                return _mazeData.blueScatterTarget;
            case MazeScatterTarget.Pink:
                return _mazeData.pinkScatterTarget;
            case MazeScatterTarget.Orange:
                return _mazeData.orangeScatterTarget;
            case MazeScatterTarget.Player:
                return _mazeData.redScatterTarget;
            default:
                return null;
        }
    }

    public Transform[] GetMazeWaypoints(MazeWaypoints target)
    {
        switch (target)
        {
            case MazeWaypoints.HomeEnter:
                return _mazeData.enterHomeWaypoints;
            case MazeWaypoints.HomeExit:
                return _mazeData.exitHomeWaypoints;
            default:
                return null;
        }
    }

    public Maze2D GetMaze2D()
    {
        return _mazeData;
    }

    public List<Transform> GetPellets()
    {
        var items = _mazeData.pellets.transform.Cast<Transform>();
        return items.ToList();
    }

    public List<Transform> GetSpawnPoints()
    {
        var items = _mazeData.spawns.transform.Cast<Transform>();
        if (_mazeData.additionalSpawns != null)
        {   
            var extras = _mazeData.additionalSpawns.transform.Cast<Transform>();
            items.Concat(extras);
        }
        return items.ToList();
    }

    public Transform GetWorldOrigin()
    {
        return GameManager.Instance.GetEntityManager().GetWorldOrigin();
    }
    public float GetWorldOriginScale()
    {
        return originScale;
    }
    public Vector3 GetWorldCoords(Vector3 target, Vector3 mazeOrigin, Vector3 worldOrigin)
    {
        Vector3 source_offset = mazeOrigin - target;
        float new_x = worldOrigin.x - (source_offset.x * originScale);
        float new_y = worldOrigin.y - (source_offset.y * originScale);
        return new Vector3(new_x, new_y, worldOrigin.z);
    }
    public Vector3 GetWorldTileCoords(Transform transform)
    {
        var spawnPos = transform.localPosition;
        var worldOrigin = GetWorldOrigin();
        var x = worldOrigin.position.x + spawnPos.x * originScale;
        var y = worldOrigin.position.y + spawnPos.y * originScale;
        var z = worldOrigin.position.z;

        return new Vector3(x, y, z);
    }

    #endregion

    #region Instantiate

    public GameObject InstantiateInWorld(GameObject prefab, Vector3 localPosition, GameObject parent)
    {
        var originScale = GetWorldOriginScale();
        var worldOrigin = GetWorldOrigin();

        var x = worldOrigin.position.x + localPosition.x * originScale;
        var y = worldOrigin.position.y + localPosition.y * originScale;
        var z = worldOrigin.position.z;

        Vector3 spawnPosition = new Vector3(x, y, z);
        Vector3 spawnRotation = Vector3.zero;

        return GameObject.Instantiate(prefab, spawnPosition, Quaternion.Euler(spawnRotation), parent.transform);
    }

    #endregion

    public void Scare(float duration)
    {
        _worldData.Scare(duration);
    }


}