using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool hasFuel = true;

    public float speedUp = 2.0f;
    public float speedX = 2.0f;


    public float MaxFuel = 100.0f;
    public float currentFuel;
    public float fuelDecrease = 1.0f;

    private Rigidbody rb;

    private float currentSpeedUp;
    private float currentSpeedX;

    private float screenCenterX;

    private bool canSwitchCam = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentFuel = MaxFuel;

        screenCenterX = Screen.width * 0.5f;

        currentSpeedX = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.isMobilePlatform)
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
      

        if (hasFuel && currentFuel > 0.0f)
        {
            currentSpeedUp = speedUp;
        }
        else
        {
            currentSpeedUp = 0.0f;
            currentFuel = 0.0f;
            if(canSwitchCam)
            {
                FindObjectOfType<CameraFollow>().SwitchCameraOffset();
                canSwitchCam = false;
            }
            
        }

        currentFuel -= fuelDecrease * Time.deltaTime;
        rb.velocity = new Vector3(currentSpeedX, currentSpeedUp, 0.0f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.0f, 2.0f), transform.position.y, transform.position.z);
    }

    private void MoveLeft()
    {

    }

    private void MoveRight()
    {

    }
}
