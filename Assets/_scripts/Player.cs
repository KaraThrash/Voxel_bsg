﻿using System;
using UnityEngine;
public class Player : MonoBehaviour {

    public Ship ship;
    public ThirdPersonCamera cam;

    public bool camControlsRotation;

    public void Update()
    {
        DetectPressedKeyOrButton();

        if (ship != null)
        {
            if (camControlsRotation)
            {
                cam.PlayerControlled();

                ship.Act();

            }
        }

    }

    public void Movement_CameraRelative_ThirdPerson()
    {


    }


    public void Movement_ShipRelative_ThirdPerson()
    {

    }

 


    public void DetectPressedKeyOrButton()
    {
        if (ship == null) { return; }

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            
            if (Input.GetKeyDown(kcode))
            {
                //ship.Control(kcode, true);
            }
            if (Input.GetKeyUp(kcode))
            {
                //ship.Control(kcode, false);
            }
        }
    }




}
