using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BobAndRotate : MonoBehaviour
{

    [Header("Bobbing Variables")]
    [SerializeField, Range(-5, 5)] float zOffset = -2;
    [SerializeField, Range(0, 2)] public float bobbingSpeed = 1f;
    [SerializeField, Range(0, 2)] public float upDownDifference = 1;
    [SerializeField] public bool waveMode = false;
    [SerializeField, Range(0, 1000)] public float waveFrequency = 1f;

    [Header("Rotation Variables")]
    [SerializeField, Range(-1000, 1000)] float xRotationSpeed = 0;
    [SerializeField, Range(-1000, 1000)] float yRotationSpeed = 0;
    [SerializeField, Range(-1000, 1000)] float zRotationSpeed = 0;

    Vector3 originalPosition;

    void Start()
    {
        transform.position += new Vector3(0, 0, zOffset);
        originalPosition = transform.position;
    }

    void Update()
    {
        if (waveMode)
        {
            transform.position = originalPosition + Mathf.Sin(Time.time * bobbingSpeed + transform.position.x * waveFrequency) * (upDownDifference / 2) * Vector3.forward;  
        }
        else
        {
            // Bobbing              
            transform.position = originalPosition + Mathf.Sin(Time.time * bobbingSpeed) * (upDownDifference / 2) * Vector3.forward;  
        }


        // Rotating
        transform.Rotate(new Vector3(xRotationSpeed, yRotationSpeed, zRotationSpeed) * Time.deltaTime); // y axis rotation Spins the coin
    }
}
