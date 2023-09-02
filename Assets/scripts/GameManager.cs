using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Life system")]
    public TMP_Text planeLifeText; //planet lives text
    public int planetLife = 3; //starting life
    public bool planetIsAlive = true;
    public Image[] hearts;

    [Header("Super system")]
    [SerializeField] private Slider superSlider; //visual clue for super meter
    public int superMeter = 0; //supermeter starting value
    private float superDuration = 5f; //supers duration here COROUTINE timer
    private float superTimer = 5f;

    [Header("Buffs")]
    Player player;
    private float playerOriginalSpeed; //players original speed
    private float playerSpeedBuff = -300f; //players buff speed
    private bool buffAvailable = true; //bool for limiting one buff at a time
    [SerializeField] GameObject[] multiplePlayers;
    [SerializeField] Transform[] spritesScale; //for buff sprite scale
    [SerializeField] TMP_Text superDurationTimerText;
    [SerializeField] Vector3 buffSize = new Vector3(2,2,2);
    [SerializeField] Vector3 buffLocation = new Vector3(0, 1, 0);
    [SerializeField] ParticleSystem speedEffect;
    [SerializeField] ParticleSystem superSizeEffect;

    [Header("Getting hit")]
    [SerializeField] GameObject hurtPanel; //visual clue for planet hit
    [SerializeField] GameObject planetDeathParticles;
    [SerializeField] GameObject playerDeathParticles;

    [Header("Scene timer")]
    public float surviveTimer = 10f;
    [SerializeField] TMP_Text timerText;

    [Header("Score system")]
    [SerializeField] TMP_Text gameOverHighScoreText;
    [SerializeField] TMP_Text scoreText;
    //private int score = 0; //points for catching hazards
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameWinPanel;
    [SerializeField] TMP_Text gameWinHighScoreText;
    [SerializeField] TMP_Text gameWinScoreText;
    [SerializeField] TMP_Text gameOverScoreText;

    [Header("Level transitioning")]
    [SerializeField] TMP_Text transitioningText;
    [SerializeField] Animator textAnimator;

    PlanetController planetController;

    public static GameManager instance;
    public bool gameStarted = false;
    [SerializeField] TMP_Text oikealleText;
    [SerializeField] TMP_Text vasemmalleText;
    public bool rightKeyPressed = false;
    public bool leftKeyPressed = false;

    private void Awake()
    {
        Time.timeScale = 1.0f;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        planetController = FindObjectOfType<PlanetController>();
        player = FindObjectOfType<Player>();
        playerOriginalSpeed = FindObjectOfType<Player>().playerSpeed;
        hurtPanel.SetActive(false);
        //gameOverHighScoreText.text = "HIGHSCORE: " + score.ToString();
        //gameWinScoreText.text = score.ToString();
        superDurationTimerText.text = superTimer.ToString("F0");
        superDurationTimerText.enabled = true;
        speedEffect.enableEmission = false;
        superSizeEffect.enableEmission = false;
    }

    void Update()
    {
        GameTutorial();
        //UpdateHeartUi();
        if (gameStarted)
        {
            surviveTimer -= Time.deltaTime;
        }
        planeLifeText.text = "PLANET LIFE: " + planetLife.ToString(); //start life
        timerText.text = surviveTimer.ToString("F0"); //survive timer rolling F0 zero decimal
        superSlider.value = superMeter;
        scoreText.text = "PISTEET: " + Score.Instance.scorePoints.ToString();

        //next level system
        if (surviveTimer <= 0)
        {
            surviveTimer = 0;
            buffAvailable = false; //prevent taking new buffs when changing levels
            StartCoroutine(NextLevel());
        }

        //super system
        if (superMeter == 1 && buffAvailable == true) //when full and buff is ready
        {
            SFXManager.instance.PlaySfx(4);
            superTimer = 5f;
            int randomBuff = Random.Range(0, 101); //random number for random buff
            buffAvailable = false; //buff off, 1 buff at a time

            if (randomBuff < 33)
            {
                //size buff TODO GET NUMBERS TO VARIABLES
                //spritesScale.localScale = new Vector3(4, 4, 4);
                //spritesScale.localScale *= 2f;
                //spritesScale[0].localPosition += buffLocation;
                //spritesScale[0].localScale += buffSize;
                for (int i = 0; i < 3; i++)
                {
                    spritesScale[i].localPosition += buffLocation;
                    spritesScale[i].localScale += buffSize;
                }
                superSizeEffect.enableEmission = true;
                StartCoroutine(RevertSizeBuff()); //buff off
            }
            else if (randomBuff > 33 && randomBuff < 66)
            {
                //multiple copies buff
                for (int i = 0; i < multiplePlayers.Length; i++) //make the copies active
                {
                    multiplePlayers[i].SetActive(true);
                }
                StartCoroutine(RevertCopyBuff()); //buff off
            }
            else
            {
                //player speed buff
                player.playerSpeed = playerSpeedBuff; //speed buff
                speedEffect.enableEmission = true;
                StartCoroutine(RevertSpeedBuff()); //buff off
            }


            //StartCoroutine(RevertSpeedBuff()); //revert buffs
        }

        if(buffAvailable == false)
        {
            superDurationTimerText.enabled = true;
            superTimer -= Time.deltaTime;
            superDurationTimerText.text = superTimer.ToString("F0");
        }
        else
        {
            superDurationTimerText.enabled = false;
        }


        if (planetLife == 0 && planetIsAlive == true) //game over sequence player not moving
        {
            player.playerAnim.SetTrigger("death");
            Instantiate(playerDeathParticles, player.spriteTransform.position, player.spriteTransform.rotation);
            SFXManager.instance.PlaySfxPitched(1);
            timerText.enabled = false; //timer text off
            buffAvailable = false; //buffs not allowed
            player.playerIsAllowedMove = false; //players movement restriction
            planetIsAlive = false; //no doubles
            StartCoroutine(GameOver());
        }
    }

    private void GameTutorial()
    {
        if(gameStarted == false)
        {
            if (!GameManager.instance.rightKeyPressed && Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameManager.instance.rightKeyPressed = true;
            }
            else if (GameManager.instance.rightKeyPressed && !GameManager.instance.leftKeyPressed && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GameManager.instance.leftKeyPressed = true;
                gameStarted = true;
            }
        }

        if (!GameManager.instance.rightKeyPressed)
        {

            oikealleText.gameObject.SetActive(true);
            textAnimator.SetTrigger("oikealle");
        }
        else
        {
            oikealleText.gameObject.SetActive(false);

            if (!GameManager.instance.leftKeyPressed)
            {
                vasemmalleText.gameObject.SetActive(true);
                textAnimator.SetTrigger("vasemmalle");
            }
            else
            {
                vasemmalleText.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator RevertSpeedBuff()
    {
        yield return new WaitForSeconds(superDuration); //superbar duration here


        //player.playerSpeed = -170f; //back to original speed
        player.playerSpeed = playerOriginalSpeed;
        speedEffect.enableEmission = false;

        //player sprite size buff
        //spritesScale.localScale = new Vector3(2,2,2); TEST
        //spritesScale.localPosition = new Vector3(0, 2f, 0); TEST
        //spritesScale.localScale -= buffSize;
        //spritesScale.localPosition = playerOriginalTransform.localPosition;

        //multiple copies buff
        /*
        for (int i = 0; i < multiplePlayers.Length; i++)
        {
            multiplePlayers[i].SetActive(false);
        }
        */

        superMeter = 0; //reset supermeter
        buffAvailable = true; //buff available
        SFXManager.instance.StopSfx();
    }

    private IEnumerator RevertSizeBuff()
    {
        yield return new WaitForSeconds(superDuration);
        superSizeEffect.enableEmission = false;
        //spritesScale[0].localScale -= buffSize;
        //spritesScale[0].localPosition -= buffLocation;
        for (int i = 0; i < 3; i++)
        {
            spritesScale[i].localPosition -= buffLocation;
            spritesScale[i].localScale -= buffSize;
        }
        superMeter = 0; //reset supermeter
        buffAvailable = true; //buff available
        SFXManager.instance.StopSfx();
    }

    private IEnumerator RevertCopyBuff()
    {
        yield return new WaitForSeconds(superDuration);
        for (int i = 0; i < multiplePlayers.Length; i++)
        {
            multiplePlayers[i].SetActive(false);
        }
        superMeter = 0; //reset supermeter
        buffAvailable = true; //buff available
        SFXManager.instance.StopSfx();
    }

    public IEnumerator GameOver()
    {
        superSlider.gameObject.SetActive(false); //UI CLEAN when transitioning
        timerText.enabled = false;
        Instantiate(planetDeathParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        gameOverHighScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("Highscore").ToString();
        gameOverScoreText.text = "YOUR SCORE: " + Score.Instance.scorePoints.ToString();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        //SceneManager.LoadScene("GameOver"); //reload gameover scene
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator PlanetHit()
    {
        planetLife -= 1;
        hurtPanel.SetActive(true);
        planetController.planetAnimator.SetBool("hit", true);
        yield return new WaitForSeconds(0.1f);
        hurtPanel.SetActive(false);
        planetController.planetAnimator.SetBool("hit", false);
    }

    /*
    public void Score(int scoreToAdd)
    {
        //ScoreNumberController.instance.SpawnScore(scoreToAdd, transform.position);
        score += scoreToAdd;
        scoreText.text = "SCORE: " + score.ToString();
        if(score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
    }
    */

    public IEnumerator NextLevel()
    {
        superSlider.gameObject.SetActive(false); //UI CLEAN when transitioning
        timerText.enabled = false;
        //multiple copies off
        for (int i = 0; i < multiplePlayers.Length; i++)
        {
            multiplePlayers[i].SetActive(false);
        }

        /*
        //size buff off
        spritesScale.localScale -= buffSize;
        spritesScale.localPosition -= buffLocation;
        */

        //speed buff off
        player.playerSpeed = playerOriginalSpeed;
        speedEffect.enableEmission = false;

        //loading text for new level
        if(SceneManager.GetActiveScene().buildIndex == 1) 
        { 
            Animator canvasAnim = FindObjectOfType<MainMenu>().GetComponent<Animator>();
            canvasAnim.SetTrigger("newLevel");
        }

        yield return new WaitForSeconds(5);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            gameWinPanel.SetActive(true);
            gameWinScoreText.text = "YOUR SCORE: " + Score.Instance.scorePoints.ToString();
            gameWinHighScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("Highscore").ToString();
            Time.timeScale = 0;
        }
        else
        {
            SceneManager.LoadScene("Game2");
        }
    }

    public void UpdateHeartUi()
    {
        for (int i = planetLife; i < hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }
    }
}
