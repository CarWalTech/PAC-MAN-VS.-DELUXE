using System.Linq;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PixelPerfectCanvasImage : IPixelPerfectCanvasComponent
{
    private Image canvasImage;
    public bool useOffset = false;
    [SerializeField, ShowField(nameof(useOffset))] public Vector3 offset = Vector3.zero;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    void OnValidate()
    {
        ValidatePixelPerfect();
    }

    void Update()
    {
        UpdatePixelPerfect();
    }

    public override void UpdatePixelPerfect()
    {
        base.UpdatePixelPerfect();
        if (!PixelPerfectCanvasSettings.CanUpdate(this)) return;
        
        
        var canvas = GetComponentInParent<Canvas>();
        var manager = PixelPerfectCanvasSettings.GetCanvasManager();
        if (!manager) return;
        if (!canvas) return;
        if (canvasImage.sprite)
        {
            var imageSize = canvasImage.sprite.rect.size;
            var pixelsPerUnit = canvasImage.sprite.pixelsPerUnit;
            var refrencePixelsPerUnit = manager.GetActualPPUScale();

            var worldWidth = imageSize.x / pixelsPerUnit;
            var worldHeight = imageSize.y / pixelsPerUnit;

            if (useOffset) rectTransform.anchoredPosition = new Vector3(offset.x, offset.y, offset.z);
            rectTransform.sizeDelta = new Vector2(worldWidth * refrencePixelsPerUnit, worldHeight * refrencePixelsPerUnit);
        }
    }
}
