/** 
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
    public GameObject StatHUD;
    [SerializeField]
    private Text startText;
    [SerializeField]
    private Text gearText;

    // Game UI
    [Header("Game UI")]
    public GameObject gameUI;
    [SerializeField]
    private Image fuelBar;
    [SerializeField]
    private Text distanceTraveled;
    [SerializeField]
    private GameObject[] numbers;

    // Game Over UI
    [Header("Game Over UI")]
    public GameObject gameOver;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text coinsCollectedText;
    [SerializeField]
    private Text highscoreText;

    //Shop UI
    [Header("Shop UI")]
    [SerializeField]
    private GameObject shopUI;
    [SerializeField]
    private Shop[] shops;

    [Header("Store UI")]
    [SerializeField]
    private GameObject storeUI;

    // Reference
    private Animator anim;
    private PlayerManager player;
    private GeneratorManager generatorManager;
    private CameraFollow camera;

    // Property Values
    private float _curFuel;
    private string _curDistance = "0";

    // Helper Variables
    private bool _gameStarted = false;
    private bool highscore = false;

    void Start()
    {
        // Find References
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        generatorManager = FindObjectOfType<GeneratorManager>();
        camera = FindObjectOfType<CameraFollow>();

        // Set start text to show high score and current gears
        SetStartText();
    }

    // Update is called once per frame
    void Update()
    {
        fuelBar.fillAmount = _curFuel;
        // Update amounts when game is playing
        if (gameStarted)
        {     
            distanceTraveled.text = $"{_curDistance} km";
        }

        // Allow for back button escape on the phone
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void StartGame()
    {
        //gears.SetActive(false);
        StatHUD.SetActive(false);
        startUI.SetActive(false);
        gameUI.SetActive(true);
        anim.SetTrigger("Start");

        // make sure correct mesh is on player
        player.StartGameMeshCheck();

        camera.GameStart();
     
        StartCoroutine(StartWait());
    }

    public void GameOver()
    {
        generatorManager.StopGenerators();
        gameStarted = false;
        StartCoroutine(GameOverWait());
    }

    public void Replay()
    {
        // Replay game, reset the necessary things
        generatorManager.TopGenerate();
       
        player.gameObject.SetActive(true);
        player.PlayerMovement().ResetMove();
        player.PlayerMovement().StartGame();

        // TODO MAKE SINGLETON
        FindObjectOfType<PoolManager>().ResetObjects();

        // set ui
        gameUI.SetActive(true);
        gameOver.SetActive(false);

        gameStarted = true;

        highscore = false;
    }

    public void Highscore(float previousScore)
    {
        highscore = true;
        highscoreText.text = $"New Highscore!\n\nPrevious Score = {previousScore} km";

        // save leaderboards
    }

    // TODO: Rework shooting
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

    public void OpenShop(int shopType)
    { 
        startUI.SetActive(false);
        shopUI.SetActive(true);
        shops[shopType].gameObject.SetActive(true);
        shops[shopType].SetStartPos();
        shops[shopType].InitItems();
    }

    public void CloseShop()
    {
        foreach(Shop shop in shops)
        {
            shop.gameObject.SetActive(false);
        }
        shopUI.SetActive(false);
        startUI.SetActive(true);
    }

    public void StoreToggle(bool value)
    {
        storeUI.SetActive(value);
        startUI.SetActive(!value);
    }

    public void BackToMenu()
    {
        camera.ToMenuOffset();
        player.gameObject.SetActive(true);
        player.PlayerMovement().ResetIdle();

        FindObjectOfType<PoolManager>().ResetObjects();

        // set ui
        startUI.SetActive(true);
        gameUI.SetActive(false);
        StatHUD.SetActive(true);
        gameOver.SetActive(false);
        SetStartText();
    }

    public void UpdateGearText()
    {
        gearText.text = $"{GearManager.instance.GetGears()}";
    }

    public void QuitGame()
    {
        Application.Quit();
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

    /** Helper Methods **/
    private void ScoreDisplay()
    {
        // Reveal results on game over
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"You Traveled\n\n {curDistance} km";
        highscoreText.gameObject.SetActive(highscore);
        coinsCollectedText.text = $"Gears Collected\n\n {GearManager.instance.levelGears}";
    }

    private void ToggleNumbers(bool value)
    {
        foreach (GameObject item in numbers)
        {
            item.SetActive(value);
        }
    }

    private IEnumerator StartWait()
    {
        player.PlayerMovement().ResetMove();
        yield return new WaitForSeconds(2.1f);
        player.PlayerMovement().StartGame();
        generatorManager.TopGenerate();
        ToggleNumbers(false);
        gameStarted = true;
    }

    private IEnumerator GameOverWait()
    {
        // Wait to reveal game over after player is destroyed
        yield return new WaitForSeconds(0.6f);
        gameUI.SetActive(false);
        gameOver.SetActive(true);
        ScoreDisplay();
    }
    
    private void SetStartText()
    {
        float score = 0;
        if (PlayerPrefs.HasKey("highscore"))
        {
            score = PlayerPrefs.GetFloat("highscore");
        }

        int coins = 0;
        if (PlayerPrefs.HasKey("coins"))
        {
            coins = PlayerPrefs.GetInt("coins");
        }

        startText.text = $"Highscore\n {score} km";
        UpdateGearText();
    }
}
