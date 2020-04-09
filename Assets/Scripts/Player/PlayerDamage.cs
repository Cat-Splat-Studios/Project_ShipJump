/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the player damage logic
**/

using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // Destroy Effects
    [Header("Destroy Effects")]
    [SerializeField]
    private GameObject[] destroyParticlePrefab;
    [SerializeField]
    private AudioClip[] destroySounds;
    [SerializeField]
    private AudioClip shieldSound;
    [SerializeField]
    private EliteAbilityIcon shieldIcon;

    // References
    [SerializeField]
    private GameObject[] shields;

    private new AudioManager audio;
    private UIDelgate ui;
    private PlayerMovement playerMovement;

    // Helper Variables
    private bool hasShield = false;
    private int shieldCount = 0;
    private bool isDoubleShield = false;
    private int shieldStack = 1;

    private void Start()
    {
        // Find References
        audio = FindObjectOfType<AudioManager>();
        ui = FindObjectOfType<UIDelgate>();
        playerMovement = GetComponent<PlayerMovement>();     
    }

    public void GotHit(GameObject obj)
    {
        if (shieldCount > 0)
        {
            shields[shieldCount - 1].SetActive(false);
            shieldCount--;

            if (shieldCount == 0)
            {
                hasShield = false;

                if (isDoubleShield)
                    shieldIcon.DisableIt();
            }
              
            // Destroy Obstacle
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            if(obstacle)
            {
                obstacle.DestroyObstacle();              
            }    
        }
        else
        {
            // Destroyed
            DestroyShip();
        }
    }

    public void SetSheildStack(int stack)
    {
        shieldStack = stack;
    }

    public void AttatchShield()
    {
        switch (shieldCount)
        {
            case 0:
                hasShield = true;
                shields[0].SetActive(true);
                shieldCount = 1;
                break;
            case 1:
                if(shieldStack > 1)
                {
                    shields[1].SetActive(true);
                    shieldCount = 2;
                }               
                break;
        }
    }

    private void DestroyShip()
    {
        playerMovement.StopMovement();

        // Will remove this when GENERATOR system is completed
        FindObjectOfType<GeneratorManager>().StopGenerators();

        // Find random sound and particle to play
        int randomParticle = Random.Range(0, destroyParticlePrefab.Length);
        int randomSound = Random.Range(0, destroySounds.Length);

        audio.PlaySound(destroySounds[randomSound]);

        GameObject particleObj = Instantiate(destroyParticlePrefab[randomParticle],
                                new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity) as GameObject;
        Destroy(particleObj, 1.5f);

        // Game over and turn off player (this is so we can reset on replay)
        ui.GameOver();
        this.gameObject.transform.position = (new Vector3(0.0f, transform.position.y, transform.position.z));
        this.gameObject.SetActive(false);
    }
 
}
