using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class ScorecardCommonsMesh : MonoBehaviour
{
    public int score;
    private int _lastScore = -1;

    public float charWidth = 0;
    public float charHeight = 0;

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
    public bool scoreInvisiblePadding = true;


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
    private void UpdateScorePriv(int score)
    {
        var paddingPassed = false;
        int numberCount = 0;
        
        int SetCharActive(GameObject obj, bool state)
        {
            if (obj.GetComponent<RectTransform>())
            {
                var rect = obj.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(charWidth, charHeight);
            }
            if (scoreCentered) obj.SetActive(state);
            else obj.SetActive(true);

            return state == true ? 1 : 0;
        }

        int GetSprite(List<int> values, int index, ref SpriteRenderer image)
        {
            if (values.Count <= index)
            {
                image.sprite = __number_0;
                return 1;
            }
            switch (values[index])
            {
                case 0:
                    if (paddingPassed)
                    {
                        image.sprite = __number_0;
                        return SetCharActive(image.gameObject, true);
                    }
                    else if (scorePadding)
                    {
                        image.sprite = __number_0;
                        return SetCharActive(image.gameObject, true);
                    }
                    else
                    {
                        image.sprite = __number_space;
                        return SetCharActive(image.gameObject, false);
                    }
                case 1:
                    paddingPassed = true;
                    image.sprite = __number_1;
                    return SetCharActive(image.gameObject, true);
                case 2:
                    paddingPassed = true;
                    image.sprite = __number_2;
                    return SetCharActive(image.gameObject, true);
                case 3:
                    paddingPassed = true;
                    image.sprite = __number_3;
                    return SetCharActive(image.gameObject, true);
                case 4:
                    paddingPassed = true;
                    image.sprite = __number_4;
                    return SetCharActive(image.gameObject, true);
                case 5:
                    paddingPassed = true;
                    image.sprite = __number_5;
                    return SetCharActive(image.gameObject, true);
                case 6:
                    paddingPassed = true;
                    image.sprite = __number_6;
                    return SetCharActive(image.gameObject, true);
                case 7:
                    paddingPassed = true;
                    image.sprite = __number_7;
                    return SetCharActive(image.gameObject, true);
                case 8:
                    paddingPassed = true;
                    image.sprite = __number_8;
                    return SetCharActive(image.gameObject, true);
                case 9:
                    paddingPassed = true;
                    image.sprite = __number_9;
                    return SetCharActive(image.gameObject, true);
                default:
                    paddingPassed = true;
                    image.sprite = __number_null;
                    return SetCharActive(image.gameObject, true);
            }
        }

        try
        {
            if (IsScoreNullified())
            {
                digit1.sprite = __number_null;
                digit2.sprite = __number_null;
                digit3.sprite = __number_null;
                digit4.sprite = __number_null;
                digit5.sprite = __number_null;

                numberCount += SetCharActive(digit1.gameObject, true);
                numberCount += SetCharActive(digit2.gameObject, true);
                numberCount += SetCharActive(digit3.gameObject, true);
                numberCount += SetCharActive(digit4.gameObject, true);
                numberCount += SetCharActive(digit5.gameObject, true);           
            }
            else if (score == 0)
            {
                digit1.sprite = __number_0;
                digit2.sprite = scorePadding ? __number_0 : __number_space;
                digit3.sprite = scorePadding ? __number_0 : __number_space;
                digit4.sprite = scorePadding ? __number_0 : __number_space;
                digit5.sprite = scorePadding ? __number_0 : __number_space;

                numberCount += SetCharActive(digit1.gameObject, true);
                numberCount += SetCharActive(digit2.gameObject, scorePadding);
                numberCount += SetCharActive(digit3.gameObject, scorePadding);
                numberCount += SetCharActive(digit4.gameObject, scorePadding);
                numberCount += SetCharActive(digit5.gameObject, scorePadding);
            }
            else
            {
                List<int> digits;
                try { digits = score.ToString("D5").Select(x=>int.Parse(x.ToString())).Reverse().ToList(); }
                catch { digits = new List<int> { 0, 0, 0, 0, 0 }; }
                
                numberCount += GetSprite(digits, 4, ref digit5);
                numberCount += GetSprite(digits, 3, ref digit4);
                numberCount += GetSprite(digits, 2, ref digit3);
                numberCount += GetSprite(digits, 1, ref digit2);
                numberCount += GetSprite(digits, 0, ref digit1);      
            }

        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to update score: " + ex);
            digit1.sprite = __number_null;
            digit2.sprite = __number_null;
            digit3.sprite = __number_null;
            digit4.sprite = __number_null;
            digit5.sprite = __number_null;

            numberCount += SetCharActive(digit1.gameObject, true);
            numberCount += SetCharActive(digit2.gameObject, true);
            numberCount += SetCharActive(digit3.gameObject, true);
            numberCount += SetCharActive(digit4.gameObject, true);
            numberCount += SetCharActive(digit5.gameObject, true);
        }

        UpdateSizePriv(numberCount);
    }

    private void UpdateSizePriv(int numberCount)
    {
        float actualWidth;
        if (scoreInvisiblePadding) actualWidth = charWidth * 5;
        else actualWidth = charWidth * numberCount;

        var container_transform = gameObject.GetComponent<RectTransform>();
        container_transform.sizeDelta = new Vector2(actualWidth, charHeight);
        container_transform.pivot = new Vector2(charWidth, charHeight / 2);
    }
    public void UpdateScore(bool force = false)
    {
        if (score != _lastScore || force) UpdateScorePriv(score);
    }
}