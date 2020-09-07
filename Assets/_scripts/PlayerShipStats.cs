using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerShipStats : MonoBehaviour
{
    public int strafetype, forwardtype, cameratype, weapontype; //for the different control patterns, layouts
    public int shipbasearmor,shipbasedamage,shipbasespeed;
    public int basearmor,basedamage,basespeed;
    public int armor,damage,speed,roll;
    public int dodgeDistance = 50;

    public int hp = 10,stamina,tempHp,tempStamina;

    public float weaponStaminaCost = 0.1f,engineStaminaCost = 0.1f, guncooldown = 0.1f;
    public float staminaRechargeRate = 1,currentstaminaRechargeBonus,staminaRechargeBonus,currentStamina;//stamina recharges faster when not being used

    public float acceleration = 3.5f,decceleration = 0.1f;//stamina recharges faster when not being used
    public float flySpeed, strafeSpeed, rollSpeed, rollMod, liftSpeed;
    public float turnSpeed, camZspeed, engineMod, ammoSelected;

    public GameObject defaultBullet, bulletSelected;
    public int currentHp = 10;
    public bool glide;
    public GameObject glideIndicator;
    public Text stamText;


    public List<Item> equipedAmmoList;


    void Start()
    {
      equipedAmmoList = new List<Item>();
        ViperSetUp();
    }


    void Update()
    {


    }


    public void SetStaminaBar()
    {

          float count = 0.5f;
          string tempstring = "";
          if(currentStamina >= 1){
              while (count < currentStamina)
              { tempstring += "."; count++; }
          }
          stamText.text = tempstring;
    }


    public void RechargeStamina()
    {
            if(currentStamina < stamina + tempStamina)
            {


              currentStamina += Time.deltaTime * (staminaRechargeRate + currentstaminaRechargeBonus);
              if(currentstaminaRechargeBonus < staminaRechargeBonus)
              {
                currentstaminaRechargeBonus += Time.deltaTime;
              }
              SetStaminaBar();
            }

    }

    public float GetStamina() { return currentStamina; }

    public bool UseStamina(float cost)
    {
          currentstaminaRechargeBonus = 0;
          if(currentStamina >= cost)
          {
            currentStamina -=  cost;
              SetStaminaBar();
            return true;
          }
          return false;
    }


    public void EquipItem(Item newItem)
    {
          //4 = ammo
          if(newItem.type == 4)
          {
            equipedAmmoList.Add(newItem);
          }
          else if (newItem.type == 5)
          {
              shipbasearmor = newItem.armor;
              shipbasedamage = newItem.damage;
              shipbasespeed = newItem.speed;
          }
          else
          {

              armor += newItem.armor;
              damage += newItem.damage;
              speed += newItem.speed;
          }

    }


    public void UnEquipItem(Item oldItem)
    {
        //4 = ammo
        if(oldItem.type == 4)
        {
            if(equipedAmmoList.Contains(oldItem))
            {equipedAmmoList.Remove(oldItem);}

        }
        else
        {
            armor -= oldItem.armor;
            damage -= oldItem.damage;
            speed -= oldItem.speed;
        }
    }


    public void ChangeWeapon(int weaponChange)
    {


    }


    public string GetStatsAsString()
    {
        string statstring = "";
        statstring += "Armor: " + armor.ToString() +"\n";
        statstring += "Damage: " + damage.ToString() +"\n";
        statstring += "Speed: " + speed.ToString() +"\n";
        return statstring;
    }


    public void ViperSetUp()
    {
        liftSpeed = speed + shipbasespeed;
        rollSpeed = (speed + shipbasespeed) / 2;

        // rollMod = playerStats
        turnSpeed = (speed + shipbasespeed) / 2;
        camZspeed = rollSpeed * 0.8f;
        flySpeed = (speed + shipbasespeed);
        engineMod = 5;
        strafeSpeed = (speed + shipbasespeed);
        acceleration = acceleration;
        decceleration = decceleration;
        weaponStaminaCost = weaponStaminaCost;
        engineStaminaCost = engineStaminaCost;
        // guncooldown = playerStats
        // cameraspeed = playerStats
        equipedAmmoList = equipedAmmoList;
        ammoSelected = 0;
        rollMod = 1;
    }

}
