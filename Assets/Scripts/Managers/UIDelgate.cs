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
    public GameObject StatHUD;
    [SerializeField]
    private Text startText;
    [SerializeField]
    private Text gearText;
    [SerializeField]
    private Text fuelText;
    [SerializeField]
    private Text shieldText;

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
    private EliteAbility[] eliteAbilities;
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
    [SerializeField]
    private GameObject[] shopTitle;

    [Header("Store UI")]
    [SerializeField]
    private GameObject storeUI;

    // Reference
    private Animator anim;
    private PlayerManager player;
    private GeneratorManager generatorManager;
    private new CameraFollow camera;

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
        player.gameObject.SetActive(true);
        player.ResetPlayer();

        // TODO MAKE SINGLETON
        FindObjectOfType<PoolManager>().ResetObjects();

        // set ui
        gameUI.SetActive(true);
        gameOver.SetActive(false);
        StatHUD.SetActive(false);
        highscore = false;
        GearManager.instance.ResetLevelGears();

        anim.SetTrigger("Start");
        StartCoroutine(StartWait());
    }

    public void Highscore(float previousScore)
    {
        highscore = true;
        highscoreText.text = $"New Personal Highscore!\n\nPrevious Score = {previousScore} km";
    }

    public void LeaderBoard()
    {
        highscoreText.text += "\n\n Added to Leaderboards.";
    }

    // TODO: Rework shooting
    public void ToggleShoot(bool value)
    {
        if (value)
        {
            shootButton.SetTrigger("up");
        }
        else
        {
            shootButton.SetTrigger("down");
        }
    }

    public void OpenShop(int shopType)
    {
        startUI.SetActive(false);
        shopUI.SetActive(true);
        shopTitle[shopType].gameObject.SetActive(true);
        shops[shopType].gameObject.SetActive(true);
        shops[shopType].SetStartPos();
        shops[shopType].InitItems();
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
        camera.ToMenuOffset();
        player.gameObject.SetActive(true);
        player.PlayerMovement().ResetIdle();

        FindObjectOfType<PoolManager>().ResetObjects();

        // set ui
        startUI.SetActive(true);
        gameUI.SetActive(false);
        StatHUD.SetActive(true);
        gameOver.SetActive(false);

        UpdateInfoText();

        GearManager.instance.ResetLevelGears();
    }

    public void UpdateInfoText()
    {
        gearText.text = $"{GearManager.instance.GetGears()}";
        startText.text = $"Highscore\n {SaveManager.instance.GetHighscore()} km";
        UpdateAbilityText();
    }

    private void UpdateAbilityText()
    {
        fuelText.text = SwapManager.EmergencyFuelCount.ToString();
        shieldText.text = SwapManager.DoubleShieldCount.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HasAuthenitcated()
    {
        startUI.SetActive(true);
        StatHUD.SetActive(true);
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

    public void resetDistance()
    {
        distanceTraveled.text = "0 km";
    }

    /** Helper Methods **/
    private void ScoreDisplay()
    {
        // Reveal results on game over
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"You Traveled\n {curDistance} km";
        highscoreText.gameObject.SetActive(highscore);
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
        player.PlayerMovement().ResetMove();
        yield return new WaitForSeconds(2.1f);
        player.PlayerMovement().StartGame();
        player.PlayerDamage().AttachDoubleShield();
        generatorManager.TopGenerate();
        ToggleNumbers(false);
        gameStarted = true;
        AbilitCheck();

    }

    private IEnumerator GameOverWait()
    {
        // Wait to reveal game over after player is destroyed
        yield return new WaitForSeconds(1.0f);
        gameUI.SetActive(false);
        gameOver.SetActive(true);
        StatHUD.SetActive(true);
        UpdateInfoText();
        ScoreDisplay();
        camera.ResetCamera();
        AdManager.instance.ButtonCheck();
        SaveManager.instance.SaveToCloud();
    }

    private void AbilitCheck()
    {
        foreach (EliteAbility ability in eliteAbilities)
        {
            ability.EnableAbility();
        }
    }
}
