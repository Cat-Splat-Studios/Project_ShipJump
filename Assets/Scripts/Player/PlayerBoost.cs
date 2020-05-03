/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the boost logic on the player
**/

using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    [SerializeField]
    private PlayerManager player;

    [Header("Boost")]
    [SerializeField]
    private float boostMax = 3.0f;
    private bool isBoost = false;
    private float boostTime = 0.0f;

    private ParticleSystem currentBoostParticle;
    private float boostInitalSize;


    // Update is called once per frame
    void Update()
    {
        if (isBoost)
        {
            // Update current boost time left
            boostTime -= Time.deltaTime;

            // Adjust boost particle size depending how much time is left
            float percent = boostTime / boostMax;
            var main = currentBoostParticle.main;
            main.startSize = boostInitalSize * percent;

            // Turn off boost if time is up
            if (boostTime <= 0.0f)
            {
                BoostOff();
            }
        }
    }

    public void BoostOn()
    {
        // Set up time remaining and adjust speed
        boostTime = boostMax;
        player.PlayerMovement().SetBoostSpeedMod(2.0f, true);

        // Engage boost!
        if (!isBoost)
        {
            isBoost = true;
            currentBoostParticle = player.GetBoostParticle();
            boostInitalSize = currentBoostParticle.startSize;
        }
        else
        {
            currentBoostParticle.startSize = boostInitalSize;
        }

    }

    public void BoostReset()
    {
        // Resets boost on replay (DONT CHANGE PARTICLE SIZE HERE)
        isBoost = false;
        boostTime = 0.0f;
    }

    public void BoostOff()
    {
        // Turn off boost (almost same as reset, except change particle size back to original size)
        player.SetBoost(false);
        player.PlayerMovement().SetBoostSpeedMod(0.0f, false);
        isBoost = false;
        boostTime = 0.0f;
    }
}
