using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    float speed = 3f;
    [SerializeField] float minMovementSpeed = 3f; //hazard speed
    [SerializeField] float maxMovementSpeed = 6f;
    Transform target; //hazardin target, transform component
    [SerializeField] GameObject destroyParticles; //player hit particle
    [SerializeField] GameObject planetHitParticles; //planet hit particle
    private AudioSource motorSound;

    GameManager manager; //reference to gamemanager


    void Start()
    {
        motorSound = GetComponent<AudioSource>();
        manager = FindObjectOfType<GameManager>(); //manager handles buffs and points
        speed = Random.Range(minMovementSpeed, maxMovementSpeed); //random speed hazard
        target = GameObject.FindGameObjectWithTag("PlanetCenter").transform; //tag from target planet
        //target = GameObject.FindGameObjectWithTag("Player").transform; //hazard target player
        //motorSound.pitch = Random.Range(0.6f, 1.2f);
    }

    void Update()
    {
        //motorSound.Play();
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); //move towards the target (alkuperäinen position, kohde position, speed)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Planet") //if collision object is "Planet" tag
        {
            //manager.planetLife -= 1;
            SFXManager.instance.PlaySfxPitched(2);
            manager.StartCoroutine("PlanetHit");
            manager.UpdateHeartUi();
            Instantiate(planetHitParticles, transform.position, Quaternion.identity); //hit particles here
            Destroy(gameObject); //destroy this gameobject
        }

        if(collision.tag == "Player")
        {
            SFXManager.instance.PlaySfxPitched(0);
            ScoreNumberController.instance.SpawnScore(1, transform.position);
            manager.superMeter += 1;
            Score.Instance.AddScore(1);
            Instantiate(destroyParticles, transform.position, Quaternion.identity); //player hit particles here
            //manager.Score(1);
            Destroy(gameObject);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
