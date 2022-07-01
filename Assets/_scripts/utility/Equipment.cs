using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment")]

public class Equipment : ScriptableObject
{

    public int armor = 0;
    public int damage = 0;
    public int speed = 0;

    public int mobility = 0;

    public int fireRate = 0;
    public int projectileSpeed = 0;
    public int bulletsPerBurst = 0;


    public int stamina_max = 0; //additive: two items of value 1 gives the ship a max stamina of 2
    public int stamina_recharge = 0;
    public int stamina_cost = 0;

    public int stamina_rechargeLockout = 0;

    public Item weapon;
    public Item engine;
    public Item chasis;

    public Item ph_other;

    public List<Item> bullets;
    public int bookmark_BulletList;

    public List<Item> consumables;
    private int bookmark_ConsumableList;
    public List<Item> storage;

    [SerializeField]
    public Dictionary<Stats, int> statMap;

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
                statList.Add(new StatClass(el, 0));
            }
        }
        return statList;
    }



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

        consumables = new List<Item>();
        bullets = new List<Item>();
        storage = new List<Item>();
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
        else if (_item.type == ItemTypes.bullet)
        {
            GetBulletList().Add(_item);
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
        if (statMap != null)
        {
            foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
            {
                if (GetStats().ContainsKey(el))
                {
                    GetStats()[el] = 1;


                }

            }

        }

        if (GetStatList() != null)
        {
           // GetStatList().Clear();

        }


        foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
        {
            if (GetStats().ContainsKey(el) )
            {
                GetStatList().Add(new StatClass(el,GetStats()[el]));


            }

        }




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
        foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
        {
            if (GetStats().ContainsKey(el) && _item.GetStats().ContainsKey(el))
            {
                GetStats()[el] = GetStats()[el] + _item.GetStats()[el];
            }

            foreach (StatClass jay in GetStatList())
            {
                if (jay.stat == el && _item.GetStats().ContainsKey(el))
                {
                    jay.value = jay.value + _item.GetStats()[el];
                }

               


            }

        }

        //armor += (_item.armor * _subtract);
        //damage += (_item.damage * _subtract);
        //speed += (_item.speed * _subtract);
        //mobility += (_item.mobility * _subtract);
    }




    public void AddItem(Item _item)
    {
        if (storage == null)
        { storage = new List<Item>(); }
        storage.Add(_item);
    }





    public void MoveBulletListBookmark(int _dir = 1)
    {
        Debug.Log(_dir);

        if (GetBulletList().Count == 0) { return; }

        bookmark_BulletList += _dir;
        if (bookmark_BulletList < 0) { bookmark_BulletList = GetBulletList().Count - 1; }
        if (bookmark_BulletList >= GetBulletList().Count) { bookmark_BulletList = 0; }


        
    }

    public void MoveConsumableListBookmark(int _dir = 1)
    {
        if (GetConsumableList().Count == 0) { return; }


        bookmark_ConsumableList += _dir;
          if (bookmark_ConsumableList < 0) { bookmark_ConsumableList = GetConsumableList().Count - 1; }
        if (bookmark_ConsumableList >= GetConsumableList().Count) { bookmark_ConsumableList = 0; }
    }

    public Item GetBullet()
    {
        if (bullets == null || bullets.Count == 0)
        { return null; }

        Debug.Log(bullets[bookmark_BulletList].name);
        return bullets[bookmark_BulletList];
    }

    public Item GetConsumable()
    {
        if (consumables == null || consumables.Count == 0)
        { return null; }
        return consumables[bookmark_ConsumableList];
    }



    public List<Item> GetConsumableList()
    {
        if (consumables == null)
        { consumables = new List<Item>(); }
        return consumables;
    }
    public List<Item> GetBulletList()
    {
        if (bullets == null)
        { bullets = new List<Item>(); }
        return bullets;
    }
    public List<Item> GetStorageList()
    {
        if (storage == null)
        { storage = new List<Item>(); }
        return storage;
    }

}
