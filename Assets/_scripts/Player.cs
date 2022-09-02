using System;
using UnityEngine;


public class Player : MonoBehaviour {

    private GameManager gameManager;


    public Ship ship;
    public ThirdPersonCamera cam;

    public Dradis dradis;

    public bool camControlsRotation;


    public void Start()
    {
        Ship().GetEquipment().ResetItems();
    }

    public void InitForLevel()
    {

        if (Ship()) 
        { 
            Ship().gameObject.SetActive(true);
            Ship().Hitpoints(1);
            Ship().CanAct(true);


            if (GameManager().FleetManager())
            {
                if (Ship().GetEquipment().weapon == null)
                {

                    if (GameManager().ItemManager().WeaponList().Count > 0)
                    {
                        Ship().GetEquipment().weapon = GameManager().ItemManager().WeaponList()[0];
                    }
                }

                if (Ship().GetEquipment().engine == null)
                {

                    if (GameManager().ItemManager().EngineList().Count > 0)
                    {
                        Ship().GetEquipment().engine = GameManager().ItemManager().EngineList()[0];
                    }
                }

                if (Ship().GetEquipment().chasis == null)
                {

                    if (GameManager().ItemManager().ChasisList().Count > 0)
                    {
                        Ship().GetEquipment().chasis = GameManager().ItemManager().ChasisList()[0];
                    }
                }


                Ship().GetEquipment().CalculateStats();
               // Ship().ApplyFleetStats(GameManager().FleetManager());



            }

        }
        if (GetCamera())
        {

            GetCamera().ZSensitivity = Ship().GetEquipment().GetStats()[Stats.mobility] * 5;
            GetCamera().XSensitivity = Ship().GetEquipment().GetStats()[Stats.mobility] * 5;
            GetCamera().YSensitivity = Ship().GetEquipment().GetStats()[Stats.mobility] * 5;
            GetCamera().gameObject.SetActive(true);
        }

        if (Ship().PrimaryEngine())
        {
            Ship().PrimaryEngine().listenForPlayerInput = true ;
            Ship().PrimaryEngine().STAT_Power(Ship().GetEquipment().GetStats()[Stats.speed]);
            Ship().PrimaryEngine().STAT_PowerSecondary(Ship().GetEquipment().GetStats()[Stats.mobility]);
            Ship().PrimaryEngine().rate_Acceleration = Ship().GetEquipment().GetStats()[Stats.acceleration];
        }

        if (Ship().SecondaryEngine())
        {
            Ship().SecondaryEngine().listenForPlayerInput = true;
            Ship().SecondaryEngine().STAT_Power(Ship().GetEquipment().GetStats()[Stats.mobility]);
            Ship().SecondaryEngine().STAT_PowerSecondary(Ship().GetEquipment().GetStats()[Stats.mobility]);
            Ship().SecondaryEngine().rate_Acceleration = Ship().GetEquipment().GetStats()[Stats.acceleration];
        }

        if (Ship().PrimaryWeapon())
        {
            Ship().PrimaryWeapon().listenForPlayerInput = true;
            Ship().PrimaryWeapon().STAT_CooldownTime(Ship().GetEquipment().GetStats()[Stats.fireRate]);
        }

    }


    public void Playing()
    {
        if (Ship() && Ship().CanAct())
        {
            InputListen();

            

            if (Ship().Hitpoints() <= 0)
            {
                Ship().CanAct(false);

                GameManager().GetPlayerDeathEvent().Invoke();
            }
        }

    }


    public void InputListen()
    {
        InputControls.TrackAxisButtons();

        if (InputControls.DpadHortAsButton())
        {
            if (InputControls.DpadHort() > 0)
            {
                Ship().GetEquipment().MoveBulletListBookmark(1);
            }
            else
            {
                Ship().GetEquipment().MoveBulletListBookmark(-1);
            }


            if (Ship().GetEquipment().GetBullet() != null)
            {
                GameManager().MenuManager().Set_PlayerBulletEquippedText(Ship().GetEquipment().GetBullet().name);
            }



        }

        if (InputControls.DpadVertAsButton())
        {
            Ship().GetEquipment().MoveConsumableListBookmark((int)Mathf.Sign(InputControls.DpadVert()));
            if (Ship().GetEquipment().GetConsumable() != null)
            {
                GameManager().MenuManager().Set_PlayerConsumableEquippedText(Ship().GetEquipment().GetConsumable().name);
            }
        }
    }










    public void Update()
    {
        //DetectPressedKeyOrButton();

       // InputListenForSpecialActions();

        if (ship != null && GameManager().GetGameState() == GameState.playing)
        {
            if (GameConstants.TYPE_A)
            {
                
                //ship.Act();
            }
            else
            {
                if (ship.CPU() && ship.CPU().GetTarget())
                {
                    cam.PlayerControlled(ship, ship.CPU().GetTarget());
             
                }
                else
                {
                    cam.PlayerControlled(ship);
                }

                
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


    public Dradis Dradis()
    {
        return dradis;
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
