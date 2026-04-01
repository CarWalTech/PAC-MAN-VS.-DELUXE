using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class SpawnData
{
    public IEntitySpawner spawner;
    public Vector3 mazeOrigin;
    public Vector3 worldOrigin;

    public SpawnData(IEntitySpawner s, Vector3 a1, Vector3 a2)
    {
        spawner = s;
        mazeOrigin = a1;
        worldOrigin = a2;
    }
}