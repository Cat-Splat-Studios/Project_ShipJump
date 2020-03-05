/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the camera movement relative to the player and states.
**/

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    [Header("Offsets")]
    [SerializeField]
    private float Upoffset;
    [SerializeField]
    private float downOffest;
    [SerializeField]
    private float menuOffset;

    private float currentOffset;
    private float startOffset;
    private float targetOffset;

    private bool isLerping = false;
    private bool onMenu = false;

    float t = 0.0f;

    void Start()
    {
        currentOffset = menuOffset;
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

                if (targetOffset == menuOffset)
                    player.transform.position = new Vector3(0.0f, player.transform.position.y, player.transform.position.z);

                if (t >= 1.0f)
                {
                    currentOffset = targetOffset;

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

    public void GameStart()
    {
        startOffset = currentOffset;
        targetOffset = Upoffset;
        isLerping = true;
    }

    public void ToMenuOffset()
    {
        startOffset = Upoffset;
        targetOffset = menuOffset;
        isLerping = true;
    }

    public void ResetCamera()
    {
        currentOffset = Upoffset;
    }

}
