/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Any logic that must happen right before game starts
**/

using UnityEngine;

public class AppStartup : MonoBehaviour
{

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
