using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;


[DefaultExecutionOrder(-10)]
public class Ghost : PlayerThemeHolder<GhostTheme>, IPlayable
{
    [SerializeField] public PlayerCharacter characterSlot;
    [SerializeField] public bool isTagGhost = false;
    [Space]

    [SerializeField] public SpriteRenderer body;
    [SerializeField] public SpriteRenderer eyes;
    [SerializeField] public SpriteRenderer blue;
    [SerializeField] public SpriteRenderer white;
    [SerializeField] public MeshRenderer spotlight;

    
    
    


    
    public Material worldMaterial { get; private set; }
    public GameObject worldObject { get; private set; } = null;
    public bool isTagged { get; private set; }
    public Color initalTagColor { get; private set; }
    public Ghost currentTagTarget { get; private set; }


    private GameObject _worldPrefab;
    private Material _material;
    private float _sightRange = 6f;
    private float _initalSightRange;
    private bool _teleported = false;
    private Vector3 _mazeOrigin;
    private Vector3 _worldOrigin;
    private GhostAI _movement;
    private GhostEyes _eyeController;
    private Rigidbody2D _rb;
    private GhostAnimator _worldAnimator = null;
    private float _speed = 1f;
    private PlayerSlot _slot = PlayerSlot.Null;
    private const float _fruitEffectDuration = 8f;


    #region Properties: State Accessors
    public bool isFrightened { 
        get {
            if (!_movement) return false;
            return _movement.GetGhostMode() == GhostAI.GhostMode.Frightened;
        } 
    }
    public bool isEaten { 
        get {
            if (!_movement) return false;
            return _movement.GetGhostMode() == GhostAI.GhostMode.Eaten;
        } 
    }
    public bool isMidFright { 
        get {
            if (!_movement) return false;
            return _movement.GetMidfrightEnabled();
        } 
    }

    #endregion

