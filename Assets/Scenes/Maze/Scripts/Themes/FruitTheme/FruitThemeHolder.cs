using EditorAttributes;
using UnityEngine;


public class FruitThemeHolder : MonoBehaviour, IThemable
{
    [SerializeField, HideProperty] private FruitTheme _activeSkin = null;
    public FruitTheme GetSkin()
    {
        return _activeSkin;
    }
    public void SetSkin(FruitTheme skin)
    {
        _activeSkin = skin;
        RefreshSkin();
    }
    public virtual void RefreshSkin()
    {
        
    }
}