/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the input logic from player and delegate to other components
**/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EMovementOptions
{
    TAP,
    DRAG
}

public class PlayerInput : MonoBehaviour
{
    private bool canMove = false;
    private bool canPurchase = true;
    private float screenCenterX;

    private EMovementOptions moveOption = EMovementOptions.TAP;
    public float touchOffset = 1.0f;

    public PlayerManager player;

    private void Start()
    {
        screenCenterX = Screen.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is in game
        if(canMove)
        {
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount > 0)
                {
                    Touch firstTouch = Input.GetTouch(0);

                    // Drag logic on mobile
                    if (moveOption == EMovementOptions.DRAG)
                    {
                        if ((firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Began) && !IsPointerOverUIObject(firstTouch.position.x, firstTouch.position.y))
                        {
                            Vector3 worldPosition;
                            Vector3 mousePos = firstTouch.position;
                            mousePos.z = 10.0f;
                            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                            if (transform.position.x < worldPosition.x - touchOffset)
                            {
                                player.PlayerMovement().GoRight();
                            }
                            else if (transform.position.x > worldPosition.x + touchOffset)
                            {
                                player.PlayerMovement().GoLeft();
                            }
                            else
                            {
                                player.PlayerMovement().DontTurn();
                            }
                        }
                    }
                    else if (moveOption == EMovementOptions.TAP)
                    {
                        // Tap logic on mobile
                        if ((firstTouch.phase == TouchPhase.Stationary || firstTouch.phase == TouchPhase.Began) && !IsPointerOverUIObject(firstTouch.position.x, firstTouch.position.y))
                        {
                            if (firstTouch.position.x > screenCenterX)
                            {
                                player.PlayerMovement().GoRight();
                            }
                            else if (firstTouch.position.x < screenCenterX)
                            {
                                player.PlayerMovement().GoLeft();
                            }
                        }
                    }

                    if (firstTouch.phase == TouchPhase.Ended || firstTouch.phase == TouchPhase.Canceled)
                    {
                        player.PlayerMovement().DontTurn();
                    }

                }
                else
                {
                    player.PlayerMovement().DontTurn();
                }
            }

            // Editor Logic
            if (Application.isEditor)
            {
                if (Input.GetMouseButton(0))
                {
                    if (moveOption == EMovementOptions.DRAG)
                    {
                        if (!IsPointerOverUIObject(Input.mousePosition.x, Input.mousePosition.y))
                        {

                            Vector3 worldPosition;

                            Vector3 mousePos = Input.mousePosition;
                            mousePos.z = 10.0f;
                            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                            if (transform.position.x < worldPosition.x - touchOffset)
                            {
                                player.PlayerMovement().GoRight();
                            }
                            else if (transform.position.x > worldPosition.x + touchOffset)
                            {
                                player.PlayerMovement().GoLeft();
                            }
                            else
                            {
                                player.PlayerMovement().DontTurn();
                            }
                        }
                    }
                    else if (moveOption == EMovementOptions.TAP)
                    {
                        if (!IsPointerOverUIObject(Input.mousePosition.x, Input.mousePosition.y))
                        {
                            if (Input.mousePosition.x > screenCenterX)
                            {
                                player.PlayerMovement().GoRight();
                            }
                            else if (Input.mousePosition.x < screenCenterX)
                            {
                                player.PlayerMovement().GoLeft();
                            }
                        }
                    }
                }
                else
                {
                    player.PlayerMovement().DontTurn();
                }
            }
        }
        else
        {
            // If on menu, enable ability to select rocket to view
            if(canPurchase)
            {
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began) && !IsPointerOverUIObject(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y))
                    {
                        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        player.GetRocketTap(raycast);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonUp(0) && !IsPointerOverUIObject(Input.mousePosition.x, Input.mousePosition.y))
                    {
                        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                        player.GetRocketTap(raycast);
                    }
                }
            }      
        }
    }

    public void ToggleMove(bool value)
    {
        canMove = value;
    }

    public void TogglePurchase(bool value)
    {
        canPurchase = value;
    }

    public void SetMoveOptions(EMovementOptions option)
    {
        moveOption = option;
    }

    private bool IsPointerOverUIObject(float xPos, float yPos)
    {
        // A check to see if player input is above UI
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(xPos, yPos);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
