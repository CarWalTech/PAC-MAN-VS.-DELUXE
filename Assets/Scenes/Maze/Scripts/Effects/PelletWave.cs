using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PelletWave : MonoBehaviour
{

    [Header("Bobbing Variables")]
    [SerializeField, Range(-100, 100)] public float zOffset = -2;
    [SerializeField, Range(0, 1000)] public float bobbingSpeed = 5f;
    [SerializeField, Range(0, 1000)] public float upDownDifference = 2;
    [SerializeField, Range(0, 1000)] public float waveFrequency = 2.5f;

    Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        transform.position = originalPosition + new Vector3(0, 0, zOffset) + Mathf.Sin(Time.time * bobbingSpeed + transform.position.x * waveFrequency + transform.position.y * waveFrequency) * (upDownDifference / 2) * Vector3.forward;  
    }
}
