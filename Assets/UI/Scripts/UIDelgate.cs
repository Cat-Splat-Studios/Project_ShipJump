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

    [SerializeField]
    private float _curFuel;
    [SerializeField]
    private string _curDistance = "0";

    private bool _gameStarted = true;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
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
        fuelBar.enabled = value;
        distanceTraveled.enabled = value;
        gameplayTitle.enabled = value;

        // can enable/disable your game over stuff here by simple setting it to !value
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
        ToggleGameplayUI(true);
        gameStarted = true;
        player.GetComponent<PlayerMovement>().startGame = true;
    }

    public void GameOver()
    {
        ToggleGameplayUI(false);
        gameStarted = false;
    }

    public void Replay()
    {
        FindObjectOfType<ObjectSpawner>().ResetSpawn();
        player.SetActive(true);
        StartGame();
    }
}
