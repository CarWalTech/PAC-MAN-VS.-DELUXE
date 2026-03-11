using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PacManAI : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] public Vector2 initialDirection = Vector2.left / 2;
    [SerializeField] public Tilemap tilemap;
    
    public float actualSpeed { get => speed * GameConstants.BASE_SPEED_FACTOR; }
    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get => _lastDirection; }
    public Vector2 NextDestination { get; set; }

    public bool locked { get; set; }

    private Vector2 _lastInputDirection;
    private Vector2 _lastDirection;
    private Vector2 _spawnPosition;
    private bool _teleporting;
    


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var position = transform.position;
        _spawnPosition = position;
        _lastDirection = initialDirection.normalized;
        NextDestination = (Vector2)position + _lastDirection / 2;
        _teleporting = false;
    }
    private void Start()
    {
        ResetState();
    }
    public void ResetState()
    {
        transform.position = _spawnPosition;
        NextDestination = _spawnPosition + initialDirection;
        _lastInputDirection = initialDirection.normalized;
        _lastDirection = initialDirection.normalized;
        enabled = true;
    }
    protected virtual void InputUpdate()
    {
        //Diffrent depending on AI
    }
    private void Update()
    {
        InputUpdate();
    }
    private void FixedUpdate()
    {
        // Debug.Log("FixedUpdate " + _lastInputDirection);

        // Move the player
        var position = (Vector2)transform.position;
        var positionVector = Vector2.MoveTowards(position, NextDestination, actualSpeed * Time.deltaTime);
        rb.MovePosition(positionVector);

        // Check if the player is centered in the tile
        var isCentered = position == NextDestination;
        if (!isCentered)
        {
            if (_teleporting == true)
            {
                transform.position = SnapToTileCenter(position);
                NextDestination = position;
                _teleporting = false;            
            }
            else return;
            
        }

        var _nextDir = locked ? _lastDirection : _lastInputDirection;

        // if is at the middle of a tile, has input and there is no wall in the direction of the input
        if (_nextDir != Vector2.zero && !DetectWallBorder(_nextDir))
        {
            NextDestination = position + _nextDir;
            _lastDirection = _nextDir;
        }
        // if is at the middle of a tile and there is no wall in the current direction then continue in the same direction
        else if (!DetectWallBorder(_lastDirection))
        {
            NextDestination = position + _lastDirection;
        }
        else
        {
            
        }

        var renderer = GetComponentInParent<PacMan>();
        if (renderer) renderer.Anim_Rotate(_lastDirection);
    }

    public void Immobilize()
    {
        _lastInputDirection = Vector2.zero;
        _lastDirection = Vector2.zero;
        NextDestination = transform.position;
    }
    public void SetInputDirection(Vector2 direction)
    {
        if (direction != Vector2.zero) _lastInputDirection = direction;
    }

    public Vector2 SnapToTileCenter(Vector2 worldPos)
    {
        // Convert world → cell
        Vector3Int cell = tilemap.WorldToCell(worldPos);

        // Convert cell → world (this gives the *bottom-left* corner)
        Vector3 cellWorld = tilemap.CellToWorld(cell);

        // Add tilemap's cell anchor to get the true center
        Vector3 offset = tilemap.cellSize * 0.5f;

        return (Vector2)(cellWorld + offset);
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public virtual void TeleportTo(Vector2 newWorldPos, Vector2 outDirection)
    {
        transform.position = newWorldPos;
        NextDestination = newWorldPos + outDirection;
        _teleporting = true;
    }

    public bool DetectWallBorder(Vector2 dir)
    {
        // Detect a tile in the direction of the dir vector parameter
        var pos = (Vector2)transform.position;
        var cellPosition = tilemap.WorldToCell(pos + dir);
        // Detect a door in the direction of the dir vector parameter using linecast
        var linecast = Physics2D.LinecastAll(pos + dir, pos);
        return linecast.Any(t => t.collider.CompareTag(tilemap.tag)) || tilemap.HasTile(cellPosition);
    }

}
