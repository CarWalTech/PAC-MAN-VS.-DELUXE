using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GenericSelectGroup : MonoBehaviour
{
    [SerializeField] protected ToggleGroup toggleGroup;
    [SerializeField] protected string activeItem = null;
    [SerializeField] protected List<GenericSelectItem> tabs = new List<GenericSelectItem>();

    private void Initialize()
    {
        toggleGroup = GetComponentInChildren<ToggleGroup>();

        tabs.Clear();
        tabs.AddRange(GetComponentsInChildren<GenericSelectItem>());

        if (Application.isPlaying) Default();
    }

    private void Default()
    {
        SetItem(activeItem);
    }

    public string GetSelection()
    {
        return activeItem;
    }
    
    public void SetItem(string itemName)
    {
        if (itemName != null)
        {
            var item = tabs.FirstOrDefault(x => x.itemId == itemName);
            if (item) {
                item.Select();
                activeItem = itemName;
                OnItemChanged(itemName);
            }
        }
    }

    public virtual void OnItemChanged(string newItem)
    {
        
    }

    private void Reset()
    {
        Initialize();
    }

    private void OnValidate()
    {
        Initialize();
    }

    public virtual void OnValueChanged(string itemName)
    {
        activeItem = itemName;
    }

    void Start()
    {
        if (Application.isPlaying) Default();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
