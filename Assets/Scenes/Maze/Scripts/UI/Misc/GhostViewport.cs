using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GhostViewport : ViewportThemeHolder
{
    public Color cardColor = Color.red;
    public PlayerNumber player = PlayerNumber.P1;

    public Image background = null;
    public Image cover = null;
    public Image mask = null;
    public Image label = null;

    public RenderTexture source = null;
    public RawImage viewport;

    public Vector3 rotation;

    private Sprite[] __player_header_sprites = new Sprite[]{};
    private Sprite __com_header_sprite = null;

    public enum SkinMode
    {
        Maze,
        MazeMain,
        Ghost
    }

    public SkinMode skinMode = SkinMode.Ghost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        UpdateSkin();
    }
    public override void RefreshSkin()
    {
        UpdateSkin();
    }

    public void SetViewport(RenderTexture src)
    {
        source = src;
    }


    void OnValidate()
    {
        UpdateSkin();
        UpdateBackground(true);
        UpdateLabels();
        UpdateRotation();
    }
    void UpdateBackground(bool force = false)
    {
        var currentColor = background != null ? background.color : Color.black;
        if (cardColor != currentColor || force)
        {
            if (background != null) background.color = cardColor;
            if (cover != null) cover.color = skinMode == SkinMode.Ghost || skinMode == SkinMode.Maze ? cardColor : Color.black;
            if (label != null) label.color = cardColor;
        }
    }
    void UpdateRotation()
    {
        if (skinMode == SkinMode.Ghost)
        {
            if (background != null) background.GetComponent<RectTransform>().localRotation = Quaternion.Euler(rotation);
            if (cover != null) cover.GetComponent<RectTransform>().localRotation = Quaternion.Euler(rotation);
            if (label != null) label.GetComponent<RectTransform>().rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            if (background != null) background.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            if (cover != null) cover.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            if (label != null) label.GetComponent<RectTransform>().rotation = Quaternion.Euler(Vector3.zero);
        }

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
                    __player_header_sprites = skin.gvm_PlayerNumbers;
                    __com_header_sprite = skin.gvm_COMHeader;
                    if (background != null) background.sprite = skin.gvm_ViewportContainer;
                    if (cover != null) cover.sprite = skin.gvm_ViewportFrame;
                    if (mask != null) mask.sprite = skin.gvm_ViewportMask;
                    break;
                case SkinMode.MazeMain:
                    __player_header_sprites = skin.gvm_PlayerNumbers;
                    __com_header_sprite = skin.gvm_COMHeader;
                    if (background != null) background.sprite = null;
                    if (cover != null) cover.sprite = null;
                    if (mask != null) mask.sprite = null;
                    break;
                case SkinMode.Ghost:
                    __player_header_sprites = skin.gv_PlayerNumbers;
                    __com_header_sprite = skin.gv_COMHeader;
                    if (background != null) background.sprite = skin.gv_ViewportContainer;
                    if (cover != null) cover.sprite = skin.gv_ViewportFrame;
                    if (mask != null) mask.sprite = skin.gv_ViewportMask;
                    break;
            }
            
            UpdateBackground();
            UpdateLabels();

        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to set skin for GhostViewport: " + ex);
        }   
    }
    void UpdateViewport()
    {
        if (viewport.texture != source)
        {
            viewport.texture = source;
            viewport.enabled = viewport.texture != null;
        }
    }
    void UpdateLabels()
    {
        try { 
            if (player == PlayerNumber.Computer)
            {
                label.sprite = __com_header_sprite;
            }
            else
            {
                if (__player_header_sprites.Count() > (int)player)
                    label.sprite = __player_header_sprites[(int)player];
            }
            
        } 
        catch (Exception ex)
        {
            Debug.LogError("Unable to update labels for GhostViewport: " + ex);
        }   
    }
    void Update()
    {
        UpdateBackground();
        UpdateLabels();
        UpdateViewport();
        UpdateRotation();
    }


}
