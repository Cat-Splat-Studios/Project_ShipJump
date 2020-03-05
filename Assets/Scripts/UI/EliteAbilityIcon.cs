using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EliteAbilityIcon : MonoBehaviour
{
    public new Animator anim;

    public EAbilityType type;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        CheckIt();
    }

    public void ActivateFuel()
    {
        anim.SetTrigger("activate");
        anim.SetBool("canUse", false);
        isFuel(true);    
    }

    public void ActivateShield()
    {
        anim.SetTrigger("activate");
        isFuel(false);
    }

    public void DisableIt()
    {
        anim.SetBool("canUse", false);
        anim.SetTrigger("disable");
    }

    public void Idle()
    {
        anim.SetBool("canUse", true);
        anim.SetTrigger("idle");
    }

    private void isFuel(bool value)
    {
        anim.SetBool("isFuel", value);
    }

    public void CheckIt()
    {
        switch (type)
        {
            case EAbilityType.DOUBLESHIELDS:
                if(SwapManager.DoubleShieldCount > 0)
                {
                    Idle();
                }
                else
                {
                    DisableIt();
                }
                break;
            case EAbilityType.EMERGENCYFUEL:
                if (SwapManager.EmergencyFuelCount > 0)
                {
                    Idle();
                }
                else
                {
                    DisableIt();
                }
                break;
        }
    }
}
