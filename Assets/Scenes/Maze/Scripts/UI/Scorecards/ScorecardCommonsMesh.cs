using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class ScorecardCommonsMesh : MonoBehaviour
{
    public int score;
    private int _lastScore = -1;

    public int charWidth = 0;
    public int charHeight = 0;
    public int charPixelsPerUnit = 0;
    public int charSpacing = 0;
    public Dictionary<int, Vector2Int> charSizeOverrides = new Dictionary<int, Vector2Int>();

    private Sprite __number_0 = null;
    private Sprite __number_1 = null;
    private Sprite __number_2 = null;
    private Sprite __number_3 = null;
    private Sprite __number_4 = null;
    private Sprite __number_5 = null;
    private Sprite __number_6 = null;
    private Sprite __number_7 = null;
    private Sprite __number_8 = null;
    private Sprite __number_9 = null;
    private Sprite __number_null = null;
    private Sprite __number_space = null;

    public SpriteRenderer digit1;
    public SpriteRenderer digit2;
    public SpriteRenderer digit3;
    public SpriteRenderer digit4;
    public SpriteRenderer digit5;

    public bool scorePadding = false;
    public bool scoreCentered = false;


    public virtual bool IsScoreNullified()
    {
        return false;
    }
    public virtual void UpdateSkin(Sprite[] chars)
    {
        
    }
    public void UpdateSkinBase(Sprite[] chars)
    {
        __number_0 = chars[0];
        __number_1 = chars[1];
        __number_2 = chars[2];
        __number_3 = chars[3];
        __number_4 = chars[4];
        __number_5 = chars[5];
        __number_6 = chars[6];
        __number_7 = chars[7];
        __number_8 = chars[8];
        __number_9 = chars[9];
        __number_null = chars[10];
        __number_space = chars[11];
    }
    private Tuple<Vector2Int, bool> UpdateChar(GameObject obj, int index, bool spacingAllowed, bool state)
    {
        int spacing = spacingAllowed && state ? charSpacing : 0;

        int pixelHeight = charHeight;
        int pixelWidth = charWidth;

        if (charSizeOverrides != null && charSizeOverrides.ContainsKey(index))
        {
            pixelWidth = charSizeOverrides[index].x;
            pixelHeight = charSizeOverrides[index].y;
        }

        pixelWidth += spacing;

        float spriteWidth = pixelWidth / (float)charPixelsPerUnit;
        float spriteHeight = pixelHeight / (float)charPixelsPerUnit;

        

    
        obj.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 0.5f);
        obj.GetComponent<LayoutElement>().preferredWidth = spriteWidth;
        obj.GetComponent<LayoutElement>().preferredHeight = spriteHeight;

        if (scoreCentered) obj.SetActive(state);
        else obj.SetActive(true);

        return new Tuple<Vector2Int, bool>(new Vector2Int(pixelWidth, pixelHeight), state);
    }
    private Vector2Int GetSprite(int index, int length, bool onlyZeroes, ref bool afterZeroes, SpriteRenderer image, int intValue)
    {
        bool isActive;
        Vector2Int spriteSize;
        bool spacingAllowed = index < length - 1;
        
        switch (intValue)
        {
            case 0:
                if (afterZeroes)
                {
                    image.sprite = __number_0;
                    (spriteSize, isActive) = UpdateChar(image.gameObject, 0, spacingAllowed, true);
                }
                else if (index == length - 1 && onlyZeroes)
                {
                    image.sprite = __number_0;
                    (spriteSize, isActive) = UpdateChar(image.gameObject, 0, spacingAllowed, true);                        
                }
                else if (scorePadding)
                {
                    image.sprite = __number_0;
                    (spriteSize, isActive) = UpdateChar(image.gameObject, 0, spacingAllowed, true);
                }
                else if (!scoreCentered)
                {
                    image.sprite = __number_space;
                    (spriteSize, isActive) = UpdateChar(image.gameObject, 11, spacingAllowed, true);
                }
                else
                {
                    image.sprite = __number_space;
                    (spriteSize, isActive) = UpdateChar(image.gameObject, 11, spacingAllowed, false);
                }
                break;
            case 1:
                afterZeroes = true;
                image.sprite = __number_1;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 1, spacingAllowed, true);
                break;
            case 2:
                afterZeroes = true;
                image.sprite = __number_2;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 2, spacingAllowed, true);
                break;
            case 3:
                afterZeroes = true;
                image.sprite = __number_3;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 3, spacingAllowed, true);
                break;
            case 4:
                afterZeroes = true;
                image.sprite = __number_4;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 4, spacingAllowed, true);
                break;
            case 5:
                afterZeroes = true;
                image.sprite = __number_5;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 5, spacingAllowed, true);
                break;
            case 6:
                afterZeroes = true;
                image.sprite = __number_6;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 6, spacingAllowed, true);
                break;
            case 7:
                afterZeroes = true;
                image.sprite = __number_7;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 7, spacingAllowed, true);
                break;
            case 8:
                afterZeroes = true;
                image.sprite = __number_8;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 8, spacingAllowed, true);
                break;
            case 9:
                afterZeroes = true;
                image.sprite = __number_9;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 9, spacingAllowed, true);
                break;
            default:
                afterZeroes = true;
                image.sprite = __number_null;
                (spriteSize, isActive) = UpdateChar(image.gameObject, 10, spacingAllowed, true);
                break;
        }
    
        if (isActive) return spriteSize;
        else return Vector2Int.zero;
    }
    private void UpdateScorePriv(int score)
    {
        try
        {
            List<int> digits;
            List<Vector2Int> charSizes = new List<Vector2Int>();

            if (IsScoreNullified()) digits = new List<int> { 10, 10, 10, 10, 10 };
            else if (score == 0) digits = new List<int> { 0, 0, 0, 0, 0 };
            else try { digits = score.ToString("D5").Select(x=>int.Parse(x.ToString())).Reverse().ToList(); }
            catch { digits = new List<int> { 0, 0, 0, 0, 0 }; }

            bool onlyZeroes = digits.All(x => x == 0);
            bool afterZeroes = false;

            charSizes.Add(GetSprite(0, digits.Count, onlyZeroes, ref afterZeroes, digit5, digits[4]));
            charSizes.Add(GetSprite(1, digits.Count, onlyZeroes, ref afterZeroes, digit4, digits[3]));
            charSizes.Add(GetSprite(2, digits.Count, onlyZeroes, ref afterZeroes, digit3, digits[2]));
            charSizes.Add(GetSprite(3, digits.Count, onlyZeroes, ref afterZeroes, digit2, digits[1]));
            charSizes.Add(GetSprite(4, digits.Count, onlyZeroes, ref afterZeroes, digit1, digits[0]));

            UpdateSizePriv(charSizes);
            _lastScore = score;
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to update score: " + ex);
            throw ex;
        }


    }
    private void UpdateSizePriv(List<Vector2Int> charSizes)
    {
        float actualWidth = 0;
        float actualCharWidth = charWidth / (float)charPixelsPerUnit;
        float actualCharHeight = charHeight / (float)charPixelsPerUnit;

        foreach (var charSize in charSizes)
            actualWidth += charSize.x / (float)charPixelsPerUnit;

        var container_transform = gameObject.GetComponent<RectTransform>();
        container_transform.sizeDelta = new Vector2(actualWidth, actualCharHeight);
        container_transform.pivot = new Vector2(0.5f, actualCharHeight / 2);
    }
    public void UpdateScore(bool force = false)
    {
        if (score != _lastScore || force) UpdateScorePriv(score);
    }
}