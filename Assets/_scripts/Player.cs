using System;
using UnityEngine;
public class Player : MonoBehaviour {

    public Ship ship;

    public void Update()
    {
        DetectPressedKeyOrButton();
    }
    public void DetectPressedKeyOrButton()
    {
        if (ship == null) { return; }

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            
            if (Input.GetKeyDown(kcode))
            {
                ship.Control(kcode, true);
            }
            if (Input.GetKeyUp(kcode))
            {
                //ship.Control(kcode, false);
            }
        }
    }




}
