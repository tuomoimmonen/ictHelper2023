using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreNumber : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] float lifeTime; //how long the floating text shows
    private float lifeCounter;

    [SerializeField] float speed = 1; //how fast the text scrolls
    void Start()
    {
        lifeCounter = lifeTime;
    }

    void Update()
    {
        if (lifeCounter > 0)
        {
            lifeCounter -= Time.deltaTime;

            if (lifeCounter <= 0)
            {
                //Destroy(gameObject); //POOL HERE TODO
                ScoreNumberController.instance.PlaceInPool(this);
            }
        }

        transform.position += Vector3.up * speed * Time.deltaTime; //text scroll up
    }

    public void Setup(int scoreToDisplay)
    {
        lifeCounter = lifeTime;
        scoreText.text = scoreToDisplay.ToString();
    }
}
