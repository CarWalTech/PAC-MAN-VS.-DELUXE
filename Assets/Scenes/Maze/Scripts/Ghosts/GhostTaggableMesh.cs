using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class GhostTaggableMesh : MonoBehaviour
{
    public enum MeshMode
    { 
        Active,
        Scared
    }
    public Material scaredMaterial;
    private Material _normalMaterial;
    private SkinnedMeshRenderer _renderer;
    public Material _activeMaterial;



    public MeshMode mode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<SkinnedMeshRenderer>();
        _normalMaterial = _renderer.sharedMaterial;
        _activeMaterial = _normalMaterial;
    }

    public void UpdateFromSource(Ghost source)
    {
        _activeMaterial = source.worldMaterial;
        if (source.isFrightened) mode = MeshMode.Scared;
        else mode = MeshMode.Active;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case MeshMode.Active:
                _renderer.sharedMaterial = _activeMaterial;
                break;
            case MeshMode.Scared:
                _renderer.sharedMaterial = scaredMaterial;
                break;
        }   
    }
}
