/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the player movement logic
**/

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Speeds
    [Header("Speeds")]
    [SerializeField]
    private float speedUp = 2.0f;
    [SerializeField]
    private float speedDown = 0.0f;
    [SerializeField]
    private float speedX = 2.0f;

    private float currentSpeedUp;
    private float currentSpeedX;

    // Fuel
    [Header("Fuel")]
    [SerializeField]
    private float maxFuel = 100.0f;
    [SerializeField]
    private float fuelDecrease = 1.0f;

    private float currentFuel;
    private bool outOfFuel = false;

    // Distance
    private float distance;
    private float startYPos;
    private bool canMove = true;

    // Lerp Logic
    private bool isLerping = false;
    public float lerpSpeed = 2.0f;
    private float t;

    // Boost
    [Header("Boost")]
    [SerializeField]
    private float boostMax = 2.0f;
    private bool isBoost = false;
    private float boostTime = 0.0f;
    private float originalSpeedUp;

    // Gears
    private int gears = 0;

    // References
    private Rigidbody rb;
    private UIDelgate ui;
    private Animator anim;
    private GeneratorManager generator;

    // Misc
    [Header("Misc")]  
    public float xClamp = 3.0f;
    [SerializeField]
    private GameObject[] thrusterObjects;
    [SerializeField]
    private GameObject boostParticle;

    private bool startGame = false;
    private float screenCenterX;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find References
        rb = GetComponent<Rigidbody>();
        ui = FindObjectOfType<UIDelgate>();
        anim = GetComponent<Animator>();
        generator = FindObjectOfType<GeneratorManager>();

        screenCenterX = Screen.width * 0.5f;
        currentSpeedX = 0.0f;

        originalSpeedUp = speedUp;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Move Logic
        if(canMove)
        {
            if (startGame)
            {
                // Check Platform
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
                    }
                    else
                    {
                        currentSpeedX = 0;
                    }

                }

                // Handle Movement Dependant of current fuel
                if (currentFuel > 0.0f)
                {
                    if (outOfFuel)
                    {
                        outOfFuel = false;
                        FindObjectOfType<CameraFollow>().MoveCameraUp();
                        generator.TopGenerate();
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
                            FindObjectOfType<CameraFollow>().MoveCameraDown();
                            FindObjectOfType<GeneratorManager>().FallGenerate();
                            ToggleThrusters(false);
                            isLerping = true;
                        }
                    }
                }

                // Speed adjustments based on fuel
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

        // Boost adjustments
        if(isBoost)
        {
            boostTime += Time.deltaTime;

            if (boostTime >= boostMax)
            {
                StopBoost();
            }
        }

        // Animate tilts
        anim.SetInteger("tilt", (int)currentSpeedX);

        // Set finished movment each frame
        rb.velocity = new Vector3(currentSpeedX, currentSpeedUp, 0.0f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xClamp, xClamp), transform.position.y, transform.position.z);
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
        currentFuel = maxFuel;
        startGame = true;
        startYPos = transform.position.y;
    }

    public void ResetMove()
    {
        canMove = true;
        gears = 0;
        transform.position = Vector3.zero;
        StartGame();
        
    }

    public void SetBoost()
    {
        boostTime = 0.0f;
        isBoost = true;
        speedUp += 3.0f;
        boostParticle.SetActive(true);
    }

    public void AddGear()
    {
        gears++;
        ui.curCoins = gears.ToString();
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

    /** Helper Methods **/

    private void ToggleThrusters(bool value)
    {
        foreach (GameObject thruster in thrusterObjects)
        {
            thruster.SetActive(value);
        }
    }

    private void SetCoins()
    {
        PlayerPrefs.SetInt("coins", gears);
    }

    private void StopBoost()
    {
        boostParticle.SetActive(false);
        speedUp = originalSpeedUp;
        isBoost = false;
        boostTime = 0.0f;
    }
}
