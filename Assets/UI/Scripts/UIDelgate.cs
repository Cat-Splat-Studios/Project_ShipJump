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

    // Start is called before the first frame update
    void Start()
    {
        
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

    void EnableGameplayUI()
    {
        fuelBar.enabled = true;
        distanceTraveled.enabled = true;
        gameplayTitle.enabled = true;
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
}
