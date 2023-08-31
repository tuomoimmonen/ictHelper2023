using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if(instance == null) //make this singleton
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); //destroy others
        }

    }
    void Start()
    {
        /*
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            DontDestroyOnLoad(this);
        }
        */
    }

    void Update()
    {
        
    }
}
