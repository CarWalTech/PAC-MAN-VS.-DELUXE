using UnityEngine;
using UnityEngine.UI;

public class PlayerPickerButton : MonoBehaviour
{

    private Button _btn;
    public PlayerCharacter selection;
    public PlayerPicker picker;


    private TMPro.TMP_Text _checkbox;
    private bool _isSelected = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClick);
        _checkbox = GetComponentInChildren<TMPro.TMP_Text>(true);

    }

    public bool GetSelected()
    {
        return _isSelected;
    }

    public void SetSelected(bool value)
    {
        _isSelected = value;
    }

    void OnClick()
    {
        picker.SetSelection(selection);
    }

    // Update is called once per frame
    void Update()
    {
        if (_checkbox)
        {
            if (_checkbox.enabled != _isSelected) _checkbox.enabled = _isSelected;
        }
    }
}
