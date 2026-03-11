using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValuePickerFloat : MonoBehaviour
{
    public Button addButton;
    public Button subtractButton;

    public TMP_Text valueText;

    public float currentValue
    {
        get => _currentValue;
        private set {
            _currentValue = value;
        }
    }
    public float defaultValue = 2;
    private float _currentValue = -1;
    public float minValue = 2;
    public float maxValue = 5;
    public float incrementAmount = 1;

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

    public void SetValue(float val)
    {
        if (val < minValue) return;
        else if (val > maxValue) return;

        currentValue = val;
        valueText.text = currentValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
