
using UnityEditor;
using UnityEngine;

public class MazeColormapLayer : MonoBehaviour
{
    [SerializeField] private RectTransform spriteTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteMask spriteMask;
    private Vector2 _lastSize = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void OnValidate()
    {
        Invoke(nameof(RefreshSprite), 0);
    }

    public void RefreshSprite()
    {
        spriteRenderer.size = spriteTransform.sizeDelta;   
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastSize != spriteTransform.sizeDelta)
        {
            RefreshSprite();
            _lastSize = spriteTransform.sizeDelta;
        }
    }
}
