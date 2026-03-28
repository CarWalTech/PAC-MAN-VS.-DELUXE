using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using EditorAttributes;
using AYellowpaper.SerializedCollections;

[Serializable]
public class GameManager_Entities
{

    public enum PlayerObjectSelector : int {
        None,
        PacManPlayer,
        PacManCOM,
        P1,
        P2,
        P3,
        P4,
        P5,
        COM1,
        COM2,
        COM3,
        COM4,
        Clyde,
        Inky,
        Blinky,
        Pinky
    }
    public enum CacheObjectSelector : int
    {
        None,
        MazeRuntime,
        MazeTiles,
        WorldRuntime,
        WorldTiles,
        WorldOrigin,
    }
    public enum PrefabSelector: int
    {
        None,
        ScorePopup
    }
    public enum GhostSelector
    {
        G1,
        G2,
        G3,
        G4
    }


    private Dictionary<string, int> pacmanDropdownValues = new ()
    {
        { "None", (int)PlayerObjectSelector.None },
        { "Player", (int)PlayerObjectSelector.PacManPlayer },
        { "Computer", (int)PlayerObjectSelector.PacManCOM },
    };
    private Dictionary<string, int> ghostDropdownValues = new ()
    {
        { "None", (int)PlayerObjectSelector.None },
        { "Players/P1", (int)PlayerObjectSelector.P1 },
        { "Players/P2", (int)PlayerObjectSelector.P2 },
        { "Players/P3", (int)PlayerObjectSelector.P3 },
        { "Players/P4", (int)PlayerObjectSelector.P4 },
        { "Players/P5", (int)PlayerObjectSelector.P5 },
        { "Computers/COM1", (int)PlayerObjectSelector.COM1 },
        { "Computers/COM2", (int)PlayerObjectSelector.COM2 },
        { "Computers/COM3", (int)PlayerObjectSelector.COM3 },
        { "Computers/COM4", (int)PlayerObjectSelector.COM4 },
        { "Arcade Ghosts/Clyde", (int)PlayerObjectSelector.Clyde },
        { "Arcade Ghosts/Inky", (int)PlayerObjectSelector.Inky },
        { "Arcade Ghosts/Blinky", (int)PlayerObjectSelector.Blinky },
        { "Arcade Ghosts/Pinky", (int)PlayerObjectSelector.Pinky }
    };

    [SerializeField, Dropdown(nameof(pacmanDropdownValues))] public int pacman = (int)PlayerObjectSelector.None;
    [SerializeField, Dropdown(nameof(ghostDropdownValues))] public int ghost1 = (int)PlayerObjectSelector.None;
    [SerializeField, Dropdown(nameof(ghostDropdownValues))] public int ghost2 = (int)PlayerObjectSelector.None;
    [SerializeField, Dropdown(nameof(ghostDropdownValues))] public int ghost3 = (int)PlayerObjectSelector.None;
    [SerializeField, Dropdown(nameof(ghostDropdownValues))] public int ghost4 = (int)PlayerObjectSelector.None;
    [Space]
    [SerializeField, SerializedDictionary("Selector", "Object")] private SerializedDictionary<PlayerObjectSelector, GameObject> entities;
    [SerializeField, SerializedDictionary("Selector", "Prefab")] private SerializedDictionary<PrefabSelector, GameObject> prefabs;
    [SerializeField, SerializedDictionary("Selector", "Cache")] private SerializedDictionary<CacheObjectSelector, GameObject> cache;



    private GameObject GetPlayer(PlayerObjectSelector selector)
    {
        switch (selector)
        {
            case PlayerObjectSelector.None:
                return null;
            default:
                if (entities == null) return null;
                else if (!entities.ContainsKey(selector)) return null;
                else return entities[selector];

        }
    }
    private GameObject GetCache(CacheObjectSelector selector)
    {
        switch (selector)
        {
            case CacheObjectSelector.None:
                return null;
            default:
                if (cache == null) return null;
                else if (!cache.ContainsKey(selector)) return null;
                else return cache[selector];

        }
    }
    public GameObject GetPrefab(PrefabSelector selector)
    {
        switch (selector)
        {
            case PrefabSelector.None:
                return null;
            default:
                if (prefabs == null) return null;
                else if (!prefabs.ContainsKey(selector)) return null;
                else return prefabs[selector];

        }
    }


