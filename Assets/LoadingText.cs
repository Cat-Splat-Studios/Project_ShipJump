using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public Text loadText;

    public float timeDuration = 0.5f;

    private float currenttime = 0.0f;

    public string[] dots;

    public string[] statements;

    private string currentStatement;

    private int dotIdx = 0;
    private int statementIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetStatement();
    }

    // Update is called once per frame
    void Update()
    {
        currenttime += Time.deltaTime;

        if (currenttime >= timeDuration)
        {
            Tick();
            currenttime = 0.0f;
        }
    }

    private void Tick()
    {
        dotIdx++;

        if (dotIdx >= dots.Length)
        {
            dotIdx = 0;
            SetStatement();
        }
        else
        {
            loadText.text = currentStatement + dots[dotIdx];
        }

        
    }

    private void SetStatement()
    {
        int rand = Random.Range(0, statements.Length);
        while(rand == statementIdx)
        {
            rand = Random.Range(0, statements.Length);
        }
        statementIdx = rand;
        currentStatement = statements[rand];
        loadText.text = currentStatement;
    }
}
