using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPicker : MonoBehaviour
{
    public Image ghostBaseDisplay;
    public Image ghostEyeDisplay;
    public Image pacmanDisplay;

    public PlayerPickerButton[] cpuOptions = {};
    public PlayerPickerButton[] playerOptions = {};
    public PlayerPicker[] otherPickers = {};
    public PlayerCharacter defaultValue = 0;
    public PlayerMode defaultMode = PlayerMode.CPU;
    public bool defaultPacMode = false;
    public int playerIndex = 0;
    public TMPro.TMP_Text playerLabel;
    public TMPro.TMP_Text titleLabel;
    public TMPro.TMP_InputField scoreLabel;
    public Button modeSwitchButton;
    public Button pacModeSwitchButton;
    private bool _currentPacMode = false;
    private int _playerIndex = -1;
    private PlayerMode _currentMode = PlayerMode.Player;
    private PlayerCharacter _currentValue = PlayerCharacter.COM;


    public bool IsReady { get; private set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSelection(defaultValue);
        SetMode(defaultMode);
        SetPacMode(defaultPacMode);
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsReady) IsReady = true;
    }



    #region Actions
    
    public void TogglePacMode()
    {
        SetPacMode(!_currentPacMode);
    }
    public void ToggleMode()
    {
        switch (_currentMode)
        {
            case PlayerMode.Player:
                SetMode(PlayerMode.CPU);
                break;
            case PlayerMode.CPU:
                SetMode(PlayerMode.Player);
                break;
        }
    }

    #endregion

    #region Getters

    public PlayerCharacter GetSelection()
    {
        return _currentValue;
    }
    public PlayerMode GetMode()
    {
        return _currentMode;
    }
    public bool GetPacMode()
    {
        return _currentPacMode;
    }
    public PlayerCharacter? GetAvaliablePlayerSelection()
    {
        var options = new List<PlayerCharacter>() { PlayerCharacter.P1, PlayerCharacter.P2, PlayerCharacter.P3, PlayerCharacter.P4, PlayerCharacter.P5 };

        foreach (var other in otherPickers)
        {
            if (options.Contains(other.GetSelection())) options.Remove(other.GetSelection());
        }

        if (options.Count != 0) return options[0];
        else return null;
    }
    public PlayerCharacter GetAvaliableCPUSelection()
    {
        var options = new List<PlayerCharacter>() { PlayerCharacter.Blinky, PlayerCharacter.Inky, PlayerCharacter.Clyde, PlayerCharacter.Pinky };

        foreach (var other in otherPickers)
        {
            if (options.Contains(other.GetSelection())) options.Remove(other.GetSelection());
        }

        if (options.Count != 0) return options[0];
        else return PlayerCharacter.COM;
    }
    public int GetPlayerIndex()
    {
        return _playerIndex;
    }
    public int GetScore()
    {
        return int.Parse(scoreLabel.text);
    }
    public static Color GetGhostColor(PlayerCharacter selection)
    {
        switch (selection)
        {
            case PlayerCharacter.COM:
                return ColorHelper.FromString("#808080");
            case PlayerCharacter.P1:
                return ColorHelper.FromString("#E00000");
            case PlayerCharacter.P2:
                return ColorHelper.FromString("#68B8E0");
            case PlayerCharacter.P3:
                return ColorHelper.FromString("#F098B0");
            case PlayerCharacter.P4:
                return ColorHelper.FromString("#00E816");
            case PlayerCharacter.P5:
                return ColorHelper.FromString("#A700FD");
            case PlayerCharacter.Blinky:
                return ColorHelper.FromString("#FF0000");
            case PlayerCharacter.Inky:
                return ColorHelper.FromString("#0CE8E8");
            case PlayerCharacter.Clyde:
                return ColorHelper.FromString("#F8BB55");
            case PlayerCharacter.Pinky:
                return ColorHelper.FromString("#FCB5FF");
            default:
                return Color.black;
        }        
    }

    #endregion

    #region Setters

    public void SetSelection(PlayerCharacter color)
    {
        _currentValue = color;
        ghostBaseDisplay.color = GetGhostColor(color);
        var allButtons = cpuOptions.Concat(playerOptions);
        foreach (var btn in allButtons) btn.SetSelected(false);
        var selectedButton = allButtons.FirstOrDefault(x => x.selection == color);
        if (selectedButton) selectedButton.SetSelected(true);


        OnNameNeedsUpdate();
        foreach (var other in otherPickers) other.OnOtherSelectionChanged(_currentValue);
    }
    public void SetScore(int score)
    {
        if (scoreLabel) scoreLabel.text = score.ToString();
    }
    public void SetMode(PlayerMode mode)
    {
        var btn_colors = modeSwitchButton.colors;

        switch (mode)
        {
            case PlayerMode.Player:
                _currentMode = mode;
                foreach (var item in cpuOptions) item.gameObject.SetActive(false);
                foreach (var item in playerOptions) item.gameObject.SetActive(true);
                btn_colors.normalColor = Color.limeGreen;
                btn_colors.pressedColor = Color.green;
                btn_colors.highlightedColor = Color.lightGreen;
                break;
            case PlayerMode.CPU:
                _currentMode = mode;
                foreach (var item in cpuOptions) item.gameObject.SetActive(true);
                foreach (var item in playerOptions) item.gameObject.SetActive(false);
                btn_colors.normalColor = Color.gray;
                btn_colors.pressedColor = Color.darkGray;
                btn_colors.highlightedColor = Color.lightGray;
                break;
        }
        
        modeSwitchButton.colors = btn_colors;
        modeSwitchButton.gameObject.GetComponent<Image>().color = btn_colors.normalColor;
        OnCurrentModeChanged(_currentMode);
    }
    public void SetPacMode(bool state)
    {
        var btn_colors = pacModeSwitchButton.colors;


        if (state)
        {
            ghostBaseDisplay.enabled = false;
            ghostEyeDisplay.enabled = false;
            pacmanDisplay.enabled = true;

            btn_colors.normalColor = Color.yellow;
            btn_colors.pressedColor = Color.yellowGreen;
            btn_colors.highlightedColor = Color.lightYellow;

        }
        else
        {
            ghostBaseDisplay.enabled = true;
            ghostEyeDisplay.enabled = true;
            pacmanDisplay.enabled = false;

            btn_colors.normalColor = Color.gray;
            btn_colors.pressedColor = Color.darkGray;
            btn_colors.highlightedColor = Color.lightGray;
        }

        _currentPacMode = state;
        pacModeSwitchButton.colors = btn_colors;
        pacModeSwitchButton.gameObject.GetComponent<Image>().color = btn_colors.normalColor;
        OnPacModeChanged();
    }

    #endregion

    #region Events

    public void OnCurrentModeChanged(PlayerMode mode)
    {
        switch (_currentValue)
        {
            case PlayerCharacter.P1:
            case PlayerCharacter.P2:
            case PlayerCharacter.P3:
            case PlayerCharacter.P4:
            case PlayerCharacter.P5:
                if (mode == PlayerMode.CPU) SetSelection(PlayerCharacter.COM);
                break;

            case PlayerCharacter.COM:
            case PlayerCharacter.Blinky:
            case PlayerCharacter.Inky:
            case PlayerCharacter.Clyde:
            case PlayerCharacter.Pinky:
                if (mode == PlayerMode.Player)
                {
                    var avaliablePlayer = GetAvaliablePlayerSelection();
                    if (avaliablePlayer != null) SetSelection(avaliablePlayer.Value);
                    else SetMode(PlayerMode.CPU);
                }
                break;
        }
        
        OnTitleNeedsUpdate();
        foreach (var item in otherPickers) item.OnTitleNeedsUpdate();
    }
    public void OnOtherSelectionChanged(PlayerCharacter color)
    {
        if (_currentMode == PlayerMode.CPU)
        {
            if (_currentValue == color && color != PlayerCharacter.COM) SetSelection(GetAvaliableCPUSelection());         
        }
        else if (_currentMode == PlayerMode.Player)
        {
            if (_currentValue == color)
            {
                var avaliablePlayer = GetAvaliablePlayerSelection();
                if (avaliablePlayer != null) SetSelection(avaliablePlayer.Value);
                else SetMode(PlayerMode.CPU);
            }
        }
        
    }
    public void OnTitleNeedsUpdate()
    {
        var index = otherPickers.Where(x => x.GetMode() == _currentMode)
                            .Append(this)
                            .OrderBy(x => x.playerIndex)
                            .ToList()
                            .IndexOf(this) + 1;
        
        switch (_currentMode)
        {
            case PlayerMode.Player:
                titleLabel.text = "P" + index;
                _playerIndex = index - 1;
                break;
            case PlayerMode.CPU:
                titleLabel.text = "COM" + index;   
                _playerIndex = -1;
                break;
        }
    }
    public void OnNameNeedsUpdate()
    {
        if (_currentPacMode)
        {
            playerLabel.text = "Pac-Man";
        }
        else
        {
            switch (_currentValue)
            {
                case PlayerCharacter.COM:
                    playerLabel.text = "COM";
                    break;
                case PlayerCharacter.P1:
                    playerLabel.text = "P1";
                    break;
                case PlayerCharacter.P2:
                    playerLabel.text = "P2";
                    break;
                case PlayerCharacter.P3:
                    playerLabel.text = "P3";
                    break;
                case PlayerCharacter.P4:
                    playerLabel.text = "P4";
                    break;
                case PlayerCharacter.P5:
                    playerLabel.text = "P5";
                    break;
                case PlayerCharacter.Blinky:
                    playerLabel.text = "Blinky";
                    break;
                case PlayerCharacter.Inky:
                    playerLabel.text = "Inky";
                    break;
                case PlayerCharacter.Clyde:
                    playerLabel.text = "Clyde";
                    break;
                case PlayerCharacter.Pinky:
                    playerLabel.text = "Pinky";
                    break;
            }
        }
    }
    public void OnPacModeChanged()
    {
        if (_currentPacMode)
        {
            foreach (var item in otherPickers)
            {
                if (item.GetPacMode()) item.SetPacMode(false);
            }
        }
        else
        {
            if (!otherPickers.Any(x => x.GetPacMode())) otherPickers.First().SetPacMode(true);    
        }

        

        OnNameNeedsUpdate();
    }

    #endregion
}
