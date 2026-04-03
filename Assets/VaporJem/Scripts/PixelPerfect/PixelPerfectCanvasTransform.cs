using System.Linq;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[ExecuteAlways]
public class PixelPerfectCanvasTransform : IPixelPerfectCanvasComponent
{
    public enum Mode
    {
        None, 
        Manual,
        Mirror,
    }

    public Mode mode = Mode.None;

    [SerializeField] public bool modifySize = true;
    [SerializeField] public bool modifyPosition = true;
    [SerializeField] public bool modifyAnchors = true;
    [SerializeField] public bool modifyPivot = true;

    [SerializeField, ShowField(nameof(mode), Mode.Mirror), FormerlySerializedAs("sourceImage")] public Image mirrorTarget;


    [SerializeField, VerticalGroup("", false, nameof(positionDelta), nameof(sizeDelta))] private Void geometryGroup;
    [SerializeField, Rename("Position")] public Vector2 positionDelta;
    [SerializeField, Rename("Size"), FormerlySerializedAs("pixelSize")] public Vector2 sizeDelta;
    [SerializeField, VerticalGroup("Anchors", false, nameof(anchorMin), nameof(anchorMax))] private Void anchorsGroup;
    [SerializeField, HideProperty, Rename("Min")] public Vector2 anchorMin;
    [SerializeField, HideProperty, Rename("Max")] public Vector2 anchorMax;
    [SerializeField] public Vector2 pivot;

    
    private RectTransform __rt = null;

    void OnValidate()
    {
        ValidatePixelPerfect();
    }

    void Update()
    {
        UpdatePixelPerfect();
    }

    RectTransform GetRectTransform()
    {
        if (__rt != null) return __rt;
        __rt = GetComponent<RectTransform>();
        return __rt;
    }
    
    public override void UpdatePixelPerfect()
    {
        base.UpdatePixelPerfect();
        if (!PixelPerfectCanvasSettings.CanUpdate(this)) return;

        var manager = PixelPerfectCanvasSettings.GetCanvasManager();
        if (!manager) return;

        var transform = GetRectTransform();
        if (!transform) return;

    
        float rPPU = manager.GetActualPPUScale();

        float worldPosX = positionDelta.x / rPPU;
        float worldPosY = positionDelta.y / rPPU;

        float worldWidth;
        float worldHeight;

        Vector2 worldPosition;
        Vector2 worldSize;

        if (mode == Mode.Mirror)
        {
            if (!mirrorTarget) return;    
            if (!mirrorTarget.sprite) return;

            float aPPU = mirrorTarget.sprite.pixelsPerUnit;
            worldWidth = mirrorTarget.sprite.rect.size.x / aPPU;
            worldHeight = mirrorTarget.sprite.rect.size.y / aPPU;
        }
        else
        {
            worldWidth = sizeDelta.x / rPPU;
            worldHeight = sizeDelta.y / rPPU;
        }

        if (modifyPivot) transform.pivot = pivot;
        if (modifyAnchors) transform.anchorMin = anchorMin;
        if (modifyAnchors) transform.anchorMax = anchorMax;

        worldPosition = new Vector2(worldPosX * rPPU, worldPosY * rPPU);
        worldSize = new Vector2(worldWidth * rPPU, worldHeight * rPPU);

        if (modifyPosition && modifySize) transform.SetFreeformSizeAndPosition(worldPosition, worldSize);
        else if (modifySize) transform.SetSize(worldSize);
        else if (modifyPosition) transform.SetFreeformSizeAndPosition(worldPosition, transform.sizeDelta);

    }
}