    public GameObject GetPacMan()
    {
        return GetPlayer((PlayerObjectSelector)pacman);
    }
    public GameObject GetGhost(GhostSelector id)
    {
        switch (id)
        {
            case GhostSelector.G1:
                return GetPlayer((PlayerObjectSelector)ghost1);
            case GhostSelector.G2:
                return GetPlayer((PlayerObjectSelector)ghost2);
            case GhostSelector.G3:
                return GetPlayer((PlayerObjectSelector)ghost3);
            case GhostSelector.G4:
                return GetPlayer((PlayerObjectSelector)ghost4);
            default:
                return null;
        }
    }


    public PlayerObjectSelector GetPacManType()
    {
        return (PlayerObjectSelector)pacman;
    }
    public PlayerObjectSelector GetGhostType(GhostSelector id)
    {
        switch (id)
        {
            case GhostSelector.G1:
                return (PlayerObjectSelector)ghost1;
            case GhostSelector.G2:
                return (PlayerObjectSelector)ghost2;
            case GhostSelector.G3:
                return (PlayerObjectSelector)ghost3;
            case GhostSelector.G4:
                return (PlayerObjectSelector)ghost4;
            default:
                return PlayerObjectSelector.None;
        }
    }


    public GameObject GetWorldCache()
    {
        return GetCache(CacheObjectSelector.WorldRuntime);
    }
    public GameObject GetWorldTiles()
    {
        return GetCache(CacheObjectSelector.WorldTiles);
    }
    public Transform GetWorldOrigin()
    {
        var result = GetCache(CacheObjectSelector.WorldOrigin);
        if (result) return result.GetComponent<Transform>();
        else return null;
    }    
    
    public GameObject GetMazeCache()
    {
        return GetCache(CacheObjectSelector.MazeRuntime);
    }
    public GameObject GetMazeTiles()
    {
        return GetCache(CacheObjectSelector.MazeTiles);
    }

    public void Setup()
    {
        void SetupGhost<T>(PlayerObjectSelector selector, GameManager_Maze.MazeScatterTarget scatterTarget) where T : GhostAI
        {
            if (entities == null) return;
            if (!entities.ContainsKey(selector)) return;

            var ghost = entities[selector];
            var maze = GameManager.Instance.GetMazeManager();

            ghost.GetComponent<T>().scatterModeTarget = maze.GetMazeScatterTarget(GameManager_Maze.MazeScatterTarget.Red);
            ghost.GetComponent<T>().enterHomeWayPoints = maze.GetMazeWaypoints(GameManager_Maze.MazeWaypoints.HomeEnter);
            ghost.GetComponent<T>().exitHomeWayPoints = maze.GetMazeWaypoints(GameManager_Maze.MazeWaypoints.HomeExit);
        }
        
        SetupGhost<RedGhostAI>(PlayerObjectSelector.Blinky, GameManager_Maze.MazeScatterTarget.Red);
        SetupGhost<BlueGhostAI>(PlayerObjectSelector.Inky, GameManager_Maze.MazeScatterTarget.Blue);
        SetupGhost<PinkGhostAI>(PlayerObjectSelector.Pinky, GameManager_Maze.MazeScatterTarget.Pink);
        SetupGhost<OrangeGhostAI>(PlayerObjectSelector.Clyde, GameManager_Maze.MazeScatterTarget.Orange);

        SetupGhost<GrayGhostAI>(PlayerObjectSelector.COM1, GameManager_Maze.MazeScatterTarget.Red);
        SetupGhost<GrayGhostAI>(PlayerObjectSelector.COM2, GameManager_Maze.MazeScatterTarget.Blue);
        SetupGhost<GrayGhostAI>(PlayerObjectSelector.COM3, GameManager_Maze.MazeScatterTarget.Pink);
        SetupGhost<GrayGhostAI>(PlayerObjectSelector.COM4, GameManager_Maze.MazeScatterTarget.Orange);

        SetupGhost<PlayerGhostAI>(PlayerObjectSelector.P1, GameManager_Maze.MazeScatterTarget.Player);
        SetupGhost<PlayerGhostAI>(PlayerObjectSelector.P2, GameManager_Maze.MazeScatterTarget.Player);
        SetupGhost<PlayerGhostAI>(PlayerObjectSelector.P3, GameManager_Maze.MazeScatterTarget.Player);
        SetupGhost<PlayerGhostAI>(PlayerObjectSelector.P4, GameManager_Maze.MazeScatterTarget.Player);
        SetupGhost<PlayerGhostAI>(PlayerObjectSelector.P5, GameManager_Maze.MazeScatterTarget.Player);
    }
}