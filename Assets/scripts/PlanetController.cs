using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public Animator planetAnimator;
    GameManager gameManager;

    public bool isAlive = true;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        planetAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if(gameManager.surviveTimer == 0)
        {
            planetAnimator.SetTrigger("nextLevel");
        }

        if(gameManager.planetLife == 0)
        {
            isAlive = false;
            planetAnimator.SetTrigger("death");
        }
        
    }
}
