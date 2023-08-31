using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] hazards; //array, what to spawn
    [SerializeField] Transform[] spawnPoints; //array, where to spawn

    [Header("Spawn timers")]
    [SerializeField] float timeBetweenSpawns = 2f; //how much time between spawns
    float nextSpawnTime; //stores the time between spawns

    void Start()
    {
        Debug.Log(Random.Range(0, 101)); //random.range min inclusive and max exclusive
    }

    void Update()
    {
        //timer system
        if(Time.time > nextSpawnTime) //if current gametime is greater than nextspawntime
        {
            GameObject randomHazard = hazards[Random.Range(0, hazards.Length)]; //random hazard
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; //fetch random element from array
            Instantiate(randomHazard, randomSpawnPoint.position, randomSpawnPoint.rotation); //what to spawn, where to spawn, what rotation
            nextSpawnTime = Time.time + timeBetweenSpawns; //timer reset
        }
    }
}
