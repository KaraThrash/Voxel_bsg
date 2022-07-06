

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class StatClass
{
    public Stats stat;
    public float value;
    public StatClass(Stats _stat, float _value)
    {
        stat = _stat;
        value = _value;
    }
}

[System.Serializable]
public class Item 
{

    public string name;
    public string referenceID = "";

    //0 = weapon //1 = chasis // 2 = engine // 3 = usable // 4 = ammo 
    //6 vehicle //7 resource
    public ItemTypes type;

    public int armor = 0;
    public int damage = 0;
    public int speed = 0;

    public int mobility = 0;
    [Min(1)]
    public int fireRate = 0; // bullets per second -> actual fire rate would be this stat / 60 
    public int projectileSpeed = 0;
    public int bulletsPerBurst = 0;
    [Min(1)]
    public int bulletlifetime = 0;

    public int stamina_max = 0; //additive: two items of value 1 gives the ship a max stamina of 2
    public int stamina_recharge = 0;
    public int stamina_cost = 0;

    public int stamina_rechargeLockout = 0;


    public int backpack_slots;
    public int pointValue; // buy/sell value

    public int subtype;

    private int count;

    public string stats;

    [SerializeField]
    public Dictionary<Stats,float> statMap;

    public List<StatClass> statList;

    public Dictionary<Stats, float> GetStats()
    {
        if (statMap == null)
        {
            statMap = new Dictionary<Stats, float>();

            foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
            {
                statMap.Add(el, 0);
            }
        }
        return statMap;
    }

    public List<StatClass> GetStatList()
    {
        if (statList == null)
        {
            statList = new List<StatClass>();

           
        }
        return statList;
    }


    public int GetCount()
    {
        return count;
    }
    public void SetCount(int value)
    {
        count = value;
    }

    public void AddCount(int value)
    {
        count += value;
    }

    public override string ToString()
    {
        string newString = "";
        string secondaryString = "";

        newString += " <" + name + ">   ";
        newString += " [" + referenceID + "]   ";
        newString += '\n';
        newString += type;

        foreach (Stats el in GetStats().Keys)
        {
            if (GetStats()[el] != 0)
            {
                if (el == Stats.armor || el == Stats.speed || el == Stats.damage)
                {
                    //newString += '\n';
                    newString += ' ';
                    newString += el.ToString() + " : " + GetStats()[el];
                }
                else 
                {
                    secondaryString += ' ';
                    secondaryString += el.ToString() + " : " + GetStats()[el];
                }
            }
            
            
        }





        return newString + '\n' + secondaryString;
    }



}
