using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class SpawnpointData : SpawnData
{
    public GameObject player;
    public Vector2 direction;

    public SpawnpointData(IEntitySpawner s, GameObject p, Vector3 a1, Vector3 a2, Vector2 a3) : base(s, a1, a2)
    {
        player = p;
        direction = a3;
    }
}