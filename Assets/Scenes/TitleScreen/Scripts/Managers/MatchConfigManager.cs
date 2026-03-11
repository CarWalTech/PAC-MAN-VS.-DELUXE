using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchConfigManager : MonoBehaviour
{


    public ValuePickerInt   GameSetting_GameTargetScore;
    public PlayerCountPicker  GameSetting_GamePlayerCount;
    public ValuePickerInt     GameSetting_GamePacManBonus;
    public ValuePickerFloat   GameSetting_GamePacManSpeed;
    public ValuePickerFloat   GameSetting_GameGhostSpeed;
    public ValuePickerFloat   GameSetting_GameGhostSight;
    public ValuePickerFloat   GameSetting_GhostFrightTime;
    public PlayerPicker       GameSetting_Player1;
    public PlayerPicker       GameSetting_Player2;
    public PlayerPicker       GameSetting_Player3;
    public PlayerPicker       GameSetting_Player4;
    public PlayerPicker       GameSetting_Player5;

    public List<PlayerPicker> GameSetting_Players
    {
        get { 
            return new List<PlayerPicker>() { 
                GameSetting_Player1, 
                GameSetting_Player2, 
                GameSetting_Player3, 
                GameSetting_Player4, 
                GameSetting_Player5 
            }; 
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (GameConfiguration.Restore_IsPossible())
            GameConfiguration.Restore_Start(this);
    }
}