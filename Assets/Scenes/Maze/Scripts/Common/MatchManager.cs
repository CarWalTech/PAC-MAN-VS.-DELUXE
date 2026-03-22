using UnityEngine;

public class MatchManager : MonoBehaviour
{
    #region Properties: Match Options
    [Header("Match Options")]
    [SerializeField] public float pacManSpeed = 1.0f;
    [SerializeField] public float ghostSight = 4.0f;
    [SerializeField] public float ghostSpeed = 1.0f;
    [SerializeField] public float ghostPauseEatenTime = 0.6f;
    [SerializeField] public float powerPelletDuration = 6.0f;
    [SerializeField] public int targetScore = 15000;
    [SerializeField] public int pacManBonus = 1000;
    [SerializeField] public int playerCount = 5;
    [SerializeField] public PlayerSlot clientPlayerID = PlayerSlot.P1;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
