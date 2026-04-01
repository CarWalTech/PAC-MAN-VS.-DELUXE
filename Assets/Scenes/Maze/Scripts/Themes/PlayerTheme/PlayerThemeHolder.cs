using EditorAttributes;
using UnityEngine;


public class PlayerThemeHolder<T> : MonoBehaviour, IThemable where T : IPlayerTheme
{
    [SerializeField, HideProperty] private T _activeSkin = null;
    public T GetSkin()
    {
        return _activeSkin;
    }
    public void SetSkin(T skin)
    {
        _activeSkin = skin;
        RefreshSkin();
    }
    public virtual void RefreshSkin()
    {
        
    }
}