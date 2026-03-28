using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MazeCamera : MonoBehaviour, IGameCamera
{
    public GameObject target;
    public GameObject shadows;

    public Vector2 mazeOrigin => _mazeMgr.mazeOrigin;
    public float mazeWidth => _mazeMgr.mazeWidth;
    public float mazeHeight => _mazeMgr.mazeHeight;
    private GameManager_Maze _mazeMgr => GameManager.Instance.GetMazeManager();

    private const float _smoothSpeed = 0.125f;
    private Vector3 _startPos;
    private Camera _cam;
    private bool _shadowsActive;
    private bool _alwaysFollow = false;
    private int _defaultPPU;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _defaultPPU = _cam.GetComponent<PixelPerfectCamera>().assetsPPU;
        SetShadowMode(false);
    }

    private void Start()
    {
        _startPos = transform.position;
    }

    public void setPPU(int ppu)
    {
        _defaultPPU = ppu;
        SetAlwaysFollow(_alwaysFollow);
    }

    public void SetShadowMode(bool value)
    {
        _shadowsActive = value;
        shadows.SetActive(_shadowsActive);
    }

    public RenderTexture GetViewport()
    {
        if (!_cam) return null;
        return _cam.targetTexture;
    }

    public void SetAlwaysFollow(bool value)
    {
        _alwaysFollow = value;
        if (_alwaysFollow)
            _cam.GetComponent<PixelPerfectCamera>().assetsPPU = _defaultPPU * 2;
        else
            _cam.GetComponent<PixelPerfectCamera>().assetsPPU = _defaultPPU;
    }

    void LateUpdate()
    {
        if (target == null) return;
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.GetCameraManager() == null) return;

        // Camera half extents
        float halfWidth  = _cam.orthographicSize * _cam.aspect;
        float halfHeight = _cam.orthographicSize;

        // Check if camera is too large on each axis
        bool tooWide = halfWidth * 2f > mazeWidth;
        bool tooTall = halfHeight * 2f > mazeHeight;

        // STEP 1 — ideal camera position (center on player)
        Vector3 desiredPosition = target.transform.position;

        // STEP 2 — compute delta from maze center
        Vector3 delta = new Vector3(
            desiredPosition.x - mazeOrigin.x,
            desiredPosition.y - mazeOrigin.y,
            0f
        );

        // STEP 3 — compute allowed movement
        float maxX = (mazeWidth  / 2f) - halfWidth;
        float maxY = (mazeHeight / 2f) - halfHeight;

        // STEP 4 — clamp only if the camera can fit on that axis
        if (!tooWide)
            delta.x = Mathf.Clamp(delta.x, -maxX, maxX);
        else
            delta.x = 0f; // lock to maze center

        if (!tooTall)
            delta.y = Mathf.Clamp(delta.y, -maxY, maxY);
        else
            delta.y = 0f; // lock to maze center

        // STEP 5 — final camera position
        Vector3 finalPos = new Vector3(
            mazeOrigin.x + delta.x,
            mazeOrigin.y + delta.y,
            _startPos.z
        );

        // STEP 6 — smooth movement
        transform.position = Vector3.Lerp(transform.position, finalPos, _smoothSpeed); 


    }
}
