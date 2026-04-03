using System.Linq;
using UnityEngine;

public class PixelPerfectCanvasSettings
{
    private static PixelPerfectCanvasSettings Instance = new PixelPerfectCanvasSettings();

    private PixelPerfectCanvasManager canvasManager = null;

    public static PixelPerfectCanvasManager GetCanvasManager()
    {
        if (Instance.canvasManager != null) return Instance.canvasManager;

        Instance.canvasManager = Object.FindObjectsByType<PixelPerfectCanvasManager>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault();
        return Instance.canvasManager;

    }

    public static bool CanUpdate(MonoBehaviour behaviour)
    {
        if (behaviour.didAwake && behaviour.didStart) return true;
        else return false;
    }
}