using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GhostViewRadar : ScorecardCommons, ISkinableBehavior
{
    public SkinManager skinManager = null;
    public Image pelletIcon;
    public Image container;

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
        return false;
    }

    void UpdateSkin()
    {
        if (!skinManager) return;
        if (!skinManager.guiTheme) return;
        skinManager.AddHook(this);

        try
        {
            container.sprite = skinManager.guiTheme.gr_Container;
            pelletIcon.sprite = skinManager.guiTheme.gr_PelletIcon;
            base.UpdateSkin(skinManager.guiTheme.gr_ScoreNumbers);
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to set skin for GhostViewRadar: " + ex);
        }   
    }

    void RefreshScore()
    {
        if (GameManager.Instance != null) score = GameManager.Instance.GetPelletsLeft();
    }

    void OnValidate()
    {
        RefreshScore();
        UpdateSkin();
        UpdateScore(true);
    }

    // Update is called once per frame
    void Update()
    {
        RefreshScore();
        UpdateScore();
    }
}
