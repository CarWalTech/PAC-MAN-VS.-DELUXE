using UnityEngine;
using UnityEngine.UI;

public class GenericSelectItem : MonoBehaviour
{
    public GenericSelectGroup itemGroup;
    public string itemId;
    private Toggle toggle;
    

    public virtual void OnValueChanged(bool state)
    {
        if (state) itemGroup.OnValueChanged(itemId);
    }

    private void Initialize()
    {
        toggle = GetComponentInChildren<Toggle>();
        if (Application.isPlaying) OnValueChanged(toggle.isOn);
        
    }

    public void Select()
    {
        var temp_toggle = GetComponentInChildren<Toggle>();
        if (temp_toggle) temp_toggle.SetIsOnWithoutNotify(true);
        else print("Unable to Select");
    }

    private void Reset()
    {
        Initialize();
    }

    private void OnValidate()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
