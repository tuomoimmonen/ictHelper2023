using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance { get; private set; }

    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
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
        //audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = mainMenuMusic;
        audioSource.Play();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip targetClip = null;

        if (scene.buildIndex == 0)
        {
            targetClip = mainMenuMusic;
            //audioSource.volume = 1f;
        }
        else if (scene.buildIndex != 0)
        {
            targetClip = gameMusic;
            audioSource.volume = 0.06f;
            
        }

        if (audioSource.clip != targetClip)
        {
            audioSource.clip = targetClip;
            audioSource.Play();
        }
    }
}
