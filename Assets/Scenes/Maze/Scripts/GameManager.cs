using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public class SpawnData
    {
        public GameObject player;
        public Vector3 mazeOrigin;
        public Vector3 worldOrigin;
        public Vector2 initalDirection;

        public SpawnData(GameObject p, Vector3 a1, Vector3 a2, Vector2 a3)
        {
            player = p;
            mazeOrigin = a1;
            worldOrigin = a2;
            initalDirection = a3;
        }
    }


    public static GameManager Instance { get; private set; } = null;
    public static bool IsReady { get; private set; } = false;



    #region Properties: Managers
    [Header("Managers")]
    [SerializeField] private CameraManager gameCameras;
    [SerializeField] private MazeManager gameMaze;
    [SerializeField] private MatchManager matchManager;
    #endregion

    #region Properties: Entities
    [Header("Chasers")]
    [SerializeField] public GameObject mazePlayer = null;
    [SerializeField] public GameObject mazeChaserP1 = null;
    [SerializeField] public GameObject mazeChaserP2 = null;
    [SerializeField] public GameObject mazeChaserP3 = null;
    [SerializeField] public GameObject mazeChaserP4 = null;

    [Header("Entities")]
    [SerializeField] public GameObject mazePacMan;
    [SerializeField] public GameObject mazePacManCOM;
    [SerializeField] public GameObject mazeGhostP1;
    [SerializeField] public GameObject mazeGhostP2;
    [SerializeField] public GameObject mazeGhostP3;
    [SerializeField] public GameObject mazeGhostP4;
    [SerializeField] public GameObject mazeGhostP5;
    [SerializeField] public GameObject mazeGhostCOM1;
    [SerializeField] public GameObject mazeGhostCOM2;
    [SerializeField] public GameObject mazeGhostCOM3;
    [SerializeField] public GameObject mazeGhostCOM4;
    [SerializeField] public GameObject mazeGhostClyde;
    [SerializeField] public GameObject mazeGhostInky;
    [SerializeField] public GameObject mazeGhostBlinky;
    [SerializeField] public GameObject mazeGhostPinky;

    #endregion

    #region Properties: Cache Objects
    [Header("Cache Objects")]
    [SerializeField] private GameObject mazeViewRuntime;
    [SerializeField] private GameObject worldViewRuntime;
    #endregion

    #region Properties: State Stuff
    private Dictionary<string, SpawnData> _spawns = new Dictionary<string, SpawnData>();
    private List<Ghost> _chasers = new List<Ghost>();
    private List<Pellet> _pellets = new List<Pellet>();
    private List<Fruit> _fruits = new List<Fruit>();
    private List<string> _current_players = new List<string>();

    private int __pellets_left = 0;

    #endregion

    #region Common


    private void LoadState()
    {
        if (GameConfiguration.GameEnterMode == GameConfiguration.EnterMode.Normal)
        {
            GameConfiguration.Event_LoadState(ref matchManager);
            mazePlayer = GameConfiguration.GetPlayerObject(this);
            gameCameras.viewMode = GameConfiguration.GetCameraFocus();
            mazeChaserP1 = matchManager.playerCount >= 2 ? GameConfiguration.GetChaserObject(this, 0) : null;
            mazeChaserP2 = matchManager.playerCount >= 3 ? GameConfiguration.GetChaserObject(this, 1) : null;
            mazeChaserP3 = matchManager.playerCount >= 4 ? GameConfiguration.GetChaserObject(this, 2) : null;
            mazeChaserP4 = matchManager.playerCount >= 5 ? GameConfiguration.GetChaserObject(this, 3) : null;
        }
        else if (GameConfiguration.GameEnterMode == GameConfiguration.EnterMode.Disabled)
        {
            GameConfiguration.Event_LoadDevState(this);
        }
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => gameMaze != null && gameMaze.isLoaded);

        LoadState();
        SetupEntities();
        SetupSpawns();
        SetupPlayers();

        gameCameras.UpdateViewports();

        IsReady = true;

        Event_StartGame();
    }

    private void Update()
    {
        if (!IsReady) return;
        OnInput();
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

    private void SetupEntities()
    {
        mazeGhostBlinky.GetComponent<RedGhostAI>().scatterModeTarget = gameMaze.GetMazeScatterTarget(MazeManager.MazeScatterTarget.Red);
        mazeGhostBlinky.GetComponent<RedGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostBlinky.GetComponent<RedGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);
        
        mazeGhostInky.GetComponent<BlueGhostAI>().scatterModeTarget = gameMaze.GetMazeScatterTarget(MazeManager.MazeScatterTarget.Blue);
        mazeGhostInky.GetComponent<BlueGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostInky.GetComponent<BlueGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);

        mazeGhostPinky.GetComponent<PinkGhostAI>().scatterModeTarget = gameMaze.GetMazeScatterTarget(MazeManager.MazeScatterTarget.Pink);
        mazeGhostPinky.GetComponent<PinkGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostPinky.GetComponent<PinkGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);

        mazeGhostClyde.GetComponent<OrangeGhostAI>().scatterModeTarget = gameMaze.GetMazeScatterTarget(MazeManager.MazeScatterTarget.Orange);
        mazeGhostClyde.GetComponent<OrangeGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostClyde.GetComponent<OrangeGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);

        mazeGhostCOM1.GetComponent<GrayGhostAI>().scatterModeTarget = gameMaze.GetMazeScatterTarget(MazeManager.MazeScatterTarget.Red);
        mazeGhostCOM1.GetComponent<GrayGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostCOM1.GetComponent<GrayGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);

        mazeGhostCOM2.GetComponent<GrayGhostAI>().scatterModeTarget = gameMaze.GetMazeScatterTarget(MazeManager.MazeScatterTarget.Blue);
        mazeGhostCOM2.GetComponent<GrayGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostCOM2.GetComponent<GrayGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);
        
        mazeGhostCOM3.GetComponent<GrayGhostAI>().scatterModeTarget = gameMaze.GetMazeScatterTarget(MazeManager.MazeScatterTarget.Pink);
        mazeGhostCOM3.GetComponent<GrayGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostCOM3.GetComponent<GrayGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);
        
        mazeGhostCOM4.GetComponent<GrayGhostAI>().scatterModeTarget = gameMaze.GetMazeScatterTarget(MazeManager.MazeScatterTarget.Orange);
        mazeGhostCOM4.GetComponent<GrayGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostCOM4.GetComponent<GrayGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);
        
        mazeGhostP1.GetComponent<PlayerGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostP1.GetComponent<PlayerGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);

        mazeGhostP2.GetComponent<PlayerGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostP2.GetComponent<PlayerGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);
        
        mazeGhostP3.GetComponent<PlayerGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostP3.GetComponent<PlayerGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);

        mazeGhostP4.GetComponent<PlayerGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostP4.GetComponent<PlayerGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);

        mazeGhostP5.GetComponent<PlayerGhostAI>().enterHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeEnter);
        mazeGhostP5.GetComponent<PlayerGhostAI>().exitHomeWayPoints = gameMaze.GetMazeWaypoints(MazeManager.MazeWaypoints.HomeExit);

    }
    private void SetupSpawns()
    {
        void CreateSpawnPoint(string key, SpawnData spawnData)
        {
            _spawns.Add(key, spawnData);
            _current_players.Add(key);
        }

        foreach (Transform nodeTransform in gameMaze.GetSpawnPoints())
        {
            GameObject nodeObject = nodeTransform.gameObject;
            var worldCoords = gameMaze.GetWorldTileCoords(nodeTransform);
            
            var mazeCoords = nodeTransform.position;

            if (nodeObject.GetComponent<PacSpawn>())
                CreateSpawnPoint("PacMan", new SpawnData(mazePlayer, mazeCoords, worldCoords, gameMaze.GetMaze2D().pacManInitalDirection));
            else if (nodeObject.GetComponent<ChaseSpawnP1>() && mazeChaserP1 != null)
                CreateSpawnPoint("ChaserP1", new SpawnData(mazeChaserP1, mazeCoords, worldCoords, gameMaze.GetMaze2D().ghostP1InitalDirection));
            else if (nodeObject.GetComponent<ChaseSpawnP2>() && mazeChaserP2 != null)
                CreateSpawnPoint("ChaserP2", new SpawnData(mazeChaserP2, mazeCoords, worldCoords, gameMaze.GetMaze2D().ghostP2InitalDirection));
            else if (nodeObject.GetComponent<ChaseSpawnP3>() && mazeChaserP3 != null)
                CreateSpawnPoint("ChaserP3", new SpawnData(mazeChaserP3, mazeCoords, worldCoords, gameMaze.GetMaze2D().ghostP3InitalDirection));
            else if (nodeObject.GetComponent<ChaseSpawnP4>() && mazeChaserP4 != null)
                CreateSpawnPoint("ChaserP4", new SpawnData(mazeChaserP4, mazeCoords, worldCoords, gameMaze.GetMaze2D().ghostP4InitalDirection));
            else if (nodeObject.GetComponent<FruitSpawn>())
                Fruit.SetupFruit(nodeObject.GetComponent<FruitSpawn>(), mazeCoords);
        }
    }
    private void SetupPlayers()
    {
        //Debug.Log(string.Format("Pac-Man || Slot:{0} | Controller:{1}", GameConfiguration.PlayerSlot_PacMan, GameConfiguration.InputSource_PacMan));
        //Debug.Log(string.Format("Ghost P1 || Slot:{0} | Controller:{1}", GameConfiguration.PlayerSlot_GhostP1, GameConfiguration.InputSource_GhostP1));
        //Debug.Log(string.Format("Ghost P2 || Slot:{0} | Controller:{1}", GameConfiguration.PlayerSlot_GhostP2, GameConfiguration.InputSource_GhostP2));
        //Debug.Log(string.Format("Ghost P3 || Slot:{0} | Controller:{1}", GameConfiguration.PlayerSlot_GhostP3, GameConfiguration.InputSource_GhostP3));
        //Debug.Log(string.Format("Ghost P4 || Slot:{0} | Controller:{1}", GameConfiguration.PlayerSlot_GhostP4, GameConfiguration.InputSource_GhostP4));


        if (mazePlayer) mazePlayer.GetComponent<PacMan>().SetPlayerID(GameConfiguration.PlayerSlot_PacMan);
        if (mazeChaserP1) mazeChaserP1.GetComponent<Ghost>().SetPlayerID(GameConfiguration.PlayerSlot_GhostP1);
        if (mazeChaserP2) mazeChaserP2.GetComponent<Ghost>().SetPlayerID(GameConfiguration.PlayerSlot_GhostP2);
        if (mazeChaserP3) mazeChaserP3.GetComponent<Ghost>().SetPlayerID(GameConfiguration.PlayerSlot_GhostP3);
        if (mazeChaserP4) mazeChaserP4.GetComponent<Ghost>().SetPlayerID(GameConfiguration.PlayerSlot_GhostP4);

        foreach (string id in _current_players)
        {
            var data = _spawns[id];
            var player = data.player;
            if (player)
            {
                if (player.GetComponent<PacMan>())
                {
                    var pacman = player.GetComponent<PacMan>();
                    pacman.SetSpeed(matchManager.pacManSpeed);
                    pacman.Setup(data, gameMaze.GetMaze2D());
                }
                else if (player.GetComponent<Ghost>())
                {
                    var ghost = player.GetComponent<Ghost>();
                    ghost.SetSpeed(matchManager.ghostSpeed);
                    ghost.SetSightRange(matchManager.ghostSight);
                    ghost.Setup(data, gameMaze.GetMaze2D());
                }
            }
        }
    }

    #endregion

    #region Events

    private void OnInput()
    {
        if (Input.GetKeyDown(KeyCode.C)) gameCameras.SwitchViewModes();
        if (Input.GetKeyDown(KeyCode.Escape) && GameConfiguration.GameExitMode != GameConfiguration.ExitMode.Disabled) GameConfiguration.Event_ReturnToMenu();
    }


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
            mazePlayer.GetComponent<PacMan>().Unfreeze();
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

            mazePlayer.GetComponent<PacMan>().ResetState();

            Event_StartGame();
        }
    }

    public void Event_EatPellet(IPlayable src, Pellet pellet)
    {
        GameConfiguration.Score_Add(src.GetPlayerID(), 10);
        __pellets_left -= 1;
        pellet.OnEat();
    }
    public void Event_EatPowerPellet(IPlayable src, PowerPellet pellet)
    {
        foreach (Ghost chaser in _chasers)
        {
            chaser.Scare(matchManager.powerPelletDuration);
        }
        gameMaze.Scare(matchManager.powerPelletDuration);
        Event_EatPellet(src, pellet);
    }
    public void Event_EatPlayer(IPlayable src)
    {
        var pacman = mazePlayer.GetComponent<PacMan>();
        pacman.Anim_DeathSequence();
        GameConfiguration.Event_SwapPacMan(src.GetPlayerID());
        GameConfiguration.Score_Sub(pacman.GetPlayerID(), matchManager.pacManBonus);
        GameConfiguration.Score_Add(src.GetPlayerID(), matchManager.pacManBonus);
        foreach (Ghost chaser in _chasers)
        {
            chaser.Freeze();
        }
        Invoke(nameof(Event_ResetGame), 3f);
    }
    public void Event_EatChaser(Ghost ghost)
    {
        ghost.Eaten();
    }
    public void Event_EatFruit_PacMan(Fruit fruit, PacMan pacman)
    {
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
        gameCameras.Teleport_FadeOut(teleportable);
    }

    public void Teleport_FadeIn(IPlayable teleportable)
    {
        gameCameras.Teleport_FadeIn(teleportable);
    }

    #endregion

    #region Getters

    public PlayerSlot GetClientID()
    {
        return matchManager.clientPlayerID;
    }

    public int GetPelletsLeft()
    {
        return __pellets_left;
    }

    public MatchManager GetMatchManager()
    {
        return matchManager;
    }

    public CameraManager GetCameraManager()
    {
        return gameCameras;
    }

    public MazeManager GetMazeManager()
    {
        return gameMaze;
    }
    
    public GameObject GetWorldViewCache()
    {
        return worldViewRuntime;
    }
    public GameObject GetMazeViewCache()
    {
        return mazeViewRuntime;
    }
    public float GetGhostPauseEatenTime()
    {
        return matchManager.ghostPauseEatenTime;
    }



    #endregion

    #region Setters


    #endregion

    #region Collectors

    public void CollectCamera(IPlayable playable)
    {
        gameCameras.AddCamera(playable);
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
