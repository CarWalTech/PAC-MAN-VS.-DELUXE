using EditorAttributes;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    

    public Sprite[] sprites = new Sprite[0];
    public float animationTime = 0.25f;
    public bool loop = true;

    
    [Space]
    [FoldoutGroup("Advanced Options", nameof(controlSpriteRenderer))]
    [SerializeField] public bool controlSpriteRenderer = true;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;
    private bool __playing = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (controlSpriteRenderer) spriteRenderer.enabled = true;
        __playing = true;
    }

    private void OnDisable()
    {
        if (controlSpriteRenderer) spriteRenderer.enabled = false;
        __playing = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if (!__playing) {
            return;    
        }

        if (!spriteRenderer.enabled) {
            return;
        }

        animationFrame++;

        if (animationFrame >= sprites.Length && loop) {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length) {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    public void Play()
    {
        if (enabled) return;
        enabled = true;
        animationFrame = -1;
        Advance();
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    public void Stop()
    {
        if (!enabled) return;
        CancelInvoke(nameof(Advance));
        enabled = false;
    }

    public void Restart()
    {
        animationFrame = -1;

        Advance();
    }

}
