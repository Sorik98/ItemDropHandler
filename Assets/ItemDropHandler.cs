using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropHandler
{
    #region private fields
    private float[] minChances;
    private float[] maxChances;

    private int minLevel;
    private int maxLevel;
    #endregion

    #region properties
    public int MinLevel
    {
        get
        {
            return minLevel;
        }
        set
        {
            minLevel = value;
        }
    }

    public int MaxLevel
    {
        get
        {
            return maxLevel;
        }
        set
        {
            maxLevel = value;
        }
    }

    public float[] MinChances
    {
        get
        {
            return minChances;
        }
        set
        {
            if (ChancesArrayIsValid(value))
                minChances = value;
        }
    }

    public float[] MaxChances
    {
        get
        {
            return maxChances;
        }
        set
        {
            if (value.Length != minChances.Length)
            {
                Debug.LogError("Max chances array must have the same length as min chances array");
                return;
            }
            if (ChancesArrayIsValid(value))
                maxChances = value;
        }
    }
    #endregion


    //example:
    // level 0 -> 100. The higher level the better chances rare loot
    // {0.025 legendary, 0.1 epic, 0.325 rare, 0.55 common} at level 0 => minChances = {0.025,0.1,0.325,0.55};
    // {0.1 legendary, 0.4 epic, 0.35 rare, 0.15 common} at level 100 => maxChances = {0.1,0.4,0.35,0.15};
    public ItemDropHandler(int minLevel, int maxLevel, float[] minChances, float[] maxChances)
    {
        if (!ChancesArrayIsValid(minChances))
            throw new ArgumentException("Min chances array is invalid");

        if (maxChances.Length != minChances.Length)
            throw new ArgumentException("Max chances array must have the same length as min chances array");

        if (!ChancesArrayIsValid(maxChances))
            throw new ArgumentException("Max chances array is invalid");

        this.minLevel = minLevel;
        this.maxLevel = maxLevel;
        this.minChances = minChances;
        this.maxChances = maxChances;
    }

    private bool ChancesArrayIsValid(float[] input)
    {
        float total = 0f;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] <= 0)
            {
                Debug.LogError("Chance value must be > 0");
                return false;
            }
            total += input[i];
        }
        if (total != 1f)
        {
            Debug.LogError("Total value of chances array must equal to 1");
            return false;
        }
        return true;
    }

    //input: the current level
    //output: is the index of the chances array, -1 for error
    public int CaculateDrop(int level)
    {
        if (!(level >= minLevel && level <= maxLevel))
        {
            Debug.LogError("Invalid level");
            return -1;
        }
        float levelFactor = (float) level / (maxLevel - minLevel);
        float[] chances = new float[minChances.Length];

        for(int i=0; i < minChances.Length; i++)
        {
            chances[i] = minChances[i] + (maxChances[i] - minChances[i]) * levelFactor;
        }

        int index = 0;
        var random = UnityEngine.Random.Range(0f, 1f);
        float th1 = 0f;
        float th2 = 0f; // two threshold
        for (int i = 0; i < chances.Length; i++)
        {
            float value = chances[i];
            th2 = th1 + value;
            if (random >= th1 && random <= th2)
            {
                index = i;
                break;
            }
            th1 += value;
        }

        return index;

    }
}
