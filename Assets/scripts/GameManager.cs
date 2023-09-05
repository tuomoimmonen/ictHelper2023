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
    [SerializeField] GameObject firewallBuff;

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
    public bool rightMouseClicked = false;
    public bool leftMouseClicked = false;   

    [SerializeField] TMP_Text gameOverScoreBoardText;
    [SerializeField] TMP_Text gameWinScoreBoardText;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        
        if (instance == null)
        {
            instance = this;
        }
        /*
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        */
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
        UpdateHeartUi();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameTutorial();
        }
        else { gameStarted = true; }

        //UpdateHeartUi();
        if (gameStarted)
        {
            surviveTimer -= Time.deltaTime;
        }
        planeLifeText.text = "PLANET LIFE: " + PlanetLifeController.instance.playerLife.ToString(); //start life
        timerText.text = surviveTimer.ToString("F0"); //survive timer rolling F0 zero decimal
        if(buffAvailable)
        {
            superSlider.value = superMeter;
        }
        scoreText.text = "PISTEET: " + Score.Instance.scorePoints.ToString();

        //next level system
        if (surviveTimer <= 0)
        {
            surviveTimer = 0;
            buffAvailable = false; //prevent taking new buffs when changing levels
            StartCoroutine(NextLevel());
        }

        //super system
        if (superMeter == 2 && buffAvailable == true) //when full and buff is ready
        {
            SFXManager.instance.PlaySfx(4);
            superTimer = 5f;
            int randomBuff = Random.Range(0, 101); //random number for random buff
            buffAvailable = false; //buff off, 1 buff at a time

            //level based buff
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                //player speed buff
                player.playerSpeed = playerSpeedBuff; //speed buff
                speedEffect.enableEmission = true;
                StartCoroutine(RevertSpeedBuff()); //buff off
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                //size buff
                for (int i = 0; i < 3; i++)
                {
                    spritesScale[i].localPosition += buffLocation;
                    spritesScale[i].localScale += buffSize;
                }
                superSizeEffect.enableEmission = true;
                StartCoroutine(RevertSizeBuff()); //buff off
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                //multiple copies buff
                for (int i = 0; i < multiplePlayers.Length; i++) //make the copies active
                {
                    multiplePlayers[i].SetActive(true);
                }
                StartCoroutine(RevertCopyBuff()); //buff off
            }
            else if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                //firewall buff
                firewallBuff.SetActive(true);
                StartCoroutine(RevertFirewallBuff());
            }
            else if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                if (randomBuff < 25)
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
                else if (randomBuff >= 25 && randomBuff <= 50)
                {
                    //multiple copies buff
                    for (int i = 0; i < multiplePlayers.Length; i++) //make the copies active
                    {
                        multiplePlayers[i].SetActive(true);
                    }
                    StartCoroutine(RevertCopyBuff()); //buff off
                }
                else if(randomBuff > 50 && randomBuff <= 75)
                {
                    //firewall buff
                    firewallBuff.SetActive(true);
                    StartCoroutine(RevertFirewallBuff());
                }
                else
                {
                    //player speed buff
                    player.playerSpeed = playerSpeedBuff; //speed buff
                    speedEffect.enableEmission = true;
                    StartCoroutine(RevertSpeedBuff()); //buff off
                }
            }
            /*
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
            */


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


        if (PlanetLifeController.instance.playerLife == 0 && planetIsAlive == true) //game over sequence player not moving
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
            if (!rightKeyPressed && Input.GetKeyDown(KeyCode.RightArrow) || !rightMouseClicked && Input.GetMouseButton(1))
            {
                rightKeyPressed = true;
                rightMouseClicked = true;
            }
            else if (rightKeyPressed && !leftKeyPressed && Input.GetKeyDown(KeyCode.LeftArrow) || rightMouseClicked && !leftMouseClicked && Input.GetMouseButton(0))
            {
                leftKeyPressed = true;
                leftMouseClicked = true;
                gameStarted = true;
            }
        }

        if (!rightKeyPressed || !rightMouseClicked)
        {

            oikealleText.gameObject.SetActive(true);
            textAnimator.SetTrigger("oikealle");
        }
        else
        {
            oikealleText.gameObject.SetActive(false);

            if (!leftKeyPressed || !leftMouseClicked)
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

    private IEnumerator RevertFirewallBuff()
    {
        yield return new WaitForSeconds(superDuration); //superbar duration here
        firewallBuff.SetActive(false); //buff pois päältä
        superMeter = 0; //superi nollattu
        buffAvailable = true; //new buff available
        SFXManager.instance.StopSfx(); //äänet pois
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
        //SimpleScoreBoard.instance.AddScore(Score.Instance.scorePoints);
        if(buffAvailable)
        {
            for (int i = 0; i < multiplePlayers.Length; i++)
            {
                multiplePlayers[i].SetActive(false);
            }

            player.playerSpeed = playerOriginalSpeed;
            speedEffect.gameObject.SetActive(false);

            firewallBuff.SetActive(false); //buff pois päältä
        }
        UpdateLoseScoreBoardText();
        superSlider.gameObject.SetActive(false); //UI CLEAN when transitioning
        timerText.enabled = false;
        Instantiate(planetDeathParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        SFXManager.instance.StopSfx();
        gameOverHighScoreText.text = "ENNÄTYSPISTEET: " + PlayerPrefs.GetInt("Highscore").ToString();
        gameOverScoreText.text = "PISTEESI: " + Score.Instance.scorePoints.ToString();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        //SceneManager.LoadScene("GameOver"); //reload gameover scene
    }

    private void UpdateLoseScoreBoardText()
    {
        SimpleScoreBoard.instance.AddScore(Score.Instance.scorePoints);
        gameOverScoreBoardText.text = "";
        for (int i = 0; i < SimpleScoreBoard.instance.topScores.Length; i++)
        {
            gameOverScoreBoardText.text += (i + 1) + ". " + SimpleScoreBoard.instance.topScores[i] + "\n";
        }
    }

    private void UpdateWinScoreBoardText()
    {
        SimpleScoreBoard.instance.AddScore(Score.Instance.scorePoints);
        gameWinScoreBoardText.text = "";
        for (int i = 0; i < SimpleScoreBoard.instance.topScores.Length; i++)
        {
            gameWinScoreBoardText.text += (i + 1) + ". " + SimpleScoreBoard.instance.topScores[i] + "\n";
        }
    }

    public void WinBackToMainMenu()
    {
        Time.timeScale = 1;
        PlanetLifeController.instance.playerLife = 3;
        Score.Instance.scorePoints = 0;
        gameWinPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        PlanetLifeController.instance.playerLife = 3;
        Score.Instance.scorePoints = 0;
        StartCoroutine(BackToMainMenu());
    }

    public IEnumerator BackToMainMenu() 
    {
        yield return new WaitForSeconds(0.5f);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        //BackgroundMusic.instance.PlayMenuMusic();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        //StartCoroutine(ExitGame());
    }

    public IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    public IEnumerator PlanetHit()
    {
        PlanetLifeController.instance.playerLife -= 1;
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
        
        //speed buff off
        player.playerSpeed = playerOriginalSpeed;
        speedEffect.enableEmission = false;

        //loading text for new level
        if(SceneManager.GetActiveScene().buildIndex < 5) 
        { 
            Animator canvasAnim = FindObjectOfType<MainMenu>().GetComponent<Animator>();
            canvasAnim.SetTrigger("newLevel");
            /*
            if (PlanetLifeController.instance.playerLife == 1)
            {
                Score.Instance.scorePoints += 1;
            }
            else if (PlanetLifeController.instance.playerLife == 2)
            {
                Score.Instance.scorePoints += 2;
            }
            else if (PlanetLifeController.instance.playerLife == 3)
            {
                Score.Instance.scorePoints += 3;
            }
            */
        }

        

        yield return new WaitForSeconds(5);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            UpdateWinScoreBoardText(); 
            gameWinPanel.SetActive(true);

            gameWinScoreText.text = "PISTEESI: " + Score.Instance.scorePoints.ToString();
            gameWinHighScoreText.text = "ENNÄTYSPISTEET: " + PlayerPrefs.GetInt("Highscore").ToString();
            Time.timeScale = 0;
        }
        else
        {
            AddScoreFromHealth();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void UpdateHeartUi()
    {
        for (int i = PlanetLifeController.instance.playerLife; i < hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }
    }

    private void AddScoreFromHealth()
    {
        if (PlanetLifeController.instance.playerLife == 1)
        {
            Score.Instance.AddScore(1);
            return;
        }
        else if (PlanetLifeController.instance.playerLife == 2)
        {
            Score.Instance.AddScore(2);
            return;
        }
        else if (PlanetLifeController.instance.playerLife == 3)
        {
            Score.Instance.AddScore(3);
            return;
        }
    }
}
