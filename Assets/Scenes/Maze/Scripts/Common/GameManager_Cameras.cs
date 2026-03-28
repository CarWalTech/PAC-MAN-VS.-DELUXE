using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public class GameManager_Cameras
{
    [SerializeField] public MatchViewMode viewMode = MatchViewMode.MazeView;
    private MatchViewMode _actualViewMode = MatchViewMode.Unset;
    [SerializeField, SerializedDictionary("Name", "Value")] private SerializedDictionary<string, PacManView> mazeViews;
    [SerializeField, SerializedDictionary("Name", "Value")] private SerializedDictionary<string, GhostsView> ghostViews;
    [SerializeField, SerializedDictionary("Player", "Camera")] private SerializedDictionary<PlayerSlot, MazeCamera> mazeCameras;
    [SerializeField, SerializedDictionary("Player", "Camera")] private SerializedDictionary<PlayerSlot, WorldCamera> worldCameras;

    private IPlayable player1 = null;
    private IPlayable player2 = null;
    private IPlayable player3 = null;
    private IPlayable player4 = null;
    private IPlayable player5 = null;
    private IGameCamera _pacmanMazeCamera = null;
    private IGameCamera _pacmanWorldCamera = null;
    private Dictionary<int, IGameCamera> _ghostMazeCameras = new Dictionary<int, IGameCamera>();
    private Dictionary<int, IGameCamera> _ghostWorldCameras = new Dictionary<int, IGameCamera>();
    private GameManager_Maze _manager => GameManager.Instance.GetMazeManager();

    public void Update()
    {
        if (_actualViewMode != viewMode) SetViewMode(viewMode);

        OnWorldCameraUpdate(PlayerSlot.P1, player1);
        OnWorldCameraUpdate(PlayerSlot.P2, player2);
        OnWorldCameraUpdate(PlayerSlot.P3, player3);
        OnWorldCameraUpdate(PlayerSlot.P4, player4);
        OnWorldCameraUpdate(PlayerSlot.P5, player5);
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

    public WorldCamera GetWorldCamera(PlayerSlot slot)
    {
        if (worldCameras == null) return null;
        if (!worldCameras.ContainsKey(slot)) return null;
        return worldCameras[slot];
    }

    public MazeCamera GetMazeCamera(PlayerSlot slot)
    {
        if (mazeCameras == null) return null;
        if (!mazeCameras.ContainsKey(slot)) return null;
        return mazeCameras[slot];
    }

    public List<WorldCamera> GetWorldCameras()
    {
        return new List<WorldCamera> { 
            GetWorldCamera(PlayerSlot.P1), 
            GetWorldCamera(PlayerSlot.P2), 
            GetWorldCamera(PlayerSlot.P3), 
            GetWorldCamera(PlayerSlot.P4), 
            GetWorldCamera(PlayerSlot.P5) 
        };
    }

    public List<MazeCamera> GetMazeCameras()
    {
        return new List<MazeCamera> { 
            GetMazeCamera(PlayerSlot.P1), 
            GetMazeCamera(PlayerSlot.P2), 
            GetMazeCamera(PlayerSlot.P3), 
            GetMazeCamera(PlayerSlot.P4), 
            GetMazeCamera(PlayerSlot.P5) 
        };
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

    private T SwitchViewTo<T>(string name) where T : class
    {
        foreach (var x in ghostViews.Values) x.SetActive(false);
        foreach (var x in mazeViews.Values) x.SetActive(false);
        
        if (typeof(T) == typeof(PacManView))
        {
            if (mazeViews.ContainsKey(name)) return mazeViews[name] as T;
            else return null;
        }
        else if (typeof(T) == typeof(GhostsView))
        {
            if (ghostViews.ContainsKey(name)) return ghostViews[name] as T;
            else return null;
        }
        else
        {
            return null;
        }
    }

    private void ActivateMazeView()
    {
        var view = SwitchViewTo<PacManView>("Original");
        view.SetActive(true);
        view.UpdateCameras(GetPacManMazeCameras());
    }

    private void ActivateModernMazeView()
    {
        var view = SwitchViewTo<PacManView>("Original_Viewport");
        view.SetActive(true);
        view.UpdateCameras(new List<IGameCamera>() { _pacmanMazeCamera, _pacmanWorldCamera });
    }

    private void ActivateGhostViewP5()
    {
        var view = SwitchViewTo<GhostsView>("Original_P5");
        view.SetActive(true);
        view.UpdateCameras(GetGhostWorldCameras());
    }

    private void ActivateGhostView()
    {
        var view = SwitchViewTo<GhostsView>("Original");
        view.SetActive(true);
        view.UpdateCameras(GetGhostWorldCameras());
    }

    private void ActivateRetroGhostView()
    {
        var view = SwitchViewTo<GhostsView>("Retro");
        view.SetActive(true);
        foreach (var x in _ghostMazeCameras)
        {
            var cam = (MazeCamera)x.Value;
            if (!isClientTarget(x.Key)) cam.SetAlwaysFollow(true);
            else cam.SetAlwaysFollow(false);
        }
        view.UpdateCameras(GetGhostMazeCameras(true));
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
            else if (index == 1)
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
                GetMazeCamera(player_id).target = playable.GetMazeObject();
                GetMazeCamera(player_id).SetShadowMode(is_ghost);
                GetWorldCamera(player_id).target = playable.GetWorldObject().transform;
                Track(is_ghost, 1, GetMazeCamera(player_id), GetWorldCamera(player_id));
                break;
            case PlayerSlot.P2:
                player2 = playable;
                GetMazeCamera(player_id).target = playable.GetMazeObject();
                GetMazeCamera(player_id).SetShadowMode(is_ghost);
                GetWorldCamera(player_id).target = playable.GetWorldObject().transform;
                Track(is_ghost, 2, GetMazeCamera(player_id), GetWorldCamera(player_id));
                break;
            case PlayerSlot.P3:
                player3 = playable;
                GetMazeCamera(player_id).target = playable.GetMazeObject();
                GetMazeCamera(player_id).SetShadowMode(is_ghost);
                GetWorldCamera(player_id).target = playable.GetWorldObject().transform;
                Track(is_ghost, 3, GetMazeCamera(player_id), GetWorldCamera(player_id));
                break;
            case PlayerSlot.P4:
                player4 = playable;
                GetMazeCamera(player_id).target = playable.GetMazeObject();
                GetMazeCamera(player_id).SetShadowMode(is_ghost);
                GetWorldCamera(player_id).target = playable.GetWorldObject().transform;
                Track(is_ghost, 4, GetMazeCamera(player_id), GetWorldCamera(player_id));
                break;
            case PlayerSlot.P5:
                player5 = playable;
                GetMazeCamera(player_id).target = playable.GetMazeObject();
                GetMazeCamera(player_id).SetShadowMode(is_ghost);
                GetWorldCamera(player_id).target = playable.GetWorldObject().transform;
                Track(is_ghost, 5, GetMazeCamera(player_id), GetWorldCamera(player_id));
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
    
    public void OnWorldCameraUpdate(PlayerSlot slot, IPlayable _player)
    {
        if (worldCameras == null) return;
        if (!worldCameras.ContainsKey(slot)) return;

        WorldCamera _worldCamera = worldCameras[slot];

        if (!_worldCamera.gameObject.activeSelf || !_worldCamera.enabled) return;

        if (_player != null) _worldCamera.SetSightRange(_player.GetSightRange());
    }

    public void UpdateViewports()
    {
        var ppu = GameManager.Instance.GetSkinManager().GetTileResolution();
        foreach (MazeCamera cam in GetMazeCameras())
        {
            RenderTexture tileViewport = cam.GetViewport();
            tileViewport.Release();
            cam.setPPU(ppu);
            tileViewport.width = _manager.mazeTileViewportSize.x * ppu;
            tileViewport.height = _manager.mazeTileViewportSize.y * ppu;
            tileViewport.Create();
        }
        foreach (WorldCamera cam in GetWorldCameras())
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
        if (teleportable == player1) RunEffect(player1, GetWorldCamera(PlayerSlot.P1));
        else if (teleportable == player2) RunEffect(player2, GetWorldCamera(PlayerSlot.P2));
        else if (teleportable == player3) RunEffect(player3, GetWorldCamera(PlayerSlot.P3));
        else if (teleportable == player4) RunEffect(player4, GetWorldCamera(PlayerSlot.P4));
        else if (teleportable == player5) RunEffect(player5, GetWorldCamera(PlayerSlot.P5));
    }

    public void Teleport_FadeIn(IPlayable teleportable)
    {
        void RunEffect(IPlayable _player, WorldCamera _worldCamera)
        {
            if (_worldCamera.gameObject.activeSelf && _worldCamera.enabled && _worldCamera.target == teleportable.GetWorldObject().transform)
                _worldCamera.GetComponent<ScreenFader>().FadeFromBlack();
        }
        if (teleportable == player1) RunEffect(player1, GetWorldCamera(PlayerSlot.P1));
        else if (teleportable == player2) RunEffect(player2, GetWorldCamera(PlayerSlot.P2));
        else if (teleportable == player3) RunEffect(player3, GetWorldCamera(PlayerSlot.P3));
        else if (teleportable == player4) RunEffect(player4, GetWorldCamera(PlayerSlot.P4));
        else if (teleportable == player5) RunEffect(player5, GetWorldCamera(PlayerSlot.P5));
    }
}
