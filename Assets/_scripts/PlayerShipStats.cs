using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipStats : MonoBehaviour
{
  public int basearmor,basedamage,basespeed;
  public int armor,damage,speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EquipItem(Item newItem)
    {
      armor += newItem.armor;
      damage += newItem.damage;
      speed += newItem.speed;
    }

    public void UnEquipItem(Item oldItem)
    {
        armor -= oldItem.armor;
          damage -= oldItem.damage;
            speed -= oldItem.speed;
    }
    public string GetStatsAsString()
    {
      string statstring = "";
      statstring += "Armor: " + armor.ToString() +"\n";
        statstring += "Damage: " + damage.ToString() +"\n";
          statstring += "Speed: " + speed.ToString() +"\n";
          return statstring;
    }
}
