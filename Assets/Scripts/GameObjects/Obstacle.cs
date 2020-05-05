/** 
* Authors: Matthew Douglas, Hisham Ata
* Purpose: To handle collision effects directed towards obstacles
**/

using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObject, ISwapper
{
#pragma warning disable 0649
    // Different meshes the obstacle can be
    [Header("Meshes")]
    [SerializeField]
    private GameObject[] Meshes;

    // Effects for obstacle when interacted
    [Header("Effects")]
    [SerializeField]
    private GameObject[] obstacleParticlePrefab;
    [SerializeField]
    private AudioClip[] destroySounds;

    [SerializeField]
    private string poolName;

    private new AudioManager audio;

    public bool isRecord = false;

    void Start()
    {
        // Find References
        audio = FindObjectOfType<AudioManager>();

        SwapIt();
    }

    public void OnEnable()
    {
        SwapIt();
    }

    public void DestroyObstacle()
    {
        // randomize sound and particle to play when destroyed
        int randomParticle = Random.Range(0, obstacleParticlePrefab.Length);
        int randomSound = Random.Range(0, destroySounds.Length);

        GameObject particleObj = Instantiate(obstacleParticlePrefab[randomParticle], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1.0f), Quaternion.identity) as GameObject;
        audio.PlaySound(destroySounds[randomSound]);


        if (!isRecord)
        {           
            Pool.RemoveObject(poolName, this.gameObject);
        }
        else
            Destroy(this.gameObject);

        Destroy(particleObj, 1.5f);
    }

    public string GetPoolName()
    {
        return poolName;
    }

    public void SwapIt()
    {
        // Turn on the correct mesh of obstacle player has selected
        foreach (GameObject mesh in Meshes)
        {
            mesh.SetActive(false);
        }

        Meshes[SwapManager.ObstacleIdx].SetActive(true);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "projectile")
        {
            DestroyObstacle();
        }
    }
}
