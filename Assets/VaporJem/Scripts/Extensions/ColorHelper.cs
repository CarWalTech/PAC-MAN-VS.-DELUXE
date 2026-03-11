using UnityEngine;

public static class ColorHelper
{
    public static Color FromString(string val)
    {
        if (ColorUtility.TryParseHtmlString(val, out Color color)) return color;
        return Color.black;
    }
}