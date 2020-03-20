/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the state of the thrusters of the current rocket
**/

using UnityEngine;

public class Thrusters : MonoBehaviour
{
    [SerializeField]
    private GameObject boostThrusters;

    public ParticleSystem boostParticle;

    [SerializeField]
    private GameObject[] thrusters;

    private float boostSize;

    public void BoostToggle(bool value)
    {
       boostThrusters.SetActive(value);

       ThrusterToggle(!value);

        if (value)
            boostSize = boostParticle.startSize;
        else
            boostParticle.startSize = boostSize;
    }

    public void ThrusterToggle(bool value)
    {
        foreach(GameObject obj in thrusters)
        {
            obj.SetActive(value);
        }
    }

}
