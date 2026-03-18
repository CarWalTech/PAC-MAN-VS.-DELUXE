using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValuePickerInt : MonoBehaviour
{
    public Button addButton;
    public Button subtractButton;

    public TMP_Text valueText;
    public TMP_Text titleText;

    public string Title;
    public int currentValue
    {
        get => _currentValue;
        private set {
            _currentValue = value;
        }
    }
    public int defaultValue = 2;
    private int _currentValue = -1;
    public int minValue = 2;
    public int maxValue = 5;
    public int incrementAmount = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetValue(defaultValue);
        addButton.onClick.AddListener(OnAdd);
        subtractButton.onClick.AddListener(OnSubtract);
    }

    public void OnSubtract()
    {
        SetValue(currentValue - incrementAmount);
    }

    public void OnAdd()
    {
        SetValue(currentValue + incrementAmount);
    }

    public void SetValue(int val)
    {
        if (val < minValue) return;
        else if (val > maxValue) return;

        currentValue = val;
        valueText.text = currentValue.ToString();
    }

    void OnValidate()
    {
        if (titleText) titleText.text = Title;
    }

    // Update is called once per frame
    void Update()
    {
        if (titleText && titleText.text != Title) titleText.text = Title;
    }
}
