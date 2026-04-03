using System;
using UnityEngine;

public abstract class IPixelPerfectCanvasComponent : MonoBehaviour
{
    private DateTime __timeSinceLastUpdate = new DateTime();
    private bool __forceUpdate = false;
    public void ValidatePixelPerfect()
    {
        __forceUpdate = true;
        UpdatePixelPerfect();
    }

    public virtual void UpdatePixelPerfect()
    {
        if (__timeSinceLastUpdate.AddSeconds(3) > DateTime.Now && __forceUpdate == false) return;
        __forceUpdate = false;
        __timeSinceLastUpdate = DateTime.Now;
    }
}