
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Store : MonoBehaviour
{
    public Text optionTitle;

    public ToggleGroup toggleGroup;

    public GameObject[] options;

    public GameObject currentTab;

    public void TabSwitch()
    {
        if (currentTab != null)
        {
            currentTab.SetActive(false);
        }
           
        var selectedToggle = getCurrentSelected();

        int option = selectedToggle.GetComponent<Tab>().idx;

        options[option].SetActive(true);

        currentTab = options[option];

        if (option < 4)
            currentTab.GetComponent<StoreOption>().ItemCheck();

        optionTitle.text = currentTab.name;
    }

    public void OpenStore()
    {
        currentTab.GetComponent<StoreOption>().ItemCheck();
    }

    private Toggle getCurrentSelected()
    {
        return toggleGroup.ActiveToggles().FirstOrDefault();
    }
}
