using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class TargetScorecard : ScorecardCommons, ISkinableBehavior
{
    public enum SkinMode
    {
        Maze,
        Ghost
    }

    public SkinManager skinManager = null;
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
        if (!skinManager) return;
        if (!skinManager.guiTheme) return;
        skinManager.AddHook(this);

        try
        {
            switch (skinMode)
            {
                case SkinMode.Maze:
                    base.UpdateSkin(skinManager.guiTheme.mts_Numbers);
                    background.sprite = skinManager.guiTheme.mts_Background;
                    break;
                case SkinMode.Ghost:
                    base.UpdateSkin(skinManager.guiTheme.gts_Numbers);
                    background.sprite = skinManager.guiTheme.gts_Background;
                    break;
            }
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
    public void RefreshSkin()
    {
        UpdateSkin();
    }
}
