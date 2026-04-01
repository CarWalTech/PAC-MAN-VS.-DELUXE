using Unity.Mathematics;
using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetDefaultScale(this RectTransform trans) {
        trans.localScale = new Vector3(1, 1, 1);
    }
    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec) {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans) {
        return trans.rect.size;
    }
    public static float GetWidth(this RectTransform trans) {
        return trans.rect.width;
    }
    public static float GetHeight(this RectTransform trans) {
        return trans.rect.height;
    }



    public static void SetFreeformSizeAndPosition(this RectTransform trans, Vector2 newPos, Vector2 newSize)
    {   


        bool stretch_x = trans.anchorMin.x != trans.anchorMax.x;
        bool stretch_y = trans.anchorMin.y != trans.anchorMax.y;

        if (trans.anchorMin.x == trans.anchorMax.x) stretch_x = false;
        if (trans.anchorMin.y == trans.anchorMax.y) stretch_y = false;

        var left = newPos.x;    //aka Pos X
        var top = newPos.y;     //aka Pos Y        
        var right = newSize.x;  //aka Width
        var bottom = newSize.y; //aka Height

        if (stretch_x && stretch_y)
        {
            trans.offsetMin = new Vector2(left, bottom);
            trans.offsetMax = new Vector2(-right, -top); 
        }
        else
        {
            if (stretch_x)
            {
                trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, newPos.y);
                trans.SetHeight(newSize.y);
                trans.offsetMin = new Vector2(left, trans.offsetMin.y);
                trans.offsetMax = new Vector2(-right, trans.offsetMax.y); 
            }
            else if (stretch_y)
            {
                trans.SetWidth(newSize.x);
                trans.anchoredPosition = new Vector2(newPos.x, trans.anchoredPosition.y);
                trans.offsetMin = new Vector2(trans.offsetMin.x, bottom);
                trans.offsetMax = new Vector2(trans.offsetMax.x, top); 
            }
            else
            {
                trans.anchoredPosition = newPos;
                trans.sizeDelta = newSize;
            }
        }

    }

    public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize) {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
    public static void SetWidth(this RectTransform trans, float newSize) {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
    public static void SetHeight(this RectTransform trans, float newSize) {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }
}