
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public string name;

    //0 = weapon //1 = chasis // 2 = engine // 3 = usable // 4 = ammo
    public int type;

    public int armor;
    public int damage;
    public int speed;
    public int mobility;
    public int subtype;



    private int count;
    public string stats;


    public int GetCount ( )
    {
        return count;
    }
    public void SetCount (int value)
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
        newString += (ItemTypes)type;
        newString += " <" + name + "> ";
        newString += " :armor: " + armor;
        newString += " :damage: " + damage;
        newString += " :speed: " + speed;
        newString += " :mobility: " + mobility;
        return newString;
    }

}


public class ItemManager : MonoBehaviour {

    private Dictionary<string, Item> masterItemList;

    private List<Item> weapon;
    private List<Item> chasis;
    private List<Item> engine;
    private List<Item> usable;
    private List<Item> ammo;

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
        List<Item> newList = new List<Item>();

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
            return AmmoList();
        }
        if (_type == ItemTypes.usable)
        {
            return UsableList();
        }


        return null;
    }





    public void AddToListByType(Item _item)
    {
        if (_item.type == (int)ItemTypes.weapon)
        {
            WeaponList().Add(_item);
        }
        if (_item.type == (int)ItemTypes.chasis)
        {
            ChasisList().Add(_item);
        }
        if (_item.type == (int)ItemTypes.engine)
        {
            EngineList().Add(_item);
        }
        if (_item.type == (int)ItemTypes.ammo)
        {
            AmmoList().Add(_item);
        }
        if (_item.type == (int)ItemTypes.usable)
        {
            UsableList().Add(_item);
        }
    }

    public Item ParseToItem(string _line)
    {
        //an item should have 7 extries for stats
        string[] text = _line.Split(',');
        Debug.Log(_line);
        if (text.Length >= 7)
        {
            Item newItem = new Item();
            newItem.name = text[0];
            
            newItem.type = (int)EnumGroups.ItemFromString(text[1]);
            newItem.armor = int.Parse(text[2]);
            newItem.damage = int.Parse(text[3]);
            newItem.speed = int.Parse(text[4]);
            newItem.mobility = int.Parse(text[5]);
            newItem.subtype = int.Parse(text[6]);
            // name;

            //    //0 = weapon //1 = chasis // 2 = engine // 3 = usable // 4 = ammo
            //type;

            //armor;
            //damage;
            // speed;
            //subtype;

        }

        return null;
    }


    public void ReadSpreadsheet()
    {
         string data = System.IO.File.ReadAllText("Assets/Resources/Items/MasterItemSheetcsv.csv");
         string[] lines  = data.Split("\n"[0]);

        int count = 0;
        foreach (string el in lines)
        {
            //skip the first header line
            if (count > 0)
            {
                Item newItem = ParseToItem(el);
                if (newItem != null)
                {
                    masterItemList.Add(newItem.name,newItem);
                    AddToListByType(newItem);
                }
                
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
    public List<Item> AmmoList()
    {
        if (ammo == null)
        { ammo = new List<Item>(); }
        return ammo;
    }
    public List<Item> UsableList()
    {
        if (usable == null)
        { usable = new List<Item>(); }
        return usable;
    }
}
