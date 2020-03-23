/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: A central hub for all of the player scripts
**/

using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, ISwapper
{
    [SerializeField]
    private GameObject[] rockets;

    // references to other player components
    private PlayerMovement movement;
    private PlayerShoot shoot;
    private PlayerCollision collision;
    private PlayerDamage damage;

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

    [HideInInspector]
    public bool canPurchase { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        // Find References
        // Need to be awake because everything accesses these regardling player  
        movement = GetComponent<PlayerMovement>();
        shoot = GetComponent<PlayerShoot>();
        collision = GetComponent<PlayerCollision>();
        damage = GetComponent<PlayerDamage>();
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

        unlockIdx = SwapManager.allRockets.IndexOf(idx);
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

    public void SwapIt()
    {
        foreach(GameObject rocket in rockets)
        {
            rocket.SetActive(false);
        }

        rockets[SwapManager.PlayerIdx].SetActive(true);
    }

    public void TempSwap(int index)
    {
        foreach (GameObject rocket in rockets)
        {
            rocket.SetActive(false);
        }

        rockets[index].SetActive(true);
    }

    public void ToggleRocket(bool forward)
    {
        if (forward)
        {
            if (unlockIdx + 1 >= SwapManager.allRockets.Count)
            {
                unlockIdx = 0;
            }
            else
            {
                ++unlockIdx;
            }
        }
        else
        {
            if (unlockIdx - 1 < 0)
            {
                unlockIdx = SwapManager.allRockets.Count - 1;
            }
            else
            {
                --unlockIdx;
            }
        }

        ToggleSwap();
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


    /** Helper Methods**/

    // checks what needs to be displayed to user
    public void ToggleSwap()
    {
        TempSwap(unlockIdx);

        if (!CheckUnlock())
        {
            priceTag.SetActive(true);
            priceText.text = SwapManager.rocketPrices[unlockIdx].ToString();
            if (GearManager.instance.CheckGears(SwapManager.rocketPrices[unlockIdx]))
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
}
