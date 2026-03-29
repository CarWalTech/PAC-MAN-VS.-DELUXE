
using EditorAttributes;
using UnityEditor;
using UnityEngine;

public class IPelletTile : RuleTile, IThemable
{
    [SerializeField, HideProperty] private PelletTheme _activeSkin = null;
    public PelletTheme GetSkin()
    {
        return _activeSkin;
    }
    public void SetSkin(PelletTheme skin)
    {
        _activeSkin = skin;
        RefreshSkin();
    }

    public AnimatedSpriteSet GetSpriteSet(PelletTheme.PelletType key)
    {
        var theme = GetSkin();
        return theme.GetSpriteSet(key);
    }

    public virtual void RefreshSkin()
    {
        EditorUtility.SetDirty(this);
    }


}