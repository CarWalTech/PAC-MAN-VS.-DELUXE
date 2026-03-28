using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class TargetScorecard : ScorecardCommons
{
    public enum SkinMode
    {
        Maze,
        Ghost
    }

    public SkinMode skinMode = SkinMode.Maze;

    public Image background;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkin();
    }
    void OnValidate()
    {
        UpdateSkin();
        UpdateScore(true);
    }
    
    void UpdateSkin()
    {
        var skin = GetSkin();
        if (skin == null) return;

        try
        {
            switch (skinMode)
            {
                case SkinMode.Maze:
                    UpdateSkinBase(skin.mts_Numbers);
                    background.sprite = skin.mts_Background;
                    break;
                case SkinMode.Ghost:
                    UpdateSkinBase(skin.gts_Numbers);
                    background.sprite = skin.gts_Background;
                    break;
            }
            UpdateScore();
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to set skin for TargetScorecard: " + ex);
        }   
    }

    void RefreshScore()
    {
        score = GameManager.Instance.GetMatchManager().targetScore;
    }

    void Update()
    {
        RefreshScore();
        UpdateScore();
    }
    public override void RefreshSkin()
    {
        UpdateSkin();
    }
}
