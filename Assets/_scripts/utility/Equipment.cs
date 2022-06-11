using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment")]
public class Equipment : ScriptableObject
{

    public int armor;
    public int damage;
    public int speed;
    public int mobility;

    public Item weapon;
    public Item engine;
    public Item chasis;
    public Item ph_other;

    public List<Item> storage;

    public void ResetItems()
    {
        weapon = null;
        engine = null;
        chasis = null;
        ph_other = null;
        armor = 0;
        speed = 0;
        damage = 0;
        mobility = 0;
    }

    public void SetItem(Item _item)
    {
        Item oldItem = null;
        if (_item.type == ItemTypes.weapon)
        {
            oldItem = weapon;
            weapon = _item;
            
        }
        else if (_item.type == ItemTypes.engine)
        {
            oldItem = engine;
            engine = _item;
        }
        else if (_item.type == ItemTypes.chasis)
        {
            oldItem = chasis;
            chasis = _item;
        }
        else 
        {
  
            oldItem = ph_other;
            ph_other = _item;
        }

        if (oldItem != null)
        {
            //AddStats(oldItem,-1);
        }
        CalculateStats();
    }

    public void CalculateStats()
    {
        armor = 0;
        speed = 0;
        damage = 0;
        mobility = 0;
        if (weapon != null) { AddStats(weapon); }
        if (engine != null) { AddStats(engine); }
        if (chasis != null) { AddStats(chasis); }
        if (ph_other != null) { AddStats(ph_other); }
    
    }

    public void AddStats(Item _item,int _subtract=1)
    { 
        armor += (_item.armor * _subtract);
        damage += (_item.damage * _subtract);
        speed += (_item.speed * _subtract);
        mobility += (_item.mobility * _subtract);
    }




    public void AddItem(Item _item)
    {
        if (storage == null)
        { storage = new List<Item>(); }
        storage.Add(_item);
    }

}
