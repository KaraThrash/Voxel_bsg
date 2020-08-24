using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViperControls : MonoBehaviour {
  public PlayerControls playerControls;

    public GameObject myplayer;
    public GameObject camera;
    public GameObject camerasphere;
    public GameObject behind;
    public Quaternion targetRotation;




    //stats for this specific ship when in use
    public int turnSpeed;
    public float flySpeed;
    public float  strafeSpeed;
    public float liftSpeed;
    public float rollSpeed;
    public float camZspeed;


    public float weaponStaminaCost = 0.1f,engineStaminaCost = 0.1f;
    public float rollMod,engineMod;
    public float acceleration = 1.5f,decceleration = 0.5f;



    public GameObject gun1;
    public GameObject gun2;
    public GameObject defaultbullet,bullet;
    private List<Item> equipedAmmoList;

    public float guncooldown;
    public float guncooldowntimer;
    public float cameraspeed;
    public float step,groundCollisionTimer;
    public float inputBuffer;

    public int heldresource,ammoSelected;

    public GameObject glideIndicator;
    public bool glide;
    private float roll;
    private Vector3 velocityDirection,targetFlyVelocity,targetStrafeVelocity; //unit vector for the controllere input direction of travel, to make dashing easier
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
      equipedAmmoList = new List<Item>();
        cameraspeed = 15;

    }

    //each ship type has its core values to modify the player stats
    public void SetUp(GameObject newmyplayer,PlayerShipStats playerStats,PlayerControls newplayerControls)
    {
            myplayer = newmyplayer;
            playerControls = newplayerControls;
            liftSpeed = playerStats.speed  + playerStats.shipbasespeed;
            rollSpeed = (playerStats.speed + playerStats.shipbasespeed) / 2;

            // rollMod = playerStats
            turnSpeed = (playerStats.speed + playerStats.shipbasespeed) / 2;
            camZspeed = rollSpeed * 0.8f;
            flySpeed = (playerStats.speed + playerStats.shipbasespeed);
            engineMod = 5;
            strafeSpeed = (playerStats.speed + playerStats.shipbasespeed) ;
            acceleration = playerStats.acceleration;
            decceleration = playerStats.decceleration;
            weaponStaminaCost = playerStats.weaponStaminaCost;
            engineStaminaCost = playerStats.engineStaminaCost;
            // guncooldown = playerStats
            // cameraspeed = playerStats
            equipedAmmoList = playerStats.equipedAmmoList;
            ammoSelected = 0;
            ChangeAmmo(ammoSelected);
    }

	public void Fly (Rigidbody shipRigidBody) {

       inputBuffer -= Time.deltaTime;

       if(groundCollisionTimer <= 0)
       {
             //lock inputs after special actions like dodge/spin as to not override their affect
              if(inputBuffer <= 0)
              {
                  Controllerflightcontrols(shipRigidBody);
                  // thirdpersonflightcontrols(shipRigidBody);
                  WeaponSystems();
              }
              else
              {

                  shipRigidBody.velocity = targetFlyVelocity + targetStrafeVelocity;
              }

        }
        else
        {

            groundCollisionTimer -= Time.deltaTime;

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
              if (Input.GetMouseButton(0) || (Input.GetAxis("10thAxis")) > 0 )
              {

                  if (playerControls.UseStamina(weaponStaminaCost) == true)
                  {

                      RaycastShootGuns();
                      guncooldowntimer = guncooldown;
                  }

              }
          }


    }


    public void ControlCamera(GameObject cam,GameObject ship)
    {

        //default is camera tracks with mouse // needs to toggle on free look
        if (Input.GetMouseButton(1))
        {
            cam.GetComponent<ThirdPersonCamera>().rollz = 0;
        }
        else
        {
            //cam.transform.position = ship.transform.position;
            // targetRotation = Quaternion.LookRotation((camera.transform.position + camera.transform.forward) - transform.position);
            // step = Mathf.Min(0.5f * Time.deltaTime, 1.5f);
             step = Mathf.Min(4 * Time.deltaTime, 1.5f);
            // cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, ship.transform.rotation, Time.deltaTime * rollMod * 0.12f * turnSpeed);
            ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, cam.transform.rotation, Time.deltaTime * rollMod * 0.3f * turnSpeed);

            //TODO: change this '20' to a reasonable variable // cam should be slightly behind ship rotation to give the semse of movement
            cam.GetComponent<ThirdPersonCamera>().rollz = roll * camZspeed * Time.deltaTime ;

        }
    }


    public void thirdpersonflightcontrols(Rigidbody shipRigidBody)
    {
      Vector3 newvel = Vector3.zero;

      float lift = 0;
      float hort = 0;
      float vert = 0;
      float curdecceleration = decceleration;
      if(Input.GetAxis("Horizontal") != 0 && playerControls.UseStamina(engineStaminaCost) == true)
      {  hort = Input.GetAxis("Horizontal"); }else{hort = 0;}
      if(Input.GetAxis("Vertical") != 0 && playerControls.UseStamina(engineStaminaCost) == true)
      {  vert = Input.GetAxis("Vertical"); }else{vert = 0;}



              if  ((Input.GetKey(KeyCode.Space) ) )
              { lift = 1;curdecceleration = acceleration; }
              else  if  ((Input.GetKey(KeyCode.LeftShift)) )

                {  lift = -1; curdecceleration = acceleration;}

                else { lift = 0;}


        if (Input.GetKey(KeyCode.E)) { roll = -1; }
        else if (Input.GetKey(KeyCode.Q))
        { roll = 1; }
        else { roll = 0; }
        // roll feels really sluggish, x10 the rollSpeed so the number is visually easier to work with
        shipRigidBody.transform.Rotate(0, 0, roll * rollSpeed * 10 * rollMod * Time.deltaTime);



        Vector3 tempvel = shipRigidBody.transform.position - (shipRigidBody.transform.position + shipRigidBody.transform.right);
        tempvel *= strafeSpeed * -hort;
        Vector3 tempvel2 = shipRigidBody.transform.position - (shipRigidBody.transform.position + shipRigidBody.transform.forward);
        tempvel2 *= flySpeed * -vert;
        Vector3 tempvel3 = transform.position - (transform.position + transform.up);
        tempvel3 *= strafeSpeed * -lift;

        newvel = tempvel + tempvel2 + tempvel3 ;
        velocityDirection = newvel;
        playerControls.SetVelocityDirection(newvel);
        shipRigidBody.velocity = Vector3.Lerp(shipRigidBody.velocity,newvel, curdecceleration * Time.deltaTime);
        shipRigidBody.angularVelocity = Vector3.Lerp(shipRigidBody.angularVelocity, Vector3.zero, curdecceleration * Time.deltaTime);
    }


    public void Controllerflightcontrols(Rigidbody shipRigidBody)
    {
      Vector3 newvel = Vector3.zero;

      float lift = 0;
      float hort = 0;
      float vert = 0;
      float curdecceleration = decceleration;

      if(Input.GetAxis("Horizontal") != 0 && playerControls.UseStamina(engineStaminaCost * Time.deltaTime) == true)
      {  hort = Input.GetAxis("Horizontal");}else{hort = 0;}

        if(Input.GetAxis("Vertical") != 0 && playerControls.UseStamina(engineStaminaCost * Time.deltaTime) == true)
        {  lift = Input.GetAxis("Vertical");}else{lift = 0;}




          //Throttle engine, forward/backward movement, hold second key for reverse otherwise go forward
              if  ((Input.GetKey(KeyCode.Space) || Input.GetAxis("9thAxis") > 0)  && playerControls.UseStamina(engineStaminaCost * Time.deltaTime) == true)
              {

                  if  ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton8)) )

                  {
                      vert = -0.5f;curdecceleration = decceleration;
                  }
                  else
                  {
                      vert = 1; curdecceleration = acceleration;
                  }


              }  else { vert = 0;}





        if (Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.Q)) { roll = 1; }
        else if (Input.GetKey(KeyCode.JoystickButton5) || Input.GetKey(KeyCode.E))
        { roll = -1; }
        //else if(Input.GetKeyUp(Keycode.Q)  || Input.GetKeyUp(Keycode.E)) { roll = 0; }
        else { roll = 0; }

        // roll feels really sluggish, x10 the rollSpeed so the number is visually easier to work with
        shipRigidBody.transform.Rotate(0, 0, roll * rollSpeed * 10 * rollMod * Time.deltaTime);



        Vector3 tempvel = shipRigidBody.transform.position - (shipRigidBody.transform.position + shipRigidBody.transform.right);
        tempvel *= strafeSpeed * -hort;
        Vector3 tempvel2 = shipRigidBody.transform.position - (shipRigidBody.transform.position + shipRigidBody.transform.forward);
        tempvel2 *= flySpeed * -vert;
        Vector3 tempvel3 = transform.position - (transform.position + transform.up);
        tempvel3 *= strafeSpeed * -lift;

        targetStrafeVelocity = tempvel + tempvel3;


        //TODO: need an input manager, to make the buttons editable
        if (Input.GetKeyDown(KeyCode.JoystickButton8) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            glide = !glide; glideIndicator.active = glide;
        }


        //if the player disables glide or runs out of stamina to use it: disable glide
        if (vert == 0 && (glide == false || playerControls.UseStamina(engineStaminaCost * Time.deltaTime) == false))
        {
            glide = false;
            targetFlyVelocity = Vector3.zero;

        }
        else
        {
            targetFlyVelocity = Vector3.Lerp(targetFlyVelocity,tempvel2, curdecceleration * Time.deltaTime);

        }

        newvel = targetStrafeVelocity + targetFlyVelocity ;
        velocityDirection = newvel;

        playerControls.SetVelocityDirection(newvel);

        shipRigidBody.velocity = Vector3.Lerp(shipRigidBody.velocity,newvel,   Time.deltaTime);
        shipRigidBody.angularVelocity = Vector3.Lerp(shipRigidBody.angularVelocity,Vector3.zero, acceleration * Time.deltaTime);
    }



    public void DodgeRoll(Rigidbody shipRigidBody)
    {
          if(inputBuffer <= 0)
          {
              inputBuffer = 0.5f;
              shipRigidBody.velocity = velocityDirection * flySpeed;
          }
    }



    public void RaycastShootGuns()
    {
        playerControls.SetLastAction("attack");
        if (bullet.GetComponent<Bullet>().lance == true)
        {

            GameObject clone2 = Instantiate(bullet, transform.position, gun2.transform.rotation) as GameObject;
            clone2.transform.parent = this.transform;

        }
        else if (bullet.GetComponent<Bullet>().boomerang == true)
        {

            GameObject clone2 = Instantiate(bullet, transform.position + (transform.forward * 5), gun2.transform.rotation) as GameObject;
            clone2.GetComponent<Bullet>().Launched(this.gameObject);
            
        }
        else
        {
          GameObject clone = Instantiate(bullet, gun2.transform.position, gun2.transform.rotation) as GameObject;
          clone.GetComponent<Rigidbody>().velocity = clone.GetComponent<Rigidbody>().velocity + targetFlyVelocity;
          clone = Instantiate(bullet, gun1.transform.position, gun2.transform.rotation) as GameObject;
          clone.GetComponent<Rigidbody>().velocity = clone.GetComponent<Rigidbody>().velocity + targetFlyVelocity;

        }

    }


  public void ChangeAmmo(int whichammo)
  {
          ammoSelected = whichammo;
          if(equipedAmmoList != null && equipedAmmoList.Count > 0){
                if(ammoSelected < equipedAmmoList.Count)
                {ammoSelected = whichammo;}else{ammoSelected = 0;}

                  string bulletname = "bullet/" + equipedAmmoList[ammoSelected].name;
                  GameObject loadammo = Resources.Load("bullet/" + equipedAmmoList[ammoSelected].name, typeof(GameObject)) as GameObject;
                  if(loadammo != null)
                  {
                    bullet = loadammo;
                  }else{bullet = defaultbullet;}
          }else{bullet = defaultbullet;}
  }



    public void HandleCollisionEnter(Collision col,Rigidbody shipRigidBody )
    {
        if (col.gameObject.tag == "BulletEnemy")
        {
              myplayer.GetComponent<Player>().vehicletakingdamage(1);
              myplayer.GetComponent<Player>().gamemanager.imageFade.StartDmgFade();

        }
        //check if it is a pickup able object
        if (col.gameObject.tag == "Resource" )
        {
                //check if its the players dropped resources from the last time theydied
                if(col.gameObject.GetComponent<PickUp>() != null && col.gameObject.GetComponent<PickUp>().playerCache == true)
                {
                  myplayer.GetComponent<Player>().PickUpCache();
                }else
                {
                  //if not the player cache send to item manager to add to inventory
                  if(col.gameObject.GetComponent<PickUp>() != null )
                  {
                      myplayer.GetComponent<Player>().gamemanager.itemManager.ItemPickUp(col.gameObject);
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
                myplayer.GetComponent<Player>().gamemanager.imageFade.StartDmgFade();
                // if(shipRigidBody.velocity.magnitude < 50 )
                // {shipRigidBody.velocity = (shipRigidBody.transform.position - col.contacts[0].point).normalized *  flySpeed * 1.5f;}
                //   else{shipRigidBody.velocity = (shipRigidBody.transform.position - col.contacts[0].point).normalized *  shipRigidBody.velocity.magnitude;}

                shipRigidBody.velocity = (shipRigidBody.transform.position - col.contacts[0].point).normalized *  shipRigidBody.velocity.magnitude;
                shipRigidBody.angularVelocity = (shipRigidBody.transform.position - col.contacts[0].point).normalized *  flySpeed  ;
                  // rb.AddForce((transform.position - col.contacts[0].point).normalized * flySpeed  ,ForceMode.Impulse);
        }
    }

    public void HandleTriggerExit(Collider col,Rigidbody shipRigidBody )
    {
      if (col.gameObject.tag == "Dock")
      {
          myplayer.GetComponent<Player>().NearDock(false);

      }
      else{}
    }


    public void HandleTriggerEnter(Collider col,Rigidbody shipRigidBody )
    {
        if (col.gameObject.tag == "Exit")
        {
            myplayer.GetComponent<Player>().endLevel();

        }
        else if (col.gameObject.tag == "Dock")
        {
            myplayer.GetComponent<Player>().NearDock(true);

        }
        else{}
    }



}
