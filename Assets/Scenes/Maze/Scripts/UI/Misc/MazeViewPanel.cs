using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class MazeViewPanel : MonoBehaviour, ISkinableBehavior
{
    public enum SkinMode
    {
        Normal,
        ViewportFrame,
        ViewportMask
    }
    public SkinManager skinManager = null;
    public Image panel;
    public SkinMode mode = SkinMode.Normal;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkin();
    }

    void UpdateSkin()
    {
        if (!skinManager) return;
        if (!skinManager.guiTheme) return;
        skinManager.AddHook(this);

        try
        {
            switch (mode)
            {
                case SkinMode.ViewportFrame:
                    panel.sprite = skinManager.guiTheme.sb_ViewportFrame;
                    break;
                case SkinMode.ViewportMask:
                    panel.sprite = skinManager.guiTheme.sb_ViewportMask;
                    break;
                default:
                    panel.sprite = skinManager.guiTheme.sb_Frame;
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

    public void RefreshSkin()
    {
        UpdateSkin();
    }
}
