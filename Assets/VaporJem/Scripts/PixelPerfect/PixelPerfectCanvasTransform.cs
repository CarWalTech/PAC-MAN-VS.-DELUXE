using System.Linq;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[ExecuteAlways]
public class PixelPerfectCanvasTransform : MonoBehaviour
{
    public enum Mode
    {
        None, 
        Manual,
        Mirror,
    }

    public Mode mode = Mode.Mirror;


    //Manual Mode
    [SerializeField, Rename("Position")] public Vector2 positionDelta;
    [SerializeField, Rename("Size"), FormerlySerializedAs("pixelSize")] public Vector2 sizeDelta;

    [Space(10f)]
    [SerializeField, FoldoutGroup("Anchors", false, nameof(anchorMin), nameof(anchorMax))] private Void groupHolder1;
    [SerializeField, HideProperty, Rename("Min")] public Vector2 anchorMin;
    [SerializeField, HideProperty, Rename("Max")] public Vector2 anchorMax;
    [SerializeField] public Vector2 pivot;

    //Mirror Mode
    [SerializeField, ShowField(nameof(mode), Mode.Mirror), FormerlySerializedAs("sourceImage")] public Image mirrorTarget;



    private RectTransform __rectTransform = null;
    private PixelPerfectCanvasManager __manager = null;
    private LayoutGroup __layoutGroup = null;

    void Awake()
    {

    }
    void Start()
    {

    }

    bool IsValid()
    {
        bool Check()
        {
            if (!__manager) return false;
            else if (!__rectTransform) return false;
            else return true;
        }
        bool valid = Check();

        if (!valid)
        {
            if (!__rectTransform) __rectTransform = GetComponent<RectTransform>();
            if (!__manager) __manager = FindObjectsByType<PixelPerfectCanvasManager>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault();
            return Check();
        }
        else return valid;

    }
    

    void Update()
    {
        if (!IsValid()) return;
    
        float rPPU = __manager.GetActualPPUScale();

        float worldPosX = positionDelta.x / rPPU;
        float worldPosY = positionDelta.y / rPPU;

        float worldWidth;
        float worldHeight;

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

        __rectTransform.pivot = pivot;
        __rectTransform.anchorMin = anchorMin;
        __rectTransform.anchorMax = anchorMax;

        __rectTransform.SetFreeformSizeAndPosition(new Vector2(worldPosX * rPPU, worldPosY * rPPU), new Vector2(worldWidth * rPPU, worldHeight * rPPU));

    }
}
