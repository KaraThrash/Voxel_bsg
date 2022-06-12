

using UnityEngine;

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

    public int stamina_max;
    public int stamina_recharge;
    public int stamina_cost;

    public int backpack_slots;
    public int pointValue; // buy/sell value

    public int subtype;

    private int count;
    public string stats;


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
        newString += " <" + name + ">   ";
        newString += '\n';
        newString += type;
        newString += "   armor: " + armor;
        newString += '\n';
        newString += "   damage: " + damage;
        newString += '\n';
        newString += "   speed: " + speed;
        newString += '\n';
        newString += "   mobility: " + mobility;
        return newString;
    }

}
