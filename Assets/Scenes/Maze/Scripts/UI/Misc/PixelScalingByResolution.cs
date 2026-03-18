
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PixelScalingByResolution : MonoBehaviour
{
    public RectTransform rectTransform;
    public CanvasScaler canvasScaler;
    public Canvas canvas;
    public Vector2 baseResolution = new Vector2(320, 180);
    public float multiplier = 0.25f;
    public bool round = false;

    void Awake()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScale();
    }

    void OnValidate()
    {
        UpdateScale();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScale();
    }

    private static float RoundTo(float value, float multipleOf) {
        return Mathf.Round(value/multipleOf) * multipleOf;
    }

    void UpdateScale()
    {
        if (!rectTransform) return;
        if (!canvasScaler) return;
        if (!canvas) return;
        
        float current_width, current_height;
        float new_scaleX, new_scaleY;
        current_width = Display.displays[canvas.worldCamera.targetDisplay].renderingWidth;
        current_height = Display.displays[canvas.worldCamera.targetDisplay].renderingHeight;

        new_scaleX = current_width / baseResolution.x * multiplier;
        new_scaleY = current_height / baseResolution.y * multiplier;
        
        if (round)
        {
          new_scaleX = RoundTo(new_scaleX, multiplier);
          new_scaleY = RoundTo(new_scaleY, multiplier);  
        } 

        if (new_scaleY > new_scaleX)
        {
            canvasScaler.scaleFactor = new_scaleX;
        }
        else
        {
            canvasScaler.scaleFactor = new_scaleY;
        }
    }
}
