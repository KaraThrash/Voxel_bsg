

using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatClass
{
    public Stats stat;
    public int value;
    public StatClass(Stats _stat, int _value)
    {
        stat = _stat;
        value = _value;
    }
}

[System.Serializable]
public class Item 
{

    public string name;

    //0 = weapon //1 = chasis // 2 = engine // 3 = usable // 4 = ammo 
    //6 vehicle //7 resource
    public ItemTypes type;

    public int armor;
    public int damage;
    public int speed;

    public int mobility;

    public int fireRate; // bullets per second -> actual fire rate would be this stat / 60 
    public int projectileSpeed;
    public int bulletsPerBurst;
    
    public int stamina_max; //additive: two items of value 1 gives the ship a max stamina of 2
    public int stamina_recharge;
    public int stamina_cost;

    public int stamina_rechargeLockout;


    public int backpack_slots;
    public int pointValue; // buy/sell value

    public int subtype;

    private int count;

    public string stats;

    [SerializeField]
    public Dictionary<Stats,int> statMap;
    public List<StatClass> statList;

    public Dictionary<Stats, int> GetStats()
    {
        if (statMap == null)
        {
            statMap = new Dictionary<Stats, int>();

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

            foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
            {
                statList.Add(new StatClass(el,0));
            }
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
