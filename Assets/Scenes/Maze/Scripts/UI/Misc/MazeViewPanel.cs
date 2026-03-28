using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class MazeViewPanel : ViewportThemeHolder
{
    public enum SkinMode
    {
        Normal,
        ViewportFrame,
        ViewportMask,
        ViewportPanel
    }

    public Image panel;
    public SkinMode mode = SkinMode.Normal;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkin();
    }

    void UpdateSkin()
    {
        var skin = GetSkin();
        if (skin == null) return;

        try
        {
            switch (mode)
            {
                case SkinMode.ViewportFrame:
                    panel.sprite = skin.sb_ViewportFrame;
                    panel.pixelsPerUnitMultiplier = panel.sprite.pixelsPerUnit;
                    break;
                case SkinMode.ViewportMask:
                    panel.sprite = skin.sb_ViewportMask;
                    panel.pixelsPerUnitMultiplier = panel.sprite.pixelsPerUnit;
                    break;
                case SkinMode.ViewportPanel:
                    panel.sprite = skin.sb_ViewportPanel;
                    panel.pixelsPerUnitMultiplier = panel.sprite.pixelsPerUnit;
                    break;
                default:
                    panel.sprite = skin.sb_Frame;
                    panel.pixelsPerUnitMultiplier = panel.sprite.pixelsPerUnit;
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to set skin for MazeViewPanel: " + ex);
        }   
    }

    void OnValidate()
    {
        UpdateSkin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void RefreshSkin()
    {
        UpdateSkin();
    }
}
