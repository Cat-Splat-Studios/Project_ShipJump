/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the state of the thrusters of the current rocket
**/

using UnityEngine;

public class Thrusters : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject boostThrusters;

    // Reference to Boost particle for manipulation of size
    // Public to access system
    public ParticleSystem boostParticle;

    [SerializeField]
    private GameObject[] thrusters;

    private float boostSize;

    public void BoostToggle(bool value)
    {
        // Turn on Boost thrusters
       boostThrusters.SetActive(value);

       ThrusterToggle(!value);

        if (value)
            boostSize = boostParticle.startSize;
        else
            boostParticle.startSize = boostSize;
    }

    public void ThrusterToggle(bool value)
    {
        // Turn normal thrusters on or off
        foreach(GameObject obj in thrusters)
        {
            obj.SetActive(value);
        }
    }

}
