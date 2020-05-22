/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Any logic that must happen right before game starts
**/

using UnityEngine;

public class AppStartup : MonoBehaviour
{
    public float buffer = -0.5f;
    public static float xClamp;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        xClamp = Camera.main.aspect * Camera.main.orthographicSize - buffer;
        Debug.Log(xClamp);
    }
}
