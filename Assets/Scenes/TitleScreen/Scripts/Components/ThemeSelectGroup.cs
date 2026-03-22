using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSelectGroup : GenericSelectGroup
{
    public MazePreview preview;

    public MazeTheme GetTheme()
    {
        if (!preview) return null;
        else return preview.getTheme(activeItem);
    }

    public override void OnItemChanged(string newItem)
    {
        base.OnItemChanged(newItem);
        preview.setTheme(newItem);
    }

    public override void OnValueChanged(string themeName)
    {
        base.OnValueChanged(themeName);
        preview.setTheme(themeName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
