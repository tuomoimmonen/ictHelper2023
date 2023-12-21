using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }
    public int scorePoints = 0;
    private TMP_Text scorePointsText;
    private GameObject[] scoreHandlerList;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       
        DontDestroyOnLoad(this);

    }
    public void AddScore(int scoreToAdd)
    {
        scorePoints += scoreToAdd;
 
        if (scorePoints > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", scorePoints);
        }
    }
}
