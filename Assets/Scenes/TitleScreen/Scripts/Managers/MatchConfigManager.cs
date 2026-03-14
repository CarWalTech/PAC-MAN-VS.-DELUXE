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

    public IEnumerator RestoreSettings()
    {
        //Issue: Timing Prevents Player 5 from Restoring Correctly
        //Solution: Use Coroutines and Yeilds to Properly do the timing

        yield return new WaitUntil(() => GameSetting_GamePlayerCount != null);
        GameSetting_GamePlayerCount.SetValue(GameConfiguration.PlayerCount);

        yield return new WaitUntil(() => GameSetting_Player1 != null);
        GameSetting_Player1.SetSelection(GameConfiguration.CharacterP1);
        GameSetting_Player1.SetScore(GameConfiguration.Score_P1);

        yield return new WaitUntil(() => GameSetting_Player2 != null);
        GameSetting_Player2.SetSelection(GameConfiguration.CharacterP2);
        GameSetting_Player2.SetScore(GameConfiguration.Score_P2);

        yield return new WaitUntil(() => GameSetting_Player3 != null);
        GameSetting_Player3.SetSelection(GameConfiguration.CharacterP3);
        GameSetting_Player3.SetScore(GameConfiguration.Score_P3);

        yield return new WaitUntil(() => GameSetting_Player4 != null);
        GameSetting_Player4.SetSelection(GameConfiguration.CharacterP4);
        GameSetting_Player4.SetScore(GameConfiguration.Score_P4);

        yield return new WaitUntil(() => GameSetting_Player5 != null);
        GameSetting_Player5.SetSelection(GameConfiguration.CharacterP5);
        GameSetting_Player5.SetScore(GameConfiguration.Score_P5);
        
        if (GameConfiguration.PlayerSlot_Next_PacMan == PlayerSlot.P1) GameSetting_Player1.SetPacMode(true);
        else if (GameConfiguration.PlayerSlot_Next_PacMan == PlayerSlot.P2) GameSetting_Player2.SetPacMode(true);
        else if (GameConfiguration.PlayerSlot_Next_PacMan == PlayerSlot.P3) GameSetting_Player3.SetPacMode(true);
        else if (GameConfiguration.PlayerSlot_Next_PacMan == PlayerSlot.P4) GameSetting_Player4.SetPacMode(true);
        else if (GameConfiguration.PlayerSlot_Next_PacMan == PlayerSlot.P5) GameSetting_Player5.SetPacMode(true);


        yield return new WaitUntil(() => GameSetting_GameGhostSight != null);
        GameSetting_GameGhostSight.SetValue(GameConfiguration.GhostSight);

        yield return new WaitUntil(() => GameSetting_GameGhostSpeed != null);
        GameSetting_GameGhostSpeed.SetValue(GameConfiguration.GhostSpeed);

        yield return new WaitUntil(() => GameSetting_GhostFrightTime != null);
        GameSetting_GhostFrightTime.SetValue(GameConfiguration.FrightTimer);

        yield return new WaitUntil(() => GameSetting_GamePacManSpeed != null);
        GameSetting_GamePacManSpeed.SetValue(GameConfiguration.PacManSpeed);

        yield return new WaitUntil(() => GameSetting_GameTargetScore != null);
        GameSetting_GameTargetScore.SetValue(GameConfiguration.TargetScore);

        yield return new WaitUntil(() => GameSetting_GamePacManBonus != null);
        GameSetting_GamePacManBonus.SetValue(GameConfiguration.PacManBonus);
    }

    void Start()
    {

    }

    void Update()
    {
        if (GameConfiguration.CanRestore)
        {
            StartCoroutine(nameof(RestoreSettings));
            GameConfiguration.CanRestore = false;
        }
    }
}