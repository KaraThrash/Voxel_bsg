
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

        if (_type == ItemTypes.ammo)
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
        if (_item.type == ItemTypes.ammo)
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
        //an item should have 7 extries for stats
        string[] text = _line.Split(',');
        
        if (text.Length >= 7)
        {
            Item newItem = new Item();
            newItem.name = text[0].Trim();
            
            newItem.type = EnumGroups.ItemFromString(text[1].Trim());
            newItem.armor = int.Parse(text[2].Trim());
            newItem.damage = int.Parse(text[3].Trim());
            newItem.speed = int.Parse(text[4].Trim());
            newItem.mobility = int.Parse(text[5].Trim());
            newItem.subtype = int.Parse(text[6].Trim());
            // name;

            //    //0 = weapon //1 = chasis // 2 = engine // 3 = usable // 4 = ammo
            //type;

            //armor;
            //damage;
            // speed;
            //subtype;
            Debug.Log("new Item: " + newItem.ToString());

            AddToListByType(newItem);
        }
         
        return null;
    }

    public Item ALT_ParseToItem(string _line)
    {
        //an item should have 7 extries for stats
        string[] text = _line.Split(',');
        if (text.Length < 5) { return null; }

            Item newItem = new Item();

        int count = 0;

        while (count < text.Length)
        {
            string[] newstat = text[count].Split(':');

            if (newstat.Length == 2)
            {
                if (newstat[0].ToLower().Equals("name"))
                {
                    newItem.name = newstat[1];
                }
                else if(newstat[0].ToLower().Equals("type"))
                {
                    newItem.type = EnumGroups.ItemFromString(newstat[1].Trim());
                }
                else if (newstat[0].ToLower().Equals("armor"))
                {
                    newItem.armor = int.Parse(newstat[1].Trim());
                }
                else if (newstat[0].ToLower().Equals("damage"))
                {
                    newItem.damage = int.Parse(newstat[1].Trim());
                }
                else if (newstat[0].ToLower().Equals("speed"))
                {
                    newItem.speed = int.Parse(newstat[1].Trim());
                }
                else if (newstat[0].ToLower().Equals("mobility"))
                {
                    newItem.mobility = int.Parse(newstat[1].Trim());
                }

                else 
                {

                }

                Debug.Log("new Item: " + newItem.ToString());

                


            }
            count++;
        }
        AddToListByType(newItem);


        return null;
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
                masterItemList.Add(newItem.name,newItem);

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
