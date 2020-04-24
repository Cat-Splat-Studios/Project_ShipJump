/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the player movement logic
**/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EMovementOptions
{
    TAP,
    DRAG
}

public class PlayerMovement : MonoBehaviour
{
    // Speeds
    [Header("Speeds")]
    [SerializeField]
    private float maxAccel = 0.05f;
    [SerializeField]
    private float speedUp = 2.0f;
    [SerializeField]
    private float speedDown = 0.0f;
    [SerializeField]
    private float speedX = 2.0f;

    private float acceleration = 0.5f;
    private float currentSpeedUp;
    private float currentSpeedX;

    // set speeds
    private float topSpeed;
    private float initialSpeed = 5.0f;

    // Fuel
    [Header("Fuel")]
    [SerializeField]
    private float maxFuel = 100.0f;

    [SerializeField]
    private float fuelDecrease = 1.0f;
    private float fuelIntakeMod = 1.0f;

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
    private float boostLimit;
    private float boostMod = 0.0f;
    private bool isBoost = false;
    private float boostTime = 0.0f;
    private ParticleSystem currentBoostParticle;
    private float boostInitalSize;
    private int currentBoostMod;
    private float scoreMod;
    public float[] boostMods;

    [Header("Sounds")]
    public AudioClip thrustUp;
    public AudioClip thrustDown;
    public AudioSource thrusters;

    // References
    private Rigidbody rb;
    private UIDelgate ui;
    private Animator anim;
    private GeneratorManager generator;
    private new AudioManager audio;
    private PlayerManager player;
    private ScoreSystem score;

    // Misc
    [Header("Misc")]  
    public float xClamp = 3.0f;

    private bool startGame = false;
    private float screenCenterX;

    private EMovementOptions moveOption = EMovementOptions.TAP;
    public float touchOffset = 1.0f;
    
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
        score = GetComponent<ScoreSystem>();

        screenCenterX = Screen.width * 0.5f;
        currentSpeedX = 0.0f;

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

