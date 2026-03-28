using UnityEngine;

public interface IMazeTile
{
    
    public Sprite GetSprite();

    public MazeTheme GetTheme();
    public void SetTheme(MazeTheme _theme);
    public void RefreshTheme();

    public void ForceUpdate();
}