using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleScoreBoard : MonoBehaviour
{
    public static SimpleScoreBoard instance;
    public int[] topScores = new int[3];
    //public TMP_Text scoreboardText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        for (int i = 0; i < topScores.Length; i++)
        {
            topScores[i] = PlayerPrefs.GetInt("TopScore" + i, 0);
        }
    }

    public void AddScore(int newScore)
    {
        for (int i = 0; i < topScores.Length; i++)
        {
            if (newScore > topScores[i])
            {
                for (int j = topScores.Length - 1; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                }
                topScores[i] = newScore;
                PlayerPrefs.SetInt("TopScore" + i, newScore);
                PlayerPrefs.Save();
                break;
            }
        }
    }

    /*
    private void UpdateScoreBoardText()
    {
        scoreboardText.text = "";
        for (int i = 0; i < topScores.Length; i++)
        {
            scoreboardText.text += (i + 1) + ". " + topScores[i] + "\n";
        }
    }
    */
}
