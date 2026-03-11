using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCountPicker : MonoBehaviour
{
    public GameObject player1Component;
    public GameObject player2Component;
    public GameObject player3Component;
    public GameObject player4Component;
    public GameObject player5Component;

    public Button addButton;
    public Button subtractButton;

    public TMP_Text valueText;

    public int currentValue
    {
        get => _currentValue;
        private set {
            _currentValue = value;
            UpdateButtons();
        }
    }
    public int defaultValue = 2;
    private int _currentValue = -1;
    private const int minValue = 2;
    private const int maxValue = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetValue(defaultValue);
        addButton.onClick.AddListener(OnAdd);
        subtractButton.onClick.AddListener(OnSubtract);
    }

    public void OnSubtract()
    {
        SetValue(currentValue - 1);
    }

    public void OnAdd()
    {
        SetValue(currentValue + 1);
    }

    public void SetValue(int val)
    {
        if (val < minValue) return;
        else if (val > maxValue) return;

        currentValue = val;
        valueText.text = currentValue.ToString();
    }
    

    void UpdateButtons()
    {
        player1Component.SetActive(currentValue >= 1);
        player2Component.SetActive(currentValue >= 2);
        player3Component.SetActive(currentValue >= 3);
        player4Component.SetActive(currentValue >= 4);
        player5Component.SetActive(currentValue >= 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
