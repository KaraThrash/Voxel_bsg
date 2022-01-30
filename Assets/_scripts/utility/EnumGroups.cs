using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SystemType { armor,body,engine,guidance,maneuver,targeting}
public enum SystemState { on,off,locked,broken,damaged, menuLocked} //menulocked for points that everything is paused by still observable

public enum PlayerTypes { left, right, observer, test, neutral, enemy }


public enum GameState { playing, inOverTime, rest, upgradeScreen, lobby, playersdead, bonusRound, tutorial, inactive,attractMode }
public enum GunType { auto,burst, semiauto, shotgun, spread, laser }


public enum AiState { 
    recovering, takingDamage, 
    pathfinding, moving, attacking, 
    adjusting, special, dying, 
    dodging,fleeing,
    idle, spawned,
    inactive,
    ragdolling
}

public enum Stance
{
    neutral,defensive,aggressive,special
}

public enum RangeBand
{
    ideal,close,far,extreme,unknown,infinity
}


public enum UpgradeType { ammo, health, chainsawReward, damage, clipsize, firerate, ammoPickupBonus }

public enum EnemyType { basic, mortar, jetpack, boss }
public enum ItemTypes { sword, shield, mortarShell,fireBall,none }





public static class EnumGroups
{



    public static string GameStateToString(GameState _type)
    {


        return _type.ToString();
    }

    public static GameState GameStateFromString(string _type)
    {

        foreach (GameState el in (GameState[])Enum.GetValues(typeof(GameState)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return GameState.playing;

    }








    public static string UpgradeTypeToString(UpgradeType _type)
    {


        return _type.ToString();
    }

    public static UpgradeType UpgradeTypeFromString(string _type)
    {

        foreach (UpgradeType el in (UpgradeType[])Enum.GetValues(typeof(UpgradeType)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return UpgradeType.ammoPickupBonus;

    }


    public static string PlayerTypeToString(PlayerTypes _type)
    {


        return _type.ToString();
    }

    public static PlayerTypes PlayerTypeFromString(string _type)
    {

        foreach (PlayerTypes el in (PlayerTypes[])Enum.GetValues(typeof(PlayerTypes)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return PlayerTypes.right;

    }






    public static string AiStateToString(AiState _type)
    {
        return _type.ToString();
    }
    public static AiState AiStateFromString(string _type)
    {
        foreach (AiState el in (AiState[])Enum.GetValues(typeof(AiState)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return AiState.pathfinding;

    }



    public static string ItemToString(ItemTypes _type)
    {


        return _type.ToString();
    }

    public static ItemTypes ItemFromString(string _type)
    {

        foreach (ItemTypes el in (ItemTypes[])Enum.GetValues(typeof(ItemTypes)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return ItemTypes.none;

    }









    public static string EnemyTypeToString(EnemyType _type)
    {
        return _type.ToString();
    }

    public static EnemyType EnemyTypeFromString(string _type)
    {

        foreach (EnemyType el in (EnemyType[])Enum.GetValues(typeof(EnemyType)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }



        return EnemyType.basic;
    }








}
