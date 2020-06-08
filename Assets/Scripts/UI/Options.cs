/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the logic of the option menu (open and close)
**/

using UnityEngine;

public class Options : MonoBehaviour
{
    // Start is called before the first frame update
   // public Animator anim;
    public GameObject background;

    public GameObject OptionMenu;

    public void OpenOptions()
    {
        //anim.SetBool("isOpen", true);
        OptionMenu.SetActive(true); 
    }

    public void CloseOptions()
    {
        //anim.SetBool("isOpen", false);
        OptionMenu.SetActive(false);
    }
}
