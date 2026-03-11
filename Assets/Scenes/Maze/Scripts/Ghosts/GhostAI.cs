using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;



public class GhostAI : MonoBehaviour
{
    public enum GhostMode
    {
        Chase,
        Scatter,
        Frightened,
        Eaten,
        LeavingHouse
    }

    #region Variables: Common
    public Tilemap tilemap;
    #endregion

    #region Variables: Movement Speeds
    [Header("Movement Speeds")]
    [SerializeField] private float speed = 1f;
    public float eatenSpeed = 20f;

    public float normalSpeed { get => speed * GameConstants.BASE_SPEED_FACTOR; }
    public float frightenedSpeed { get => speed * GameConstants.FRIGHT_SPEED_FACTOR; }
    #endregion

    #region Variables: Durations
    [Header("Durations")]
    public float chaseDuration = 20f;
    public float scatterDuration = 7f;
    public float frightenedDuration = 8f;
    #endregion

    #region Variables: Targets
    [Header("Targets")]
    public GameObject chaseModeTarget;
    public GameObject scatterModeTarget;
    #endregion

    #region Variables: Position Settings
    [Header("Position Settings")]
    public Vector2 initDirection = Vector2.left / 2;
    public Vector2 NextTileDestination { get; set; }
    public bool isInGhostHouse;
    public bool locked { get; set; }
    #endregion

    #region Variables: Ghost Waypoints
    [Header("Ghost Waypoints")]
    public Transform[] enterHomeWayPoints;
    public Transform[] exitHomeWayPoints;
    #endregion

    #region Variables: Private Fields
    private Vector2 _direction;
    private GhostMode _ghostMode;
    private bool _hasChangedMode;
    public Rigidbody2D _rigidbody2D { get; private set; }
    private bool _ghostHomeReached = false;
    private int _currentWayPointDestinationIndex;
    private Vector3 _spawnPoint;
    private bool _isMidfright;

    #endregion

    #region Functions: Unity Messages / Core

    public virtual void Start()
    {
        // Get components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        var pos = transform.position;

        // Set spawn point
        _spawnPoint = pos;

        // Set initial direction
        _direction = initDirection;
        NextTileDestination = (Vector2)pos + initDirection;

        // Set initial mode
        if (isInGhostHouse) _ghostMode = GhostMode.LeavingHouse;
    }
    private void FixedUpdate()
    {
        if (!GameManager.IsReady) return;

        bool isCOM = GetIsComputerGhost();
        if (isCOM)
        {
            switch (_ghostMode)
            {
                case GhostMode.Scatter:
                    Scatter();
                    break;
                case GhostMode.Chase:
                    Chase();
                    break;
                case GhostMode.Frightened:
                    Frightened();
                    break;
                case GhostMode.Eaten:
                    Eaten();
                    break;
                case GhostMode.LeavingHouse:
                    LeavingHouse();
                    break;
                default:
                    // Debug.LogError("Invalid Ghost Mode at 'FixedUpdate'");
                    break;
            }  
        }
        else
        {
            switch (_ghostMode)
            {
                case GhostMode.Scatter:
                case GhostMode.Chase:
                case GhostMode.Frightened:
                    UserControl();
                    break;
                case GhostMode.Eaten:
                    Eaten();
                    break;
                case GhostMode.LeavingHouse:
                    LeavingHouse();
                    break;
                default:
                    // Debug.LogError("Invalid Ghost Mode at 'FixedUpdate'");
                    break;
            }  
        }
    }
    public void Reset()
    {
        transform.position = _spawnPoint;
        if (isInGhostHouse) _ghostMode = GhostMode.LeavingHouse;

        NextTileDestination = (Vector2)transform.position + initDirection;
    }

    #endregion

    #region Functions: Getters
    
