using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : Manager
{



    public float food;
    public float storage_food;
    public float fuel;
    public float storage_fuel;
    public float morale;
    public float pop;




    [SerializeField]
    public Dictionary<Stats, float> statMap;

    public Dictionary<Stats, float> GetStats()
    {
        if (statMap == null)
        {
            statMap = new Dictionary<Stats, float>();

            foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
            {
                statMap.Add(el, GameConstants.DefaultStatValue_Ship(el));
            }
        }
        return statMap;
    }





}
