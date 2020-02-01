using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  
    // Speeds
    public float speedUp = 2.0f;
    public float speedX = 2.0f;
    private float currentSpeedUp;
    private float currentSpeedX;

    // Fuel
    public float maxFuel = 100.0f;
    public float fuelDecrease = 1.0f;
    private float currentFuel;


    // References
    private Rigidbody rb;
    private UIDelgate ui;

    // Misc
    private float screenCenterX;
    private bool canSwitchCam = true;

    // distance
    private float distance;
    private float startYPos;
    private bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ui = FindObjectOfType<UIDelgate>();
        ui.curDistance = 34.0f.ToString();
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
                currentSpeedUp = speedUp;
            }
            else
            {
                currentSpeedUp = 0.0f;
                currentFuel = 0.0f;
                if (canSwitchCam)
                {
                    FindObjectOfType<CameraFollow>().SwitchCameraOffset();
                    canSwitchCam = false;
                }

            }

            // Move player
            rb.velocity = new Vector3(currentSpeedX, currentSpeedUp, 0.0f);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.0f, 2.0f), transform.position.y, transform.position.z);

            // Adjust fuel
            currentFuel -= fuelDecrease * Time.deltaTime;
            ui.curFuel = currentFuel / 100;

            // Adjust Distance
            distance = Mathf.Round(transform.position.y - startYPos);
            ui.curDistance = distance.ToString();
        }  
    }

    public void Boost()
    {

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
}
