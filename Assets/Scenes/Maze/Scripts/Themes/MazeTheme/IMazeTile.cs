using EditorAttributes;
using UnityEditor;
using UnityEngine;

public class IMazeTile : RuleTile, IThemable
{
    [SerializeField, HideProperty] private MazeTheme _activeSkin = null;
    public MazeTheme GetSkin()
    {
        return _activeSkin;
    }
    public void SetSkin(MazeTheme skin)
    {
        _activeSkin = skin;
        RefreshSkin();
    }

    public virtual void RefreshSkin()
    {
        EditorUtility.SetDirty(this);
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }
}