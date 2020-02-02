using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  
    // Speeds
    public float speedUp = 2.0f;
    public float speedDown = 0.0f;
    public float speedX = 2.0f;
    private float currentSpeedUp;
    private float currentSpeedX;

    // Fuel
    public float maxFuel = 100.0f;
    public float fuelDecrease = 1.0f;
    private float currentFuel;
    private bool outOfFuel = false;


    // References
    private Rigidbody rb;
    private UIDelgate ui;

    // Misc
    public float xClamp = 3.0f;
    private float screenCenterX;
    public bool startGame = false;
    public GameObject[] thrusterObjects;
    public GameObject boostParticle;


    //shooting
    public bool canShoot;
    public GameObject projectilePrefab;
    public GameObject projectileSpawn;
    public GameObject shootButton;

    // distance
    private float distance;
    private float startYPos;
    private bool canMove = true;

    // lerp variables
    private bool isLerping = false;
    public float lerpSpeed = 2.0f;
    private float t;


    // boost
    private bool isBoost = false;
    public float boostMax = 2.0f;
    private float boostTime = 0.0f;

    private float originalSpeedUp;

    //coins
    int coins = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ui = FindObjectOfType<UIDelgate>();
        currentFuel = maxFuel;

        screenCenterX = Screen.width * 0.5f;

        currentSpeedX = 0.0f;

        originalSpeedUp = speedUp;

        shootButton.SetActive(canShoot);

        // will move to a startgame method
       
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            if (startGame)
            {
                if (Application.isMobilePlatform)
                {
                    if (Input.touchCount > 0)
                    {
                        // get the first one
                        Touch firstTouch = Input.GetTouch(0);

                        // if it began this frame
                        if (firstTouch.phase == TouchPhase.Stationary)
                        {
                            if (firstTouch.position.x > screenCenterX)
                            {
                                currentSpeedX = speedX;
                            }
                            else if (firstTouch.position.x < screenCenterX)
                            {
                                currentSpeedX = -speedX;
                            }
                        }

                        //Ray ray = Camera.main.ScreenPointToRay(firstTouch.position);
                        //RaycastHit hit;
                        //if (Physics.Raycast(ray, out hit))
                        //{
                        //    if (canShoot)
                        //    {
                        //        Instantiate(projectilePrefab, projectileSpawn.transform);
                        //        canShoot = false;
                        //    }
                        //}
                    }
                    else
                    {
                        currentSpeedX = 0;
                    }
                }

                if (Application.isEditor)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (Input.mousePosition.x > screenCenterX)
                        {
                            currentSpeedX = speedX;
                        }
                        else if (Input.mousePosition.x < screenCenterX)
                        {
                            currentSpeedX = -speedX;
                        }

                        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        //RaycastHit hit;
                        //if (Physics.Raycast(ray, out hit))
                        //{
                        //    if (canShoot)
                        //    {
                        //        Instantiate(projectilePrefab, projectileSpawn.transform);
                        //        canShoot = false;
                        //    }
                        //}
                    }
                    else
                    {
                        currentSpeedX = 0;
                    }

                }

                if (currentFuel > 0.0f)
                {
                    if (outOfFuel)
                    {
                        outOfFuel = false;
                        FindObjectOfType<CameraFollow>().SwitchCameraOffset();
                        FindObjectOfType<ObjectSpawner>().isFalling = false;
                        FindObjectOfType<ObjectSpawner>().ClearObjects();
                        ToggleThrusters(true);
                        isLerping = true;
                    }
                    currentSpeedUp = speedUp;
                }
                else
                {
                    if(!isBoost)
                    {
                        currentSpeedUp = speedDown;
                        currentFuel = 0.0f;
                        if (!outOfFuel)
                        {
                            outOfFuel = true;
                            FindObjectOfType<CameraFollow>().SwitchCameraOffset();
                            FindObjectOfType<ObjectSpawner>().isFalling = true;
                            ToggleThrusters(false);
                            isLerping = true;
                        }
                    }
                }

                if (isLerping)
                {
                    if (outOfFuel)
                    {
                        currentSpeedUp = Mathf.Lerp(speedUp, speedDown, t);
                    }
                    else
                    {
                        currentSpeedUp = Mathf.Lerp(speedDown, speedUp, t);
                    }

                    t += lerpSpeed * Time.deltaTime;

                    if (t >= 1.0f)
                    {
                        if (outOfFuel)
                        {
                            currentSpeedUp = speedDown;
                        }
                        else
                        {
                            currentSpeedUp = speedUp;
                        }
                        isLerping = false;
                        t = 0.0f;
                    }
                }

                // Adjust fuel
                currentFuel -= fuelDecrease * Time.deltaTime;
                ui.curFuel = currentFuel / 100;

                // Adjust Distance
                distance = Mathf.Round(transform.position.y - startYPos);
                ui.curDistance = distance.ToString();

            }
            else
            {
                currentSpeedUp = 5.0f;
            }

            // Move player
          
          
        }

        if(isBoost)
        {
            boostTime += Time.deltaTime;

            if (boostTime >= boostMax)
            {
                StopBoost();
            }
        }


        Debug.Log("Move");
        rb.velocity = new Vector3(currentSpeedX, currentSpeedUp, 0.0f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xClamp, xClamp), transform.position.y, transform.position.z);
    }

    public void Shoot()
    {
        Instantiate(projectilePrefab, projectileSpawn.transform);
        canShoot = false;
        shootButton.SetActive(false);
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        if(currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

    public void StopMovement()
    {
        if(isBoost)
        {
            StopBoost();
        }
        
        canMove = false;
        startGame = false;
        CheckScore();
        SetCoins();
    }
    public void StartGame()
    {
        startGame = true;
        startYPos = transform.position.y;
    }

    public void ResetMove()
    {
        canMove = true;
        currentFuel = maxFuel;
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        StartGame();
        
    }

    private void CheckScore()
    {
        if (PlayerPrefs.HasKey("highscore"))
        {
            float highscore = PlayerPrefs.GetFloat("highscore");
            if (distance > highscore)
            {
                PlayerPrefs.SetFloat("highscore", distance);
                ui.Highscore(highscore);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("highscore", distance);
            ui.Highscore(0.0f);
        }
    }

    private void ToggleThrusters(bool value)
    {
        foreach (GameObject thruster in thrusterObjects)
        {
            thruster.SetActive(value);
        }
    }

    public void SetBoost()
    {
        boostTime = 0.0f;
        isBoost = true;
        speedUp += 3.0f;
        boostParticle.SetActive(true);
    }

    public void AddCoin()
    {
        coins++;
        ui.curCoins = coins.ToString();
    }

    private void SetCoins()
    {
        PlayerPrefs.SetInt("coins", coins);
    }

    private void StopBoost()
    {
        boostParticle.SetActive(false);
        speedUp = originalSpeedUp;
        isBoost = false;
        boostTime = 0.0f;
    }
}
