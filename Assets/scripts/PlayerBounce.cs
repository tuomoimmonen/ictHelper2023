using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    public float bounceForce = 10f; // Adjust this to control the bounce strength
    [SerializeField] GameObject[] hitEffect;

    WaveSpawner waveSpawner;

    private void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>(); 
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Hazard enemy = collision.GetComponent<Hazard>();
            int randomEffect = Random.Range(0, hitEffect.Length);
            Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                if(enemy.firstTimeHit == true)
                {
                    //waveSpawner.enemiesAlive--;
                    Instantiate(hitEffect[randomEffect], transform.position, Quaternion.identity);
                    //ScoreNumberController.instance.SpawnScore(1, collision.transform.position);
                    //enemy.firstTimeHit = true;
                }
                //Instantiate(hitEffect[randomEffect], transform.position, Quaternion.identity);
                //ScoreNumberController.instance.SpawnScore(1, collision.transform.position);
                Vector2 bounceDirection = (collision.transform.position - transform.position).normalized;
                enemyRigidbody.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
