using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants 
{
    //default time that controls are locked out from events like collisions with the enviroment or taking damage
    public static float SYSTEM_STUN = 0.2f;

    public static float FOCAL_LENGTH = 15.0f;



    public static float BULLET_SPEED = 30.0f;
    public static float BULLET_DAMAGE = 1.0f;


    public static float PICKUP_RANGE = 5.0f;

    public static bool TYPE_A = false;// manually control the ship rotation, the camera focuses in front of the ship



    public static float TIME_BETWEEN_ATTACKS = 153.0f;
    public static float INPUT_BETWEEN_ATTACKS = 33;
    public static float MENU_BETWEEN_ATTACKS = 33;
    public static float JUMPS_BETWEEN_ATTACKS = 3;
    public static float DISTANCE_BETWEEN_ATTACKS = 33;





    public static float DefaultStatValue_Ship(Stats _stat)
    {
        if (_stat == Stats.armor)
        { return 1; }
        if (_stat == Stats.damage)
        { return 1; }
        if (_stat == Stats.speed)
        { return 1; }
        if (_stat == Stats.mobility)
        { return 1; }
        if (_stat == Stats.fireRate)
        { return 0.2f; }
        if (_stat == Stats.projectileSpeed)
        { return 1; }
        if (_stat == Stats.bulletsperburst)
        { return 1; }
        if (_stat == Stats.bulletlifetime)
        { return 1; }

        if (_stat == Stats.stamina_max)
        { return 100; }
        if (_stat == Stats.stamina_cost)
        { return 1; }
        if (_stat == Stats.stamina_rechargeLockout)
        { return 0.1f; }
        if (_stat == Stats.storage)
        { return 1; }
        if (_stat == Stats.pointValue)
        { return 1; }
        if (_stat == Stats.sensor)
        { return 0; }




        return 0;
    }







    public static float DefaultStatValue_Bullet(Stats _stat)
    {

        if (_stat == Stats.armor)
        { return 1; }
        if (_stat == Stats.damage)
        { return 1; }
        if (_stat == Stats.speed)
        { return 1; }
        if (_stat == Stats.mobility)
        { return 1; }
        if (_stat == Stats.fireRate)
        { return 0.2f; }
        if (_stat == Stats.projectileSpeed)
        { return 1; }
        if (_stat == Stats.bulletsperburst)
        { return 1; }
        if (_stat == Stats.bulletlifetime)
        { return 1; }

        if (_stat == Stats.stamina_max)
        { return 100; }
        if (_stat == Stats.stamina_cost)
        { return 1; }
        if (_stat == Stats.stamina_rechargeLockout)
        { return 0.1f; }
        if (_stat == Stats.storage)
        { return 1; }
        if (_stat == Stats.pointValue)
        { return 1; }
        if (_stat == Stats.sensor)
        { return 0; }




        return 0;
    }


    public static float DefaultStatValue_Engine(Stats _stat)
    {
        if (_stat == Stats.speed)
        { return 1; }
        if (_stat == Stats.mobility)
        { return 1; }

        return 0;
    }

    public static void DefaultStatValue_Engine(Item _item)
    {
        if (_item.GetStats()[Stats.speed] == 0)
        {
            _item.GetStats()[Stats.speed] = 1;
        }
        if (_item.GetStats()[Stats.mobility] == 0)
        {
            _item.GetStats()[Stats.mobility] = 1;
        }
        

    }

    public static float DefaultStatValue_Weapon(Stats _stat)
    {
        if (_stat == Stats.damage)
        { return 1; }
        if (_stat == Stats.fireRate)
        { return 0.2f; }
        if (_stat == Stats.projectileSpeed)
        { return 1; }
        if (_stat == Stats.bulletsperburst)
        { return 1; }


        return 0;
    }

    public static void DefaultStatValue_Weapon(Item _item)
    {
        if (_item.GetStats()[Stats.damage] == 0)
        {
            _item.GetStats()[Stats.damage] = 1;
        }
        if (_item.GetStats()[Stats.fireRate] == 0)
        {
            _item.GetStats()[Stats.fireRate] = 0.2f;
        }
        if (_item.GetStats()[Stats.projectileSpeed] == 0)
        {
            _item.GetStats()[Stats.projectileSpeed] = 1;
        }
        if (_item.GetStats()[Stats.bulletlifetime] == 0)
        {
            _item.GetStats()[Stats.bulletlifetime] = 1;
        }
        if (_item.GetStats()[Stats.bulletsperburst] == 0)
        {
            _item.GetStats()[Stats.bulletsperburst] = 1;
        }
        
        
        
        
        

    }

    public static void DefaultStatValue_Chasis(Item _item)
    {
        if (_item.GetStats()[Stats.armor] == 0)
        {
            _item.GetStats()[Stats.armor] = 1;
        }


    }

    public static float DefaultStatValue_Chasis(Stats _stat)
    {
        if (_stat == Stats.armor)
        { return 1; }



        return 0;
    }


    public static float DefaultStatValue_Fleet(Item _item)
    {
     

        if (_item.GetStats()[Stats.food] == 0)
        {
            _item.GetStats()[Stats.food] = 10;
        }
        else if (_item.GetStats()[Stats.producefood] == 0)
        {
            _item.GetStats()[Stats.producefood] = -1;
        }
        else if (_item.GetStats()[Stats.fuel] == 0)
        {
            _item.GetStats()[Stats.fuel] = 10;
        }
        else if (_item.GetStats()[Stats.producefuel] == 0)
        {
            _item.GetStats()[Stats.producefuel] = -1;
        }
        else if (_item.GetStats()[Stats.pop] == 0)
        {
            _item.GetStats()[Stats.pop] = 1;
        }
        else if (_item.GetStats()[Stats.modifyfoodproduction] == 0)
        {
            _item.GetStats()[Stats.modifyfoodproduction] = 1;
        }
        else if (_item.GetStats()[Stats.modifyfuelproduction] == 0)
        {
            _item.GetStats()[Stats.modifyfuelproduction] = 1;
        }
        //else if (_item.GetStats()[Stats.producepop] == 0)
        //{
        //    _item.GetStats()[Stats.producepop] = 1;
        //}


        return 0;
    }


}
