/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the fuel logic on the player
**/

using UnityEngine;

public class PlayerFuel : MonoBehaviour
{
    [SerializeField]
    UIDelgate ui;

    public float fuelIntakeAmount = 25.0f;

    // fuel modifciations
    private float fuelDecrease = 1.0f;
    private float fuelIntakeMod = 1.0f;

    // fuel states
    private float maxFuel = 100.0f;
    private float currentFuel;

    private bool fuelOn = false;

    // Start is called before the first frame update
    void Start()
    {
        FillUp();
    }

    // Update is called once per frame
    void Update()
    {
        // Adjust fuel
        if(fuelOn && HasFuel())
        {
            currentFuel -= (fuelDecrease) * Time.deltaTime;
            if (currentFuel < 0.0f)
                currentFuel = 0.0f;
        }

        // Update UI
        ui.curFuel = currentFuel / 100;
    }

    public void SetFuelMods(float fuelBurn, float fuelIntake)
    {
        // Set fuel mods based on rocket stats
        fuelDecrease = fuelBurn;
        fuelIntakeMod = fuelIntake;
    }

    public void AddFuel(bool isFalling)
    {
        // Add fuel and cap it at max if over
        // Adjust fuel intake by modifer of rocket
        float fuelAdded;
        if (!isFalling)
            fuelAdded = fuelIntakeAmount * fuelIntakeMod;
        else
            fuelAdded = (fuelIntakeAmount * 2) * fuelIntakeMod;
        
        currentFuel += fuelAdded;
        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

    public void FillUp()
    {
        // Reset fuel
        currentFuel = maxFuel;
    }

    public void  ToggleFuel(bool value)
    {
        fuelOn = value;
    }

    public bool HasFuel()
    {
        // Check for fuel to determine if rocket needs to start falling backwards
        return currentFuel > 0.0f;
    }
}