    public float GetSpeed()
    {
        switch (_ghostMode)
            {
                case GhostMode.Scatter:
                case GhostMode.Chase:
                    return normalSpeed;
                case GhostMode.Frightened:
                    return frightenedSpeed;
                case GhostMode.LeavingHouse:
                case GhostMode.Eaten:
                    return eatenSpeed;
                default:
                    return normalSpeed;
            }  
    }
    public Vector2 GetDirection()
    {
        if (!GetIsComputerGhost())
        {
            switch (_ghostMode)
            {
                case GhostMode.Scatter:
                case GhostMode.Chase:
                case GhostMode.Frightened:
                    return GetUserControlDirection();
                case GhostMode.Eaten:
                    return _direction;
                case GhostMode.LeavingHouse:
                    return _direction;
                default:
                    return _direction;
            }  
        }
        return _direction;
    }
    protected virtual Vector2 GetUserControlDirection()
    {
        return _direction;
    }
    public bool GetMidfrightEnabled()
    {
        return _isMidfright;
    }
    public GhostMode GetGhostMode()
    {
        return _ghostMode;
    }
    protected virtual bool GetIsComputerGhost()
    {
        return true;
    }
    private List<Vector2> GetPossibleDirections()
    {
        if (locked) return new List<Vector2>() { _direction };

        var up = Vector2.up;
        var down = Vector2.down;
        var left = Vector2.left;
        var right = Vector2.right;

        var possibleDirections = new List<Vector2>();
        if (!DetectWallsAndDoors(up))
            possibleDirections.Add(up);
        if (!DetectWallsAndDoors(down))
            possibleDirections.Add(down);
        if (!DetectWallsAndDoors(left))
            possibleDirections.Add(left);
        if (!DetectWallsAndDoors(right))
            possibleDirections.Add(right);

        return possibleDirections;
    }
    
    #endregion

    #region Functions: Setters

    public void SetGhostMode(GhostMode ghostMode, bool forceChange = false)
    {
        if (_ghostMode is GhostMode.Eaten or GhostMode.LeavingHouse && forceChange is false)
            return; // If ghost is eaten or is leaving home, it can't change mode.

        // If ghost is leaving home, it offset the position to the next tile
        if (_ghostMode == GhostMode.LeavingHouse)
        {
            NextTileDestination = (Vector2)transform.position + initDirection;
            _direction = initDirection.normalized;
        }

        // Change mode
        _ghostMode = ghostMode;

        // Update ghost and music
        switch (_ghostMode)
        {
            case GhostMode.Scatter:
                Anim_OnNormalMoveRun();
                Timer_StartScatter();
                break;
            case GhostMode.Chase:
                Timer_StartChase();
                Anim_OnNormalMoveRun();
                break;
            case GhostMode.Frightened:
                Timer_StartFright();
                Anim_OnFrightened();
                SFX_PacmanChase();
                break;
            case GhostMode.Eaten:
                Anim_OnGhostEatenPause();
                break;
            case GhostMode.LeavingHouse:
                Anim_OnLeavingHouse();
                break;
            default:
                // Debug.LogError("Invalid Ghost Mode at 'SetGhostMode'");
                throw new ArgumentOutOfRangeException();
        }

        _hasChangedMode = true;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    #endregion

    #region Functions: Ghost Modes
    
    protected virtual void UserControl()
    {

    }
    protected virtual void Chase()
    {
        // Different depending on ghost
    }
    protected virtual void Scatter()
    {
        ChaseTarget(scatterModeTarget, normalSpeed);
    }
    private void Frightened()
    {
        var position = (Vector2)transform.position;

        // Move ghost
        MoveGhost(position, frightenedSpeed);

        // Check if ghost reached next tile
        var isCentered = position == NextTileDestination;
        if (!isCentered)
            return;

        if (_hasChangedMode)
        {
            _direction *= -1;
            _hasChangedMode = false;
        }
        else
        {
            // Check where the ghost can go
            var possibleDirections = GetPossibleDirections();

            // if two or more possible directions then delete the opposite direction (preventing the ghost from going back)
            if (possibleDirections.Count > 1)
                for (var i = 0; i < possibleDirections.Count; i++)
                    if (possibleDirections[i] == -_direction)
                        possibleDirections.RemoveAt(i);

            // Choose a random direction
            var randomDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];

            // Set new direction
            _direction = randomDirection;
        }

        NextTileDestination = position + _direction;
    }
    private void Eaten()
    {
        CancelInvoke();
        if (!_ghostHomeReached)
        {
            ChaseTarget(enterHomeWayPoints[0].transform.position, eatenSpeed);
            if (Vector2.Distance(transform.position, enterHomeWayPoints[0].transform.position) <= 0.5f)
                _ghostHomeReached = true;
        }
        else if (FollowPath(enterHomeWayPoints, ref _currentWayPointDestinationIndex, eatenSpeed))
        {
            _ghostHomeReached = false;
            SetGhostMode(GhostMode.LeavingHouse, true);
        }
    }
    private void LeavingHouse()
    {
        if (FollowPath(exitHomeWayPoints, ref _currentWayPointDestinationIndex, normalSpeed))
            SetGhostMode(GhostMode.Scatter, true);
    }

