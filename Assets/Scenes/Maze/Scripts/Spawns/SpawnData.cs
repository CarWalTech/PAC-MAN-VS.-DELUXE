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
    public GameObject player;
    public Vector3 mazeOrigin;
    public Vector3 worldOrigin;
    public Vector2 initalDirection;

    public SpawnData(GameObject p, Vector3 a1, Vector3 a2, Vector2 a3)
    {
        player = p;
        mazeOrigin = a1;
        worldOrigin = a2;
        initalDirection = a3;
    }
}