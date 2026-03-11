using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public InterfaceTheme guiTheme = null;

    private List<ISkinableBehavior> _hook_list = new List<ISkinableBehavior>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshSkin();
    }

    public void AddHook(ISkinableBehavior behaviour)
    {
        if (!_hook_list.Contains(behaviour))
        {
            _hook_list.Add(behaviour);
        }
            
    }

    public void OnValidate()
    {
        RefreshSkin();
    }

    void RefreshSkin()
    {
        for (int i = 0; i < _hook_list.Count; i++)
        {
            var hook = _hook_list[i];
            hook.RefreshSkin();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
