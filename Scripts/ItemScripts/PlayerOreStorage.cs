using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerOreStorage : MonoBehaviour 
{
    private Dictionary<string, int> _oreValues = new Dictionary<string, int>();

    public void AddOre(string orename)
    {
        if (_oreValues.ContainsKey(orename))
        {
            _oreValues[orename]++;
        }
        else
        {
            _oreValues.Add(orename, 1);
        }

    }
}

