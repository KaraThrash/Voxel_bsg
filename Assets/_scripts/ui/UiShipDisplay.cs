using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiShipDisplay : MonoBehaviour
{
    public Ship ship;

    public Text hp;
    public Text stamina;
    public Text weaponCooldown;



    void FixedUpdate()
    {
        if (ship != null)
        {
            SetHP(ship.Hitpoints());
            SetStamina(ship.stamina);
        }



    }


    public void SetHP(string _text)
    {
        if (hp != null)
        { hp.text = _text; }
    }

    public void SetHP(float _text)
    {
        if (hp != null && _text > 0)
        {
            string bartext = "";
            if (_text > 0)
            {
    
                while (bartext.Length < _text)
                {
                    bartext = bartext + ":";
                }

                if (_text % 1 > 0)
                { bartext = bartext + "."; }
            }
            stamina.text = bartext;
        }
    }


    public void SetStamina(string _text)
    {
        if (stamina != null)
        { stamina.text = _text; }
    }

    public void SetStamina(float _text)
    {
        if (stamina != null && _text > 0)
        {
            string bartext = "";
            if ( _text > 0)
            {

                while (bartext.Length < _text)
                {
                    bartext = bartext + ".";
                }
            }
            stamina.text = bartext;
        }
    }


    public void SetWeapons(string _text)
    {
        if (weaponCooldown != null)
        { weaponCooldown.text = _text; }
    }


}
