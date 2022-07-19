
using System;
using System.Collections.Generic;
using UnityEngine;




public class ItemManager : Manager {

    public GameObject PREFAB_pickup;

    public Transform PARENT_pickup;
    private string parentPickupName = "PARENT_Pickup";

    private Dictionary<string, Item> playerItemList;

    private List<Item> weapon;
    private List<Item> chasis;
    private List<Item> engine;
    private List<Item> usable;
    private List<Item> bullet;
    private List<Item> vehicles; 
    private List<Item> computers;

    public void Start()
    {
        ReadSpreadsheet();

    }



    public void DropItem(ItemTypes _item,int _quantity, Vector3 _pos)
    {

        GameObject clone;

        if (Parent_Pickup().childCount > 1 && Parent_Pickup().GetChild(0).GetComponent<PickUp>().onMap == false)
        {
            clone = Parent_Pickup().GetChild(0).gameObject;
        }
        else 
        {
            clone = Instantiate(PREFAB_pickup, _pos,transform.rotation);
        }

        clone.transform.parent = null;
        clone.transform.parent = Parent_Pickup();
        clone.transform.position = _pos;
        clone.SetActive(true);
        clone.GetComponent<PickUp>().Init(_item,_quantity);
    }
    

 





    public Item GetPlayerItem(string _id)
    {
        if (playerItemList.ContainsKey(_id))
        {
            return playerItemList[_id];
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
        if (_type == ItemTypes.computer)
        {
            return ComputerList();
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
        if (_item.type == ItemTypes.computer)
        {
            ComputerList().Add(_item);
        }
        if (_item.type == ItemTypes.fleet)
        {
             FleetShip newfleetship = new FleetShip(_item);
             GameManager().FleetManager().AddShip(newfleetship);
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
        Dictionary<Stats, float> newStats = new Dictionary<Stats, float>();

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
                    

                    newItem.SubType().bulletType = EnumGroups.BulletTypeFromString(statline[1].Trim());
                    newItem.SubType().fleetShipType = EnumGroups.FleetShipTypeFromString(statline[1].Trim());

                    if (newItem.type == ItemTypes.bullet)
                    {
                        newItem.subtype = (int)EnumGroups.BulletTypeFromString(statline[1].Trim());
                    }
                    if (newItem.type == ItemTypes.fleet)
                    {
                        newItem.subtype = (int)EnumGroups.FleetShipTypeFromString(statline[1].Trim());
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


        //If a stat that is important for the item to function assign it now
        if (newItem.type == ItemTypes.chasis)
        {
            GameConstants.DefaultStatValue_Chasis(newItem);
        }
        else if (newItem.type == ItemTypes.engine)
        {
            GameConstants.DefaultStatValue_Engine(newItem);
        }
        else if (newItem.type == ItemTypes.weapon)
        {
            GameConstants.DefaultStatValue_Weapon(newItem);
        }


        //this is only so the stats are visible in the inspector
        foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
        {
            newItem.GetStatList().Add(new StatClass(el, newItem.GetStats()[el]));
        }

        Debug.Log("new Item: " + newItem.ToString());



       


        return newItem;
    }



    public void ReadSpreadsheet()
    {
        string data;
        try
        {
            //   data = System.IO.File.ReadAllText("Assets/Resources/Items/readable_itemsheet.csv");

            var loadedData = Resources.Load<TextAsset>("Items/readable_itemsheet");
            string[] lines = loadedData.ToString().Split('\n');

            int count = 0;
            foreach (string el in lines)
            {
                //skip the first header line

                Item newItem = ALT_ParseToItem(el.Trim().ToLower());

                if (newItem != null)
                {

                    MasterItems.AddNewItem(newItem);

                    AddToListByType(newItem);

                    //AddToListByType(newItem);
                }
                if (count > 0)
                {
                }

                count++;
            }

        }
        catch (Exception ex)
        {

        }
         

      //  float.TryParse(text[0], x);
    }






    public Transform Parent_Pickup()
    {
        if (PARENT_pickup == null)
        {
            if (parentPickupName.Length < 1)
            {
                parentPickupName = "PARENT_Pickup";// + this.GetType().ToString();
            }

            GameObject findParent = GameObject.Find(parentPickupName);


            if (findParent == null)
            {
                PARENT_pickup = new GameObject(parentPickupName).transform;
            }
            else { PARENT_pickup = findParent.transform; }
        }

        return PARENT_pickup;
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

    public List<Item> ComputerList()
    {
        if (computers == null)
        { computers = new List<Item>(); }
        return computers;
    }

}