                        if(moveOption == EMovementOptions.DRAG)
                        {
                            if ((firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Began) && !IsPointerOverUIObject(firstTouch.position.x, firstTouch.position.y))
                            {
                                Vector3 worldPosition;
                                Vector3 mousePos = firstTouch.position;
                                mousePos.z = 10.0f;
                                worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                                if (transform.position.x < worldPosition.x - touchOffset)
                                {
                                    currentSpeedX = speedX;
                                }
                                else if (transform.position.x > worldPosition.x + touchOffset)
                                {
                                    currentSpeedX = -speedX;
                                }
                                else
                                {
                                    currentSpeedX = 0;
                                }
                            }
                        } 
                        else if(moveOption == EMovementOptions.TAP)
                        {
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
                        if(moveOption == EMovementOptions.DRAG)
                        {
                            if (!IsPointerOverUIObject(Input.mousePosition.x, Input.mousePosition.y))
                            {

                                Vector3 worldPosition;

                                Vector3 mousePos = Input.mousePosition;
                                mousePos.z = 10.0f;
                                worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                                if (transform.position.x < worldPosition.x - touchOffset)
                                {
                                    currentSpeedX = speedX;
                                }
                                else if (transform.position.x > worldPosition.x + touchOffset)
                                {
                                    currentSpeedX = -speedX;
                                }
                                else
                                {
                                    currentSpeedX = 0;
                                }
                            }
                           
                        }
                        else if (moveOption == EMovementOptions.TAP)
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
                            //if(SwapManager.EmergencyFuelCount > 0 && !usedEmergencyFuel)
                            //{
                            //    AddFuel(100.0f);
                            //    SwapManager.EmergencyFuelCount--;
                            //    usedEmergencyFuel = true;
                            //    fuelIcon.ActivateFuel();

                            //}

                            outOfFuel = true;
                            audio.PlaySound(thrustDown);
                            player.SetThrusters(false);
                            thrusters.Stop();
                            FindObjectOfType<CameraFollow>().MoveCameraDown();
                            FindObjectOfType<GeneratorManager>().FallGenerate();
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
                currentFuel -= (fuelDecrease) * Time.deltaTime;

                // Adjust speed
                if (speedUp < topSpeed)
                {
                    speedUp += (acceleration * Time.deltaTime);

                    if (speedUp > topSpeed)
                        speedUp = topSpeed;
                }
                else
                    speedUp = topSpeed;

                speedDown = -speedUp;

                ui.curSpeed = (speedUp + boostMod).ToString("f2");
                ui.UpdateSpeed();

                // Adjust Distance
                distance = Mathf.Round(transform.position.y - startYPos);


                // Boost adjustments
                if (isBoost)
                {
                    boostTime -= Time.deltaTime;

                    float percent = boostTime / boostLimit;
                    var main = currentBoostParticle.main;
                    main.startSize = boostInitalSize * percent;

                    if (boostTime <= 0.0f)
                    {
                        StopBoost();
                    }
                }

                score.ScoreUpdate(distance, speedUp, scoreMod);

            }
            else
            {
                currentSpeedUp = 5.0f;
            }  
        }

     
        // set ui fuel
        ui.curFuel = currentFuel / 100;

        // Animate tilts
        anim.SetInteger("tilt", (int)currentSpeedX);

        // Set finished movment each frame
        rb.velocity = new Vector3(currentSpeedX, currentSpeedUp + boostMod, 0.0f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xClamp, xClamp), transform.position.y, transform.position.z);
    }

    /** Game State Adjustments **/

    // Switch input controls
    public void SetMoveOptions(EMovementOptions option)
    {
        moveOption = option;
    }

    public void StopMovement()
    {
        if (isBoost)
        {
            StopBoost();
        }

        currentSpeedX = 0;
        currentSpeedUp = 0;

        canMove = false;
        startGame = false;
        outOfFuel = false;
        player.SetThrusters(true);
        score.CheckScore();
        //player.MagnetOff();
        GearManager.instance.ToggleDoubleGears(false);
        ui.UpdateGearText();


    }

    public void StartGame()
    {
        startYPos = transform.position.y;
        startGame = true;    
    }

    public void ResetMove()
    {
        // MovementReset();
        currentFuel = maxFuel;
        transform.position = Vector3.zero;
        canMove = true;
        speedUp = initialSpeed;
        currentBoostMod = 0;
        scoreMod = boostMods[0];
        score.ResetScore();
        MovementUIReset();
        acceleration = maxAccel;
        boostLimit = boostMax;

    }

    public void ResetIdle()
    {
        currentFuel = maxFuel;
        currentSpeedX = 0.0f;
        canMove = true;
        transform.position = Vector3.zero;
        startGame = false;
    }

    public void SetBoost()
    {
        if (currentBoostMod < 3)
        {
            currentBoostMod++;
            scoreMod = boostMods[currentBoostMod];
        }

        boostTime = boostLimit;
        boostMod = 2.0f;

        if (!isBoost)
        {
            isBoost = true;
            currentBoostParticle = player.GetBoostParticle();
            boostInitalSize = currentBoostParticle.startSize;
        }
        else
        {
            currentBoostParticle.startSize = boostInitalSize;
        }
    }

    /** Stat adjustments **/
    // Speed from Rocket Stats
    public void SetTopSpeed(float topSpeed)
    {
        this.topSpeed = topSpeed;
        speedUp = initialSpeed;
    }

    // Fuel from Rocket Stats
    public void SetFuelMods(float fuelBurn, float fuelIntake)
    {
        fuelDecrease = fuelBurn;
        fuelIntakeMod = fuelIntake;
    }

    public void AddFuel(float amount)
    {
        float fuelAdded = amount * fuelIntakeMod;
        currentFuel += fuelAdded;
        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

    public void RetroFitEngines()
    {
        acceleration = maxAccel + (maxAccel * 0.75f);
    }
    public void NitroFuel()
    {
        boostLimit = boostMax + 2.0f;
    }

    /** MISC **/
    // get up speed to adjust projectile speed
    public float GetUpSpeed()
    {
        return speedUp;
    }
    /** Helper Methods **/

    private void StopBoost()
    {
        GetComponent<PlayerManager>().SetBoost(false);
        isBoost = false;
        boostTime = 0.0f;
        boostMod = 0.0f;
        currentBoostMod = 0;
        scoreMod = boostMods[currentBoostMod];
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

    private void MovementUIReset()
    {
        ui.curSpeed = speedUp.ToString("f2");
        ui.UpdateSpeed();
        ui.resetDistance();
    }
}
