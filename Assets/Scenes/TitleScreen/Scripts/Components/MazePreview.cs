using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazePreview : MonoBehaviour
{
    public List<LevelConfiguration> mazes = new List<LevelConfiguration>();
    public List<MazeTheme> themes = new List<MazeTheme>();
    public CanvasGroup canvasGroup = null;
    private GameObject container = null;
    private string _currentPreviewId = null;
    private string _currentThemeId = null;


    public LevelConfiguration getMaze(string mazeId)
    {
        var matches = mazes.Where(x => x.levelUUID == mazeId).ToList();
        if (matches.Count == 0) return null;
        return matches[0];
    }

    public MazeTheme getTheme(string themeId)
    {
        var matches = themes.Where(x => x.themeUUID == themeId).ToList();
        if (matches.Count == 0) return null;
        return matches[0];
    }

    public void setMaze(string mazeId)
    {
        _currentPreviewId = mazeId;
        StopAllCoroutines();
        StartCoroutine(RefreshAwaited());
    }

    public void setTheme(string themeId)
    {
        _currentThemeId = themeId;
        StopAllCoroutines();
        StartCoroutine(RefreshAwaited());
    }

    IEnumerator RefreshAwaited()
    {
        yield return new WaitForSeconds(0.1f);
        Refresh();
    }

    public void Refresh()
    {
        var matches = mazes.Where(x => x.levelUUID == _currentPreviewId).ToList();
        if (matches.Count == 0) return;
        if (container) Destroy(container);

        
        container = new GameObject("Container");
        container.transform.parent = gameObject.transform;
        container.transform.localPosition = new Vector3(0,0,0);
        container.transform.localScale = new Vector3(16,16,16);
        var mazeResult = Instantiate(matches[0].levelTiles, container.transform);
        
        if (_currentThemeId != null)
        {
            var mazeData = mazeResult.GetComponent<Maze2D>();
            var matchingThemes = themes.Where(x => x.themeUUID == _currentThemeId).ToList();
            if (matchingThemes.Count == 0) return;
            if (mazeData.colorLayer) mazeData.colorLayer.SetActive(matchingThemes[0].GetMazeRules().supportsRecolors);
            foreach (Tilemap m in container.GetComponentsInChildren<Tilemap>())
            {
                foreach (MazeTile t in m.GetTiles<MazeTile>())
                    t.SetSkin(matchingThemes[0]);
                m.RefreshAllTiles();
            }
        }
        else
        {
            var mazeData = mazeResult.GetComponent<Maze2D>();
            if (mazeData.colorLayer) mazeData.colorLayer.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup)
        {   
            if (container && canvasGroup.alpha == 0 && container.activeSelf) container.SetActive(false);
            else if (container && canvasGroup.alpha != 0 && !container.activeSelf) container.SetActive(true);
        }
    }
}
