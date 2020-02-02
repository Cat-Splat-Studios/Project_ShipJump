using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDelgate : MonoBehaviour
{

    [SerializeField]
    Image fuelBar;
    [SerializeField]
    Text distanceTraveled;
    [SerializeField]
    Text gameplayTitle;

    public GameObject startUI;
    public GameObject gameOver;
    public GameObject gameUI;
    public Text scoreText;
    public Text highscoreText;
    public Text startText;

    [SerializeField]
    private float _curFuel;
    [SerializeField]
    private string _curDistance = "0";

    private bool _gameStarted = false;
    private bool highscore = false;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        float score = 0;
        if(PlayerPrefs.HasKey("highscore"))
        {
            score = PlayerPrefs.GetFloat("highscore");
        }

        startText.text = $"Your Current Highscore\n\n {score} meters";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            fuelBar.fillAmount = _curFuel;
            distanceTraveled.text = _curDistance;
        }
    }

    void ToggleGameplayUI(bool value)
    {
        // can enable/disable your game over stuff here by simple setting it to !value
       
        gameUI.SetActive(value);
        gameOver.SetActive(!value);
    }

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

    public void StartGame()
    {
        startUI.SetActive(false);
        Debug.Log("STATA");
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
        FindObjectOfType<ObjectSpawner>().ResetSpawn();
        highscore = false;
        player.SetActive(true);
        player.GetComponent<PlayerMovement>().ResetMove();
        StartGame();
    }

    public void Highscore(float previousScore)
    {
        highscore = true;
        highscoreText.text = $"New Highscore!\n\nPrevious Score = {previousScore} meters";
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ScoreDisplay()
    {
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"You Traveled\n\n {curDistance}";
        highscoreText.gameObject.SetActive(highscore);

    }

    private IEnumerator GameOverWait()
    {
        yield return new WaitForSeconds(0.6f);
        ToggleGameplayUI(false);
        ScoreDisplay();
    }
}
