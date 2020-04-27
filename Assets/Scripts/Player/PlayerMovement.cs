/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the player movement input and output logic
**/

using System.Collections.Generic;
using UnityEngine;

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

    private bool outOfFuel = false;

    // Distance
    private float distance;
    private float startYPos;

    // Lerp Logic
    private bool isLerping = false;
    public float lerpSpeed = 2.0f;
    private float t;

    // Boost
    private float boostMod = 0.0f;
    private bool isBoost = false;

    [Header("Sounds")]
    public AudioClip thrustUp;
    public AudioClip thrustDown;
    public AudioSource thrusters;

    // References
    [Header("Player References")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private PlayerManager player;
    [SerializeField]
    private PlayerFuel fuel;
    [SerializeField]
    private ScoreSystem score;

    [Header("Other References")]
    [SerializeField]
    private CameraFollow cameraFollow;
    [SerializeField]
    private GeneratorManager generator;
    [SerializeField]
    private new AudioManager audio;
    [SerializeField]
    private UIDelgate ui;

    // Misc
    [Header("Misc")]  
    public float xClamp = 3.0f;

    private bool startGame = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find References
        currentSpeedX = 0.0f;      
    }

    // Update is called once per frame
    void Update()
    {
        // Update Move Logic
            if (startGame)
            {           
                // Handle Movement Dependant of current fuel
                if (fuel.HasFuel())
                {
                    if (outOfFuel)
                    {
                        // This is when you gain fuel again after out of fuel
                        outOfFuel = false;

                        // Turn on thrusters again
                        player.SetThrusters(true);
                        thrusters.Play();
                        audio.PlaySound(thrustUp);
                        
                        // Adjust camera back to bottom and turn on top object generations
                        cameraFollow.MoveCameraUp();
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
                        if (!outOfFuel)
                        {
                            // This is for when you run out of fuel
                            outOfFuel = true;

                            // Turn off thrusters
                            audio.PlaySound(thrustDown);
                            player.SetThrusters(false);
                            thrusters.Stop();

                            // Move camera up to look at incoming objects from behind
                            cameraFollow.MoveCameraDown();
                            generator.FallGenerate();
                            isLerping = true;            
                        }
                    }
                }

                // Speed adjustments based on fuel changes
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

                // Update Score
                score.ScoreUpdate(distance, speedUp);
            }
            else
            {
                currentSpeedUp = 5.0f;
            }  

        // Animate tilts
        anim.SetInteger("tilt", (int)currentSpeedX);

        // Set finished movment each frame
        rb.velocity = new Vector3(currentSpeedX, currentSpeedUp + boostMod, 0.0f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xClamp, xClamp), transform.position.y, transform.position.z);
    }

    /** Game State Adjustments **/

    public void StopMovement()
    {
        // Stop all movement as round has ended
        currentSpeedX = 0;
        currentSpeedUp = 0;

        startGame = false;
        outOfFuel = false;

        // Update the gears you have
        ui.UpdateGearText();
    }

    public void StartGame()
    {
        // Reset distance tracking and start movement
        startYPos = transform.position.y;
        startGame = true;
    }

    public void ResetMove()
    {
        // Set back to zero position and reset different variables
        transform.position = Vector3.zero;
        speedUp = initialSpeed;
        score.ResetScore();
        MovementUIReset();
        acceleration = maxAccel;
    }

    public void ResetIdle()
    {
        // When switching back to menu
        currentSpeedX = 0.0f;
        transform.position = Vector3.zero;
        startGame = false;
    }

    /** Stat adjustments **/
    // Speed from Rocket Stats
    public void SetTopSpeed(float topSpeed)
    {
        this.topSpeed = topSpeed;
        speedUp = initialSpeed;
    }

    /** X axis movement **/
    public void GoLeft()
    {
        currentSpeedX = -speedX;
    }

    public void GoRight()
    {
        currentSpeedX = speedX;
    }

    public void DontTurn()
    {
        currentSpeedX = 0;
    }

    public void SetBoostSpeedMod(float mod, bool boost)
    {
        // Adjust modification if in boost
        boostMod = mod;
        isBoost = boost;
    }

    /** MISC **/
    // get up speed to adjust projectile speed
    public float GetUpSpeed()
    {
        return speedUp;
    }

    /** Helper Methods **/
    private void MovementUIReset()
    {
        ui.curSpeed = speedUp.ToString("f2");
        ui.UpdateSpeed();
        ui.resetDistance();
    }
}
