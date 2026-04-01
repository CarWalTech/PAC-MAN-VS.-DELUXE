using System.Linq;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PixelPerfectCanvasImage : MonoBehaviour
{
    private Image canvasImage;
    public bool useOffset = false;
    [SerializeField, ShowField(nameof(useOffset))] public Vector3 offset = Vector3.zero;
    private RectTransform rectTransform;
    private PixelPerfectCanvasManager __manager = null;

    void Awake()
    {
        canvasImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (!__manager) __manager = FindObjectsByType<PixelPerfectCanvasManager>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault();
        
        var canvas = GetComponentInParent<Canvas>();
        if (!__manager) return;
        if (!canvas) return;
        if (canvasImage.sprite)
        {
            var imageSize = canvasImage.sprite.rect.size;
            var pixelsPerUnit = canvasImage.sprite.pixelsPerUnit;
            var refrencePixelsPerUnit = __manager.GetActualPPUScale();

            var worldWidth = imageSize.x / pixelsPerUnit;
            var worldHeight = imageSize.y / pixelsPerUnit;

            if (useOffset) rectTransform.anchoredPosition = new Vector3(offset.x, offset.y, offset.z);
            rectTransform.sizeDelta = new Vector2(worldWidth * refrencePixelsPerUnit, worldHeight * refrencePixelsPerUnit);
        }
    }
}
