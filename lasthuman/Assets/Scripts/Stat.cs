using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField]
    private BarScript bar;

    [SerializeField]
    private float maxValue;

    [SerializeField]
    private float currentValue;

    public float CurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            this.currentValue = value;
            // bar value need to be equal to currentValue
            bar.Value = currentValue;
        }
    }

    // for upgrading for example armour? 
    public float MaxValue
    {
        get
        {
            return maxValue;
        }

        set
        {   
            this.maxValue = value;
            bar.MaxValue = maxValue;
        }
    }
}
