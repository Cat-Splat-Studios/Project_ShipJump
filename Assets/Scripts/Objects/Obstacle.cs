/** 
* Authors: Matthew Douglas, Hisham Ata
* Purpose: To handle collision effects directed towards obstacles
**/

using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObject
{
    // Destroy Effects
    [Header("Destroy Effects")]
    [SerializeField]
    private GameObject[] obstacleParticlePrefab;
    [SerializeField]
    private AudioClip[] destroySounds;

    private new AudioManager audio;

    [SerializeField]
    private string poolName;

    void Start()
    {
        // Find References
        audio = FindObjectOfType<AudioManager>();
    }

    public void DestroyObstacle()
    {
        int randomParticle = Random.Range(0, obstacleParticlePrefab.Length);
        int randomSound = Random.Range(0, destroySounds.Length);
        GameObject particleObj = Instantiate(obstacleParticlePrefab[randomParticle], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1.0f), Quaternion.identity) as GameObject;
        audio.PlaySound(destroySounds[randomSound]);
        Destroy(particleObj, 1.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "projectile")
        {
            DestroyObstacle();
        }
    }

    public string GetPoolName()
    {
        return poolName;
    }
}
