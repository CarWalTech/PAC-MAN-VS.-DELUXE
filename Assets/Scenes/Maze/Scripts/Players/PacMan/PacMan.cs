using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacMan : PlayerThemeHolder<PlayerThemePacman>, IPlayable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AnimatedSprite movingSequence;
    [SerializeField] private AnimatedSprite deathSequence;
    

    public bool isDead { get => deathSequence.enabled; }
    public GameObject worldObject { get; private set; } = null;


    private bool _teleported = false;
    private PacManAI _movement;
    private Vector3 _mazeOrigin;
    private Vector3 _worldOrigin;
    private GameObject _worldPrefab;
    private CircleCollider2D _cc;
    private PacManAnimator _worldAnimator = null;
    private float _speed = 1f;
    private PlayerSlot _slot = PlayerSlot.Null;


    #region Unity Messages / Core Functions
    private void Awake()
    {
        _cc = GetComponent<CircleCollider2D>();

        if (GetComponent<ComputerPacManAI>()) _movement = GetComponent<ComputerPacManAI>();
        else if (GetComponent<PlayerPacManAI>()) _movement = GetComponent<PlayerPacManAI>();
        else _movement = GetComponent<PacManAI>();

        Freeze();
    }
    void OnValidate()
    {
        RefreshSkin();        
    }
    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.IsReady && GameManager.Instance != null);
        RefreshSkin();
        worldObject = Instantiate(_worldPrefab, _worldOrigin, Quaternion.Euler(Vector3.zero), GameManager.Instance.GetWorldViewCache().transform);
        _worldAnimator = worldObject.GetComponentInChildren<PacManAnimator>(true);
        GameManager.Instance.CollectCamera(this);
    }
    private void Update()
    {
        if (!GameManager.IsReady) return;
        if (!worldObject) return;
        if (!_worldAnimator) return;

        Anim_UpdateModel();
    }
    #endregion

    #region IPlayable Functions

    public PlayerType GetPlayerType()
    {
        return PlayerType.PacMan;
    }
    public PlayerSlot GetPlayerID()
    {
        return _slot;
    }
    public void SetPlayerID(PlayerSlot slot)
    {
        _slot = slot;
    }

    public float GetSightRange()
    {
        return 5f;
    }
    public void Unfreeze()
    {
        _movement.enabled = true;
    }
    public void Freeze()
    {
        _movement.enabled = false;
    }

    public void Lock()
    {
        _movement.locked = true;
    }

    public void Unlock()
    {
        _movement.locked = false;
    }

    public bool IsLocked()
    {
        return _movement.locked;
    }

    public GameObject GetWorldObject()
    {
        return worldObject;
    }
    public GameObject GetMazeObject()
    {
        return gameObject;
    }
    public void TeleportTo(Vector2 newWorldPos, Vector2 outDirection)
    {
        _movement.TeleportTo(newWorldPos, outDirection);
        _teleported = true;
    }
    public void Setup(SpawnData spawnData, Maze2D mazeData)
    {
        _mazeOrigin = spawnData.mazeOrigin;
        _worldOrigin = spawnData.worldOrigin;
        transform.position = _mazeOrigin;
        _teleported = true;
        gameObject.SetActive(true);

        _movement.SetSpeed(_speed);
        _movement.tilemap = mazeData.walls.GetComponent<Tilemap>();
        if (_movement == GetComponent<ComputerPacManAI>())
        {
            var movement = (ComputerPacManAI)_movement;
            movement.pellets = mazeData.pellets;
        }
    }
    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        _cc.enabled = true;
        deathSequence.enabled = false;
        _teleported = true;
        _movement.ResetState();
        Freeze();
        gameObject.SetActive(true);
    }
    #endregion

    #region Events

    public void EatFruit()
    {
        
    }

    public void EatPellet()
    {
        IEnumerator Post()
        {
            yield return new WaitForSeconds(0.25f);
            _movement.speedMod = 0f;
        }
        StopCoroutine(Post());
        _movement.speedMod = 0.25f;
        StartCoroutine(Post());
    }

    #endregion

    #region Getters



    #endregion

    #region Setters

    public void SetSpeed(float value)
    {
        _speed = value;
    }

    #endregion

    #region Theme

    public override void RefreshSkin()
    {
        var skin = GetSkin();

        void ApplyAnimation(string group, ref AnimatedSprite animatedSprite)
        {
            var result = skin.GetSpriteSet(group);
            if (result == null) return;

            animatedSprite.sprites = result.sprites.ToArray();
            animatedSprite.animationTime = result.animationTime;
            animatedSprite.loop = result.loop;
        }
        
        if (!skin) return;

        var result = skin.GetSpriteSet("Moving");
        if (result == null) return;
        spriteRenderer.sprite = result.sprites[0];

        ApplyAnimation("Moving", ref movingSequence);
        ApplyAnimation("Death", ref deathSequence);
        _worldPrefab = skin.model;
    }

    #endregion

    #region Animations
    public void Anim_Rotate(Vector2 direction)
    {
        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
    public void Anim_DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        _cc.enabled = false;
        Freeze();
        Anim_Rotate(new Vector2(0,0));
        deathSequence.enabled = true;
        deathSequence.Restart();
    }
    public void Anim_UpdateModel()
    {
        float rotateSpeed = _movement.actualSpeed;
        float moveSpeed = _movement.actualSpeed;

        Vector3 mazeRotation = gameObject.transform.rotation.eulerAngles;
        Vector3 worldRotation = worldObject.transform.rotation.eulerAngles;

        Quaternion currentRotation = worldObject.transform.rotation;
        Vector3 currentPosition = worldObject.transform.position;

        Quaternion acutalRotation = Quaternion.Euler(worldRotation.x, worldRotation.y, mazeRotation.z);
        Vector3 actualPosition = GameManager.Instance.GetMazeManager().GetWorldCoords(transform.position, _mazeOrigin, _worldOrigin);

        if (_teleported)
        {
            worldObject.transform.rotation = acutalRotation;
            worldObject.transform.position = actualPosition;
            _teleported = false;
        }
        else
        {
            worldObject.transform.rotation = Quaternion.RotateTowards(currentRotation, acutalRotation, rotateSpeed);
            worldObject.transform.position = Vector3.MoveTowards(currentPosition, actualPosition, moveSpeed);
        }
        _worldAnimator.OnUpdate(this);
    }
    #endregion
}
