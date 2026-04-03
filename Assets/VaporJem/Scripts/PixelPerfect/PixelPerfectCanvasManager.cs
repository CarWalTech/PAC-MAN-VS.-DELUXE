

using EditorAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[ExecuteAlways]
public class PixelPerfectCanvasManager : IPixelPerfectCanvasComponent
{
    public class CanvasScale
    {
        public float scaleX;
        public float scaleY;
        public float displayWidth;
        public float displayHeight;

        private bool _isNull = true;
        public bool isNull => _isNull;

        public static CanvasScale Null = new CanvasScale();

        public CanvasScale()
        {
            _isNull = true;
        }

        public CanvasScale(float x, float y, float width, float height)
        {
            scaleX = x;
            scaleY = y;
            displayWidth = width;
            displayHeight = height;
            _isNull = false;
        }

        private static float RoundTo(float value, float multipleOf) {
            return Mathf.Round(value/multipleOf) * multipleOf;
        }

        public static CanvasScale GetParams(PixelPerfectCanvasScaler target, Vector2 baseResolution, float multiplier, bool round)
        {   
            int targetDisplay = target.canvasCamera.targetDisplay;

            float current_width, current_height;
            float new_scaleX, new_scaleY;
            if (Display.displays.Length <= targetDisplay) return CanvasScale.Null;
            
            current_width = Display.displays[targetDisplay].renderingWidth;
            current_height = Display.displays[targetDisplay].renderingHeight;

            new_scaleX = current_width / baseResolution.x * multiplier;
            new_scaleY = current_height / baseResolution.y * multiplier;
            
            if (round)
            {
                new_scaleX = RoundTo(new_scaleX, multiplier);
                new_scaleY = RoundTo(new_scaleY, multiplier);  
            } 

            return new CanvasScale(new_scaleX, new_scaleY, current_width, current_height);
        }

        public float GetScaleFactor(PixelPerfectCanvasScaler target)
        {
            if (scaleY > scaleX) return scaleX;
            else return scaleY;
        }
    
        public void UpdateContainerSize(PixelPerfectCanvasScaler target, ref RectTransform containerRect, ref RectTransform canvasRect)
        {
            Vector2 actualSizeDelta = new Vector2(canvasRect.sizeDelta.x * canvasRect.localScale.x, canvasRect.sizeDelta.y * canvasRect.localScale.y);
            containerRect.sizeDelta = actualSizeDelta;
            containerRect.localScale = new Vector3(1,1,1);
            //var width = (displayWidth / scaleX) * target.canvas.transform.localScale.x;
            //var height = (displayHeight / scaleY) * target.canvas.transform.localScale.y;
        }
    }

    public Vector2 baseResolution = new Vector2(320, 180);
    public float multiplier = 0.25f;
    public bool round = false;
 
    [FoldoutGroup("Pixel Perfect Camera Settings", nameof(referenceResolution), nameof(assetsPixelsPerUnit), nameof(cropFrame), nameof(gridSnapping))]
    [SerializeField] private Void __pixelPerfectCamGrpHolder;

    [SerializeField, HideProperty] public Vector2Int referenceResolution;
    [SerializeField, HideProperty] public int assetsPixelsPerUnit;
    [SerializeField, HideProperty] public PixelPerfectCamera.CropFrame cropFrame;
    [SerializeField, HideProperty] public PixelPerfectCamera.GridSnapping gridSnapping;


    void OnValidate()
    {
        ValidatePixelPerfect();
    }

    void Update()
    {
        UpdatePixelPerfect(); 
    }

    public float GetActualPPUScale()
    {
        return assetsPixelsPerUnit;
    }

    void UpdateItem(PixelPerfectCanvasScaler target)
    {
        if (!target.rectTransform) return;
        if (!target.canvasScaler) return;
        if (!target.canvas) return;
        if (!target.pixelPerfectCamera) return;
        if (!target.canvasCamera) return;
        if (!target.container) return;

        var scale = CanvasScale.GetParams(target, baseResolution, multiplier, round);
        if (scale.isNull) return;

        var containerRect = target.container.GetComponent<RectTransform>();
        if (!containerRect) return;

        var canvasRect = target.canvas.GetComponent<RectTransform>();
        if (!canvasRect) return;

        //Update Pixel Perfect Camera
        target.pixelPerfectCamera.assetsPPU = assetsPixelsPerUnit;
        target.pixelPerfectCamera.refResolutionX = referenceResolution.x;
        target.pixelPerfectCamera.refResolutionY = referenceResolution.y;
        target.pixelPerfectCamera.cropFrame = cropFrame;
        target.pixelPerfectCamera.gridSnapping = gridSnapping;
        
        //Update Canvas Scaler
        target.canvasScaler.scaleFactor = scale.GetScaleFactor(target);
        target.canvasScaler.referencePixelsPerUnit = assetsPixelsPerUnit;

        //Update Container /  Canvas Sizes
        scale.UpdateContainerSize(target, ref containerRect, ref canvasRect);
    }

    public override void UpdatePixelPerfect()
    {
        base.UpdatePixelPerfect();
        if (!PixelPerfectCanvasSettings.CanUpdate(this)) return;
    
        var items = FindObjectsByType<PixelPerfectCanvasScaler>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var item in items)
        {
            UpdateItem(item);
        }
    }
}
