using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLifeController : MonoBehaviour
{
    public static PlanetLifeController instance;

    public int playerLife = 3;
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

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
