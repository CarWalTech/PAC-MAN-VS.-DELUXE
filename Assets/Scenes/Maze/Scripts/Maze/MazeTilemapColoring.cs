
using System;
using EditorAttributes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class MazeTilemapColoring : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Tilemap outputTilemap;
    [SerializeField] private TilemapRenderer spriteRenderer;
    [SerializeField] private Tilemap inputTilemap;
    [SerializeField] public Material material;
    [SerializeField, ButtonField(nameof(Refresh), "Refresh")] private EditorAttributes.Void refreshHolder;


    private Vector2 _lastSizeDelta = Vector2.zero;
    private Vector3 _lastPosition = Vector3.zero;

    void Awake()
    {
        
    }

    void Start()
    {
        CheckSizeChanges();
    }

    void OnValidate()
    {
        Invoke(nameof(CheckSizeChanges), 0f);
    }

    public void Refresh()
    {
        UpdateTilemap();
        foreach (var item in GetComponentsInChildren<MazeColormapLayer>())
            item.RefreshSprite();
    }

    void CheckSizeChanges()
    {
        var needsUpdate = false;
        if (_lastSizeDelta != rectTransform.sizeDelta)
        {
            needsUpdate = true;
            _lastSizeDelta = rectTransform.sizeDelta;
        }
        if (_lastPosition != rectTransform.position)
        {
            needsUpdate = true;
            _lastPosition = rectTransform.position;
        }

        if (needsUpdate)
        {
            Refresh();
        }
    }

    Vector3 GetTileAnchor()
    {
        var x = -rectTransform.localPosition.x + inputTilemap.tileAnchor.x;
        var y = -rectTransform.localPosition.y - rectTransform.sizeDelta.y - inputTilemap.tileAnchor.y + 1;
        var z = inputTilemap.tileAnchor.z;

        return new Vector3(x, y, z);
    }

    Vector2 GetChunkPos()
    {
        var x = (int)rectTransform.localPosition.x - 2;
        var y = (int)rectTransform.localPosition.y - 2;
        return new Vector2(x, y);
    }

    Vector2 GetChunkSize()
    {
        var x = (int)rectTransform.sizeDelta.x + 4;
        var y = (int)rectTransform.sizeDelta.y + 4;
        return new Vector2(x, y);
    }

    Vector3 GetChunkCullingBounds()
    {   
        var anchor = GetTileAnchor();
        return new Vector3(anchor.x, Math.Abs(anchor.y) * 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSizeChanges();
    }

    void UpdateTilemap()
    {
        if (!outputTilemap) return;
        if (!inputTilemap) return;
        if (!material) return;
        if (!spriteRenderer) return;


        spriteRenderer.material = material;
        spriteRenderer.detectChunkCullingBounds = TilemapRenderer.DetectChunkCullingBounds.Manual;
        spriteRenderer.chunkCullingBounds = GetChunkCullingBounds();

        outputTilemap.tileAnchor = GetTileAnchor();
        outputTilemap.Overwrite(inputTilemap, GetChunkPos(), GetChunkSize());
    }
}
