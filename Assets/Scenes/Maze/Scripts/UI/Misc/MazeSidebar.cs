using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;


public class MazeSidebar : MonoBehaviour
{
    public HorizontalOrVerticalLayoutGroup verticalLayoutGroup = null;
    public MazeScorecard player1 = null;
    public MazeScorecard player2 = null;
    public MazeScorecard player3 = null;
    public MazeScorecard player4 = null;
    public MazeScorecard player5 = null;
    public int numberOfPlayers = 5;
    public bool changeSpacing = false;
    public int normalSpacing = 0;
    public int excessSpacing = -27;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        UpdateLabels();
    }


    void OnValidate()
    {
        UpdateLabels();
    }

    int GetNumberOfPlayers()
    {
        if (Application.isPlaying && GameManager.Instance != null) return GameManager.Instance.GetMatchManager().playerCount;
        else return numberOfPlayers;
    }

    void UpdateLabels()
    {
        if (GetNumberOfPlayers() == 5)
        {
            if (player5) player5.gameObject.SetActive(true);
            if (verticalLayoutGroup && changeSpacing) verticalLayoutGroup.spacing = excessSpacing;
            else verticalLayoutGroup.spacing = normalSpacing;
        }
        else
        {
            if (player5) player5.gameObject.SetActive(false);
            if (verticalLayoutGroup) verticalLayoutGroup.spacing = normalSpacing;
        }
    }
    void Update()
    {
        UpdateLabels();
    }


}
