using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAbilityType
{
    EMERGENCYFUEL,
    DOUBLESHIELDS
}

public class EliteAbility : MonoBehaviour
{
    [SerializeField]
    private EAbilityType abilityType;

    private Animator anim;
    private PlayerManager player;

    private bool canUse = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerManager>();

      
    }

    public void UseAbiliy()
    {
        if(canUse)
        {
            switch (abilityType)
            {
                case EAbilityType.EMERGENCYFUEL:
                    player.PlayerMovement().AddFuel(60);
                    SwapManager.EmergencyFuelCount--;
                    break;
                case EAbilityType.DOUBLESHIELDS:
                    player.PlayerDamage().AttachDoubleShield();
                    SwapManager.DoubleShieldCount--;
                    break;
            }

            anim.SetTrigger("down");
            canUse = false;
        }
        
    }

    public void EnableAbility()
    {

        // check if you have any of those ability
        int count = 0;
        switch (abilityType)
        {
            case EAbilityType.EMERGENCYFUEL:
                count = SwapManager.EmergencyFuelCount;
                break;
            case EAbilityType.DOUBLESHIELDS:
                count = SwapManager.DoubleShieldCount;
                break;
        }

        if (count > 0)
        {
            canUse = true;
            anim.SetTrigger("up");
        }
        //show if there is some   
    }
}
