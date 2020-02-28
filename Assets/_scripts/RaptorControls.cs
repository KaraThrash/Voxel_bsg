using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorControls : MonoBehaviour
{
  private Vector3 newvel;
  public GameObject myplayer;
  public GameObject camera;
  public GameObject camerasphere;
  public GameObject behind;
  public Quaternion targetRotation;


  private Rigidbody rb;
  public int turnSpeed;
  public float flySpeed,brakeSpeed;
  public float  strafeSpeed;
  public float liftSpeed;
  private float lift;
  private float hort;
  private float vert;
  private float roll;
  public float rollSpeed;
  public float rollMod;
  private float mouseX;
  private float mouseY;
  public GameObject gun1;
  public GameObject gun2;
  public GameObject bullet;
  public float guncooldown;
  public float guncooldowntimer;
  public float cameraspeed;
  public float step,groundCollisionTimer;

  public float distspeed;
  public int heldresource;
// Use this for initialization
void Start () {

      cameraspeed = 15;
      distspeed = 12;
  }
public void SetUp(GameObject newplayer,GameObject newcam)
{
  myplayer = newplayer;
  camera = newcam; camerasphere = newcam;
}

public void Fly (Rigidbody shipRigidBody)  {
     //KeyboardFlightControls();

     if(groundCollisionTimer <= 0)
     {
       thirdpersonflightcontrols(shipRigidBody);

     }
      else{

        groundCollisionTimer -= Time.deltaTime;

      }


      // ControllerFlight();

  }

  public void WeaponSystems()
  {
    if (guncooldowntimer > 0)
    {

        guncooldowntimer -= Time.deltaTime;
    }

    if (Input.GetMouseButton(0))
    {

        if (guncooldowntimer <= 0)
        {

            RaycastShootGuns();

            guncooldowntimer = guncooldown;
        }

    }
  }


  public void ControlCamera(GameObject cam,GameObject ship)
  {

    //default is camera tracks with mouse // needs to toggle on free look
    // if (Input.GetMouseButton(1))
    // {
    //     cam.GetComponent<ThirdPersonCamera>().rollz = 0;
    // }
    // else {}
      cam.transform.position = ship.transform.position;
      // targetRotation = Quaternion.LookRotation((camera.transform.position + camera.transform.forward) - transform.position);
      step = Mathf.Min(rollSpeed * Time.deltaTime, 1.5f);

      ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, cam.transform.rotation, step);

      cam.GetComponent<ThirdPersonCamera>().rollz = roll * 20 * Time.deltaTime ;


  }


  public void thirdpersonflightcontrols(Rigidbody shipRigidBody)
  {
      hort = Input.GetAxis("Horizontal");
      vert = Input.GetAxis("Vertical");





      if (Input.GetKey(KeyCode.D)) { roll = -1; }
      else if (Input.GetKey(KeyCode.A))
      { roll = 1; }
      else { roll = 0; }

      shipRigidBody.transform.Rotate(0, 0, roll * rollSpeed * Time.deltaTime);


      shipRigidBody.AddForce(shipRigidBody.transform.forward * (flySpeed * vert) );

      //brakes
      if (Input.GetMouseButton(1))
      {shipRigidBody.velocity = Vector3.Lerp(shipRigidBody.velocity,Vector3.zero,brakeSpeed * Time.deltaTime);}


  }


  public void RaycastShootGuns()
  {
      if (bullet.GetComponent<Bullet>().lance == true)
      {
          GameObject clone = Instantiate(bullet, gun1.transform.position, gun1.transform.rotation) as GameObject;
          clone.transform.parent = this.transform;
          GameObject clone2 = Instantiate(bullet, gun2.transform.position, gun2.transform.rotation) as GameObject;
          clone2.transform.parent = this.transform;

      }
      else
      {
          Instantiate(bullet, gun1.transform.position, gun1.transform.rotation);
          Instantiate(bullet, gun2.transform.position, gun2.transform.rotation);
      }

  }

  public void HandleCollisionEnter(Collision col,Rigidbody shipRigidBody )
  {
      if (col.gameObject.tag == "BulletEnemy")
      {
          myplayer.GetComponent<Player>().vehicletakingdamage(1);

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
      if (col.gameObject.tag == "Enviroment" )
      {
        //bounce off the ground on contact
        //TODO calculate the right amount of bounce
          groundCollisionTimer = 0.1f;
          myplayer.GetComponent<Player>().gamemanager.imageFade.StartDmgFade();
          shipRigidBody.velocity = (shipRigidBody.transform.position - col.contacts[0].point).normalized *  flySpeed;
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
  // public void OnCollisionEnter(Collision col)
  // {
  //     HandleCollisionEnter(col,rb);
  // }
  //
  //
  // public void OnTriggerEnter(Collider col)
  // {
  //     HandleTriggerEnter(col,rb);
  // }
  //
  // public void OnTriggerExit(Collider col)
  // {
  //   HandleTriggerExit(col,rb);
  //
  // }

}
