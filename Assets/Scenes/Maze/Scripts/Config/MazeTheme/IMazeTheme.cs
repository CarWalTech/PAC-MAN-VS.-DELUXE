public interface IMazeTheme
{
    public MazeTheme GetMazeTheme();
    public bool HasRecolorSupport();
    public bool HasBackgroundSupport();
    public bool HasBackgroundRecolorSupport();
}