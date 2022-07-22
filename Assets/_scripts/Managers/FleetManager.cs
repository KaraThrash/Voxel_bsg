using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetManager : Manager
{



    public float food;
    public float produce_food;
    public float storage_food;
    public float fuel;
    public float produce_fuel;
    public float storage_fuel;

    public float morale;
    public float pop;
    [SerializeField]
    public List<FleetShip> ships;


    public Dictionary<Stats, float> statMap;





    //update stats/resources check for end of turn conditions etc [e.g. refine ore to fuel, lose morale from hungar, or pop from starvation]
    public void EndOfTurn()
    {
        foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
        {
            GetStats()[el] =  GameConstants.DefaultStatValue_Fleet(el);
        }




        foreach (FleetShip el in ships)
        {
           
            foreach (Stats jay in (Stats[])Enum.GetValues(typeof(Stats)))
            {
                GetStats()[jay] = GetStats()[jay] + el.GetStats()[jay];
            }

        }


        food = food + (GetStats()[Stats.producefood] + (GetStats()[Stats.producefood] * GetStats()[Stats.modifyfoodproduction]));
        fuel = fuel + (GetStats()[Stats.producefuel] + (GetStats()[Stats.producefuel] * GetStats()[Stats.modifyfuelproduction]));
        pop = pop + (GetStats()[Stats.producepop] + (GetStats()[Stats.producepop] * GetStats()[Stats.modifypopproduction]));


    }


    public void AddShip(FleetShip _ship)
    {
        Ships().Add(_ship);

        food += _ship.GetStats()[Stats.food];
        fuel += _ship.GetStats()[Stats.fuel];
        produce_food += _ship.GetStats()[Stats.producefood];
        produce_fuel += _ship.GetStats()[Stats.producefuel];

    }






    public void GainResource(Stats _resource,float _quantity)
    {
        GetStats()[_resource] += _quantity;

        if (_resource == Stats.pointValue)
        {
            MenuManager().Set_PlayerCurrency(GetStats()[_resource]);
        }
    }






    public List<FleetShip> Ships()
    {
        if (ships == null)
        {
            ships = new List<FleetShip>();
        }

        return ships;
    }


    public Dictionary<Stats, float> GetStats()
    {
        if (statMap == null)
        {
            statMap = new Dictionary<Stats, float>();

            foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
            {
                statMap.Add(el, GameConstants.DefaultStatValue_Ship(el));
            }
        }
        return statMap;
    }





}




[SerializeField]
public class FleetShip : Item
{
    public FleetShipType fleetShipType;

    public FleetShip(Item _item)
    {
        name = _item.name;
        referenceID = _item.referenceID;

        SubType().fleetShipType = _item.SubType().fleetShipType;

        foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
        {
            if (_item.GetStats().ContainsKey(el))
            {
                GetStats()[el] = _item.GetStats()[el];
            }
        }
    }


}