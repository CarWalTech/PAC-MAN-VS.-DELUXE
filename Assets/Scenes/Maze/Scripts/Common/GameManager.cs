using System;
using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;
    public static bool IsReady { get; private set; } = false;


    [SerializeField] private GameManager_Cameras cameras;
    [SerializeField] private GameManager_Maze maze;
    [SerializeField] private GameManager_Match matchSettings;
    [SerializeField] private GameManager_Skin skin;
    [SerializeField] private GameManager_Entities entities;

    private Dictionary<string, SpawnData> _spawns = new Dictionary<string, SpawnData>();
    private List<Ghost> _chasers = new List<Ghost>();
    private List<Pellet> _pellets = new List<Pellet>();
    private List<Fruit> _fruits = new List<Fruit>();
    private List<string> _current_players = new List<string>();
    private int __pellets_left = 0;
    private int __currentGhostsEaten = 0;


    #region Common

    public void RefreshSkin()
    {
        skin.RefreshSkin();
    }

    private void LoadState()
    {
        if (GameConfiguration.GameEnterMode == GameConfiguration.EnterMode.Normal)
        {
            skin.guiTheme = GameConfiguration.GuiTheme;
            skin.mazeTheme = GameConfiguration.MazeTheme;
            skin.RefreshSkin();

            maze.level = GameConfiguration.MazeData;
            maze.Setup(skin.mazeTheme, skin.pelletTheme);

            GameConfiguration.Event_LoadState(ref matchSettings);

            entities.pacman = (int)GameConfiguration.GetPacmanSelector();
            entities.ghost1 = (int)GameConfiguration.GetGhostSelector(this, matchSettings.playerCount >= 2 ? (int)GameManager_Entities.GhostSelector.G1 : -1);
            entities.ghost2 = (int)GameConfiguration.GetGhostSelector(this, matchSettings.playerCount >= 3 ? (int)GameManager_Entities.GhostSelector.G2 : -1);
            entities.ghost3 = (int)GameConfiguration.GetGhostSelector(this, matchSettings.playerCount >= 4 ? (int)GameManager_Entities.GhostSelector.G3 : -1);
            entities.ghost4 = (int)GameConfiguration.GetGhostSelector(this, matchSettings.playerCount >= 5 ? (int)GameManager_Entities.GhostSelector.G4 : -1);

            cameras.viewMode = GameConfiguration.GetCameraFocus();

            
        }
        else if (GameConfiguration.GameEnterMode == GameConfiguration.EnterMode.Disabled)
        {
            maze.Setup(skin.mazeTheme, skin.pelletTheme);
            GameConfiguration.Event_LoadDevState();
        }
    }

    void Start()
    {
        LoadState();
        entities.Setup();
        SetupSpawns();
        SetupPlayers();

        cameras.UpdateViewports();

        IsReady = true;

        Event_StartGame();
    }

    private void Update()
    {
        cameras.Update();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    #endregion

    #region Setup


    private void SetupSpawns()
    {
        void CreateSpawnPoint(string key, SpawnData spawnData)
        {
            _spawns.Add(key, spawnData);
            _current_players.Add(key);
        }

        foreach (Transform pelletTransform in maze.GetPellets())
        {
            GameObject pelletObject = pelletTransform.gameObject;

            if (pelletObject.GetComponent<Pellet>())
                pelletObject.GetComponent<Pellet>().SetSkin(skin.pelletTheme);

            else if (pelletObject.GetComponent<PowerPellet>())
                pelletObject.GetComponent<PowerPellet>().SetSkin(skin.pelletTheme);
        }

        foreach (Transform nodeTransform in maze.GetSpawnPoints())
        {
            GameObject nodeObject = nodeTransform.gameObject;

            var worldCoords = maze.GetWorldTileCoords(nodeTransform);
            var mazeCoords = nodeTransform.position;

            if (nodeObject.GetComponent<PacSpawn>())
                CreateSpawnPoint("PacMan", new SpawnData(entities.GetPacMan(), mazeCoords, worldCoords, maze.GetMaze2D().pacManInitalDirection));
            else if (nodeObject.GetComponent<ChaseSpawnP1>() && entities.GetGhost(GameManager_Entities.GhostSelector.G1) != null)
                CreateSpawnPoint("ChaserP1", new SpawnData(entities.GetGhost(GameManager_Entities.GhostSelector.G1), mazeCoords, worldCoords, maze.GetMaze2D().ghostP1InitalDirection));
            else if (nodeObject.GetComponent<ChaseSpawnP2>() && entities.GetGhost(GameManager_Entities.GhostSelector.G2) != null)
                CreateSpawnPoint("ChaserP2", new SpawnData(entities.GetGhost(GameManager_Entities.GhostSelector.G2), mazeCoords, worldCoords, maze.GetMaze2D().ghostP2InitalDirection));
            else if (nodeObject.GetComponent<ChaseSpawnP3>() && entities.GetGhost(GameManager_Entities.GhostSelector.G3) != null)
                CreateSpawnPoint("ChaserP3", new SpawnData(entities.GetGhost(GameManager_Entities.GhostSelector.G3), mazeCoords, worldCoords, maze.GetMaze2D().ghostP3InitalDirection));
            else if (nodeObject.GetComponent<ChaseSpawnP4>() && entities.GetGhost(GameManager_Entities.GhostSelector.G4) != null)
                CreateSpawnPoint("ChaserP4", new SpawnData(entities.GetGhost(GameManager_Entities.GhostSelector.G4), mazeCoords, worldCoords, maze.GetMaze2D().ghostP4InitalDirection));
            else if (nodeObject.GetComponent<FruitSpawn>())
                Fruit.SetupFruit(nodeObject.GetComponent<FruitSpawn>(), mazeCoords);
        }
    }
    private void SetupPlayers()
    {
        if (entities.GetPacMan()) entities.GetPacMan().GetComponent<PacMan>().SetPlayerID(GameConfiguration.PlayerSlot_PacMan);
        if (entities.GetGhost(GameManager_Entities.GhostSelector.G1)) entities.GetGhost(GameManager_Entities.GhostSelector.G1).GetComponent<Ghost>().SetPlayerID(GameConfiguration.PlayerSlot_GhostP1);
        if (entities.GetGhost(GameManager_Entities.GhostSelector.G2)) entities.GetGhost(GameManager_Entities.GhostSelector.G2).GetComponent<Ghost>().SetPlayerID(GameConfiguration.PlayerSlot_GhostP2);
        if (entities.GetGhost(GameManager_Entities.GhostSelector.G3)) entities.GetGhost(GameManager_Entities.GhostSelector.G3).GetComponent<Ghost>().SetPlayerID(GameConfiguration.PlayerSlot_GhostP3);
        if (entities.GetGhost(GameManager_Entities.GhostSelector.G4)) entities.GetGhost(GameManager_Entities.GhostSelector.G4).GetComponent<Ghost>().SetPlayerID(GameConfiguration.PlayerSlot_GhostP4);

        foreach (string id in _current_players)
        {
            var data = _spawns[id];
            var player = data.player;
            if (player)
            {
                if (player.GetComponent<PacMan>())
                {
                    var pacman = player.GetComponent<PacMan>();
                    pacman.skin = skin.pacmanTheme;
                    pacman.SetSpeed(matchSettings.pacManSpeed);
                    pacman.Setup(data, maze.GetMaze2D());
                }
                else if (player.GetComponent<Ghost>())
                {
                    var ghost = player.GetComponent<Ghost>();
                    ghost.skin = skin.ghostTheme;
                    ghost.SetSpeed(matchSettings.ghostSpeed);
                    ghost.SetSightRange(matchSettings.ghostSight);
                    ghost.Setup(data, maze.GetMaze2D());
                }
            }
        }
    }

    #endregion

    #region Events


    private void OnDestroy()
    {
        if (Instance == this)
        {
            IsReady = false;
            Instance = null;
        }
    }

    #endregion

    #region Commands

    public void Event_SwitchCameras()
    {
        cameras.SwitchViewModes();
    }

    public void Event_PauseGame()
    {
        if (GameConfiguration.GameExitMode != GameConfiguration.ExitMode.Disabled) GameConfiguration.Event_ReturnToMenu();
    }

    public void Event_SetupGame()
    {
        __pellets_left = _pellets.Count;
    }

    public void Event_StartGame()
    {
        IEnumerator Coroutine()
        {
            yield return new WaitForSecondsRealtime(3);
            foreach (Ghost chaser in _chasers)
            {
                chaser.Unfreeze();
            }
            entities.GetPacMan().GetComponent<PacMan>().Unfreeze();
        }
        Event_SetupGame();
        StartCoroutine(Coroutine());
    }
    public void Event_ResetGame()
    {
        if (GameConfiguration.GameExitMode != GameConfiguration.ExitMode.Disabled) GameConfiguration.Event_ReturnToMenu();
        else
        {
            foreach (Pellet pellet in _pellets)
            {
                pellet.ResetState();
            }

            foreach (Ghost chaser in _chasers)
            {
                chaser.ResetState();
            }

            foreach (Fruit fruit in _fruits)
            {
                fruit.ResetState();
            }

            entities.GetPacMan().GetComponent<PacMan>().ResetState();

            Event_StartGame();
        }
    }

    public void Event_SpawnScore(Vector3 origin, int points, float lifespan, ScorePopup.PopupBehavior behavior)
    {
        var mazeScorePopupPrefab = entities.GetPrefab(GameManager_Entities.PrefabSelector.ScorePopup);
        if (!mazeScorePopupPrefab) return;

        IEnumerator StartSpawn()
        {
            yield return new WaitForSeconds(0.1f);
            var result = Instantiate(mazeScorePopupPrefab, origin, Quaternion.Euler(Vector3.zero), GetMazeViewCache().transform);
            result.GetComponent<ScorePopup>().score = points;
            result.GetComponent<ScorePopup>().lifespan = lifespan;
            result.GetComponent<ScorePopup>().behavior = behavior;
            result.GetComponent<ScorePopup>().skin = skin.popupTheme;
            result.GetComponent<ScorePopup>().Spawn();
        }
        StartCoroutine(StartSpawn());
    }

    public void Event_EatPellet(IPlayable src, Pellet pellet)
    {
        if (src.GetMazeObject().GetComponent<PacMan>()) src.GetMazeObject().GetComponent<PacMan>().EatPellet();
        GameConfiguration.Score_Add(src.GetPlayerID(), pellet.points);
        Event_SpawnScore(pellet.transform.position, pellet.points, 1f, ScorePopup.PopupBehavior.Stationary);
        __pellets_left -= 1;
        pellet.OnEat();
    }
    public void Event_EatPowerPellet(IPlayable src, PowerPellet pellet)
    {
        __currentGhostsEaten = 0;
        foreach (Ghost chaser in _chasers) chaser.Scare(matchSettings.powerPelletDuration);
        maze.Scare(matchSettings.powerPelletDuration);
        Event_EatPellet(src, pellet);
    }
    public void Event_EatPlayer(IPlayable src)
    {
        var pacman = entities.GetPacMan().GetComponent<PacMan>();
        pacman.Anim_DeathSequence();
        GameConfiguration.Event_SwapPacMan(src.GetPlayerID());
        GameConfiguration.Score_Sub(pacman.GetPlayerID(), matchSettings.pacManBonus);
        GameConfiguration.Score_Add(src.GetPlayerID(), matchSettings.pacManBonus);
        foreach (Ghost chaser in _chasers) chaser.Freeze();
        Invoke(nameof(Event_ResetGame), 3f);
    }
    public void Event_EatChaser(Ghost ghost, PacMan pacman)
    {
        int points;
        __currentGhostsEaten += 1;
        switch (__currentGhostsEaten)
        {
            case 1:
                points = 200;
                break;
            case 2:
                points = 400;
                break;
            case 3:
                points = 800;
                break;
            case 4:
                points = 1600;
                break;
            case 5:
                points = 3200;
                break;
            default:
                points = 3200;
                break;
        }
        
        GameConfiguration.Score_Add(pacman.GetPlayerID(), points);
        Event_SpawnScore(ghost.transform.position, points, 2f, ScorePopup.PopupBehavior.MoveUp);
        ghost.Eaten();
    }
    public void Event_EatFruit_PacMan(Fruit fruit, PacMan pacman)
    {
        GameConfiguration.Score_Add(pacman.GetPlayerID(), fruit.points);
        Event_SpawnScore(fruit.transform.position, fruit.points, 2f, ScorePopup.PopupBehavior.MoveUp);
        pacman.EatFruit();
        fruit.OnEat();
    }
    public void Event_EatFruit_Ghost(Fruit fruit, Ghost ghost)
    {
        ghost.EatFruit();
        fruit.OnEat();
    }

    #endregion

    #region Teleporting

    public void Teleport_FadeOut(IPlayable teleportable)
    {
        cameras.Teleport_FadeOut(teleportable);
    }

    public void Teleport_FadeIn(IPlayable teleportable)
    {
        cameras.Teleport_FadeIn(teleportable);
    }

    #endregion

    #region Getters

    public PlayerSlot GetClientID()
    {
        return matchSettings.clientPlayerID;
    }

    public int GetPelletsLeft()
    {
        return __pellets_left;
    }

    public int GetPelletsEaten()
    {
        return _pellets.Count - __pellets_left;
    }

    public GameManager_Match GetMatchManager()
    {
        return matchSettings;
    }

    public GameManager_Skin GetSkinManager()
    {
        return skin;
    }

    public GameManager_Cameras GetCameraManager()
    {
        return cameras;
    }

    public GameManager_Maze GetMazeManager()
    {
        return maze;
    }

    public GameManager_Entities GetEntityManager()
    {
        return entities;
    }
    
    public GameObject GetWorldViewCache()
    {
        return entities.GetWorldCache();
    }
    public GameObject GetMazeViewCache()
    {
        return entities.GetMazeCache();
    }
    public float GetGhostPauseEatenTime()
    {
        return matchSettings.ghostPauseEatenTime;
    }



    #endregion

    #region Setters


    #endregion

    #region Collectors

    public void CollectCamera(IPlayable playable)
    {
        cameras.AddCamera(playable);
    }

    public void CollectPellet(Pellet pellet)
    {
        _pellets.Add(pellet);
        __pellets_left = _pellets.Count;
    }

    public void CollectChaser(Ghost ghost)
    {
        _chasers.Add(ghost);
    }

    public void CollectFruit(Fruit fruit)
    {
        _fruits.Add(fruit);
    }

    #endregion

}
