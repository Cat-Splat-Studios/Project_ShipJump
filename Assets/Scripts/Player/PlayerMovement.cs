/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the player movement logic
**/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private float boostMod = 0.0f;
    private bool isBoost = false;
    private float boostTime = 0.0f;
    private float originalSpeedUp;

    [Header("Sounds")]
    public AudioClip thrustUp;
    public AudioClip thrustDown;
    public AudioSource thrusters;

    // Gears
    private int gears = 0;

    // References
    private Rigidbody rb;
    private UIDelgate ui;
    private Animator anim;
    private GeneratorManager generator;
    private new AudioManager audio;
    private PlayerManager player;

    // Misc
    [Header("Misc")]  
    public float xClamp = 3.0f;
    [SerializeField]
    private GameObject[] thrusterObjects;
    [SerializeField]
    private GameObject boostParticle;
    [SerializeField]
    private EliteAbilityIcon fuelIcon;

    private bool startGame = false;
    private float screenCenterX;

    private bool usedEmergencyFuel = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find References
        rb = GetComponent<Rigidbody>();
        ui = FindObjectOfType<UIDelgate>();
        anim = GetComponent<Animator>();
        generator = FindObjectOfType<GeneratorManager>();
        audio = FindObjectOfType<AudioManager>();
        player = GetComponent<PlayerManager>();

        screenCenterX = Screen.width * 0.5f;
        currentSpeedX = 0.0f;

        originalSpeedUp = speedUp;

        currentFuel = maxFuel;
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
                        if ((firstTouch.phase == TouchPhase.Stationary || firstTouch.phase == TouchPhase.Began) && !IsPointerOverUIObject(firstTouch.position.x, firstTouch.position.y))
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

                        if(firstTouch.phase == TouchPhase.Ended || firstTouch.phase == TouchPhase.Canceled)
                        {
                            currentSpeedX = 0;
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
                        if(!IsPointerOverUIObject(Input.mousePosition.x, Input.mousePosition.y))
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
                        player.SetThrusters(true);
                        audio.PlaySound(thrustUp);
                        thrusters.Play();
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
                            if(SwapManager.EmergencyFuelCount > 0 && !usedEmergencyFuel)
                            {
                                AddFuel(60.0f);
                                SwapManager.EmergencyFuelCount--;
                                usedEmergencyFuel = true;
                                fuelIcon.ActivateFuel();

                            }
                            else
                            {
                                outOfFuel = true;
                                audio.PlaySound(thrustDown);
                                player.SetThrusters(false);
                                thrusters.Stop();
                                FindObjectOfType<CameraFollow>().MoveCameraDown();
                                FindObjectOfType<GeneratorManager>().FallGenerate();
                                ToggleThrusters(false);
                                isLerping = true;
                            }
             
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

        if(distance < 100)
        {
            speedUp = 8;
            speedX = 7.0f;
            fuelDecrease = 6.0f;

        }
        else if (distance < 300)
        {
            speedUp = 9;
            speedX = 7.5f;
            fuelDecrease = 6.5f;
        }
        else if (distance < 500)
        {
            speedUp = 10;
            speedX = 8.0f;
            fuelDecrease = 7.0f;
        }
        else if (distance < 800)
        {
            speedUp = 11;
            speedX = 8.5f;
            fuelDecrease = 7.5f;
        }
        else if (distance < 1000)
        {
            speedUp = 12;
            fuelDecrease = 8.0f;
            speedX = 9.0f;
        }
        else
        {
            speedUp = 13;
            fuelDecrease = 8.5f;
            speedX = 10.0f;
        }

        // set ui fuel
        ui.curFuel = currentFuel / 100;

        // Animate tilts
        anim.SetInteger("tilt", (int)currentSpeedX);

        // Set finished movment each frame
        rb.velocity = new Vector3(currentSpeedX, currentSpeedUp + boostMod, 0.0f);
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

        currentSpeedX = 0;
        currentSpeedUp = 0;
        
        canMove = false;
        startGame = false;
        CheckScore();
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
        currentFuel = maxFuel;
        startYPos = transform.position.y;
        ui.resetDistance();
        gears = 0;
        transform.position = Vector3.zero;  
    }

    public void SetBoost()
    {
        boostTime = 0.0f;
        isBoost = true;
        boostMod = 3.0f;
    }

    public void ResetIdle()
    {
        currentFuel = maxFuel;
        currentSpeedX = 0.0f;
        canMove = true;
        transform.position = Vector3.zero;
        startGame = false;
    }

    private void CheckScore()
    {
        float highscore = SaveManager.instance.GetHighscore();
        if (distance > highscore)
        {
            ui.Highscore(highscore);
            GPGSUtils.instance.SubmitScore((int)distance);
            SaveManager.instance.SetHighScore((int)distance);
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

    private void StopBoost()
    {
        GetComponent<PlayerManager>().SetBoost(false);
        speedUp = originalSpeedUp;
        isBoost = false;
        boostTime = 0.0f;
        boostMod = 0.0f;
    }

    // Check if user input is over ui (dont move player if so)
    private bool IsPointerOverUIObject(float xPos, float yPos)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(xPos, yPos);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
