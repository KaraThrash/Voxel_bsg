using System;

    public enum Maneuver { barrelRoll, spinAround }
    public enum SystemType { armor, body, engine, guidance, maneuver, targeting, weapon }
    public enum SystemState { on, off, locked, broken, damaged, menuLocked, maneuver } //menulocked for points that everything is paused by still observable

    public enum InGameEvent { none, objectiveLost, fleetShipLost }
    public enum SubID { none, A, B, C, D, E, F, G, Boss } //to create small differences [strafe directions and order for grouping Wings of fighters]

    public enum BulletType { basic, missile, lance, boomerang, spiral,spread }


    public enum PlayerTypes { left, right, observer, test, neutral, enemy }


    public enum GameState { playing, inOverTime, rest, upgradeScreen, lobby, playersdead, bonusRound, tutorial, inactive, attractMode }
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
    public enum EnviromentType { space, atmosphere, radiated, sand }

public enum ItemTypes
{ 
    weapon, chasis,
    engine, usable,
    ammo,none,
    vehicle, // 6
    resource
}
public enum ResourceType { none, currency, food, fuel, morale, pop, raptors, vipers }




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
