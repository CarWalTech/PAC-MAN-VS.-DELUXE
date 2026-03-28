using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class SizeFitter : MonoBehaviour {
    
    public RectTransform source = null;

    void OnValidate()
    {

    }
    void Update()
    {
        CheckForChanges();
    }
    public void CheckForChanges() {
        if (!source) return;

        Vector2 sizeDelta = source.sizeDelta;
        Vector3 scaleDelta = source.lossyScale;

        float sizeScaleX = sizeDelta.x * scaleDelta.x;
        float sizeScaleY = sizeDelta.y * scaleDelta.y;

        GetComponent<RectTransform>().sizeDelta = new Vector2(sizeScaleX, sizeScaleY);
    }
}