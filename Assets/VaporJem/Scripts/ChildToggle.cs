using EditorAttributes;
using UnityEngine;

public class ChildToggle : MonoBehaviour
{
    [SerializeField, OnValueChanged(nameof(OnDisableChildrenToggled))] private bool disableChildren;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisableChildrenToggled()
    {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(!disableChildren);
        }
    }
}
