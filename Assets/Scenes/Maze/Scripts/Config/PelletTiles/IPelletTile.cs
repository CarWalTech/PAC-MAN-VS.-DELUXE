using UnityEngine;

public interface IPelletTile
{
    
    public AnimatedSpriteSet GetSpriteSet();

    public PelletTheme GetTheme();
    public void SetTheme(PelletTheme _theme);
    public void RefreshTheme();

    public void ForceUpdate();
}