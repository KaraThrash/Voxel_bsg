using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
  public GameObject playerShip,viperShip,raptorShip,tankShip,turnShip;
  public GameObject lockOnTarget;
  public GameObject camerasphere;
  public bool inMenu;
  public PlayerShipStats playerStats;
    public Player player;
  private Rigidbody rb;
  public PlayerSpecialActions playerSpecialActions;
  public float lockOutWeapons,lockOutEngines, inputBuffer,groundCollisionTimer, guncooldowntimer;
  public string lastAction;
  private Vector3 velocityDirection,forwardVelocity,strafeVelocity;
    private PlayerMovementTypes playerMovement;
  public float leftRightAxis,updownAxis,accelerationAxis, lastActionTimer, lastActionCutOffTime= 3.0f;
    private float gunoffset = 1f; //so bullets dont step on eachother
    public TrailRenderer trail;
    public ParticleSystem enginesystem;
    void Start()
    {
              rb = GetComponent<Rigidbody>();
            playerMovement = GetComponent<PlayerMovementTypes>();
    }


    void Update()
    {

        // PlayerInputs();
        if (lastAction != "") 
        {
            lastActionTimer += Time.deltaTime;
            if (lastActionTimer >= lastActionCutOffTime)
            { lastAction = ""; lastActionTimer = 0; }
        }

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
        inputBuffer -= Time.deltaTime;

        if (trail != null) { trail.time = 3 * (rb.velocity.magnitude / playerStats.speed); }

       

        if (groundCollisionTimer <= 0)
        {
            //lock inputs after special actions like dodge/spin as to not override their affect
            if (inputBuffer <= 0)
            {

                if (lockOutWeapons <= 0)
                {
                   WeaponSystems();

                }
                if (lockOutEngines <= 0)
                {
                    //seperate strafe and forward to be able to maneuver while gliding
                    strafeVelocity = playerMovement.StrafeMovement(playerStats.forwardtype,GetComponent<PlayerControls>(), playerStats, rb);
                    forwardVelocity = playerMovement.ForwardMovement(playerStats.strafetype, GetComponent<PlayerControls>(), playerStats, rb);

                    //do some ships have a minium velocity, or is the engine a dial and not a button hold
                    //if (forwardVelocity.magnitude < playerStats.minVelocityMagnitude) { forwardVelocity = transform.forward * playerStats.minVelocityMagnitude; }


                    if (enginesystem != null)
                    {
                        if (forwardVelocity.magnitude > 0.5f)
                        {
                            enginesystem.startSpeed = 3 * (rb.velocity.magnitude / playerStats.speed);
                            enginesystem.emissionRate = 75 * (rb.velocity.magnitude / playerStats.speed);
                        }
                        else { enginesystem.emissionRate = 0; }
                    }

                    velocityDirection = strafeVelocity + forwardVelocity;

                    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.deltaTime);
                    rb.velocity = Vector3.Lerp(rb.velocity, velocityDirection, Time.deltaTime);

                    transform.Rotate(0, 0, playerMovement.ViperRoll(GetComponent<PlayerControls>(), playerStats, rb));
                    playerMovement.ControlCamera(playerStats, camerasphere, this.gameObject);
                }
            }
            else
            {

                rb.velocity = forwardVelocity + strafeVelocity;
            }

        }
        else
        {

            groundCollisionTimer -= Time.deltaTime;

        }


                if(lockOutEngines > 0 || lockOutWeapons > 0)
                {  
                      lockOutEngines -= Time.deltaTime;lockOutWeapons -= Time.deltaTime;
                }

                if(playerStats != null)
                {
                      playerStats.RechargeStamina();

                }
    }


    public void WeaponSystems()
    {
        if (guncooldowntimer > 0)
        {
            guncooldowntimer -= Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButton(0) || (Input.GetAxis("10thAxis")) > 0)
            {

                if (UseStamina(playerStats.weaponStaminaCost) == true)
                {

                    FireGuns();
                    guncooldowntimer = playerStats.guncooldown;
                }

            }
        }


    }

    public void FireGuns()
    {
        SetLastAction("attack");
        GameObject bullet = playerStats.bulletSelected;

        if (bullet.GetComponent<Bullet>().lance == true)
        {

            GameObject clone2 = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
            clone2.transform.parent = this.transform;

        }
        else if (bullet.GetComponent<Bullet>().boomerang == true)
        {

            GameObject clone2 = Instantiate(bullet, transform.position + (transform.forward * 5), transform.rotation) as GameObject;
            clone2.GetComponent<Bullet>().Launched(this.gameObject);

        }
        else
        {
            Transform gunparent = playerShip.transform.Find("gunparent");
            if (gunparent != null)
            {
                foreach (Transform gun in gunparent)
                {
                    if (gunoffset == 1)
                    {
                        GameObject clone = Instantiate(bullet, gun.position , gun.rotation) as GameObject;
                        //clone.GetComponent<Rigidbody>().velocity = clone.GetComponent<Rigidbody>().velocity ;
                        clone.GetComponent<Bullet>().SetRelativeVelocity(GetForwardVelocity());
                    }
                    gunoffset *= -1;
                }
                gunoffset *= -1;
            }
            

        }

    }

    public string GetLastAction()
    {
        return lastAction;
    }


    public void SetLastAction(string action)
    {
        lastActionTimer = 0;
        lastAction = action;
    }


    public void AttemptDodgeRoll()
    {
      int cost = -1;
      if(UseStamina(cost) || true)
      {
        // playerShip.SendMessage("DodgeRoll",rb);
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


    public void ChangeShip(PlayerShipStats newplayerStats,int changeto)
    {
        if(rb == null){  rb = GetComponent<Rigidbody>();}


          playerStats = newplayerStats;
          //only the active ship type should be enabled
          SetShipObjectsInactive();
          if(changeto == 0)
          {
            rb.useGravity = false;
          viperShip.active = true;

          playerShip = viperShip;
          playerShip.GetComponent<ViperControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());



          }else  if(changeto == 1)
          {
            playerShip = raptorShip;
            rb.useGravity = false;
            raptorShip.active = true;
            if(playerShip.GetComponent<RaptorControls>() != null)
            {

              playerShip.GetComponent<RaptorControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());
            }

          }else  if(changeto == 2)
          {
              rb.useGravity = true;
          tankShip.active = true;
          playerShip = tankShip;
            playerShip.GetComponent<TankControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());

          }else  if(changeto == 3)
          {
                  rb.useGravity = false;
              turnShip.active = true;
              playerShip = turnShip;
                playerShip.GetComponent<TurningShip>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());
          }

    }


    public void SetShipStats(PlayerShipStats newplayerStats)
    {
        if(rb == null){  rb = GetComponent<Rigidbody>();}

            playerStats = newplayerStats;

            if(playerShip.GetComponent<ViperControls>() != null)
            {viperShip.GetComponent<ViperControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());}
            else if(playerShip.GetComponent<RaptorControls>() != null)
            {raptorShip.GetComponent<RaptorControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());}
            // raptorShip.GetComponent<RaptorControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());

            // tankShip.GetComponent<TankControls>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());
            // turnShip.GetComponent<TurningShip>().SetUp(playerStats.gameObject,newplayerStats,GetComponent<PlayerControls>());


    }


    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "BulletEnemy")
        {
           player.vehicletakingdamage(1);
            player.gamemanager.imageFade.StartDmgFade();

        }
        //check if it is a pickup able object
        if (col.gameObject.tag == "Resource")
        {
            //check if its the players dropped resources from the last time theydied
            if (col.gameObject.GetComponent<PickUp>() != null && col.gameObject.GetComponent<PickUp>().playerCache == true)
            {
                player.PickUpCache();
            }
            else
            {
                //if not the player cache send to item manager to add to inventory
                if (col.gameObject.GetComponent<PickUp>() != null)
                {
                    player.gamemanager.itemManager.ItemPickUp(col.gameObject);
                }

            }


            //heldresource = col.gameObject.GetComponent<PickUp>().type;
            Destroy(col.gameObject);

        }

        if (col.gameObject.tag == "Enviroment" && groundCollisionTimer <= 0)
        {
            //bounce off the ground on contact
            //TODO calculate the right amount of bounce
            groundCollisionTimer = 0.5f;
            player.gamemanager.imageFade.StartDmgFade();
            rb.velocity = (transform.position - col.contacts[0].point).normalized * rb.velocity.magnitude;
            rb.angularVelocity = (transform.position - col.contacts[0].point).normalized * playerStats.flySpeed;
        }

    }


    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Exit")
        {
           player.endLevel();

        }
        else if (col.gameObject.tag == "Dock")
        {
            player.NearDock(true);

        }
        else { }

    }


    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Dock")
        {
            player.NearDock(false);

        }
        else { }


    }

    public Vector3 GetStrafeVelocity()
    { return strafeVelocity; }
    public Vector3 GetForwardVelocity()
    { return forwardVelocity; }

}
