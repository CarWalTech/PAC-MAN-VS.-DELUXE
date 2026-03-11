using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacManView : MonoBehaviour
{
    public RawImage viewport1 = null;
    public RenderTexture source1 = null;

    public bool useWorldView = false;

    public RawImage viewport2 = null;
    public RenderTexture source2 = null;

    private Camera _cam;

    void Start()
    {
        _cam = GetComponentInChildren<Camera>();
    }

    public void SetActive(bool val)
    {
        _cam.gameObject.SetActive(val);
    }
    public void UpdateCameras(List<IGameCamera> cameras)
    {
        void UpdateViewport(int index, ref RenderTexture source)
        {
            if (cameras.Count >= index + 1)
            {
                var item = cameras[index];
                if (item == null) source = null;
                else source = item.GetViewport();
            }
            else
            {
                source = null;
            }
        }

        UpdateViewport(0, ref source1);
        if (useWorldView) UpdateViewport(1, ref source2);
    }

    // Update is called once per frame
    void Update()
    {
        if (viewport1.texture != source1)
        {
            viewport1.texture = source1;
            viewport1.enabled = viewport1.texture != null;
        }

        if (useWorldView)
        {
            if (viewport2.texture != source2)
            {
                viewport2.texture = source2;
                viewport2.enabled = viewport2.texture != null;
            }
        }

    }
}
