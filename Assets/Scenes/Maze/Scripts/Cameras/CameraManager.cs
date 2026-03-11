using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public PacManView mazeView;
    public PacManView modernMazeView;
    public GhostsView ghostsView;
    public GhostsView ghostsViewP5;
    public GhostsView ghostsRetroView;

    [Header("Targets")]
    [SerializeField] public MatchViewMode viewMode = MatchViewMode.MazeView;
    private MatchViewMode _actualViewMode = MatchViewMode.Unset;

    [Header("Maze Cameras")]
    public MazeCamera viewportP1;
    public MazeCamera viewportP2;
    public MazeCamera viewportP3;
    public MazeCamera viewportP4;
    public MazeCamera viewportP5;

    [Header("World Cameras")]
    public WorldCamera worldViewportP1;
    public WorldCamera worldViewportP2;
    public WorldCamera worldViewportP3;
    public WorldCamera worldViewportP4;
    public WorldCamera worldViewportP5;

    [Header("Players")]
    public IPlayable player1 = null;
    public IPlayable player2 = null;
    public IPlayable player3 = null;
    public IPlayable player4 = null;
    public IPlayable player5 = null;

    private IGameCamera _pacmanMazeCamera = null;
    private IGameCamera _pacmanWorldCamera = null;
    private Dictionary<int, IGameCamera> _ghostMazeCameras = new Dictionary<int, IGameCamera>();
    private Dictionary<int, IGameCamera> _ghostWorldCameras = new Dictionary<int, IGameCamera>();
    private MazeManager _manager => GameManager.Instance.GetMazeManager();

    void Start()
    {
        //viewportP1.gameObject.SetActive(false);
        //viewportP2.gameObject.SetActive(false);
        //viewportP3.gameObject.SetActive(false);
        //viewportP4.gameObject.SetActive(false);
        //viewportP5.gameObject.SetActive(false);
        //worldViewportP1.gameObject.SetActive(false);
        //worldViewportP2.gameObject.SetActive(false);
        //worldViewportP3.gameObject.SetActive(false);
        //worldViewportP4.gameObject.SetActive(false);
        //worldViewportP5.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_actualViewMode != viewMode) SetViewMode(viewMode);

        OnWorldCameraUpdate(worldViewportP1, player1);
        OnWorldCameraUpdate(worldViewportP2, player2);
        OnWorldCameraUpdate(worldViewportP3, player3);
        OnWorldCameraUpdate(worldViewportP4, player4);
        OnWorldCameraUpdate(worldViewportP5, player5);
    }


    private bool isClientTarget(int val)
    {
        switch (val)
        {
            case 1:
                return GameManager.Instance.GetClientID() == PlayerSlot.P1;
            case 2:
                return GameManager.Instance.GetClientID() == PlayerSlot.P2;
            case 3:
                return GameManager.Instance.GetClientID() == PlayerSlot.P3;
            case 4:
                return GameManager.Instance.GetClientID() == PlayerSlot.P4;
            case 5:
                return GameManager.Instance.GetClientID() == PlayerSlot.P5;
            default:
                return false;
        }
    }

    public List<IGameCamera> GetPacManMazeCameras()
    {
        return new List<IGameCamera>() { _pacmanMazeCamera, _pacmanWorldCamera };
    }
    public List<KeyValuePair<int, IGameCamera>> GetGhostWorldCameras(bool prioritizeClient = false)
    {
        var results = _ghostWorldCameras.OrderBy(kvp => kvp.Key);
        if (prioritizeClient) results = results.OrderBy(kvp => isClientTarget(kvp.Key) == false);
        return results.ToList();
    }

    public List<KeyValuePair<int, IGameCamera>> GetGhostMazeCameras(bool prioritizeClient = false)
    {
        var results = _ghostMazeCameras.OrderBy(kvp => kvp.Key);
        if (prioritizeClient) results = results.OrderBy(kvp => isClientTarget(kvp.Key) == false);
        return results.ToList();
            
    }

    public void ResetMazeViewSettings()
    {
        foreach (var x in _ghostMazeCameras.Values)
        {
            var cam = (MazeCamera)x;
            cam.SetAlwaysFollow(false);
        }
    }

    private void ActivateMazeView()
    {
        ghostsView.SetActive(false);
        ghostsRetroView.SetActive(false);
        modernMazeView.SetActive(false);
        mazeView.SetActive(true);
        ghostsViewP5.SetActive(false);
        mazeView.UpdateCameras(GetPacManMazeCameras());
    }

    private void ActivateModernMazeView()
    {
        mazeView.SetActive(false);
        ghostsView.SetActive(false);
        ghostsRetroView.SetActive(false);
        modernMazeView.SetActive(true);
        ghostsViewP5.SetActive(false);
        mazeView.UpdateCameras(new List<IGameCamera>() { _pacmanMazeCamera, _pacmanWorldCamera });
    }

    private void ActivateGhostViewP5()
    {
        ResetMazeViewSettings();
        mazeView.SetActive(false);
        ghostsRetroView.SetActive(false);
        modernMazeView.SetActive(false);
        ghostsView.SetActive(false);
        ghostsViewP5.SetActive(true);
        ghostsViewP5.UpdateCameras(GetGhostWorldCameras());
    }

    private void ActivateGhostView()
    {
        ResetMazeViewSettings();
        mazeView.SetActive(false);
        ghostsRetroView.SetActive(false);
        modernMazeView.SetActive(false);
        ghostsView.SetActive(true);
        ghostsViewP5.SetActive(false);
        ghostsView.UpdateCameras(GetGhostWorldCameras());
    }

    private void ActivateRetroGhostView()
    {
        mazeView.SetActive(false);
        ghostsView.SetActive(false);
        modernMazeView.SetActive(false);
        ghostsRetroView.SetActive(true);
        ghostsViewP5.SetActive(false);
        foreach (var x in _ghostMazeCameras)
        {
            var cam = (MazeCamera)x.Value;
            if (!isClientTarget(x.Key)) cam.SetAlwaysFollow(true);
            else cam.SetAlwaysFollow(false);
        }
        ghostsRetroView.UpdateCameras(GetGhostMazeCameras(true));
    }

    private void SetViewMode(MatchViewMode _mode)
    {
        switch (_mode)
        {
            case MatchViewMode.MazeView:
                ActivateMazeView();
                break;
            case MatchViewMode.WorldView:
                ActivateGhostView();
                break;
            case MatchViewMode.WorldViewP5:
                ActivateGhostViewP5();
                break;
            case MatchViewMode.RetroWorldView:
                ActivateRetroGhostView();
                break;
            case MatchViewMode.ModernMazeView:
                ActivateModernMazeView();
                break;
        }

        _actualViewMode = _mode;
    }
    
    public void AddCamera(IPlayable playable)
    {
        void Track(bool ghost, int index, MazeCamera maze, WorldCamera world)
        {
            if (ghost)
            {
                _ghostMazeCameras.Add(index, maze);
                _ghostWorldCameras.Add(index, world);
            }
            else
            {
                _pacmanMazeCamera = maze;
                _pacmanWorldCamera = world;
            }
        }

        var player_id = playable.GetPlayerID();
        if (player_id == PlayerSlot.Null) return;

        var is_ghost = playable.GetPlayerType() == PlayerType.Ghost;
        
        switch (player_id)
        {
            case PlayerSlot.P1:
                player1 = playable;
                viewportP1.target = playable.GetMazeObject();
                viewportP1.SetShadowMode(is_ghost);
                worldViewportP1.target = playable.GetWorldObject().transform;
                Track(is_ghost, 1, viewportP1, worldViewportP1);

                break;
            case PlayerSlot.P2:
                player2 = playable;
                viewportP2.target = playable.GetMazeObject();
                viewportP2.SetShadowMode(is_ghost);
                worldViewportP2.target = playable.GetWorldObject().transform;
                if (is_ghost) _ghostMazeCameras.Add(2, viewportP2);
                if (is_ghost) _ghostWorldCameras.Add(2, worldViewportP2);
                break;
            case PlayerSlot.P3:
                player3 = playable;
                viewportP3.target = playable.GetMazeObject();
                viewportP3.SetShadowMode(is_ghost);
                worldViewportP3.target = playable.GetWorldObject().transform;
                if (is_ghost) _ghostMazeCameras.Add(3, viewportP3);
                if (is_ghost) _ghostWorldCameras.Add(3, worldViewportP3);
                break;
            case PlayerSlot.P4:
                player4 = playable;
                viewportP4.target = playable.GetMazeObject();
                viewportP4.SetShadowMode(is_ghost);
                worldViewportP4.target = playable.GetWorldObject().transform;
                if (is_ghost) _ghostMazeCameras.Add(4, viewportP4);
                if (is_ghost) _ghostWorldCameras.Add(4, worldViewportP4);
                break;
            case PlayerSlot.P5:
                player5 = playable;
                viewportP5.target = playable.GetMazeObject();
                viewportP5.SetShadowMode(is_ghost);
                worldViewportP5.target = playable.GetWorldObject().transform;
                if (is_ghost) _ghostMazeCameras.Add(5, viewportP5);
                if (is_ghost) _ghostWorldCameras.Add(5, worldViewportP5);
                break;
        }
        RefreshCameras();
    }
    
    public void SwitchViewModes()
    {
        switch (viewMode)
        {
            case MatchViewMode.MazeView:
                viewMode = MatchViewMode.RetroWorldView;
                break;
            case MatchViewMode.RetroWorldView:
                viewMode = MatchViewMode.ModernMazeView;
                break;
            case MatchViewMode.ModernMazeView:
                viewMode = MatchViewMode.WorldView;
                break;
            case MatchViewMode.WorldView:
                viewMode = MatchViewMode.MazeView;
                break;
            
        }
    }
    
    public void OnWorldCameraUpdate(WorldCamera _worldCamera, IPlayable _player)
    {
        if (!_worldCamera.gameObject.activeSelf || !_worldCamera.enabled) return;

        if (_player != null) _worldCamera.SetSightRange(_player.GetSightRange());
    }

    public void UpdateViewports()
    {
        foreach (MazeCamera cam in new List<MazeCamera> { viewportP1, viewportP2, viewportP3, viewportP4, viewportP5 })
        {
            RenderTexture tileViewport = cam.GetViewport();
            tileViewport.Release();
            tileViewport.width = _manager.mazeTileViewportSize.x * _manager.mazeTileResolution;
            tileViewport.height = _manager.mazeTileViewportSize.y * _manager.mazeTileResolution;
            tileViewport.Create();
        }
        foreach (WorldCamera cam in new List<WorldCamera> { worldViewportP1, worldViewportP2, worldViewportP3, worldViewportP4, worldViewportP5 })
        {
            //TODO: Actually Setup Sizes
            //RenderTexture tileViewport = cam.GetViewport();
            //tileViewport.Release();
            //tileViewport.width = tileViewportSize.x * tileResolution;
            //tileViewport.height = tileViewportSize.y * tileResolution;
            //tileViewport.Create();
        }
    }

    public void RefreshCameras()
    {
        SetViewMode(_actualViewMode);
    }

    public void Teleport_FadeOut(IPlayable teleportable)
    {
        void RunEffect(IPlayable _player, WorldCamera _worldCamera)
        {
            if (_worldCamera.gameObject.activeSelf && _worldCamera.enabled && _worldCamera.target == teleportable.GetWorldObject().transform)
                _worldCamera.GetComponent<ScreenFader>().FadeToBlack();
        }
        if (teleportable == player1) RunEffect(player1, worldViewportP1);
        else if (teleportable == player2) RunEffect(player2, worldViewportP2);
        else if (teleportable == player3) RunEffect(player3, worldViewportP3);
        else if (teleportable == player4) RunEffect(player4, worldViewportP4);
        else if (teleportable == player5) RunEffect(player5, worldViewportP5);
    }

    public void Teleport_FadeIn(IPlayable teleportable)
    {
        void RunEffect(IPlayable _player, WorldCamera _worldCamera)
        {
            if (_worldCamera.gameObject.activeSelf && _worldCamera.enabled && _worldCamera.target == teleportable.GetWorldObject().transform)
                _worldCamera.GetComponent<ScreenFader>().FadeFromBlack();
        }
        if (teleportable == player1) RunEffect(player1, worldViewportP1);
        else if (teleportable == player2) RunEffect(player2, worldViewportP2);
        else if (teleportable == player3) RunEffect(player3, worldViewportP3);
        else if (teleportable == player4) RunEffect(player4, worldViewportP4);
        else if (teleportable == player5) RunEffect(player5, worldViewportP5);
    }
}
