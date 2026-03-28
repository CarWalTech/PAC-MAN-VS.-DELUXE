
using EditorAttributes;
using UnityEngine;

public class SkinableBehavior : MonoBehaviour, ISkinableBehavior
{
    [SerializeField, HideProperty] private ViewportTheme _activeSkin = null;
    public ViewportTheme GetSkin()
    {
        return _activeSkin;
    }
    public void SetSkin(ViewportTheme skin)
    {
        _activeSkin = skin;
        RefreshSkin();
    }
    public virtual void RefreshSkin()
    {
        
    }
}