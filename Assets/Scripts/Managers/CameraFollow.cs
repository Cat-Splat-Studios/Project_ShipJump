/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the camera movement relative to the player and states.
**/

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

    void Start()
    {
        currentOffset = Upoffset;
    }

    void Update()
    {
        if(player)
        {
            if (!isLerping)
            {
                // Follow player with correct offset (either above or below player depending on if player is moving forward or backwards)
                transform.position = new Vector3(0.0f, player.transform.position.y + currentOffset, transform.position.z);
            }
            else
            {
                // Smooth transition movement
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

    public void MoveCameraUp()
    {
        startOffset = currentOffset;
        targetOffset = Upoffset;
        isLerping = true;
    }

    public void MoveCameraDown()
    {
        startOffset = currentOffset;
        targetOffset = downOffest;
        isLerping = true;
    }

}
