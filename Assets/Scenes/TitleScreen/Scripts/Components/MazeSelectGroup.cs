using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MazeSelectGroup : GenericSelectGroup
{
    public MazePreview preview;

    public LevelConfiguration GetMaze()
    {
        if (!preview) return null;
        else return preview.getMaze(activeItem);
    }

    public override void OnItemChanged(string newItem)
    {
        base.OnItemChanged(newItem);
        preview.setMaze(newItem);
    }

    public override void OnValueChanged(string themeName)
    {
        base.OnValueChanged(themeName);
        preview.setMaze(themeName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
