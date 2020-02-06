﻿/** 
* Authors: Matthew Douglas, Hisham Ata
* Purpose: To handle main UI logic within the game
**/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIDelgate : MonoBehaviour
{
    // Start UI
    [Header("Start UI")]
    public GameObject startUI;

    [SerializeField]
    private Text startText;
    [SerializeField]
    private Text coinText;

    // Game UI
    [Header("Game UI")]
    public GameObject gameUI;

    [SerializeField]
    private Image fuelBar;
    [SerializeField]
    private Text distanceTraveled;
    [SerializeField]
    private Text gameCoinText;

    // Game Over UI
    [Header("Game Over UI")]
    public GameObject gameOver;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text coinsCollectedText;
    [SerializeField]
    private Text highscoreText;

    // Reference
    private Animator anim;
    private GameObject player;

    // Property Values
    private float _curFuel;
    private string _curDistance = "0";
    private string _curCoins = "0";

    // Helper Variables
    private bool _gameStarted = false;
    private bool highscore = false;

    void Start()
    {
        // Find References
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");

        // Set start text to show high score and current gears
        float score = 0;
        if(PlayerPrefs.HasKey("highscore"))
        {
            score = PlayerPrefs.GetFloat("highscore");
        }

        int coins = 0;
        if (PlayerPrefs.HasKey("coins"))
        {
            coins = PlayerPrefs.GetInt("coins");
        }

        startText.text = $"Current Highscore\n\n {score} km";
        coinText.text = $"Current Gears\n\n {coins}";
    }

    // Update is called once per frame
    void Update()
    {
        // Update amounts when game is playing
        if (gameStarted)
        {
            fuelBar.fillAmount = _curFuel;
            distanceTraveled.text = $"{_curDistance} km";
            gameCoinText.text = _curCoins;
        }

        // Allow for back button escape on the phone
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void StartGame()
    {
        startUI.SetActive(false);
        ToggleGameplayUI(true);
        gameStarted = true;
        player.GetComponent<PlayerMovement>().StartGame();
        FindObjectOfType<ObjectSpawner>().isPlaying = true;
    }

    public void GameOver()
    {
        FindObjectOfType<ObjectSpawner>().isPlaying = false;
        gameStarted = false;
        StartCoroutine(GameOverWait());
    }

    public void Replay()
    {
        // Replay game, reset the necessary things
        FindObjectOfType<ObjectSpawner>().ResetSpawn();
        highscore = false;
        player.SetActive(true);
        player.GetComponent<PlayerMovement>().ResetMove();
        StartGame();
    }

    void ToggleGameplayUI(bool value)
    {
        gameUI.SetActive(value);
        gameOver.SetActive(!value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Highscore(float previousScore)
    {
        highscore = true;
        highscoreText.text = $"New Highscore!\n\nPrevious Score = {previousScore} km";
    }

    public void ToggleShoot(bool value)
    {
        if (value)
        {
            anim.SetTrigger("Engage");
        }
        else
        {
            anim.SetTrigger("Disable");
        }
    }

    /** Getters and Setters of properties **/
    public bool gameStarted
    {
        get
        {
            return _gameStarted;
        }
        set
        {
            _gameStarted = value;
            if (_gameStarted)
            {

            }
        }
    }
    public float curFuel
    {
        get
        {
            return _curFuel;
        }
        set
        {
            _curFuel = Mathf.Clamp(value, 0, 1);
        }
    }
    public string curDistance
    {
        get
        {
            return _curDistance;
        }
        set
        {
            _curDistance = value.ToString();
        }
    }

    public string curCoins
    {
        get
        {
            return _curCoins;
        }
        set
        {
            _curCoins = value.ToString();
        }
    }

    /** Helper Methods **/
    private void ScoreDisplay()
    {
        // Reveal results on game over
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"You Traveled\n\n {curDistance} km";
        highscoreText.gameObject.SetActive(highscore);
        coinsCollectedText.text = $"Gears Collected\n\n {curCoins}";

    }

    private IEnumerator GameOverWait()
    {
        // Wait to reveal game over after player is destroyed
        yield return new WaitForSeconds(0.6f);
        ToggleGameplayUI(false);
        ScoreDisplay();
    }
}