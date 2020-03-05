/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the logic of the option menu (open and close)
**/

using UnityEngine;

public class Options : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;

    public void OpenOptions()
    {
        anim.SetTrigger("open");
    }

    public void CloseOptions()
    {
        anim.SetTrigger("close");
    }
}
