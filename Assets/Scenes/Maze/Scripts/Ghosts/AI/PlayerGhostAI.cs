using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerGhostAI : GhostAI, IPlayerInput
{
    private Vector2 _lastInputDirection;
    private Vector2 _lastDirection;


    public override void Start()
    {
        _lastDirection = GetDirection().normalized;
        _lastInputDirection = GetDirection().normalized;
        base.Start();
    }

    private float CurrentSpeed()
    {
        switch (GetGhostMode())
        {
            case GhostMode.Scatter:
                return normalSpeed;
            case GhostMode.Chase:
                return normalSpeed;
            case GhostMode.Frightened:
                return frightenedSpeed;
            default:
                return normalSpeed;
        }
    }

    private bool DetectWallBorder(Vector2 dir)
    {
        // Detect a tile in the direction of the dir vector parameter
        var pos = (Vector2)transform.position;
        var cellPosition = tilemap.WorldToCell(pos + dir);
        // Detect a door in the direction of the dir vector parameter using linecast
        var linecast = Physics2D.LinecastAll(pos + dir, pos);
        return linecast.Any(t => t.collider.CompareTag(tilemap.tag)) || tilemap.HasTile(cellPosition);
    }

    protected override bool GetIsComputerGhost()
    {
        return false;
    }

    public void SetInputDirection(Vector2 direction)
    {
        if (direction != Vector2.zero) _lastInputDirection = direction;
    }

    protected override void UserControl()
    {
        base.Anim_OnUserControl();

        if (!_rigidbody2D) return;
        // Debug.Log("FixedUpdate " + _lastInputDirection);

        float speed = CurrentSpeed();

        // Move the player
        var position = (Vector2)transform.position;
        var positionVector = Vector2.MoveTowards(position, NextTileDestination, speed * Time.deltaTime);
        _rigidbody2D.MovePosition(positionVector);

        // Check if the player is centered in the tile
        var isCentered = position == NextTileDestination;
        if (!isCentered) return;

        var _nextDir = locked ? _lastDirection : _lastInputDirection;

        // if is at the middle of a tile, has input and there is no wall in the direction of the input
        if (_nextDir != Vector2.zero && !DetectWallBorder(_nextDir))
        {
            NextTileDestination = position + _nextDir;
            _lastDirection = _nextDir;
        }
        // if is at the middle of a tile and there is no wall in the current direction then continue in the same direction
        else if (!DetectWallBorder(_lastDirection))
        {
            NextTileDestination = position + _lastDirection;
        }
        else
        {
            
        }
    }

    protected override Vector2 GetUserControlDirection()
    {
        return _lastDirection;
    }

    public void Input_OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        // Set the new direction based on the current input
        if (direction == Vector2.up) {
            SetInputDirection(Vector2.up);
        }
        else if (direction == Vector2.down) {
            SetInputDirection(Vector2.down);
        }
        else if (direction == Vector2.left) {
            SetInputDirection(Vector2.left);
        }
        else if (direction == Vector2.right) {
            SetInputDirection(Vector2.right);
        }
    }

    public void Input_OnPause(InputAction.CallbackContext context)
    {
        if (context.canceled) return;
        GameManager.Instance.Event_PauseGame();
    }

    public void Input_OnSwitchViews(InputAction.CallbackContext context)
    {
        if (context.canceled) return;
        GameManager.Instance.Event_SwitchCameras();
    }
}