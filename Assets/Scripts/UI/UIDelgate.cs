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
    private Animator shootButton;
    [SerializeField]
    private GameObject[] numbers;
    [SerializeField]
    private Text speedText;
    [SerializeField]
    private Text scoreGameText;
    [SerializeField]
    private Text modText;

    // Game Over UI
    [Header("Game Over UI")]
    public GameObject gameOver;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text distanceText;
    [SerializeField]
    private Text coinsCollectedText;
    [SerializeField]
    private GameObject highscoreText;
    [SerializeField]
    private GameObject highDistanceText;

    //Shop UI
    [Header("Shop UI")]
    [SerializeField]
    private GameObject shopUI;
    [SerializeField]
    private Shop[] shops;
    [SerializeField]
    private GameObject[] shopTitle;

    [Header("Store UI")]
    [SerializeField]
    private GameObject storeUI;

    [Header("Authenticated Objects")]
    public Button[] onlineButtons;
    public GameObject signInButton;
    

    // Reference
    private Animator anim;
    private PlayerManager player;
    private GeneratorManager generatorManager;
    private new CameraFollow camera;

    // Property Values
    private float _curFuel;
    private string _curScore;
    private string _curDistance = "0";
    private string _curSpeed;

    // Helper Variables
    private bool _gameStarted = false;

    void Start()
    {
        // Find References
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        generatorManager = FindObjectOfType<GeneratorManager>();
        camera = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        fuelBar.fillAmount = _curFuel;

        // Allow for back button escape on the phone
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void UpdateScoreText()
    {
        distanceTraveled.text = $"{_curDistance} km";
        scoreGameText.text = $"Score: {_curScore}";
    }

    public void UpdateSpeed()
    {
        speedText.text = $"Speed: {_curSpeed}";
    }

    public void StartGame()
    {
        //gears.SetActive(false);
        StatHUD.SetActive(false);
        startUI.SetActive(false);
        gameUI.SetActive(true);
        anim.SetTrigger("Start");

        // make sure correct mesh is on player
        player.StartGame();
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
        player.gameObject.SetActive(true);
        player.ResetPlayer();

        // TODO MAKE SINGLETON
        FindObjectOfType<PoolManager>().ResetObjects();

        // set ui
        gameUI.SetActive(true);
        gameOver.SetActive(false);
        StatHUD.SetActive(false);
        ResetHighscore();
        GearManager.instance.ResetLevelGears();

        anim.SetTrigger("Start");
        StartCoroutine(StartWait());
    }

    public void Highscore()
    {
        highscoreText.SetActive(true);
    }

    public void HighDistance()
    {
        highDistanceText.SetActive(true);
    }

    public void ResetHighscore()
    {
        highscoreText.SetActive(false);
        highDistanceText.SetActive(false);
    }

    // TODO: Rework shooting
    public void ToggleShoot(bool value)
    {
        shootButton.SetBool("isUp", value);
    }

    public void OpenShop(int shopType)
    {
        startUI.SetActive(false);
        shopUI.SetActive(true);
        shopTitle[shopType].gameObject.SetActive(true);
        shops[shopType].gameObject.SetActive(true);
        shops[shopType].SetStartPos();
        shops[shopType].InitItems();
        SwapManager.instance.Preview(shops[shopType].shopType, shops[shopType].current_index);
    }

    public void CloseShop()
    {
        int count = 0;
        foreach (Shop shop in shops)
        {
            shop.gameObject.SetActive(false);
            shopTitle[count++].SetActive(false);
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
        ResetHighscore();
        camera.ToMenuOffset();
        player.gameObject.SetActive(true);
        player.PlayerMovement().ResetIdle();

        FindObjectOfType<PoolManager>().ResetObjects();

        // set ui
        startUI.SetActive(true);
        gameUI.SetActive(false);
        StatHUD.SetActive(true);
        gameOver.SetActive(false);

        player.BackToMenu();

        if (!player.PlayerMovement().thrusters.isPlaying)
            player.PlayerMovement().thrusters.Play();

        GearManager.instance.ResetLevelGears();
    }
    public void UpdateGearText()
    {
        gearText.text = $"{GearManager.instance.GetGears()}";
        
    }

    public void UpdateHighscoreText()
    {
        startText.text = $"{player.Score().GetHighscore()}";
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void HasAuthenitcated()
    {
        startUI.SetActive(true);
        StatHUD.SetActive(true);
        UpdateGearText();
        UpdateHighscoreText();

        foreach(Shop shop in shops)
        {
            shop.SetStartPos();
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

    public string curScore
    {
        get
        {
            return _curScore;
        }
        set
        {
            _curScore = value;
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

    public string curSpeed
    {
        get
        {
            return _curSpeed;
        }
        set
        {
            _curSpeed = value;
        }
    }

    public void resetDistance()
    {
        distanceTraveled.text = "0 km";
    }
    
    public void toggleOnlineButtons(bool value)
    { 

        foreach(Button btn in onlineButtons)
        {
            btn.interactable = value;
        }

        signInButton.SetActive(!value);
    }

    public void SetModText(float mod)
    {
        modText.text = $"x {mod}";
    }

    /** Helper Methods **/
    private void ScoreDisplay()
    {
        // Reveal results on game over
        scoreText.gameObject.SetActive(true);
        distanceText.gameObject.SetActive(true);
        scoreText.text = $"Final Score\n {curScore}";
        distanceText.text = $"You Traveled\n {curDistance} km";

        coinsCollectedText.text = $"Gears Collected\n {GearManager.instance.levelGears}";
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
        player.ResetPlayer();
        yield return new WaitForSeconds(2.1f);
        player.StartPlayer();
        generatorManager.TopGenerate();
        ToggleNumbers(false);
        gameStarted = true;
    }

    private IEnumerator GameOverWait()
    {
        // Wait to reveal game over after player is destroyed
        yield return new WaitForSeconds(1.0f);
        gameUI.SetActive(false);
        gameOver.SetActive(true);
        StatHUD.SetActive(true);
        ScoreDisplay();
        camera.ResetCamera();
        player.PlayerShoot().TurnOff();
        AdManager.instance.ButtonCheck();
        CloudSaving.instance.SaveGame();
    }
}
