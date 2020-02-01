using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDelgate : MonoBehaviour
{
    [SerializeField]
    Image fuelBar;
    [SerializeField]
    Text distanceTraveled;

    [SerializeField]
    private float _curFuel;
    [SerializeField]
    private string _curDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fuelBar.fillAmount = _curFuel;
        distanceTraveled.text = _curDistance;
    }

    public float curFuel
    {
        get
        {
            return curFuel;
        }
        set
        {
            curFuel = Mathf.Clamp(value, 0, 1);
        }
    }

    public string curDistance
    {
        get
        {
            return curDistance;
        }
        set
        {
            curDistance = value.ToString();
        }
    }
}
