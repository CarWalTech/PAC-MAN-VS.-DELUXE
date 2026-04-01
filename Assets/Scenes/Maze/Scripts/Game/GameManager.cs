using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

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

    void Start()
    {
        if (GameConfiguration.GameEnterMode == GameConfiguration.EnterMode.Normal)
            GameConfiguration.MatchEvent_SetupMatch();
        else if (GameConfiguration.GameEnterMode == GameConfiguration.EnterMode.Disabled)
            GameConfiguration.MatchEvent_SetupDev();

        entities.Setup();

        foreach (Transform spawnTransform in maze.GetSpawnPoints())
        {
            GameObject spawnPoint = spawnTransform.gameObject;

            if (!spawnPoint.GetComponent<IEntitySpawner>())
                continue;
            else if (CollectSpawnPoint<Spawnpoint_Pacman>("PacMan", spawnPoint, entities.GetPacMan(), maze.GetMaze2D().pacManInitalDirection))
                continue;
            else if (CollectSpawnPoint<Spawnpoint_Ghost1>("ChaserP1", spawnPoint, entities.GetGhost(GameManager_Entities.GhostSelector.G1), maze.GetMaze2D().ghostP1InitalDirection))
                continue;
            else if (CollectSpawnPoint<Spawnpoint_Ghost2>("ChaserP2", spawnPoint, entities.GetGhost(GameManager_Entities.GhostSelector.G2), maze.GetMaze2D().ghostP2InitalDirection))
                continue;
            else if (CollectSpawnPoint<Spawnpoint_Ghost3>("ChaserP3", spawnPoint, entities.GetGhost(GameManager_Entities.GhostSelector.G3), maze.GetMaze2D().ghostP3InitalDirection))
                continue;
            else if (CollectSpawnPoint<Spawnpoint_Ghost4>("ChaserP4", spawnPoint, entities.GetGhost(GameManager_Entities.GhostSelector.G4), maze.GetMaze2D().ghostP4InitalDirection))
                continue;
            else if (CollectSpawner<Spawner_Fruit>("FruitSpawner", spawnPoint))
                continue;
        }

        AssignPlayerSlot<PacMan>(entities.GetPacMan(), GameConfiguration.PlayerSlot_PacMan);
        AssignPlayerSlot<Ghost>(entities.GetGhost(GameManager_Entities.GhostSelector.G1), GameConfiguration.PlayerSlot_GhostP1);
        AssignPlayerSlot<Ghost>(entities.GetGhost(GameManager_Entities.GhostSelector.G2), GameConfiguration.PlayerSlot_GhostP2);
        AssignPlayerSlot<Ghost>(entities.GetGhost(GameManager_Entities.GhostSelector.G3), GameConfiguration.PlayerSlot_GhostP3);
        AssignPlayerSlot<Ghost>(entities.GetGhost(GameManager_Entities.GhostSelector.G4), GameConfiguration.PlayerSlot_GhostP4);

        foreach (KeyValuePair<string, SpawnData> pair in _spawns)
        {
            if (pair.Value is not SpawnpointData)
                continue;

            var data = (SpawnpointData)pair.Value;
            var player = data.player;
            if (player)
            {
                if (player.GetComponent<PacMan>())
                {
                    print("pacman");
                    var pacman = player.GetComponent<PacMan>();
                    pacman.SetSpeed(matchSettings.pacManSpeed);
                    pacman.Setup(data, maze.GetMaze2D());
                }
                else if (player.GetComponent<Ghost>())
                {
                    var ghost = player.GetComponent<Ghost>();
                    ghost.SetSpeed(matchSettings.ghostSpeed);
                    ghost.SetSightRange(matchSettings.ghostSight);
                    ghost.Setup(data, maze.GetMaze2D());
                }
            }
        }

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

    #region Assignment

    public void AssignPlayerSlot<T>(GameObject player, PlayerSlot slot) where T : IPlayable
    {
        if (player) player.GetComponent<T>().SetPlayerID(slot);
    }

    #endregion

    #region Collectors

    public bool CollectSpawnPoint<T>(string key, GameObject spawnTarget, GameObject spawnEntity, Vector2 direction) where T : IEntitySpawnpoint
    {
        var spawnTransform = spawnTarget.transform;
        var spawnPoint = spawnTarget.GetComponent<T>();
        if (spawnPoint != null)
        {
            _spawns.Add(key, new SpawnpointData(spawnPoint, spawnEntity, spawnTransform.position, maze.GetWorldTileCoords(spawnTransform), direction));
            _current_players.Add(key);
            return true;
        }
        else return false;
            
    }
    public bool CollectSpawner<T>(string key, GameObject spawnTarget) where T : IEntitySpawner
    {
        var spawnTransform = spawnTarget.transform;
        var spawnPoint = spawnTarget.GetComponent<T>();
        if (spawnPoint != null)
        {
            _spawns.Add(key, new SpawnData(spawnPoint, spawnTransform.position, maze.GetWorldTileCoords(spawnTransform)));
            return true;
        }
        else return false;
    }
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

    #region Refresh

    public void RefreshSkin()
    {
        skin.RefreshSkin();
    }

    #endregion

}
