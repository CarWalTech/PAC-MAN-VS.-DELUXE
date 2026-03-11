using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Maze3DAnimator))]
public class Maze3D : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    private Maze3DAnimator _animations = null;
    private Vector3 _initalPosition = Vector3.zero;
    public bool isScared { get; private set; } = false;

    private void Awake()
    {
        _initalPosition = transform.localPosition;
        _animations = GetComponentInChildren<Maze3DAnimator>(true);
    }

    void Start()
    {
        ResetState();
    }

    private void ResetState()
    {
        isScared = false;
    }

    public void Freeze()
    {
        CancelInvoke();
    }

    public void Scare(float duration)
    {
        CancelInvoke();
        isScared = true;
        Invoke(nameof(EndScare), duration);
    }

    public void EndScare()
    {
        isScared = false;
    }

    public void Adjust()
    {
        transform.localPosition = _initalPosition + offset;
    }

    private void Update()
    {

    }
}
