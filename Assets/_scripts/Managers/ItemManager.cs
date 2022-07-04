
using System;
using System.Collections.Generic;
using UnityEngine;




public class ItemManager : Manager {

    private Dictionary<string, Item> masterItemList;

    public List<Item> weapon;
    private List<Item> chasis;
    private List<Item> engine;
    private List<Item> usable;
    private List<Item> bullet;
    private List<Item> vehicles;

    public void Start()
    {
        ReadSpreadsheet();

    }

    public Item GetItem()
    {
        if (masterItemList.Count > 0)
        { 
        
        }
        return null;
    }

    public List<Item> GetAllByType(ItemTypes _type)
    {
        //The reason this are broken down to presorted lists is to avoid querying the inventory
        //This way in the future if there are a large number of items or types of items
        // it wont create a cascading search cost
        if (_type == ItemTypes.weapon)
        {
            return WeaponList();
        }
        if (_type == ItemTypes.chasis)
        {
            return ChasisList();
        }
        if (_type == ItemTypes.engine)
        {
            return EngineList();
        }

        if (_type == ItemTypes.bullet)
        {
            return BulletList();
        }
        if (_type == ItemTypes.usable)
        {
            return UsableList();
        }
        if (_type == ItemTypes.vehicle)
        {
            return VehicleList();
        }

        return WeaponList();
    }





    public void AddToListByType(Item _item)
    {
        if (_item.type == ItemTypes.weapon)
        {
            WeaponList().Add(_item);
        }
        if (_item.type == ItemTypes.chasis)
        {
            ChasisList().Add(_item);
        }
        if (_item.type == ItemTypes.engine)
        {
            EngineList().Add(_item);
        }
        if (_item.type == ItemTypes.bullet)
        {
            BulletList().Add(_item);
        }
        if (_item.type == ItemTypes.usable)
        {
            UsableList().Add(_item);
        }
        if (_item.type == ItemTypes.vehicle)
        {
            VehicleList().Add(_item);
        }
    }

    public Item ParseToItem(string _line)
    {
        string lowerline = _line.ToLower();
        //an item should have 7 extries for stats
        string[] text = lowerline.Split(',');
        Item newItem = new Item();

        foreach (string el in text)
        {
            string[] statline = el.Split(':');
            if (statline.Length == 2)
            {
                if (statline[0].ToLower() == "name") { newItem.name = statline[1]; }
                if (statline[0].ToLower() == "type")
                { newItem.type = EnumGroups.ItemFromString(statline[1].Trim()); }

                if (statline[0].ToLower() == "armor") { newItem.armor = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "speed") { newItem.speed = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "damage") { newItem.damage = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "mobility") { newItem.mobility = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "firerate") { newItem.fireRate = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "projectileSpeed") { newItem.projectileSpeed = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "bulletsperburst") { newItem.bulletsPerBurst = int.Parse(statline[1].Trim()); }
                
                if (statline[0].ToLower() == "staminamax") { newItem.stamina_max = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "staminarecharge") { newItem.stamina_recharge = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "staminacost") { newItem.stamina_cost = int.Parse(statline[1].Trim()); }
                if (statline[0].ToLower() == "staminarechargelockout") { newItem.stamina_rechargeLockout = int.Parse(statline[1].Trim()); }




            }
        }
        Debug.Log("new Item: " + newItem.ToString());

        AddToListByType(newItem);
  
         
        return null;
    }

    
    public Item ALT_ParseToItem(string _line)
    {
        string lowerline = _line.ToLower();

        string[] text = lowerline.Split(',');
        Item newItem = new Item();

        foreach (string el in text)
        {
            string[] statline = el.Split(':');
            if (statline.Length == 2)
            {
                if (statline[0].ToLower() == "name") { newItem.name = statline[1]; }
                else if (statline[0].ToLower() == "id") { newItem.referenceID = statline[1]; }
                else if (statline[0].ToLower() == "type")
                { newItem.type = EnumGroups.ItemFromString(statline[1].Trim()); }
                else if (statline[0].ToLower() == "subtype")
                {
                    if (newItem.type == ItemTypes.bullet)
                    {
                        newItem.subtype = (int)EnumGroups.BulletTypeFromString(statline[1].Trim());
                    }

                }
                else
                {
                    newItem.GetStats()[EnumGroups.StatsFromString(statline[0])] = float.Parse(statline[1].Trim());
                   // newItem.GetStatList().Add(new StatClass(EnumGroups.StatsFromString(statline[0]), int.Parse(statline[1].Trim())));
                }
            }
        }

        if (newItem.referenceID.Length <= 1) { newItem.referenceID = newItem.name; }

        foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
        {
            newItem.GetStatList().Add(new StatClass(el, newItem.GetStats()[el]));
        }

        Debug.Log("new Item: " + newItem.ToString());



       


        return newItem;
    }



    public void ReadSpreadsheet()
    {
         string data = System.IO.File.ReadAllText("Assets/Resources/Items/readable_itemsheet.csv");
         string[] lines  = data.Split('\n');

        int count = 0;
        foreach (string el in lines)
        {
            //skip the first header line
            
            Item newItem = ALT_ParseToItem(el.Trim().ToLower());

            if (newItem != null)
            {

                MasterItems.AddNewItem( newItem);

                AddToListByType(newItem);

                //AddToListByType(newItem);
            }
            if (count > 0)
            {
            }
            
            count++;
        }

      //  float.TryParse(text[0], x);
    }


    public List<Item> WeaponList()
    {
        if (weapon == null)
        { weapon = new List<Item>(); }
        return weapon;
    }

    public List<Item> ChasisList()
    {
        if (chasis == null)
        { chasis = new List<Item>(); }
        return chasis;
    }

    public List<Item> EngineList()
    {
        if (engine == null)
        { engine = new List<Item>(); }
        return engine;
    }
    public List<Item> BulletList()
    {
        if (bullet == null)
        { bullet = new List<Item>(); }
        return bullet;
    }
    public List<Item> UsableList()
    {
        if (usable == null)
        { usable = new List<Item>(); }
        return usable;
    }

    public List<Item> VehicleList()
    {
        if (vehicles == null)
        { vehicles = new List<Item>(); }
        return vehicles;
    }

}
