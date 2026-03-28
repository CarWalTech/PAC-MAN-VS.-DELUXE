using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class ScorecardCommons : SkinableBehavior
{
    public int score;
    private int _lastScore = -1;

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

    public Image digit1;
    public Image digit2;
    public Image digit3;
    public Image digit4;
    public Image digit5;

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
    private void UpdateScorePriv(int score)
    {
        var paddingPassed = false;
        
        void SetCharActive(GameObject obj, bool state)
        {
            if (scoreCentered) obj.SetActive(state);
            else obj.SetActive(true);
        }

        void GetSprite(List<int> values, int index, ref Image image)
        {
            if (values.Count <= index)
            {
                image.sprite = __number_0;
                return;
            }
            switch (values[index])
            {
                case 0:
                    if (paddingPassed)
                    {
                        image.sprite = __number_0;
                        SetCharActive(image.gameObject, true);
                        return;
                    }
                    else if (scorePadding)
                    {
                        image.sprite = __number_0;
                        SetCharActive(image.gameObject, true);
                        return;
                    }
                    else
                    {
                        image.sprite = __number_space;
                        SetCharActive(image.gameObject, false);
                        return;
                    }
                case 1:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_1;
                    return;
                case 2:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_2;
                    return;
                case 3:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_3;
                    return;
                case 4:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_4;
                    return;
                case 5:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_5;
                    return;
                case 6:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_6;
                    return;
                case 7:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_7;
                    return;
                case 8:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_8;
                    return;
                case 9:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_9;
                    return;
                default:
                    paddingPassed = true;
                    SetCharActive(image.gameObject, true);
                    image.sprite = __number_null;
                    return;
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

                SetCharActive(digit1.gameObject, true);
                SetCharActive(digit2.gameObject, true);
                SetCharActive(digit3.gameObject, true);
                SetCharActive(digit4.gameObject, true);
                SetCharActive(digit5.gameObject, true);                
            }
            else if (score == 0)
            {
                digit1.sprite = __number_0;
                digit2.sprite = scorePadding ? __number_0 : __number_space;
                digit3.sprite = scorePadding ? __number_0 : __number_space;
                digit4.sprite = scorePadding ? __number_0 : __number_space;
                digit5.sprite = scorePadding ? __number_0 : __number_space;

                SetCharActive(digit1.gameObject, true);
                SetCharActive(digit2.gameObject, scorePadding);
                SetCharActive(digit3.gameObject, scorePadding);
                SetCharActive(digit4.gameObject, scorePadding);
                SetCharActive(digit5.gameObject, scorePadding);
            }
            else
            {
                List<int> digits;
                try { digits = score.ToString("D5").Select(x=>int.Parse(x.ToString())).Reverse().ToList(); }
                catch { digits = new List<int> { 0, 0, 0, 0, 0 }; }
                
                GetSprite(digits, 4, ref digit5);
                GetSprite(digits, 3, ref digit4);
                GetSprite(digits, 2, ref digit3);
                GetSprite(digits, 1, ref digit2);
                GetSprite(digits, 0, ref digit1);      
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

            SetCharActive(digit1.gameObject, true);
            SetCharActive(digit2.gameObject, true);
            SetCharActive(digit3.gameObject, true);
            SetCharActive(digit4.gameObject, true);
            SetCharActive(digit5.gameObject, true);
        }
    }
    public void UpdateScore(bool force = false)
    {
        if (score != _lastScore || force) UpdateScorePriv(score);
    }
}