using UnityEngine;

public class WallTile : MonoBehaviour
{
    public bool isColored = false;

    private MazeTheme theme { get => tile?.GetTheme(); }
    
    public SpriteRenderer spriteRenderer;
    private IMazeTile tile = null;



    void Awake()
    {

    }

    void Start()
    {
        
    }

    void OnValidate()
    {
        RefreshSprite();
    }

    void Update()
    {
        RefreshSprite();
    }

    public void RefreshSprite()
    {
        //if (tile == null) return;
        //if (!spriteRenderer) return;
        //spriteRenderer.sprite = tile.GetSprite();
    }

    public void Setup(IMazeTile _tile)
    {
        tile = _tile;
        RefreshSprite();
    }
}
