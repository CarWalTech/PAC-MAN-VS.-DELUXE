using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    private SpriteRenderer spriteRenderer;
    private Ghost ghost;

    private Vector2 direction = Vector2.zero;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ghost = GetComponentInParent<Ghost>();
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    public void setDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void Update()
    {
        if (direction == Vector2.up) {
            spriteRenderer.sprite = up;
        }
        else if (direction == Vector2.down) {
            spriteRenderer.sprite = down;
        }
        else if (direction == Vector2.left) {
            spriteRenderer.sprite = left;
        }
        else if (direction == Vector2.right) {
            spriteRenderer.sprite = right;
        }
    }

}
