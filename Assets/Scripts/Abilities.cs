using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAbilityState
{
    NONE,
    GEARMAGNENT,
    NITROFUEL,
    RETROFITENGINES
}


public class Abilities : MonoBehaviour
{
    public int durationCount;
    public PlayerManager player;

    private EAbilityState currentAbility = EAbilityState.NONE;
    private int duration = 0;

    public EAbilityState getAbility()
    {
        return currentAbility;
    }

    public void SetAbility(EAbilityState ability)
    {
        currentAbility = ability;
        duration = durationCount;
    }

    public void AbilityTick()
    {
        if(duration > 0)
        {
            duration--;
            if (duration <= 0)
            {
                currentAbility = EAbilityState.NONE;
            }
        }
           
    }

    public void ActivateAbility()
    {
        if(HasAbility())
        {
            switch(currentAbility)
            {
                case EAbilityState.GEARMAGNENT:
                    player.MagnetOn();
                    break;
                case EAbilityState.NITROFUEL:
                    player.PlayerMovement().NitroFuel();
                    break;
                case EAbilityState.RETROFITENGINES:
                    player.PlayerMovement().RetroFitEngines();
                    break;
            }

            AbilityTick();
        }
    }

    private bool HasAbility()
    {
        return currentAbility != EAbilityState.NONE;
    }
}
