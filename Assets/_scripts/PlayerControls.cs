using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
  public GameObject playerShip,viperShip,raptorShip,tankShip,turnShip;
  public GameObject lockOnTarget;
  public GameObject camerasphere;
  public bool inMenu;
  public PlayerShipStats playerStats;
  private Rigidbody rb;
  public PlayerSpecialActions playerSpecialActions;
  public float lockOutWeapons,lockOutEngines;
  private Vector3 velocityDirection;

  public float leftRightAxis,updownAxis,accelerationAxis;
    // Start is called before the first frame update
    void Start()
    {
              rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

      PlayerInputs();
    }
    public void PlayerInputs()
    {
      updownAxis = Input.GetAxis("Vertical");
      leftRightAxis = Input.GetAxis("Horizontal");
    }
    public void ControlShip()
    {
      // if(rb == null){  rb = GetComponent<Rigidbody>();}
        playerSpecialActions.ListenToButtonPresses();

          if(playerShip.GetComponent<ViperControls>() != null)
          {
            if(lockOutWeapons <= 0)
            {playerShip.GetComponent<ViperControls>().WeaponSystems();}
            if(lockOutEngines <= 0)
            {
              playerShip.GetComponent<ViperControls>().ControlCamera(camerasphere,this.gameObject);
                playerShip.GetComponent<ViperControls>().Fly(rb);
            }



          }else if(playerShip.GetComponent<RaptorControls>() != null)
            {
              playerShip.GetComponent<RaptorControls>().Fly(rb);
              playerShip.GetComponent<RaptorControls>().WeaponSystems();
              playerShip.GetComponent<RaptorControls>().ControlCamera(camerasphere,this.gameObject);
            }
            else if(playerShip.GetComponent<TankControls>() != null)
              {
                playerShip.GetComponent<TankControls>().Fly(rb);
                playerShip.GetComponent<TankControls>().WeaponSystems();
                playerShip.GetComponent<TankControls>().ControlCamera(camerasphere,this.gameObject);
              }
              else if(playerShip.GetComponent<TurningShip>() != null)
                {
                  playerShip.GetComponent<TurningShip>().Fly(rb);
                  playerShip.GetComponent<TurningShip>().WeaponSystems();
                  playerShip.GetComponent<TurningShip>().ControlCamera(camerasphere,this.gameObject);
                }
              else{}

              if(lockOutEngines > 0 || lockOutWeapons > 0)
              {  lockOutEngines -= Time.deltaTime;lockOutWeapons -= Time.deltaTime;}
          if(playerStats != null)
          {
            playerStats.RechargeStamina();

          }
    }




    public void AttemptDodgeRoll()
    {
      print("dodge");
      int cost = -1;
      if(UseStamina(cost) || true)
      {
        playerShip.SendMessage("DodgeRoll",rb);
        // lockOutEngines = 0.5f;
        //default backwards //TODO: neautral barrel roll?
        // if(velocityDirection == Vector3.zero){  rb.velocity = playerShip.transform.forward *  -playerStats.dodgeDistance;}
        // else{  rb.velocity = velocityDirection.normalized * playerStats.dodgeDistance;}

      }

    }
    public bool UseStamina(float cost)
    {

      if(playerStats != null )
      {
          return playerStats.UseStamina(cost);

      }

        return false;
    }
    public void SetVelocityDirection(Vector3 newDir)
    {
      velocityDirection = newDir;
    }

    public void SetShipObjectsInactive()
    {
      viperShip.active = false;
      tankShip.active = false;
      raptorShip.active = false;
      turnShip.active = false;
        rb.useGravity = false;
    }
    public void ChangeShip(PlayerShipStats newplayerStats)
    {
        if(rb == null){  rb = GetComponent<Rigidbody>();}
      playerStats = newplayerStats;
      SetShipObjectsInactive();
          if(viperShip.active == true)
          {
            playerShip = raptorShip;
              rb.useGravity = false;
            raptorShip.active = true;

            if(playerShip.GetComponent<RaptorControls>() != null)
            {

              playerShip.GetComponent<RaptorControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());
            }

          }else  if(turnShip.active == true)
          {

              rb.useGravity = false;
            viperShip.active = true;

            playerShip = viperShip;

          }else  if(raptorShip.active == true)
          {
              rb.useGravity = true;
          tankShip.active = true;
          playerShip = tankShip;

            playerShip.GetComponent<TankControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());
          }else  if(tankShip.active == true)
          {
                  rb.useGravity = false;
              turnShip.active = true;
              playerShip = turnShip;

          }

    }

    public void SetShipStats(PlayerShipStats newplayerStats)
    {
        if(rb == null){  rb = GetComponent<Rigidbody>();}

            playerStats = newplayerStats;

            // raptorShip.GetComponent<RaptorControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());
              viperShip.GetComponent<ViperControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());
            // tankShip.GetComponent<TankControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());
            // turnShip.GetComponent<TurningShip>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());


    }


    public void OnCollisionEnter(Collision col)
    {
      if(playerShip.GetComponent<RaptorControls>() != null)
      {
        playerShip.GetComponent<RaptorControls>().HandleCollisionEnter(col,rb);
      }else
      {
        if(playerShip.GetComponent<ViperControls>() != null)
        {
          playerShip.GetComponent<ViperControls>().HandleCollisionEnter(col,rb);
        }else{}
      }

    }


    public void OnTriggerEnter(Collider col)
    {
      if(playerShip.GetComponent<RaptorControls>() != null)
      {
        playerShip.GetComponent<RaptorControls>().HandleTriggerEnter(col,rb);
      }else
      {
        if(playerShip.GetComponent<ViperControls>() != null)
        {
          playerShip.GetComponent<ViperControls>().HandleTriggerEnter(col,rb);
        }else{}
      }

    }

    public void OnTriggerExit(Collider col)
    {
      if(playerShip.GetComponent<RaptorControls>() != null)
      {
        playerShip.GetComponent<RaptorControls>().HandleTriggerExit(col,rb);
      }else
      {
        if(playerShip.GetComponent<ViperControls>() != null)
        {
          playerShip.GetComponent<ViperControls>().HandleTriggerExit(col,rb);
        }else{}
      }


    }
}
