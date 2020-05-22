/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the generators within the level
**/

using System.Collections;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Generators")]
    [SerializeField]
    private GameObject[] topGenerators;
    [SerializeField]
    private GameObject[] bottomGeneators;

    [Header("Collectors")]
    [SerializeField]
    private GameObject topCollector;
    [SerializeField]
    private GameObject bottomCollector;

    [SerializeField]
    private float offset = 15.0f;

    [SerializeField]
    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        if(player)
        {
            InitClamps();
        }

        StopGenerators();
    }

    private void Update()
    {
        // keep the manager at same offset for spawning from player
        transform.position = new Vector3(0.0f, player.gameObject.transform.position.y, 0.0f);
    }

    public void TopGenerate()
    {
        // Turn on top generators
        ToggleTopGenerators(true);
        ToggleBottomGenerators(false);
    }

    public void FallGenerate()
    {
        // Turn on bottom generators because player is falling
        ToggleTopGenerators(false);
        ToggleBottomGenerators(true);
    }

    public void StopGenerators()
    {
        // Turn off all generators
        ToggleTopGenerators(false);
        ToggleBottomGenerators(false);
    }

    private void InitClamps()
    {
        // Set the clamps on the x-axis to prevent objects spawning off screen
        foreach (GameObject generator in topGenerators)
        {
            Generator gen = generator.GetComponent<Generator>();

            gen.SetClamp(AppStartup.xClamp);
            generator.transform.localPosition = new Vector3(0.0f, offset, 0.0f);
        }

        topCollector.transform.localPosition = new Vector3(0.0f, offset + 5.0f, 0.0f);

        foreach (GameObject generator in bottomGeneators)
        {
            generator.GetComponent<Generator>().SetClamp(AppStartup.xClamp);
            generator.transform.localPosition = new Vector3(0.0f, -offset, 0.0f);
        }

        bottomCollector.transform.localPosition = new Vector3(0.0f, -offset - 5.0f, 0.0f);
    }


    /** Helper Methods **/
    private void ToggleTopGenerators(bool value)
    {
        foreach (GameObject generator in topGenerators)
        {
            generator.SetActive(value);
        }      
    }

    private void ToggleBottomGenerators(bool value)
    {
        foreach (GameObject generator in bottomGeneators)
        {
            generator.SetActive(value);
        }
    }
}
