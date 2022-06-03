using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stats_Ship")]
public class Stats_Ship : ScriptableObject
{
    public float hitpoints;
    public float max_Hitpoints;

    public float thrust;
    public float lateralThrust;

    public float torque;

    public float acceleration;

    public float rate_StaminaRegen;//per second
    public float stamina;
    public float max_Stamina;

    public float max_Thrust;
    public float max_LateralThrust;


}
