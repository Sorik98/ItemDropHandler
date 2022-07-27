using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public int runTime = 1000;
    public int level = 50;
    public float[] minChances = { 0.025f, 0.1f, 0.325f, 0.55f };
    public float[] maxChances = { 0.1f, 0.4f, 0.35f, 0.15f };
    public int minLevel = 0;
    public int maxLevel = 100;
    private ItemDropHandler dropHandler;
    // Start is called before the first frame update
    void Start()
    {
        dropHandler = new ItemDropHandler(minLevel, maxLevel, minChances, maxChances);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) Calculate();
    }
    private void Calculate()
    {
        dropHandler.MinLevel = minLevel;
        dropHandler.MaxLevel = maxLevel;
        dropHandler.MinChances = minChances;
        dropHandler.MaxChances = maxChances;

        float[] result = new float[minChances.Length];
        for(int i=0; i<runTime; i++)
        {
            int index = dropHandler.CaculateDrop(level);
            if (index == -1) throw new System.Exception("Error");
            result[index]++;
        }
        var str = "";
        for(int i=0; i<result.Length; i++)
        {
            str += i + ": " + result[i] / runTime + " % ; ";
        }
        Debug.Log(str);
    }

}
