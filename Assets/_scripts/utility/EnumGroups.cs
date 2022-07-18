using System;

    public enum Maneuver { barrelRoll, spinAround }
    public enum SystemType { armor, body, engine, guidance, maneuver, targeting, weapon }
    public enum SystemState { on, off, locked, broken, damaged, menuLocked, maneuver } //menulocked for points that everything is paused by still observable

    public enum InGameEvent
    {
        none, objectiveLost, fleetShipLost,
        enemyKilled,playerDamaged

    }
    public enum SubID { none, A, B, C, D, E, F, G, Boss } //to create small differences [strafe directions and order for grouping Wings of fighters]




    public enum PlayerTypes { left, right, observer, test, neutral, enemy }




    //what to track for the pursuit element: for QoL and ease of use this is a changable option for the player
    public enum TimeType { time, playerInput, ftlJumps, distanceTraveled, manualControl, realtime, menuScreens }

    public enum GameState { playing, menu_OutofGame, menu_inGame,rest, upgradeScreen, lobby, playersdead, bonusRound, tutorial, inactive, attractMode }
   


    public enum GunType { auto, burst, semiauto, shotgun, spread, laser }




    public enum AiState
    {
        recovering, takingDamage,
        pathfinding, moving,
        attackWindUp, attacking, postAttack,
        adjusting, special, dying,
        dodging, fleeing,
        idle, spawned,
        inactive,
        ragdolling
    }

    public enum Stance
    {
        idle, neutral, defensive, aggressive, special
    }

    public enum RangeBand
    {
        ideal, melee, close, mid, far, extreme, unknown, infinity
    }
    public enum RelativeFacing
    {
        unknown, chicken, behind, away, backToBack
        //chicken: facing each other - behind the target - is facing away from the target
        //backToBack - both facing away
    }

    public enum UpgradeType { ammo, health, chainsawReward, damage, clipsize, firerate, ammoPickupBonus }

    public enum EnemyType { basic, mortar, jetpack, boss }

public enum ActorType { none,enemy,npc,objective }

    public enum EnviromentType { space, atmosphere, radiated, sand }

public enum ItemTypes
{ 
    weapon, chasis,
    engine, usable,
    bullet,none,
    vehicle, // 6
    computer,
    fleet,
    resource,
    currency
}


public enum BulletType { basic, missile, lance, boomerang, spiral, spread,zigzag }

public enum FleetShipType
{
    basic, unique, producer, storage, action, booster
}



public enum ResourceType { none, currency, food, fuel, morale, pop, raptors, vipers }


public class ItemSubType
{

    public ItemSubType()
    {
        bulletType = BulletType.lance;
        fleetShipType = FleetShipType.storage;
        resourceType = ResourceType.morale;
    }

    public BulletType bulletType;
    public FleetShipType fleetShipType;
    public ResourceType resourceType;

}



[System.Serializable]
public enum Stats
{
    none
    , armor
        , stability

    , damage

    , speed

    , mobility
    , fireRate
    , projectileSpeed
    , bulletsperburst
    , bulletlifetime

    , sensor

    , stamina_max
    , stamina_cost
    , stamina_rechargeLockout

    , storage

    , pointValue
    ,food, producefood, modifyfoodproduction,
    fuel, producefuel, modifyfuelproduction,
    pop, producepop,modifypopproduction

}


//Name:cpu0,id:e-c-0,type:computer,armor:0,damage:0,speed:0,mobility:0,fireRate:0,projectileSpeed:0,bulletsperburst:0,bulletlifetime:0,sensor:0,stamina_max:0,stamina_cost:0,stamina_rechargeLockout:0,storage:0,pointValue:0,food:0,producefood:0,modifyfoodproduction:0,fuel:0,producefuel:0,modifyfuelproduction:0,pop:0,producepop:0,modifypopproduction:0,subtype:0


/// visuals

public enum SFX { basic,bulletImpact,shipImpact };

/// end visuals





//This was put here as an test during a rubber duck conversation and left here as a curiosity to think about later
//probably will remove this later
public enum Grades { F = 0, D = 1, C = 2, B = 3, A = 4 };
public static class Extensions
{
    public static Grades minPassing = Grades.D;
    public static bool Passing(this Grades grade)
    {
        return grade >= minPassing;
    }
}

// end of code mentioned in the previous comment

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


    public static Stats StatsFromString(string _type)
    {

        foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
        {
            if (el.ToString().ToLower().Equals(_type.ToLower()))
            { return el; }
        }


        return Stats.none;

    }

    public static BulletType BulletTypeFromString(string _type)
    {

        foreach (BulletType el in (BulletType[])Enum.GetValues(typeof(BulletType)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return BulletType.basic;

    }

    public static FleetShipType FleetShipTypeFromString(string _type)
    {

        foreach (FleetShipType el in (FleetShipType[])Enum.GetValues(typeof(FleetShipType)))
        {
            if (el.ToString().Equals(_type))
            { return el; }
        }


        return FleetShipType.basic;

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
