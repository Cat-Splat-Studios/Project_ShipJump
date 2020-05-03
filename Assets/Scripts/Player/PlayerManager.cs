/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: A central hub for all of the player scripts for ease access
**/

using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, ISwapper
{
    // All rocket meshes 
    [SerializeField]
    private GameObject[] rockets;

    // View indexs for main UI viewing
    [SerializeField]
    private int[] viewRocketIdx;

    // references to other player components
    [SerializeField]
    private PlayerInput input;
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private PlayerFuel fuel;
    [SerializeField]
    private PlayerBoost boost;
    [SerializeField]
    private PlayerShoot shoot;
    [SerializeField]
    private PlayerCollision collision;
    [SerializeField]
    private PlayerDamage damage;

    // TODO: Move into a manager object (singleton)
    [SerializeField]
    private ScoreSystem score;

    // shop logic
    [Header("Rocket Shop Logic")]
    [SerializeField]
    private GameObject priceTag;
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private MessageBox confirmPurchase;

    [Header("Rocket Select")]
    public RocketCard rocketCard;

    private int unlockIdx;
    private int viewIdx;


    private void Start()
    {
        // Create card list for viewing rocket stats
        rocketCard.InitImageList();
    }

    public void RocketPurchaseConfirm(bool confirmed)
    {
        // When a rocket is purchase, unlock it
        if(confirmed)
        {
            GearManager.instance.RemoveGears(SwapManager.rocketPrices[unlockIdx]);
            SwapManager.instance.PurchaseAsset(unlockIdx, EAssetType.ROCKET);
            ToggleSwap();
        }
    }

    public void InitUnlock()
    {
        // Check player unlock and display appropriate mesh
        int idx = SwapManager.PlayerIdx;
        

        // Double check this rocket has been unlock, default otherwise
        if (!SwapManager.PlayerUnlocks.Contains(idx))
        {
            SwapManager.PlayerIdx = 0;
            idx = 0;
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

    /** Getters for Each Player Component **/
    public PlayerInput PlayerInput()
    {
        return input;
    }

    public PlayerMovement PlayerMovement()
    {
        return movement;
    }

    public PlayerFuel Fuel()
    {
        return fuel;
    }

    public PlayerBoost Boost()
    {
        return boost;
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

    public ScoreSystem Score()
    {
        return score;
    }
    

    // Set the correct booster particle on the appropriate mesh
    public void SetBoost(bool value)
    {
        rockets[SwapManager.PlayerIdx].GetComponent<Thrusters>().BoostToggle(value);
    }

    // Get the boost particle on appropriate mesh for resizing
    public ParticleSystem GetBoostParticle()
    {
        return rockets[SwapManager.PlayerIdx].GetComponent<Thrusters>().boostParticle;
    }

    // Set the correct thruster particles on the appropriate mesh
    public void SetThrusters(bool value)
    {
        rockets[SwapManager.PlayerIdx].GetComponent<Thrusters>().ThrusterToggle(value);
    }

    // Set the appropriate rocket mesh based on player selection
    public void SwapIt()
    {
        foreach(GameObject rocket in rockets)
        {
            rocket.SetActive(false);
        }

        rockets[SwapManager.PlayerIdx].SetActive(true);

        // Set the rocket stats up for this rocket
        Rocket stat = rockets[SwapManager.PlayerIdx].GetComponent<Rocket>();

        if(stat)
        {
            SetStats(stat.GetTopSpeed(), stat.GetFuelBurn(), stat.GetFuelIntake(), stat.GetShieldStack(), stat.GetSize() == ERocketSize.LARGE);
        }
    }

    public void TempSwap(int index)
    {
        // A temporary swap for viewing on main menu
        // DO NOT SET player index when doing this
        foreach (GameObject rocket in rockets)
        {
            
            Rocket stat = rocket.GetComponent<Rocket>();
            if(stat)
                rocket.SetActive(stat.unlockIdx == index);
        }
    }

    public void ToggleRocket(bool forward)
    {
        // When clicking arrows to toggle through rockets for viewing
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
    }

    public void StartGameMeshCheck()
    {
        // Check to make sure player unlock this rocket, set rocket to this
        // If player did not unlock, rocket will switch to previously played rocker
        if (CheckUnlock())
        {
            SwapManager.PlayerIdx = SwapManager.allRockets[unlockIdx];

        }

        SwapIt();
    }

    public void BackToMenu()
    {
        // Turn rocket view card option on and switch back to viewing rockets
        PlayerInput().TogglePurchase(true);

        for(int i = 0; i < viewRocketIdx.Length; ++i)
        {
            if (SwapManager.PlayerIdx == viewRocketIdx[i])
            {
                viewIdx = i;
                unlockIdx = viewRocketIdx[i];
            }
                     
        }
        
        ToggleSwap();
    }

    public void ResetPlayer()
    {
        // Reset all player components that need resetting
        PlayerMovement().ResetMove();
        PlayerShoot().TurnOff();
        PlayerInput().TogglePurchase(false);
        Fuel().FillUp();
        Boost().BoostReset();
    }


    public void StartPlayer()
    {
        // When game starts, initialize all components that need it
        PlayerMovement().StartGame();
        PlayerInput().ToggleMove(true);
        Fuel().ToggleFuel(true);
    }

    public void DeadPlayer()
    {       
        // When player is destroyed, disable components where needed and reset
        GearManager.instance.ToggleDoubleGears(false);
        Fuel().ToggleFuel(false);
        SetThrusters(true);
        Boost().BoostOff();
        Score().CheckScore();
        PlayerInput().ToggleMove(false);
        PlayerMovement().StopMovement();
    }

    /** Helper Methods**/

    public void ToggleSwap()
    {

        // checks what needs to be displayed to user for rockets
        TempSwap(viewRocketIdx[viewIdx]);
        unlockIdx = viewRocketIdx[viewIdx];

        // Check if you unlock this, display price or indicate that you own it
        if (!CheckUnlock())
        {
            priceTag.SetActive(true);
            priceText.text = SwapManager.rocketPrices[viewIdx].ToString();
        }
        else
        {
            priceText.text = "Owned";
        }
    }

    public void GetRocketTap(Ray raycast)
    {
        // checks if you tap rocket, initiates rocket card view
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            Debug.Log("Something Hit");
            if (raycastHit.collider.tag == "Player")
            {
                FindObjectOfType<AudioManager>().PressButton(1);
                SelectRocket();
            }
        }
    }

    private void SelectRocket()
    {
        // Initiate rocket card view
        rocketCard.gameObject.SetActive(true);
        if (!CheckUnlock())
            rocketCard.InitCardBuy(viewIdx, SwapManager.rocketPrices[viewIdx]);
        else
            rocketCard.InitCardView(viewIdx);
    }

    private bool CheckUnlock()
    {
        return SwapManager.PlayerUnlocks.Contains(unlockIdx);
    }

    private void SetStats(float topSpeed, float fuelEfficiency, float fuelIntake, int shieldStack, bool isLarge)
    {
        // Set up rocket stats in the appropriate components
        PlayerMovement().SetTopSpeed(topSpeed);
        PlayerMovement().SetAcceleration(!isLarge);
        Fuel().SetFuelMods(fuelEfficiency, fuelIntake);
        PlayerDamage().SetSheildStack(shieldStack);
        PlayerCollision().SetHitBox(isLarge);

    }
}
