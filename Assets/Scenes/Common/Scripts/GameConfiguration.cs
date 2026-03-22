using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public static class GameConfiguration
{
    public static class Defaults
    {
        public readonly struct FloatField
        {
            public FloatField(float _min, float _max, float _def, float _incrmnt)
            {
                min = _min;
                max = _max;
                def = _def;
                incrmnt = _incrmnt;
            }
            public float min { get; }
            public float max { get; }
            public float def { get; }
            public float incrmnt { get; }
        }
        public readonly struct IntField
        {
            public IntField(int _min, int _max, int _def, int _incrmnt)
            {
                min = _min;
                max = _max;
                def = _def;
                incrmnt = _incrmnt;
            }
            public int min { get; }
            public int max { get; }
            public int def { get; }
            public int incrmnt { get; }
        }

        public static FloatField PacManSpeed = new FloatField(0.4f, 1.9f, 1f, 0.1f);
        public static FloatField GhostSpeed =  new FloatField(0.5f, 2f, 1f, 0.1f);
        public static FloatField GhostSight =  new FloatField(4f, 8f, 6f, 0.5f);
        public static FloatField FrightTimer =  new FloatField(3f, 10f, 6f, 0.25f);
    }

    public enum EnterMode
    {
        Disabled = 0,
        Normal = 1
    }
    public enum ExitMode
    {
        Disabled = 0,
        Developer = 1
    }


    public static ViewportTheme GuiTheme { get; private set; } = null;
    public static LevelConfiguration MazeData { get; private set; } = null;
    public static MazeTheme MazeTheme { get; private set; } = null;

    public static PlayerCharacter CharacterP1 { get; private set; } = PlayerCharacter.P1;
    public static PlayerCharacter CharacterP2 { get; private set; } = PlayerCharacter.COM;
    public static PlayerCharacter CharacterP3 { get; private set; } = PlayerCharacter.COM;
    public static PlayerCharacter CharacterP4 { get; private set; } = PlayerCharacter.COM;
    public static PlayerCharacter CharacterP5 { get; private set; } = PlayerCharacter.COM;

    public static EnterMode GameEnterMode { get; set; } = EnterMode.Disabled;
    public static ExitMode GameExitMode { get; set; } = ExitMode.Disabled;

    public static PlayerSlot PlayerSlot_Client { get; private set; } = PlayerSlot.P1;
    public static PlayerSlot PlayerSlot_PacMan { get; private set; } = PlayerSlot.P1;
    public static PlayerSlot PlayerSlot_Next_PacMan { get; private set; } = PlayerSlot.Null;
    public static PlayerSlot PlayerSlot_GhostP1 { get; private set; } = PlayerSlot.P2;
    public static PlayerSlot PlayerSlot_GhostP2 { get; private set; } = PlayerSlot.P3;
    public static PlayerSlot PlayerSlot_GhostP3 { get; private set; } = PlayerSlot.P4;
    public static PlayerSlot PlayerSlot_GhostP4 { get; private set; } = PlayerSlot.P5;

    public static PlayerInputSource InputSource_PacMan { get; private set; } = PlayerInputSource.Gamepad1;
    public static PlayerInputSource InputSource_GhostP1 { get; private set; } = PlayerInputSource.CPU;
    public static PlayerInputSource InputSource_GhostP2 { get; private set; } = PlayerInputSource.CPU;
    public static PlayerInputSource InputSource_GhostP3 { get; private set; } = PlayerInputSource.CPU;
    public static PlayerInputSource InputSource_GhostP4 { get; private set; } = PlayerInputSource.CPU;

    public static int TargetScore { get; private set; } = 15000;
    public static int PlayerCount { get; private set; } = 2;
    public static int PacManBonus { get; private set; } = 1000;
    public static float PacManSpeed { get; private set; } = 1.0f;
    public static float GhostSight { get; private set; } = 4.0f;
    public static float GhostSpeed { get; private set; } = 1.0f;
    public static float FrightTimer { get; private set; } = 6.0f;
    
    public static int Score_P1 { get; private set; } = 0;
    public static int Score_P2 { get; private set; } = 0;
    public static int Score_P3 { get; private set; } = 0;
    public static int Score_P4 { get; private set; } = 0;
    public static int Score_P5 { get; private set; } = 0;

    public static bool CanRestore = false;

    public static MatchViewMode GetCameraFocus()
    {
        if (PlayerSlot_Client == InputSource_PacMan.ToSlot())
            return MatchViewMode.MazeView;
        else if (PlayerSlot_Client == InputSource_GhostP1.ToSlot())
            return PlayerCount >= 5 ? MatchViewMode.WorldViewP5 : MatchViewMode.WorldView;
        else if (PlayerSlot_Client == InputSource_GhostP2.ToSlot())
            return PlayerCount >= 5 ? MatchViewMode.WorldViewP5 : MatchViewMode.WorldView;
        else if (PlayerSlot_Client == InputSource_GhostP3.ToSlot())
            return PlayerCount >= 5 ? MatchViewMode.WorldViewP5 : MatchViewMode.WorldView;
        else if (PlayerSlot_Client == InputSource_GhostP4.ToSlot())
            return PlayerCount >= 5 ? MatchViewMode.WorldViewP5 : MatchViewMode.WorldView;
        else
            return MatchViewMode.MazeView;
    }
    public static GameObject GetChaserObject(GameManager manager, int index)
    {
        List<PlayerSlot> players = new List<PlayerSlot>() { PlayerSlot.P1, PlayerSlot.P2, PlayerSlot.P3, PlayerSlot.P4, PlayerSlot.P5 };
        players.Remove(PlayerSlot_PacMan);
        PlayerSlot actualChaserId = players[index];
        PlayerCharacter actualSelection;
        
        switch (actualChaserId)
        {
            case PlayerSlot.P1:
                actualSelection = CharacterP1;
                break;
            case PlayerSlot.P2:
                actualSelection = CharacterP2;
                break;
            case PlayerSlot.P3:
                actualSelection = CharacterP3;
                break;
            case PlayerSlot.P4:
                actualSelection = CharacterP4;
                break;
            case PlayerSlot.P5:
                actualSelection = CharacterP5;
                break;
            default:
                return null;
                
        }

        switch (actualSelection)
        {
            case PlayerCharacter.COM:
                switch (index)
                {
                    case 0:
                        return manager.mazeGhostCOM1;
                    case 1:
                        return manager.mazeGhostCOM2;
                    case 2:
                        return manager.mazeGhostCOM3;
                    case 3:
                        return manager.mazeGhostCOM4;
                    default:
                        return null;
                }
            case PlayerCharacter.P1:
                return manager.mazeGhostP1;
            case PlayerCharacter.P2:
                return manager.mazeGhostP2;
            case PlayerCharacter.P3:
                return manager.mazeGhostP3;
            case PlayerCharacter.P4:
                return manager.mazeGhostP4;
            case PlayerCharacter.P5:
                return manager.mazeGhostP5;
            case PlayerCharacter.Blinky:
                return manager.mazeGhostBlinky;
            case PlayerCharacter.Inky:
                return manager.mazeGhostInky;
            case PlayerCharacter.Clyde:
                return manager.mazeGhostClyde;
            case PlayerCharacter.Pinky:
                return manager.mazeGhostPinky;
            default:
                return null;
        }



    }
    public static GameObject GetPlayerObject(GameManager manager)
    {
        if (InputSource_PacMan == PlayerInputSource.CPU) return manager.mazePacManCOM;
        else return manager.mazePacMan;
    }
    public static PlayerCharacter GetCharacter(PlayerSlot slot)
    {
        switch (slot)
        {
            case PlayerSlot.P1:
                return CharacterP1;
            case PlayerSlot.P2:
                return CharacterP2;
            case PlayerSlot.P3:
                return CharacterP3;
            case PlayerSlot.P4:
                return CharacterP4;
            case PlayerSlot.P5:
                return CharacterP5;
            default:
                return PlayerCharacter.COM;
        }
    }
    
    public static void Event_StartGame(MatchConfigManager gameConfig)
    {

        void AssignPacman()
        {
            List<PlayerSlot> players = new List<PlayerSlot>() { PlayerSlot.P1, PlayerSlot.P2, PlayerSlot.P3, PlayerSlot.P4, PlayerSlot.P5 };
            players.Remove(PlayerSlot_PacMan);

            var pac_player_index = 0;
            var pac_player_controller = PlayerInputSource.CPU;
            var pac_player = gameConfig.GameSetting_Players.FirstOrDefault(x => x.GetPacMode());
            if (pac_player)
            {
                pac_player_index = gameConfig.GameSetting_Players.IndexOf(pac_player);
                pac_player_controller = pac_player.GetMode() == PlayerMode.CPU ? PlayerInputSource.CPU : (PlayerInputSource)pac_player_index;
            }

            PlayerSlot_PacMan = (PlayerSlot)pac_player_index;
            PlayerSlot_Next_PacMan = PlayerSlot_PacMan;
            InputSource_PacMan = pac_player_controller;
        }

        void AssignChaser(int index)
        {
            List<PlayerSlot> players = new List<PlayerSlot>() { PlayerSlot.P1, PlayerSlot.P2, PlayerSlot.P3, PlayerSlot.P4, PlayerSlot.P5 };
            players.Remove(PlayerSlot_PacMan);

            var ghost_player = gameConfig.GameSetting_Player1;
            var slot = players.ElementAt(index);
            var source = PlayerInputSource.CPU;
            switch (slot)
            {
                case PlayerSlot.P1:
                    source = (PlayerInputSource)gameConfig.GameSetting_Player1.GetPlayerIndex();
                    break;
                case PlayerSlot.P2:
                    source = (PlayerInputSource)gameConfig.GameSetting_Player2.GetPlayerIndex();
                    break;
                case PlayerSlot.P3:
                    source = (PlayerInputSource)gameConfig.GameSetting_Player3.GetPlayerIndex();
                    break;
                case PlayerSlot.P4:
                    source = (PlayerInputSource)gameConfig.GameSetting_Player4.GetPlayerIndex();
                    break;
                case PlayerSlot.P5:
                    source = (PlayerInputSource)gameConfig.GameSetting_Player5.GetPlayerIndex();
                    break;
            }

            switch (index)
            {
                case 0:
                    InputSource_GhostP1 = source;
                    PlayerSlot_GhostP1 = slot;
                    break;
                case 1:
                    InputSource_GhostP2 = source;
                    PlayerSlot_GhostP2 = slot;
                    break;
                case 2:
                    InputSource_GhostP3 = source;
                    PlayerSlot_GhostP3 = slot;
                    break;
                case 3:
                    InputSource_GhostP4 = source;
                    PlayerSlot_GhostP4 = slot;
                    break;
            }
        }

        PlayerCount = gameConfig.GameSetting_GamePlayerCount.currentValue;


        GuiTheme = gameConfig.GameSetting_GameGuiTheme.GetTheme();
        MazeData = gameConfig.GameSetting_GameMaze.GetMaze();
        MazeTheme = gameConfig.GameSetting_GameMazeTheme.GetTheme();
        
        CharacterP1 = gameConfig.GameSetting_Player1.GetSelection();
        CharacterP2 = gameConfig.GameSetting_Player2.GetSelection();
        CharacterP3 = gameConfig.GameSetting_Player3.GetSelection();
        CharacterP4 = gameConfig.GameSetting_Player4.GetSelection();
        CharacterP5 = gameConfig.GameSetting_Player5.GetSelection();

        GhostSight = gameConfig.GameSetting_GameGhostSight.currentValue;
        GhostSpeed = gameConfig.GameSetting_GameGhostSpeed.currentValue;
        FrightTimer = gameConfig.GameSetting_GhostFrightTime.currentValue;
        PacManSpeed = gameConfig.GameSetting_GamePacManSpeed.currentValue;
        TargetScore = gameConfig.GameSetting_GameTargetScore.currentValue;
        PacManBonus = gameConfig.GameSetting_GamePacManBonus.currentValue;

        Score_P1 = gameConfig.GameSetting_Player1.GetScore();
        Score_P2 = gameConfig.GameSetting_Player2.GetScore();
        Score_P3 = gameConfig.GameSetting_Player3.GetScore();
        Score_P4 = gameConfig.GameSetting_Player4.GetScore();
        Score_P5 = gameConfig.GameSetting_Player5.GetScore();

        var scores = new int[] {Score_P1, Score_P2, Score_P3, Score_P4, Score_P5};
        if (!scores.Any(x => x >= PacManBonus))
        {
            if (gameConfig.GameSetting_Player1.GetPacMode()) Score_P1 += PacManBonus;
            else if (gameConfig.GameSetting_Player2.GetPacMode()) Score_P2 += PacManBonus;
            else if (gameConfig.GameSetting_Player3.GetPacMode()) Score_P3 += PacManBonus;
            else if (gameConfig.GameSetting_Player4.GetPacMode()) Score_P4 += PacManBonus;
            else if (gameConfig.GameSetting_Player5.GetPacMode()) Score_P5 += PacManBonus;
        }

        AssignPacman();
        AssignChaser(0);
        AssignChaser(1);
        AssignChaser(2);
        AssignChaser(3);
        
        Debug.Log(string.Format("Pac-Man || Slot:{0} | Controller:{1}", PlayerSlot_PacMan, InputSource_PacMan));
        Debug.Log(string.Format("Ghost P1 || Slot:{0} | Controller:{1}", PlayerSlot_GhostP1, InputSource_GhostP1));
        Debug.Log(string.Format("Ghost P2 || Slot:{0} | Controller:{1}", PlayerSlot_GhostP2, InputSource_GhostP2));
        Debug.Log(string.Format("Ghost P3 || Slot:{0} | Controller:{1}", PlayerSlot_GhostP3, InputSource_GhostP3));
        Debug.Log(string.Format("Ghost P4 || Slot:{0} | Controller:{1}", PlayerSlot_GhostP4, InputSource_GhostP4));

        GameEnterMode = EnterMode.Normal;
        GameExitMode = ExitMode.Developer;
        GameSceneManager.LoadScene("Maze");
    }
    public static void Event_SwapPacMan(PlayerSlot transferTo)
    {
        PlayerSlot_Next_PacMan = transferTo;
    }
    public static void Event_LoadState(ref MatchManager manager)
    {
        manager.targetScore = TargetScore;
        manager.clientPlayerID = PlayerSlot_Client;
        manager.powerPelletDuration = FrightTimer;
        manager.pacManSpeed = PacManSpeed;
        manager.ghostSight = GhostSight;
        manager.ghostSpeed = GhostSpeed;
        manager.pacManBonus = PacManBonus;
        manager.playerCount = PlayerCount;
    }
    public static void Event_LoadDevState(GameManager manager)
    {
        if (manager.mazePlayer == manager.mazePacMan)
        {
            CharacterP1 = PlayerCharacter.P1;
        }
        else if (manager.mazePlayer == manager.mazePacManCOM)
        {
            CharacterP1 = PlayerCharacter.COM;
        }
        
        CharacterP2 = manager.mazeChaserP1.GetComponent<Ghost>().characterSlot;
        CharacterP3 = manager.mazeChaserP2.GetComponent<Ghost>().characterSlot;
        CharacterP4 = manager.mazeChaserP3.GetComponent<Ghost>().characterSlot;
        CharacterP5 = manager.mazeChaserP4.GetComponent<Ghost>().characterSlot;
    }
    public static void Event_ReturnToMenu()
    {
        GameSceneManager.LoadScene("TitleScreen");
        CanRestore = true;
    }

    public static int Score_Get(PlayerSlot slot)
    {
        switch (slot)
        {
            case PlayerSlot.P1:
                return Score_P1;
            case PlayerSlot.P2:
                return Score_P2;
            case PlayerSlot.P3:
                return Score_P3;
            case PlayerSlot.P4:
                return Score_P4;
            case PlayerSlot.P5:
                return Score_P5;
            default:
                return 0;
        }
    }
    public static void Score_Set(PlayerSlot slot, int amount)
    {
        switch (slot)
        {
            case PlayerSlot.P1:
                Score_P1 = amount;
                break;
            case PlayerSlot.P2:
                Score_P2 = amount;
                break;
            case PlayerSlot.P3:
                Score_P3 = amount;
                break;
            case PlayerSlot.P4:
                Score_P4 = amount;
                break;
            case PlayerSlot.P5:
                Score_P5 = amount;
                break;
        }
    }
    public static void Score_Sub(PlayerSlot slot, int amount)
    {
        switch (slot)
        {
            case PlayerSlot.P1:
                Score_P1 -= amount;
                break;
            case PlayerSlot.P2:
                Score_P2 -= amount;
                break;
            case PlayerSlot.P3:
                Score_P3 -= amount;
                break;
            case PlayerSlot.P4:
                Score_P4 -= amount;
                break;
            case PlayerSlot.P5:
                Score_P5 -= amount;
                break;
        }
    }
    public static void Score_Add(PlayerSlot slot, int amount)
    {
        switch (slot)
        {
            case PlayerSlot.P1:
                Score_P1 += amount;
                break;
            case PlayerSlot.P2:
                Score_P2 += amount;
                break;
            case PlayerSlot.P3:
                Score_P3 += amount;
                break;
            case PlayerSlot.P4:
                Score_P4 += amount;
                break;
            case PlayerSlot.P5:
                Score_P5 += amount;
                break;
        }
    }

}

