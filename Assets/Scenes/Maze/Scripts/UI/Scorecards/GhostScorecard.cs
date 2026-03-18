using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GhostScorecard : ScorecardCommons, ISkinableBehavior
{
    public SkinManager skinManager = null;

    public bool suppressCOMScore = false;

    public Color cardColor = Color.red;
    public PlayerNumber player = PlayerNumber.P1;
    public PlayerType character = PlayerType.Ghost;
    public PlayerSlot inputSlot = PlayerSlot.P1;



    [Header("Images")]
    public GameObject playerTitleContainer;
    public GameObject playerTitleAltContainer;
    public Image playerPacImage;
    public Image playerGhostImage;
    public Image playerGhostEyesImage;
    public Image playerPrefixImage;
    public Image playerNumberImage;
    public Image backgroundImage;


    private Sprite[] __player_index_sprites = new Sprite[]{};

    // Start is called once before the first execution of Update after the MonoBehaviour is created



    void Start()
    {
        UpdateSkin();
    }

    public void RefreshSkin()
    {
        UpdateSkin();
    }

    public override bool IsScoreNullified()
    {
        return player == PlayerNumber.Computer && suppressCOMScore;
    }

    void UpdateSkin()
    {
        if (!skinManager) return;
        if (!skinManager.guiTheme) return;
        skinManager.AddHook(this);

        try
        {
            __player_index_sprites = skinManager.guiTheme.gs_HeaderNumbers;
            playerPrefixImage.sprite = skinManager.guiTheme.gs_PlayerHeader;
            playerTitleAltContainer.GetComponentInChildren<Image>(true).sprite = skinManager.guiTheme.gs_COMHeader;
            backgroundImage.sprite = skinManager.guiTheme.gs_Container;
            playerGhostImage.sprite = skinManager.guiTheme.gs_GhostBase;
            playerGhostEyesImage.sprite = skinManager.guiTheme.gs_GhostEyes;
            playerPacImage.sprite = skinManager.guiTheme.gs_PacMan;
            UpdateSkinBase(skinManager.guiTheme.gs_ScoreNumbers);
            UpdateScore();
            UpdateLabels();
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to set skin for GhostScorecard: " + ex);
        }   
    }

    void UpdateBackground(bool force = false)
    {
        if (backgroundImage != null)
        {
            if (cardColor != backgroundImage.color || force)
            {
                backgroundImage.color = cardColor;   
                playerPrefixImage.color = cardColor; 
                playerNumberImage.color = cardColor; 
                playerGhostImage.color = cardColor;
            }
        }
    }

    void UpdateIcon()
    {
        if (character == PlayerType.PacMan)
        {
            playerPacImage.enabled = true;
            playerGhostImage.enabled = false;
            playerGhostEyesImage.enabled = false;
        }
        else
        {
            playerPacImage.enabled = false;
            playerGhostImage.enabled = true;
            playerGhostEyesImage.enabled = true;
        }
    }

    void UpdateLabels()
    {
        try { 
            if (player == PlayerNumber.Computer)
            {
                playerTitleAltContainer.SetActive(true);
                playerTitleContainer.SetActive(false);
            }
            else
            {
                if (__player_index_sprites.Count() > (int)player)
                    playerNumberImage.sprite = __player_index_sprites[(int)player];
                    
                playerTitleAltContainer.SetActive(false);
                playerTitleContainer.SetActive(true);
            }
            
        } 
        catch (Exception ex)
        {
            Debug.LogError("Unable to update labels for GhostScorecard: " + ex);
        }   
    }

    void OnValidate()
    {
        UpdateSkin();
        UpdateScore(true);
        UpdateBackground(true);
        UpdateLabels();
        UpdateIcon();
    }

    bool IsPlayerActive()
    {
        if (GameManager.Instance == null) return true;

        if (inputSlot == PlayerSlot.P1) return true;
        else if (inputSlot == PlayerSlot.P2) return true;
        else if (inputSlot == PlayerSlot.P3) return GameManager.Instance.GetMatchManager().playerCount >= 3;
        else if (inputSlot == PlayerSlot.P4) return GameManager.Instance.GetMatchManager().playerCount >= 4;
        else if (inputSlot == PlayerSlot.P5) return GameManager.Instance.GetMatchManager().playerCount >= 5;
        else return false;
    }

    void RefreshData()
    {
        gameObject.SetActive(IsPlayerActive());
        
        var sel_char = GameConfiguration.GetCharacter(inputSlot);
        cardColor = PlayerPicker.GetGhostColor(sel_char);
        score = GameConfiguration.Score_Get(inputSlot);
        
        switch (sel_char)
        {
            case PlayerCharacter.COM:
                suppressCOMScore = true;
                player = PlayerNumber.Computer;
                break;
            case PlayerCharacter.Blinky:
            case PlayerCharacter.Inky:
            case PlayerCharacter.Clyde:
            case PlayerCharacter.Pinky:
                suppressCOMScore = false;
                player = PlayerNumber.Computer;
                break;
            case PlayerCharacter.P1:
                player = PlayerNumber.P1;
                break;
            case PlayerCharacter.P2:
                player = PlayerNumber.P2;
                break;
            case PlayerCharacter.P3:
                player = PlayerNumber.P3;
                break;
            case PlayerCharacter.P4:
                player = PlayerNumber.P4;
                break;
            case PlayerCharacter.P5:
                player = PlayerNumber.P5;
                break;

        }

        if (GameConfiguration.PlayerSlot_PacMan == inputSlot)
            character = PlayerType.PacMan;
        else
            character = PlayerType.Ghost;
    }

    void Update()
    {
        RefreshData();
        UpdateScore();
        UpdateBackground();
        UpdateLabels();
        UpdateIcon();
    }
}
