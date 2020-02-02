﻿using System.Collections;
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
    public bool startGame = true;

    // distance
    private float distance;
    private float startYPos;
    private bool canMove = true;

    // lerp variables
    private bool isLerping = false;
    public float lerpSpeed = 2.0f;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ui = FindObjectOfType<UIDelgate>();
        currentFuel = maxFuel;

        screenCenterX = Screen.width * 0.5f;

        currentSpeedX = 0.0f;

        // will move to a startgame method
        startYPos = transform.position.y;
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

                if (currentFuel > 0.0f)
                {
                    if (outOfFuel)
                    {
                        outOfFuel = false;
                        FindObjectOfType<CameraFollow>().SwitchCameraOffset();
                        FindObjectOfType<ObjectSpawner>().isPlaying = true;
                        isLerping = true;
                    }
                    currentSpeedUp = speedUp;
                }
                else
                {
                    currentSpeedUp = speedDown;
                    currentFuel = 0.0f;
                    if (!outOfFuel)
                    {
                        outOfFuel = true;
                        FindObjectOfType<CameraFollow>().SwitchCameraOffset();
                        FindObjectOfType<ObjectSpawner>().isPlaying = false;
                        isLerping = true;
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

            // Move player
            Debug.Log("Move");
            rb.velocity = new Vector3(currentSpeedX, currentSpeedUp, 0.0f);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xClamp, xClamp), transform.position.y, transform.position.z);
          
        }  
    }

    public void Boost(float speedIncrease)
    {
        StartCoroutine(BoostEffect(speedIncrease));
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
        canMove = false;
        CheckScore();
    }

    private void CheckScore()
    {
        if(PlayerPrefs.HasKey("highscore"))
        {
            float highscore = PlayerPrefs.GetFloat("highscore");
            if(distance > highscore)
            {
                PlayerPrefs.SetFloat("highScore", distance);
            }
        }
    }

    private IEnumerator BoostEffect(float increase)
    {
        speedUp += increase;
        yield return new WaitForSeconds(3.0f);
        speedUp -= increase;
    } 
}
