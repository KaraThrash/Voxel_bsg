using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerShipStats : MonoBehaviour
{
  public int basearmor,basedamage,basespeed;
  public int armor,damage,speed;
  public int dodgeDistance = 50;

  public int hp = 10,stamina,tempHp,tempStamina;

  public float staminaRechargeRate = 1,currentstaminaRechargeBonus,staminaRechargeBonus,currentStamina;//stamina recharges faster when not being used
  public int currentHp = 10;
  public Text stamText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
    public bool UseStamina(float cost)
    {
        currentstaminaRechargeBonus = 0;
        if(currentStamina >= cost)
        {
          currentStamina -= Time.deltaTime * cost;
            SetStaminaBar();
          return true;
        }
        return false;
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
