/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the generators within the level
**/

using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
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

    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
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
        ToggleTopGenerators(true);
        ToggleBottomGenerators(false);

        //bottomCollector.SetActive(true);
        //topCollector.SetActive(false);
    }

    public void FallGenerate()
    {
        ToggleTopGenerators(false);
        ToggleBottomGenerators(true);

        //bottomCollector.SetActive(false);
        //topCollector.SetActive(true);
    }

    public void StopGenerators()
    {
        ToggleTopGenerators(false);
        ToggleBottomGenerators(false);

        //bottomCollector.SetActive(false);
        //topCollector.SetActive(false);
    }

    private void InitClamps()
    {
        foreach (GameObject generator in topGenerators)
        {
            Generator gen = generator.GetComponent<Generator>();

            gen.SetClamp(player.PlayerMovement().xClamp);
            generator.transform.localPosition = new Vector3(0.0f, offset, 0.0f);
        }

        topCollector.transform.localPosition = new Vector3(0.0f, offset + 5.0f, 0.0f);

        foreach (GameObject generator in bottomGeneators)
        {
            generator.GetComponent<Generator>().SetClamp(player.PlayerMovement().xClamp);
            generator.transform.localPosition = new Vector3(0.0f, -offset, 0.0f);
        }

        bottomCollector.transform.localPosition = new Vector3(0.0f, -offset - 5.0f, 0.0f);

    }

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
