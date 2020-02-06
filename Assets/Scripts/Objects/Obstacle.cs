using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject[] obstacleParticlePrefab;
    public AudioClip[] destroySounds;

    private AudioManager audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "projectile")
        {
            int randomParticle = Random.Range(0, obstacleParticlePrefab.Length);
            int randomSound = Random.Range(0, destroySounds.Length);
            GameObject particleObj = Instantiate(obstacleParticlePrefab[randomParticle], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1.0f), Quaternion.identity) as GameObject;
            audio.PlaySound(destroySounds[randomSound]);
            Destroy(particleObj, 1.5f);
            Destroy(this.gameObject);
        }
    }
}
