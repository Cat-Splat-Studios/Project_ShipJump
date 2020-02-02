using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player;
    public float Upoffset;
    public float downOffest;

    private float currentOffset;
    private float startOffset;
    private float targetOffset;

    private bool isLerping = false;

    float t = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        currentOffset = Upoffset;
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
        {
            if (!isLerping)
            {
                transform.position = new Vector3(0.0f, player.transform.position.y + currentOffset, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(0.0f, Mathf.Lerp(player.transform.position.y + startOffset,
                    player.transform.position.y + targetOffset, t), transform.position.z);

                t += 1.5f * Time.deltaTime;


                if (t >= 1.0f)
                {
                    if (targetOffset == Upoffset)
                    {

                        currentOffset = Upoffset;


                    }
                    else if (targetOffset == downOffest)
                    {

                        currentOffset = downOffest;

                    }

                    isLerping = false;
                    t = 0.0f;
                }

            }
        }
      
    }

    public void SwitchCameraOffset()
    {
        startOffset = currentOffset;
        if(currentOffset == Upoffset)
        {
            targetOffset = downOffest;
        }
        else if (currentOffset == downOffest)
        {
            targetOffset = Upoffset;
        }

        isLerping = true;
    }
}
