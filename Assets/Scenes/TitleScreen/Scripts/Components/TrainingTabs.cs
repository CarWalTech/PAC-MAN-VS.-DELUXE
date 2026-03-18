using UnityEngine;

public class TrainingTabs : MonoBehaviour
{
    public TabMenu sidebar;
    public void OnPageIndexChanged(int index)
    {
        if (index == 0) sidebar.JumpToPage(0);
        else if (index >= 1 && index <= 2) sidebar.JumpToPage(1);
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
