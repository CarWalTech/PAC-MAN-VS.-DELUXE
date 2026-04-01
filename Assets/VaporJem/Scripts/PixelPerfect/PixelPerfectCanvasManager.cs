
using EditorAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[ExecuteAlways]
public class PixelPerfectCanvasManager : MonoBehaviour
{
    public Vector2 baseResolution = new Vector2(320, 180);
    public float multiplier = 0.25f;
    public bool round = false;

    [FoldoutGroup("Pixel Perfect Camera Settings", nameof(referenceResolution), nameof(assetsPixelsPerUnit), nameof(cropFrame), nameof(gridSnapping))]
    [SerializeField] private Void __pixelPerfectCamGrpHolder;

    [SerializeField, HideProperty] public Vector2Int referenceResolution;
    [SerializeField, HideProperty] public int assetsPixelsPerUnit;
    [SerializeField, HideProperty] public PixelPerfectCamera.CropFrame cropFrame;
    [SerializeField, HideProperty] public PixelPerfectCamera.GridSnapping gridSnapping;

    void Awake()
    {

    }

    void Start()
    {
        UpdateScale();
    }

    void OnValidate()
    {
        UpdateScale();
    }

    void Update()
    {
        UpdateScale();
    }

    public float GetActualPPUScale()
    {
        return assetsPixelsPerUnit;
    }

    public static float RoundTo(float value, float multipleOf) {
        return Mathf.Round(value/multipleOf) * multipleOf;
    }

    void UpdateItem(PixelPerfectCanvasScaler scaler)
    {
        if (!scaler.rectTransform) return;
        if (!scaler.canvasScaler) return;
        if (!scaler.canvas) return;
        if (!scaler.pixelPerfectCamera) return;
        if (!scaler.canvasCamera) return;
        

        scaler.pixelPerfectCamera.assetsPPU = assetsPixelsPerUnit;
        scaler.pixelPerfectCamera.refResolutionX = referenceResolution.x;
        scaler.pixelPerfectCamera.refResolutionY = referenceResolution.y;
        scaler.pixelPerfectCamera.cropFrame = cropFrame;
        scaler.pixelPerfectCamera.gridSnapping = gridSnapping;
        
        float current_width, current_height;
        float new_scaleX, new_scaleY;
        if (Display.displays.Length <= scaler.canvasCamera.targetDisplay) return;
        
        current_width = Display.displays[scaler.canvasCamera.targetDisplay].renderingWidth;
        current_height = Display.displays[scaler.canvasCamera.targetDisplay].renderingHeight;

        new_scaleX = current_width / baseResolution.x * multiplier;
        new_scaleY = current_height / baseResolution.y * multiplier;
        
        if (round)
        {
          new_scaleX = RoundTo(new_scaleX, multiplier);
          new_scaleY = RoundTo(new_scaleY, multiplier);  
        } 

        if (new_scaleY > new_scaleX) scaler.canvasScaler.scaleFactor = new_scaleX;
        else scaler.canvasScaler.scaleFactor = new_scaleY;
        scaler.canvasScaler.referencePixelsPerUnit = assetsPixelsPerUnit;
    }

    void UpdateScale()
    {
        var items = FindObjectsByType<PixelPerfectCanvasScaler>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var item in items) UpdateItem(item);
    }
}