    #endregion

    #region Functions: Chase Target / Follow Path

    private bool FollowPath(Transform[] waypoints, ref int currentWaypoint, float speed)
    {
        if (transform.position != waypoints[currentWaypoint].position)
        {
            var p = Vector2.MoveTowards(transform.position,
                waypoints[currentWaypoint].position,
                speed * Time.deltaTime);
            _rigidbody2D.MovePosition(p);
        }
        else
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                _currentWayPointDestinationIndex = 0;
                return true;
            }
            else if (_ghostMode == GhostMode.Eaten)
            {
                Anim_OnFollowPathMoveEaten(enterHomeWayPoints, _currentWayPointDestinationIndex);
            }
            else if (_ghostMode == GhostMode.LeavingHouse)
            {
                Anim_OnFollowPathMoveRun(exitHomeWayPoints, _currentWayPointDestinationIndex);
            }
        }

        return false;
    }
    protected void ChaseTarget(GameObject target, float speed)
    {
        ChaseTarget(target.transform.position, speed);
    }
    protected void ChaseTarget(Vector2 targetPos, float speed)
    {
        var position = (Vector2)transform.position;

        // Move ghost
        MoveGhost(position, speed);

        // Check if ghost reached next tile
        var isCentered = position == NextTileDestination;
        if (!isCentered)
            return;

        var possibleDirections = GetPossibleDirections();

        // if two or more possible directions then delete the opposite direction (preventing the ghost from going back)
        if (possibleDirections.Count > 1)
            possibleDirections.Where(direction => direction == -_direction).ToList()
                .ForEach(direction => possibleDirections.Remove(direction));
        // for (var i = 0; i < possibleDirections.Count; i++)
        //     if (possibleDirections[i] == -_direction)
        //         possibleDirections.RemoveAt(i);

        // Calculate the shortest distance to the target
        CalculateNextTileDestination(possibleDirections, position, targetPos);

        // Update animation each tile
        switch (_ghostMode)
        {
            case GhostMode.Chase or GhostMode.Scatter:
                Anim_OnNormalMoveRun();
                break;
            case GhostMode.Eaten:
                Anim_OnNormalMoveEaten();
                break;
        }
    }

    #endregion

    #region Functions: Collision Detection / Movement
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            if (_ghostMode is GhostMode.Frightened or GhostMode.Eaten)
                SetGhostMode(GhostMode.Eaten);
            else
                GameManager.Instance.Event_EatPlayer(GetComponentInParent<Ghost>());
    }
    private void CalculateNextTileDestination(List<Vector2> possibleDirections, Vector2 position, Vector2 targetPos)
    {
        var shortestDistance = float.MaxValue;
        var shortestDirection = Vector2.zero;

        foreach (var direction in possibleDirections)
        {
            var distance = Vector2.Distance(targetPos, position + direction);

            if (!(distance < shortestDistance)) continue;

            shortestDistance = distance;
            shortestDirection = direction;
        }

        _direction = shortestDirection;
        NextTileDestination = position + _direction;
    }
    private void MoveGhost(Vector2 position, float speed)
    {
        var positionVector = Vector2.MoveTowards(position, NextTileDestination, speed * Time.deltaTime);
        _rigidbody2D.MovePosition(positionVector);
    }
    private bool DetectWallsAndDoors(Vector2 dir)
    {

        var pos = (Vector2)transform.position;
        var cellPosition = tilemap.WorldToCell(pos + dir);
        var linecast = Physics2D.LinecastAll(pos + dir, pos);
        return linecast.Any(t => t.collider.CompareTag(tilemap.tag)) || tilemap.HasTile(cellPosition);
    }

    #endregion

    #region Animations

    private void Anim_OnFrightened()
    {
        var renderer = GetComponentInParent<Ghost>();
        if (renderer) renderer.Anim_OnFrightened();
    }
    public void Anim_OnNormalMoveRun()
    {
        _isMidfright = false;
        Anim_OnChangeRunAnimationSprites(GetDirection());
    }
    public void Anim_OnNormalMoveEaten()
    {
        _isMidfright = false;
        Anim_OnChangeEatenAnimationSprites(GetDirection());
    }
    private void Anim_OnLeavingHouse()
    {
        _isMidfright = false;
        var renderer = GetComponentInParent<Ghost>();
        if (renderer) renderer.Anim_OnNormal();
    }
    private void Anim_OnMidlifeFrightened()
    {
        _isMidfright = true;
        var renderer = GetComponentInParent<Ghost>();
        if (renderer) renderer.Anim_OnMidlifeFrightened();
    }
    private void Anim_OnFollowPathMoveRun(Transform[] waypoints, int currentWaypoint)
    {
        Vector2 dir = waypoints[currentWaypoint].position - transform.position;
        Anim_OnChangeRunAnimationSprites(dir);
    }
    private void Anim_OnFollowPathMoveEaten(Transform[] waypoints, int currentWaypoint)
    {
        Vector2 dir = waypoints[currentWaypoint].position - transform.position;
        Anim_OnChangeEatenAnimationSprites(dir);
    }
    private void Anim_OnEatenPause()
    {
        var renderer = GetComponentInParent<Ghost>();
        if (renderer) renderer.Anim_OnEatenPause();
    }
    private void Anim_OnEatenUnpause()
    {
        var renderer = GetComponentInParent<Ghost>();
        if (renderer) renderer.Anim_OnEatenUnpause();
    }
    private void Anim_OnChangeRunAnimationSprites(Vector2 dir)
    {
        var renderer = GetComponentInParent<Ghost>();
        if (renderer) renderer.Anim_OnRun(dir);
    }
    private void Anim_OnChangeEatenAnimationSprites(Vector2 dir)
    {
        var renderer = GetComponentInParent<Ghost>();
        if (renderer) renderer.Anim_OnPostEaten(dir);
    }
    private void Anim_OnGhostEatenPause()
    {
        IEnumerator Coroutine()
        {
            Anim_OnEatenPause();
            SFX_Eaten();
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
            yield return new WaitForSecondsRealtime(GameManager.Instance.GetGhostPauseEatenTime());
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            Anim_OnNormalMoveEaten();
            Anim_OnEatenUnpause();
            SFX_GoHome();
        }
        StartCoroutine(Coroutine());
    }
    
    public void Anim_OnUserControl()
    {
        // Different depending on AI
        switch (_ghostMode)
        {
            case GhostMode.Chase or GhostMode.Scatter:
                Anim_OnNormalMoveRun();
                break;
            case GhostMode.Eaten:
                Anim_OnNormalMoveEaten();
                break;
        }
    }

    #endregion

    #region SoundFX
    private void SFX_PacmanChase()
    {
        
    }
    private void SFX_Eaten()
    {

    }
    private void SFX_GoHome()
    {

    }

    #endregion

    #region Timers

    private void Timer_StartScatter()
    {
        CancelInvoke();
        Invoke(nameof(Timer_EndScatter), scatterDuration);
    }
    private void Timer_EndScatter()
    {
        SetGhostMode(GhostMode.Chase);
    }
    private void Timer_StartChase()
    {
        CancelInvoke();
        Invoke(nameof(Timer_EndChase), chaseDuration);
    }
    private void Timer_EndChase()
    {
        SetGhostMode(GhostMode.Scatter);
    }  
    private void Timer_StartFright()
    {
        CancelInvoke();
        Invoke(nameof(Timer_EndFright), frightenedDuration);
        Invoke(nameof(Timer_StartFrightMidlife), frightenedDuration / 2f);
    }
    private void Timer_StartFrightMidlife()
    {
        if (_ghostMode == GhostMode.Frightened) Anim_OnMidlifeFrightened();
    }
    private void Timer_EndFright()
    {
        SetGhostMode(GhostMode.Scatter);
        Anim_OnLeavingHouse();
    }

    #endregion
}