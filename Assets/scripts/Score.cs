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
    //[SerializeField] TMP_Text gameWinScorePointsText;
    //[SerializeField] TMP_Text gameWinHighScorePointsText; 
    //[SerializeField] TMP_Text gameOverScorePointsText;
    //[SerializeField] TMP_Text gameOverHighScorePointsText;

    private void Awake()
    {
        if(Instance != this) //make this singleton
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject); //destroy others
            return;
        }

        //Instance = this;
        DontDestroyOnLoad(this);
        //scorePointsText = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
    }
    void Start()
    {
        
        //scorePointsText = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
        //scorePointsText.text = "Score: " + scorePoints.ToString();
        //Debug.Log(scorePointsText.gameObject);
        
    }
    void Update()
    {
        
    }

    public void AddScore(int scoreToAdd)
    {
        scorePoints += scoreToAdd;
        //scorePointsText.text = "SCORE: " + scorePoints.ToString();
        if (scorePoints > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", scorePoints);
        }
    }
}