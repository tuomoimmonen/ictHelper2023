using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] spawnObjects;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;
    public int enemiesPerWave = 5;
    public float waveInterval = 5f;

    private int currentWave = 0;
    private int maxWave = 3;

    [SerializeField] TMP_Text waveAnnouncerText;

    public int enemiesAlive = 0;

    [SerializeField] float timeBeforeWave = 1f;
    [SerializeField] float announcerDuration = 2f;
    [SerializeField] float timeBetweenWaves = 5f;
    [SerializeField] float timeBetweenSpawns = 2f;
    private bool waveInProgress = false;

    PlanetController planet;
    private void Start()
    {
        planet = FindObjectOfType<PlanetController>();
        //StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if(GameManager.instance.gameStarted)
        {
            StartCoroutine(SpawnWaves());
        }
    }

    private IEnumerator SpawnWaves()
    {
        //StartCoroutine(WaveAnnouncer());
        yield return new WaitForSeconds(timeBeforeWave);
        while (planet.isAlive == true)
        {
            if (enemiesAlive == 0 && !waveInProgress)
            {
                waveInProgress = true;
                yield return StartCoroutine(SpawnWave());
                waveInProgress = false;
            }
            yield return null;
        }

    }

    private IEnumerator SpawnWave()
    {
        waveAnnouncerText.gameObject.SetActive(true);
        waveAnnouncerText.text = "Wave " + (currentWave + 1) + " alkaa!";
        yield return new WaitForSeconds(announcerDuration);
        waveAnnouncerText.gameObject.SetActive(false);
        if (currentWave > 1)
        {
            enemiesPerWave++;
        }
        StartCoroutine(SpawnEnemy());
        currentWave++;
        maxWave++;
        yield return new WaitForSeconds(timeBetweenWaves);
    }
    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            int randomEnemy = Random.Range(0, spawnObjects.Length);
            Instantiate(spawnObjects[randomEnemy], spawnPoints[randomSpawnPoint].position, transform.rotation);
            enemiesAlive++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

    }
}
