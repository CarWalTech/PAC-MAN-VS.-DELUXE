
using EditorAttributes;
using UnityEngine;

public class PelletThemeHolder : MonoBehaviour, IThemable
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
    public virtual void RefreshSkin()
    {
        
    }
}