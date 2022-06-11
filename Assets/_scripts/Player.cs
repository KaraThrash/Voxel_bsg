using System;
using UnityEngine;


public class Player : MonoBehaviour {

    private GameManager gameManager;
    public Ship ship;
    public ThirdPersonCamera cam;

    public bool camControlsRotation;


    public void InitForLevel()
    {

        if (Ship()) 
        { 
            Ship().gameObject.SetActive(true);
            Ship().Hitpoints(1);
            Ship().CanAct(true);

        }
        if (GetCamera())
        { 
            GetCamera().gameObject.SetActive(true);
        }

    }


    public void Playing()
    {
        if (Ship() && Ship().CanAct())
        {
            if (Ship().Hitpoints() <= 0)
            {
                Ship().CanAct(false);
                GameManager().GetPlayerDeathEvent().Invoke();
            }
        }

    }













    public void Update()
    {
        //DetectPressedKeyOrButton();

       // InputListenForSpecialActions();

        if (ship != null)
        {
            if (GameConstants.typeA)
            {
                
                //ship.Act();
            }
            else { cam.PlayerControlled(); }

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
           // cam.PlayerControlled();
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





    public void EquipItem(Item _item)
    {
        if (Ship() == null)
        { return; }

        Ship().EquipItem(_item);

        GameManager().MenuManager().DisplayEquipmentTotals(Ship().GetEquipment());

    }




    public void EnemyDeathEvent(Enemy _enemy)
    {


    }




    public Ship Ship()
    {
        if (ship == null)
        {

        }

        return ship;
    }

    public ThirdPersonCamera GetCamera()
    {

        return cam;
    }

    public GameManager GameManager()
    {
        if (gameManager == null)
        { gameManager = FindObjectOfType<GameManager>(); }

        return gameManager;
    }


    public Vector3 Position()
    {
        if (Ship())
        {
            return Ship().transform.position;
        }
        return transform.position;
    }



}
