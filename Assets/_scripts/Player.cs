﻿using System;
using UnityEngine;
public class Player : MonoBehaviour {

    public Ship ship;
    public ThirdPersonCamera cam;

    public bool camControlsRotation;

    public void Update()
    {
        //DetectPressedKeyOrButton();

       // InputListenForSpecialActions();

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




    public void InputListenForSpecialActions()
    {
        if (ship.primaryEngine.GetComponent<EngineBasic>().throttle_A != 0 && ship.primaryEngine.GetComponent<EngineBasic>().throttle_A + ship.primaryEngine.GetComponent<EngineBasic>().vertical == 0)
        {
            ship.primaryEngine.GetComponent<EngineBasic>().StartManeuver(Maneuver.spinAround);

        }
        if (ship.primaryEngine.GetComponent<EngineBasic>().GetSystemState() == SystemState.maneuver)
        {
            // cam.transform.LookAt(ship.primaryEngine.GetComponent<EngineBasic>().maneuverRotation);

            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation(ship.primaryEngine.GetComponent<EngineBasic>().maneuverRotation - cam.transform.position, cam.transform.up), Time.deltaTime * 12);
        }
        else
        {
            cam.PlayerControlled();
        }
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
