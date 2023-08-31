using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNumberController : MonoBehaviour
{
    public static ScoreNumberController instance; //singleton

    public ScoreNumber numberToSpawn;
    public Transform scoreDisplayCanvas;

    private List<ScoreNumber> numberPool = new List<ScoreNumber>();

    private void Awake()
    {
        instance = this;
    }

    public void SpawnScore(int scoreAmount, Vector3 spawnPosition)
    {
        ScoreNumber newScore = GetFromPool();

        newScore.Setup(scoreAmount);
        newScore.gameObject.SetActive(true);
        newScore.transform.position = spawnPosition;
    }

    public ScoreNumber GetFromPool()
    {
        ScoreNumber numberToOutput = null;
        if (numberPool.Count == 0)
        {
            numberToOutput = Instantiate(numberToSpawn, scoreDisplayCanvas);
        }
        else
        {
            numberToOutput = numberPool[0];
            numberPool.RemoveAt(0);
        }
        return numberToOutput;
    }
    
    public void PlaceInPool(ScoreNumber numberToPlace)
    {
        numberToPlace.gameObject.SetActive(false);
        numberPool.Add(numberToPlace);
    }

}
