using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    public AnimatedSprite up;
    public AnimatedSprite down;
    public AnimatedSprite left;
    public AnimatedSprite right;

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
            left.Stop();
            right.Stop();
            down.Stop();
            up.Play();
        }
        else if (direction == Vector2.down) {
            left.Stop();
            right.Stop();
            up.Stop();
            down.Play();
        }
        else if (direction == Vector2.left) {
            up.Stop();
            right.Stop();
            down.Stop();
            left.Play();
        }
        else if (direction == Vector2.right) {
            up.Stop();
            left.Stop();
            down.Stop();
            right.Play();
        }
    }

}
