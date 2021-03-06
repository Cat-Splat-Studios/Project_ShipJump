﻿/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Carousel Container to hold the shop items for particular categories     
**/

using UnityEngine;

public class Shop : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Item References")]
    public EAssetType shopType;
    [SerializeField]
    private RectTransform[] shopItems;
    [SerializeField]
    private ItemInfo[] itemInfos;

    [Header("Control Options")]
    [SerializeField]
    private RectTransform view_window;
    [SerializeField]
    private float image_gap = 30;
    [SerializeField]
    private int swipeThrustHold = 30;
    [SerializeField]
    public int current_index;

    [Header("Audio")]
    [SerializeField]
    private new AudioManager audio;
    [SerializeField]
    private AudioClip clip;

    private bool canSwipe;
    private float image_width;
    private float lerpTimer;
    private float lerpPosition;
    private float mousePositionStartX;
    private float mousePositionEndX;
    private float dragAmount;
    private float screenPosition;
    private float lastScreenPosition;

    // Start is called before the first frame update
    void Start()
    {
        SetStartPos();
    }

    // finds the current selected item to focus on
    public void SetStartPos()
    {
        // set the first thing opening up to 
        // i do not think this works, need to adjust
        switch (shopType)
        {
            case EAssetType.BACKGROUND:
                view_window = shopItems[SwapManager.BackgroundIdx];
                break;
            case EAssetType.MUSIC:
                view_window = shopItems[SwapManager.MusicIdx];
                break;
            case EAssetType.OBSTACLE:
                view_window = shopItems[SwapManager.ObstacleIdx];
                break;
        }
   
        image_width = view_window.rect.width;
        for (int i = 1; i < shopItems.Length; i++)
        {
            shopItems[i].anchoredPosition = new Vector2(((image_width + image_gap) * i), 0);
        }
    }


    void Update()
    {
        lerpTimer = lerpTimer + Time.deltaTime;

        if (lerpTimer < 0.333f)
        {
            screenPosition = Mathf.Lerp(lastScreenPosition, lerpPosition * -1, lerpTimer * 3);
            lastScreenPosition = screenPosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            canSwipe = true;
            mousePositionStartX = Input.mousePosition.x;
        }


        if (Input.GetMouseButton(0))
        {
            if (canSwipe)
            {
                mousePositionEndX = Input.mousePosition.x;
                dragAmount = mousePositionEndX - mousePositionStartX;
                screenPosition = lastScreenPosition + dragAmount;
            }
        }

        if (Mathf.Abs(dragAmount) > swipeThrustHold && canSwipe)
        {
            canSwipe = false;
            lastScreenPosition = screenPosition;
            if (current_index < shopItems.Length)
                OnSwipeComplete();
            else if (current_index == shopItems.Length && dragAmount < 0)
                lerpTimer = 0;
            else if (current_index == shopItems.Length && dragAmount > 0)
                OnSwipeComplete();
        }

        for (int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].anchoredPosition = new Vector2(screenPosition + ((image_width + image_gap) * i), 0);
        }
    }

    void OnSwipeComplete()
    {
        lastScreenPosition = screenPosition;

        if (dragAmount > 0)
        {
            if (dragAmount >= swipeThrustHold)
            {
                if (current_index == 0)
                {
                    lerpTimer = 0; lerpPosition = 0;
                }
                else
                {
                    current_index--;
                    lerpTimer = 0;
                    if (current_index < 0)
                        current_index = 0;
                    lerpPosition = (image_width + image_gap) * current_index;
                }
            }
            else
            {
                lerpTimer = 0;
            }
        }
        else if (dragAmount < 0)
        {
            if (Mathf.Abs(dragAmount) >= swipeThrustHold)
            {
                if (current_index == shopItems.Length - 1)
                {
                    lerpTimer = 0;
                    lerpPosition = (image_width + image_gap) * current_index;
                }
                else
                {
                    lerpTimer = 0;
                    current_index++;
                    lerpPosition = (image_width + image_gap) * current_index;
                }
            }
            else
            {
                lerpTimer = 0;
            }
        }
        dragAmount = 0;

        // Previews the item selected
        SwapManager.instance.Preview(shopType, current_index);
        audio.PlaySound(clip);
    }

    public void GoToIndex(int value)
    {
        current_index = value;
        lerpTimer = 0;
        lerpPosition = (image_width + image_gap) * current_index;
        screenPosition = lerpPosition * -1;
        lastScreenPosition = screenPosition;
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].anchoredPosition = new Vector2(screenPosition + ((image_width + image_gap) * i), 0);
        }
    }

    public void GoToIndexSmooth(int value)
    {
        current_index = value;
        lerpTimer = 0;
        lerpPosition = (image_width + image_gap) * current_index;
    }

    public void InitItems()
    {
        // Set the items in the shop
        foreach (ItemInfo item in itemInfos)
        {
            item.SetPurchase();
        }
    }

}
