using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5f; //player speed PUBLIC FOR BUFF
    private float inputX; //capture player left right input
    private float inputY; //up and down input

    public Animator playerAnim;
    [SerializeField] public Transform spriteTransform;

    GameManager gameManager;
    private Transform myTransform;

    public bool playerIsAllowedMove = true;
    private bool canTransition = true;

    [SerializeField] GameObject playerDeathParticles;

    void Start()
    {
        myTransform = GetComponent<Transform>();
        playerAnim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        //inputY = Input.GetAxisRaw("Vertical"); //raw value up and down 
        //Debug.Log(inputX); //test
        //transform.Rotate(Vector3.forward * inputY * playerSpeed * Time.deltaTime); //up and down movement
        if(playerIsAllowedMove == true)
        {
            inputX = Input.GetAxisRaw("Horizontal"); //raw value from left and right -1 left, 1 right, 0 center
            transform.Rotate(Vector3.forward * inputX * playerSpeed * Time.deltaTime); //making the player move

            if (Input.touchCount > 0) //start touch input, index error if not
            {
                //var lets the compiler declare the variably type
                //int halfWidth == var halfWidth
                var halfWidth = Screen.width / 2; //half the screen is "buttons"

                var touch = Input.GetTouch(0);
                var touchPosition = touch.position;

                if (touchPosition.x < halfWidth)
                {
                    transform.Rotate(-Vector3.forward * playerSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Vector3.forward * playerSpeed * Time.deltaTime);
                }
            }
        }

        //animator for rolling
        if(inputX != 0 || Input.touchCount > 0)
        {
            playerAnim.SetBool("isMoving", true);
        }
        else
        {
            SFXManager.instance.PlaySfx(5);
            playerAnim.SetBool("isMoving", false);
            spriteTransform.rotation = Quaternion.identity; //sprite original position
            //SFXManager.instance.StopOneSfx();
        }

        //next level transition
        if (gameManager.surviveTimer == 0 && canTransition == true) 
        {
            //playerAnim.SetBool("isMoving", false); //prevent rolling
            canTransition = false;
            playerIsAllowedMove = false;
            //Vector3 resetPosition = new Vector3(0, 0, 0);
            //transform.position = resetPosition;
            transform.rotation = Quaternion.identity;
            playerAnim.SetTrigger("nextLevel");
        }
        /*
        if(gameManager.planetLife == 0 && gameManager.planetIsAlive == true)
        {
            playerAnim.SetTrigger("death");
            Instantiate(playerDeathParticles, spriteTransform.position, spriteTransform.rotation);
        }
        */
    }
}
