﻿/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: A central hub for all of the player scripts
**/

using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, ISwapper
{
    [SerializeField]
    private GameObject[] rockets;

    [SerializeField]
    private int[] viewRocketIdx;

    // references to other player components
    private PlayerMovement movement;
    private PlayerShoot shoot;
    private PlayerCollision collision;
    private PlayerDamage damage;
    private Abilities abilities;

    // shop loic
    [Header("Rocket Shop Logic")]
    [SerializeField]
    private GameObject priceTag;
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private MessageBox confirmPurchase;

    private int unlockIdx;
    private int viewIdx;

    [HideInInspector]
    public bool canPurchase { get; private set; }

    public Magnet magnet;

    // Start is called before the first frame update
    void Awake()
    {
        // Find References
        // Need to be awake because everything accesses these regardling player  
        movement = GetComponent<PlayerMovement>();
        shoot = GetComponent<PlayerShoot>();
        collision = GetComponent<PlayerCollision>();
        damage = GetComponent<PlayerDamage>();
        abilities = GetComponent<Abilities>();
    }

    void Start()
    {
        InitUnlock();
    }

    // Handles selection of ship for purchase input
    private void Update()
    {
        if(canPurchase)
        {
            if(Application.platform == RuntimePlatform.Android)
            {
                if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    GetRocketTap(raycast);
                }
            }
            else
            {
                if(Input.GetMouseButtonUp(0))
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                    GetRocketTap(raycast);
                }
            }
           
        }
    }

    public void RocketPurchaseConfirm(bool confirmed)
    {
        if(confirmed)
        {
            GearManager.instance.RemoveGears(SwapManager.rocketPrices[unlockIdx]);
            FindObjectOfType<SwapManager>().PurchaseAsset(unlockIdx, EAssetType.ROCKET);
            ToggleSwap();
        }
    }

    public void InitUnlock()
    {
        int idx = 0;

        if(PlayerPrefs.HasKey("playerIdx"))
        {
            idx = PlayerPrefs.GetInt("playerIdx");
        }
        else
        {
            idx = SwapManager.PlayerIdx;
        }


        for(int i = 0; i < viewRocketIdx.Length; ++i)
        {
            if(viewRocketIdx[i] == idx)
            {
                viewIdx = i;
            }
        }

        ToggleSwap();
    }

    // Movment
    public PlayerMovement PlayerMovement()
    {
        return movement;
    }

    public PlayerShoot PlayerShoot()
    {
        return shoot;
    }

    public PlayerCollision PlayerCollision()
    {
        return collision;
    }

    public PlayerDamage PlayerDamage()
    {
        return damage;
    }
    
    public void SetBoost(bool value)
    {
        rockets[SwapManager.PlayerIdx].GetComponent<Thrusters>().BoostToggle(value);
    }

    public ParticleSystem GetBoostParticle()
    {
        return rockets[SwapManager.PlayerIdx].GetComponent<Thrusters>().boostParticle;
    }

    public void SetThrusters(bool value)
    {
        rockets[SwapManager.PlayerIdx].GetComponent<Thrusters>().ThrusterToggle(value);
    }

    public void MagnetOn()
    {
        magnet.gameObject.SetActive(true);
    }

    public void MagnetOff()
    {
        magnet.gameObject.SetActive(false);
    }

    public void SwapIt()
    {
        foreach(GameObject rocket in rockets)
        {
            rocket.SetActive(false);
        }

        rockets[SwapManager.PlayerIdx].SetActive(true);

        Rocket stat = rockets[SwapManager.PlayerIdx].GetComponent<Rocket>();

        if(stat)
        {
            SetStats(stat.GetTopSpeed(), stat.GetFuelBurn(), stat.GetFuelIntake(), stat.GetShieldStack(), stat.GetSize() == ERocketSize.LARGE);
        }
    }

    public void TempSwap(int index)
    {
        foreach (GameObject rocket in rockets)
        {
            
            Rocket stat = rocket.GetComponent<Rocket>();
            if(stat)
                rocket.SetActive(stat.unlockIdx == index);
        }
    }

    public void ToggleRocket(bool forward)
    {
        if (forward)
        {
            if (viewIdx + 1 >= SwapManager.allRockets.Count)
            {
                viewIdx = 0;
            }
            else
            {
                ++viewIdx;
            }
        }
        else
        {
            if (viewIdx - 1 < 0)
            {
                viewIdx = SwapManager.allRockets.Count - 1;
            }
            else
            {
                --viewIdx;
            }
        }

        ToggleSwap();
    }

    public void StartGame()
    {
        StartGameMeshCheck();
        abilities.ActivateAbility();
    }

    public void StartGameMeshCheck()
    {
        if (CheckUnlock())
        {
            SwapManager.PlayerIdx = SwapManager.allRockets[unlockIdx];

        }

        SwapIt();
        TogglePurchase(false);
    }

    public void ResetPlayer()
    {
        PlayerMovement().ResetMove();
        PlayerShoot().TurnOff();
    }

    public void TogglePurchase(bool value)
    {
        canPurchase = value;
    }

    public void InitRocketStats()
    {
        // called every time when round starts
    }

    // Stat gettings


    /** Helper Methods**/

    // checks what needs to be displayed to user
    public void ToggleSwap()
    {
        TempSwap(viewRocketIdx[viewIdx]);
        unlockIdx = viewRocketIdx[viewIdx];


        if (!CheckUnlock())
        {
            priceTag.SetActive(true);
            priceText.text = SwapManager.rocketPrices[viewIdx].ToString();
            if (GearManager.instance.CheckGears(SwapManager.rocketPrices[viewIdx]))
            {
                actionText.text = "Tap rocket to purchase";
                canPurchase = true;
            }
            else
            {
                actionText.text = "Not enough Gears to purchase";
                canPurchase = false;
            }
        }
        else
        {
            priceTag.SetActive(false);
            canPurchase = false;
        }
    }

    // checks if you tap rocket, initiates purchase confirm
    private void GetRocketTap(Ray raycast)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            Debug.Log("Something Hit");
            if (raycastHit.collider.tag == "Player" && !CheckUnlock())
            {
                FindObjectOfType<AudioManager>().PressButton(1);
                confirmPurchase.SetPrompt($"Purchase Rocket\n\n {rockets[unlockIdx].name}?", SwapManager.rocketPrices[unlockIdx].ToString(), RocketPurchaseConfirm);
            }
        }
    }

    private bool CheckUnlock()
    {
        return SwapManager.PlayerUnlocks.Contains(unlockIdx);
    }

    private void SetStats(float topSpeed, float fuelEfficiency, float fuelIntake, int shieldStack, bool isLarge)
    {
        PlayerMovement().SetTopSpeed(topSpeed);
        PlayerMovement().SetFuelMods(fuelEfficiency, fuelIntake);
        PlayerDamage().SetSheildStack(shieldStack);
        PlayerCollision().SetHitBox(isLarge);
    }
}
