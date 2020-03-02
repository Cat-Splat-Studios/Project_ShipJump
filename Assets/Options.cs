using System.Collections;
using System.Collections.Generic;
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
