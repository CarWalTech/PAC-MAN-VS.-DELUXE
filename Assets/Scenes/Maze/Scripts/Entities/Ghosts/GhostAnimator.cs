using UnityEngine;

public class GhostAnimator : MonoBehaviour
{
    public Animator animatonController;
    private Ghost _source = null;

    private GhostTaggableMesh _mainMesh;

    void Awake()
    {
        _mainMesh = GetComponentInChildren<GhostTaggableMesh>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_source) return;

        _mainMesh.UpdateFromSource(_source);
        animatonController.SetBool("Eaten", _source.isEaten);
        animatonController.SetBool("Midfright", _source.isMidFright);
        animatonController.SetBool("Frightened", _source.isFrightened);
    }

    public void OnUpdate(Ghost ghost)
    {
        if (!_source) _source = ghost;
    }
}
