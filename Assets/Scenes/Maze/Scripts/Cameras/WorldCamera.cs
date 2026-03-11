using UnityEngine;

public class WorldCamera : MonoBehaviour, IGameCamera
{
    public Transform target;
    [SerializeField] private float sightRange = 4f;

    [SerializeField] private float zoomSmoothTime = 0.25f;
    private Camera _cam;
    private float _currentOffset;
    private float _currentHeight;

    private float _offsetVelocity;
    private float _heightVelocity;

    private float _targetOffset;
    private float _targetHeight;

    void Start()
    {
        _cam = GetComponent<Camera>();
        _currentOffset = -(10 * sightRange) + 10;
        _currentHeight = _currentOffset;
        _targetOffset = _currentOffset;
        _targetHeight = _currentHeight;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Smoothly glide toward target zoom
        _currentOffset = Mathf.SmoothDamp(
            _currentOffset,
            _targetOffset,
            ref _offsetVelocity,
            zoomSmoothTime
        );

        _currentHeight = Mathf.SmoothDamp(
            _currentHeight,
            _targetHeight,
            ref _heightVelocity,
            zoomSmoothTime
        );

        Vector3 desiredPosition = new Vector3(
            target.position.x,
            target.position.y + _currentOffset,
            _currentHeight
        );

        transform.position = desiredPosition;
        transform.rotation = Quaternion.Euler(-45, 0, 0);
    }

    public RenderTexture GetViewport()
    {
        return _cam.targetTexture;
    }

    public void SetSightRange(float value)
    {
        if (sightRange != value)
        {
            sightRange = value;

            _targetOffset = -(10 * sightRange) + 10;
            _targetHeight = _targetOffset;
        }
    }
}
