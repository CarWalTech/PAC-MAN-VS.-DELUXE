using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GhostViewRadar : ScorecardCommons
{
    public Image pelletIcon;
    public Image container;

    void Start()
    {
        UpdateSkin();
    }

    public override void RefreshSkin()
    {
        UpdateSkin();
    }

    public override bool IsScoreNullified()
    {
        return false;
    }

    void UpdateSkin()
    {
        var skin = GetSkin();
        if (skin == null) return;

        try
        {
            container.sprite = skin.gr_Container;
            pelletIcon.sprite = skin.gr_PelletIcon;
            UpdateSkinBase(skin.gr_ScoreNumbers);
            UpdateScore();
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
