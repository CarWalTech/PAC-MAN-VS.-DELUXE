
using EditorAttributes;
using Flexy.AssetRefs;
using Flexy.AssetRefs.AssetLoaders;
using Flexy.AssetRefs.LoadExtensions;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class MazeThemeHolder : MonoBehaviour, IThemable
{
    [SerializeField, HideProperty] private MazeTheme _activeMazeSkin = null;
    [SerializeField, HideProperty] private PelletTheme _activePelletSkin = null;
    [SerializeField, HideProperty] private ThemableManager _themeManager = null;

    public ThemableManager GetFallbackTheme()
    {
        if (_themeManager == null)
        {
            var result = new AssetRef<ThemableManager>(AssetDatabase.AssetPathToGUID("Assets/Scenes/Maze/Skins/DevelopmentThemes.asset"));
            _themeManager = result.LoadAssetSync();    
        }
        return _themeManager;
    }
    
    public IPelletTheme GetPelletSkin()
    {
        if (_activePelletSkin) return _activePelletSkin;
        else return GetFallbackTheme();
    }
    public IMazeTheme GetMazeSkin()
    {
        if (_activeMazeSkin) return _activeMazeSkin;
        else return GetFallbackTheme();
    }

    public MazeTheme.MazeRules GetMazeRules()
    {
        return GetMazeSkin().GetMazeRules();
    }

    public void SetSkin(MazeTheme skin1, PelletTheme skin2)
    {
        _activeMazeSkin = skin1;
        _activePelletSkin = skin2;
        RefreshSkin();
    }
    
    public virtual void RefreshSkin()
    {
        
    }
}