using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GuiThemeSelectGroup : GenericSelectGroup
{
    public List<InterfaceTheme> themes = new List<InterfaceTheme>();

    public InterfaceTheme GetTheme()
    {
        var matches = themes.Where(x => x.themeUUID == activeItem).ToList();
        if (matches.Count == 0) return null;
        return matches[0];
    }

    public override void OnItemChanged(string newItem)
    {
        base.OnItemChanged(newItem);
    }

    public override void OnValueChanged(string themeName)
    {
        base.OnValueChanged(themeName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
