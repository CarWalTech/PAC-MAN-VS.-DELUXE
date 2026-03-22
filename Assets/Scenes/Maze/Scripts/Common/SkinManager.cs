using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{



    [CustomEditor(typeof(SkinManager))]
    public class RefreshButton : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SkinManager myScript = (SkinManager)target;
            if (GUILayout.Button("Force Refresh"))
            {
                myScript.RefreshSkin();
            }
        }

    }

    public ViewportTheme guiTheme = null;

    private List<ISkinableBehavior> _hook_list = new List<ISkinableBehavior>();
    
    public void ForceRefresh()
    {
        
    }

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
