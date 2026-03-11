using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class Maze3DAnimator : MonoBehaviour
{
    
    public bool automaticNeonEffects = true;
    public Material neonMaterial = null;
    public Material neonMaterialAlt = null;

    #region Properties: Private Fields
    private Animator _controller;
    private Maze3D _source;
    private SkinnedMeshRenderer _neonMesh = null;
    private Dictionary<AnimatorControllerParameterType, List<string>> _knownParams = new Dictionary<AnimatorControllerParameterType, List<string>>();
    private Dictionary<string, bool> _boolValues = new Dictionary<string, bool>();
    private Dictionary<string, int> _intValues = new Dictionary<string, int>();
    private Dictionary<string, float> _floatValues = new Dictionary<string, float>();
    #endregion

    #region Unity Messages

    void Awake()
    {
        _controller = GetComponent<Animator>();
        _source = GetComponent<Maze3D>();

        _knownParams.Add(AnimatorControllerParameterType.Bool, new List<string>());
        _knownParams.Add(AnimatorControllerParameterType.Float, new List<string>());
        _knownParams.Add(AnimatorControllerParameterType.Int, new List<string>());
        _knownParams.Add(AnimatorControllerParameterType.Trigger, new List<string>());

        for (int i = 0; i < _controller.parameterCount; i++)
        {
            var param = _controller.parameters[i];
            _knownParams[param.type].Add(param.name);

            if (param.type == AnimatorControllerParameterType.Bool)
                _boolValues.Add(param.name, param.defaultBool);
            else if (param.type == AnimatorControllerParameterType.Int)
                _intValues.Add(param.name, param.defaultInt);
            else if (param.type == AnimatorControllerParameterType.Float)
                _floatValues.Add(param.name, param.defaultFloat);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        SetNeonState(_source.isScared);
    }

    #endregion

    #region Getters

    public SkinnedMeshRenderer GetNeonMesh()
    {
        if (_neonMesh != null) return _neonMesh;

        var target = Identifier.FindGameObject(gameObject, "NeonMesh");
        if (!target) return null;
        if (target.GetComponent<SkinnedMeshRenderer>() == null) return null;
        _neonMesh = target.GetComponent<SkinnedMeshRenderer>();

        return _neonMesh;
    }

    #endregion

    #region Setters

    public void SetNeonState(bool value)
    {
        SetAnimNeon(value);
        SetAnimBool("NeonState", value);
    }


    public void SetAnimNeon(bool value)
    {
        if (!automaticNeonEffects) return;
        if (!neonMaterial) return;
        if (!neonMaterialAlt) return;
        
        SkinnedMeshRenderer neonTargetMesh = GetNeonMesh();
        if (!neonTargetMesh) return;
        neonTargetMesh.sharedMaterial = value ? neonMaterialAlt : neonMaterial;
    }

    public void SetAnimBool(string name, bool value)
    {
        bool exists = _knownParams[AnimatorControllerParameterType.Bool].Contains(name);
        if (exists) _controller.SetBool(name, value);

        if (!_boolValues.ContainsKey(name)) _boolValues.Add(name, value);
        else _boolValues[name] = value;
    }

    public void SetAnimInteger(string name, int value)
    {
        bool exists = _knownParams[AnimatorControllerParameterType.Int].Contains(name);
        if (exists) _controller.SetInteger(name, value);

        if (!_intValues.ContainsKey(name)) _intValues.Add(name, value);
        else _intValues[name] = value;
    }

    public void SetAnimFloat(string name, float value)
    {   
        bool exists = _knownParams[AnimatorControllerParameterType.Float].Contains(name);
        if (exists) _controller.SetFloat(name, value);

        if (!_boolValues.ContainsKey(name)) _floatValues.Add(name, value);
        else _floatValues[name] = value;
    }

    #endregion
}
