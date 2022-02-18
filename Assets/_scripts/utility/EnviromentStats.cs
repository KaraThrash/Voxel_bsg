using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enviroment")]
public class EnviromentStats : ScriptableObject
{
    [Tooltip("")]
    public bool gravity;
    public float gravityPower; //if gravity is on but the power is zero, then land vehicles raycast down to determine falling, and flying types just hover
    [Header(" ")]
    public float airFriction;

}