    #region Unity Messages / Core Functions
    private void Awake()
    {
        if (GetComponent<GhostAI>()) _movement = GetComponent<GhostAI>();
        else if (GetComponent<GrayGhostAI>()) _movement = GetComponent<GrayGhostAI>();
        else if (GetComponent<PlayerGhostAI>()) _movement = GetComponent<PlayerGhostAI>();
        else if (GetComponent<RedGhostAI>()) _movement = GetComponent<RedGhostAI>();
        else if (GetComponent<BlueGhostAI>()) _movement = GetComponent<BlueGhostAI>();
        else if (GetComponent<PinkGhostAI>()) _movement = GetComponent<PinkGhostAI>();
        else if (GetComponent<OrangeGhostAI>()) _movement = GetComponent<OrangeGhostAI>();
        else _movement = GetComponent<GhostAI>();

        _eyeController = GetComponentInChildren<GhostEyes>();
        _rb = GetComponent<Rigidbody2D>();

        _initalSightRange = _sightRange;
        initalTagColor = body.color;

        Freeze();
    }
    void OnValidate()
    {
        RefreshSkin();        
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.IsReady && GameManager.Instance != null);
        spotlight.gameObject.SetActive(true);
        RefreshSkin();
        worldMaterial = _material;
        worldObject = Instantiate(_worldPrefab, _worldOrigin, Quaternion.Euler(Vector3.zero), GameManager.Instance.GetWorldViewCache().transform);
        _worldAnimator = worldObject.GetComponentInChildren<GhostAnimator>(true);
        GameManager.Instance.CollectCamera(this);
    }
    public void Update()
    {
        if (!GameManager.IsReady) return;
        if (!worldObject) return;
        if (!_worldAnimator) return;

        Anim_UpdateSight();
        Anim_UpdateModel();
    }
    #endregion

    #region IPlayable Functions

    public void Setup(SpawnpointData spawnData, Maze2D mazeData)
    {
        _teleported = true;
        _mazeOrigin = spawnData.mazeOrigin;
        _worldOrigin = spawnData.worldOrigin;
        transform.position = _mazeOrigin;
        gameObject.SetActive(true);
        GameManager.Instance.CollectChaser(this);
        _movement.tilemap = mazeData.walls.GetComponent<Tilemap>();
        _movement.SetSpeed(_speed);
        _movement.initDirection = spawnData.direction;
        _initalSightRange = _sightRange;
        Anim_OnNormal();
    }
    public void ResetState()
    {
        CancelInvoke();
        Timer_EndFruitEffect();
        _sightRange = _initalSightRange;
        _teleported = true;
        Anim_OnNormal();
        gameObject.SetActive(true);
        _movement.Reset();
        Freeze();
        if (isTagGhost) Tag();
    }
    public void TeleportTo(Vector2 newWorldPos, Vector2 outDirection)
    {
        _movement.transform.position = newWorldPos;
        _movement.NextTileDestination = newWorldPos + outDirection;
        _teleported = true;
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
    public PlayerSlot GetPlayerID()
    {
        return _slot;
    }
    public void SetPlayerID(PlayerSlot slot)
    {
        _slot = slot;
    }
    public PlayerType GetPlayerType()
    {
        return PlayerType.Ghost;
    }

    #endregion

    #region Ghost Interactions
    public void Scare(float duration)
    {
        if (isTagGhost && !isTagged) return;
        _movement.frightenedDuration = duration;
        _movement.SetGhostMode(GhostAI.GhostMode.Frightened);
    }
    public void Eaten()
    {
        _movement.SetGhostMode(GhostAI.GhostMode.Eaten);
    }
    public void Tag(Ghost target = null)
    {
        if (target)
        {
            body.color = target.body.color;
            worldMaterial = target.GetMaterial();
            currentTagTarget = target;
            isTagged = true;
        }
        else
        {
            body.color = initalTagColor;
            worldMaterial = _material;
            currentTagTarget = null;
            isTagged = false;
        }
    }
    public void EatFruit()
    {
        Timer_StartFruitEffect();
    }

    #endregion

    #region Getters

    public Material GetMaterial()
    {
        return _material;
    }

    public float GetSightRange()
    {
        return _sightRange;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    #endregion

    #region Setters

    public void SetSpeed(float value)
    {
        _speed = value;
    }

    public void SetSightRange(float value)
    {
        _sightRange = value;
    }

    #endregion

    #region Timers

    private void Timer_StartFruitEffect()
    {
        CancelInvoke();
        SetSightRange(_initalSightRange + 2f);
        Invoke(nameof(Timer_EndFruitEffect), _fruitEffectDuration);
    }
    private void Timer_EndFruitEffect()
    {
        SetSightRange(_initalSightRange);
    }  

    #endregion

    #region On Collision Functions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var mode = _movement.GetGhostMode();
        switch (mode)
        {
            case GhostAI.GhostMode.Chase:
            case GhostAI.GhostMode.Scatter:
                OnNormalCollision(collision);
                break;
            case GhostAI.GhostMode.Frightened:
                OnScaredCollision(collision);
                break;
        }
    }
    private void OnNormalCollision(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && (!isTagGhost || isTagged))
        {
            if (isTagGhost) GameManager.Instance.Event_EatPlayer(currentTagTarget);
            else GameManager.Instance.Event_EatPlayer(this);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Chaser") && isTagGhost && !isTagged)
        {
            var passerGhost = collision.gameObject.GetComponent<Ghost>();
            if (!passerGhost?.isTagGhost ?? false) Tag(passerGhost);
        }
    }
    private void OnScaredCollision(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.Event_EatChaser(this, collision.gameObject.GetComponent<PacMan>());
            if (isTagGhost) Tag();
        }
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

        _worldPrefab = skin.model;
        _material = skin.materials[characterSlot];

        var a_body = body.GetComponent<AnimatedSprite>();
        var a_blue = blue.GetComponent<AnimatedSprite>();
        var a_white = white.GetComponent<AnimatedSprite>();
        var a_eyes = eyes.GetComponent<GhostEyes>();

        var result = skin.GetSpriteSet("Moving");
        if (result == null) return;
        body.sprite = result.sprites[0];

        var eyes_result = skin.GetSpriteSet("Eyes_Right");
        if (eyes_result == null) return;
        eyes.sprite = eyes_result.sprites[0];

        ApplyAnimation("Moving", ref a_body);
        ApplyAnimation("Frightened", ref a_blue);
        ApplyAnimation("MidFright", ref a_white);

        ApplyAnimation("Eyes_Up", ref a_eyes.up);
        ApplyAnimation("Eyes_Down", ref a_eyes.down);
        ApplyAnimation("Eyes_Left", ref a_eyes.left);
        ApplyAnimation("Eyes_Right", ref a_eyes.right);
    }

    #endregion

    #region Animations
    public void Anim_OnFrightened()
    {
        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        blue.GetComponent<AnimatedSprite>().Restart();
    }
    public void Anim_OnMidlifeFrightened()
    {
        blue.enabled = false;
        white.enabled = true;
        white.GetComponent<AnimatedSprite>().Restart();
    }
    public void Anim_OnNormal()
    {
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }
    public void Anim_OnRun(Vector2 dir)
    {
        _eyeController.setDirection(dir);
    }
    public void Anim_OnEatenUnpause()
    {
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }
    public void Anim_OnEatenPause()
    {
        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = false;
        white.enabled = false;
    }
    public void Anim_OnPostEaten(Vector2 dir)
    {
        _eyeController.setDirection(dir);
    }

    public void Anim_UpdateSight()
    {
        var desiredScale = new Vector3(_sightRange * 0.175f, _sightRange * 0.175f, _sightRange * 0.175f);
        var currentScale = spotlight.gameObject.transform.localScale;
        spotlight.gameObject.transform.localScale = Vector3.MoveTowards(currentScale, desiredScale, 0.175f * Time.fixedDeltaTime);
    }
    public void Anim_UpdateModel()
    {
        Vector2 direction;
        if (_movement.GetGhostMode() == GhostAI.GhostMode.LeavingHouse || _movement.GetGhostMode() == GhostAI.GhostMode.Eaten)
            direction = _eyeController.GetDirection();
        else
            direction = _movement.GetDirection();

        float rotateSpeed = _movement.GetSpeed();
        float moveSpeed = _movement.GetSpeed();
        float angle = Mathf.Atan2(direction.y, direction.x);

        Vector3 mazeRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward).eulerAngles;
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
