using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ShipSystem { armor, body, engine, gun, navigation }

public enum PlayerTypes { left, right, observer, test, neutral, enemy }
public enum GameState { playing, inOverTime, rest, upgradeScreen, lobby, playersdead, bonusRound, tutorial, inactive,attractMode }
public enum GunType { auto, semiauto, shotgun, spread, laser }


public enum GunState { idle, reload, shoot, hookshot, chainsaw, broken, menu, power,sandbox }

//Try to determine the pace the players are performing at and adjust accordingly when possible
//thinking of Resident Evil 4 
public enum CurrentPace { normal, slow, fast, losing, winning, confused }
public enum playerCommands { shootStart, shootEnd, hookshotStart, hookshotEnd, chainsawStart, chainsawEnd, reload }
public enum DeathCondition { bullet, headshot, explosion, largeExplosion, chainsaw, hookshot, none }

public enum LevelTypes { basic,bonus,upgrade,tutorial,other }
public enum RoundEndCondition { time, kills, points, endless }

public enum AiState { 
    recovering, pregrabbed, grabbed, 
    launched, takingDamage, 
    pathfinding, moving, attacking, 
    adjusting, special, dying, 
    idle, spawned,
    chainsawed,tutorial,inactive,
    ragdolling
}

public enum LocationType { barricade, mortar, air, stage }

public enum UpgradeType { ammo, health, chainsawReward, damage, clipsize, firerate, ammoPickupBonus }
public enum EnemySound { idle, spawn, attack, hurt, bodyhurt, limbhurt, headhurt, death, chainsaw, hookshot, air, explosion, voice, land, walk, jog, run, jump, special, pickup, load }
public enum AnimationEvent { spawn, attack, mortarFire, die, impact, stand,land, rage, pickup, drop }
public enum ConditionalDamage { headshot,bomb,basic,immune,none }

public enum EnemyType { basic, mortar, jetpack, boss }
public enum SurfaceMaterial { flesh, sand, bone, metal, ground, stone, fire, none }
public enum BodyParts { chest, head, arm, leg, other, armor, weapon }
public enum ItemTypes { sword, shield, mortarShell,fireBall,none }
public enum GameEvents { shootGun, gunHitBullet, gunHitEnemy, gunMiss, shootHookshot, hitHookshot, endHookshot, activateChainsaw, hitChainsaw, endChainsaw, reload, enemyDie, damageEvent, takeDamage, playerDie, dryFire, enemySpawn, roundEnd, standardInterval, useUpgrade, startGame }
public enum SpawnZone { middle,middleClose,middleFar,middleVeryClose,middleVeryFar,side,sideClose,sideFar,leftside,rightSide,ground,air }
public enum ActionList { idle,attack,impact,block,special,gethit,spawn,land } //us an int to send to the ragdolls animator











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








    public static string SurfaceMaterialToString(SurfaceMaterial _type)
    {

        return _type.ToString();
    }

    public static SurfaceMaterial SurfaceMaterialToString(string _type)
    {

        foreach (SurfaceMaterial el in (SurfaceMaterial[])Enum.GetValues(typeof(SurfaceMaterial)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return SurfaceMaterial.flesh;

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



    public static string DeathConditionToString(DeathCondition _type)
    {
        return _type.ToString();
    }
    public static DeathCondition DeathConditionFromString(string _type)
    {
        foreach (DeathCondition el in (DeathCondition[])Enum.GetValues(typeof(DeathCondition)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return DeathCondition.bullet;

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


    public static string GunStateToString(GunState _type)
    {


        return _type.ToString();
    }

    public static GunState GunStateFromString(string _type)
    {

        foreach (GunState el in (GunState[])Enum.GetValues(typeof(GunState)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }
        

        return GunState.idle;

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



    public static string LocationTypeToString(LocationType _type)
    {


        return _type.ToString();
    }

    public static LocationType LocationTypeFromString(string _type)
    {

        foreach (LocationType el in (LocationType[])Enum.GetValues(typeof(LocationType)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return LocationType.barricade;

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
    public static string ConditionalDamageToString(ConditionalDamage _type)
    {
        return _type.ToString();
    }

    public static ConditionalDamage ConditionalDamageFromString(string _type)
    {

        foreach (ConditionalDamage el in (ConditionalDamage[])Enum.GetValues(typeof(ConditionalDamage)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }



        return ConditionalDamage.basic;
    }








}
