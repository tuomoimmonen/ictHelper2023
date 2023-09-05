using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuScoreBoard : MonoBehaviour
{
    [SerializeField] TMP_Text winScores;
    void Start()
    {
        
    }

    void Update()
    {
        //SimpleScoreBoard.instance.AddScore(Score.Instance.scorePoints);
        winScores.text = "";
        for (int i = 0; i < SimpleScoreBoard.instance.topScores.Length; i++)
        {
            winScores.text += (i + 1) + ". " + SimpleScoreBoard.instance.topScores[i] + "\n";
        }
    }


}
