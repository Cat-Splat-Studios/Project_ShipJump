/** 
* Authors: Matthew Douglas, Hisham Ata
* Purpose: To handle collision effects directed towards obstacles
**/

using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObject, ISwapper
{
    [Header("Meshes")]
    [SerializeField]
    private GameObject[] Meshes;
    // Destroy Effects
    [Header("Destroy Effects")]
    [SerializeField]
    private GameObject[] obstacleParticlePrefab;
    [SerializeField]
    private AudioClip[] destroySounds;

    private new AudioManager audio;

    [SerializeField]
    private string poolName;

    private int meshIdx = 0;

    void Start()
    {
        // Find References
        audio = FindObjectOfType<AudioManager>();

        SwapIt();
    }

    public void DestroyObstacle()
    {
        int randomParticle = Random.Range(0, obstacleParticlePrefab.Length);
        int randomSound = Random.Range(0, destroySounds.Length);
        GameObject particleObj = Instantiate(obstacleParticlePrefab[randomParticle], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1.0f), Quaternion.identity) as GameObject;
        audio.PlaySound(destroySounds[randomSound]);
        Destroy(particleObj, 1.5f);
        Pool.RemoveObject(poolName, this.gameObject);
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

    public void SwapIt()
    {
        foreach(GameObject mesh in Meshes)
        {
            mesh.SetActive(false);
        }

        int idx = 0;

        if(PlayerPrefs.HasKey("obstacleIdx"))
        {
            idx = PlayerPrefs.GetInt("obstacleIdx");
        }
        else
        {
            idx = SwapManager.ObstacleIdx;
        }
        Meshes[idx].SetActive(true);

    }
}
