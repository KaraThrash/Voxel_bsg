using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTypes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ControlCamera(PlayerShipStats playerStats,GameObject cam, GameObject ship)
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
            float step = Mathf.Min(4 * Time.deltaTime, 1.5f);
            // cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, ship.transform.rotation, Time.deltaTime * rollMod * 0.12f * turnSpeed);
            ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, cam.transform.rotation, Time.deltaTime * playerStats.rollMod * 0.3f * playerStats.turnSpeed);

            //TODO: change this '20' to a reasonable variable // cam should be slightly behind ship rotation to give the semse of movement
            cam.GetComponent<ThirdPersonCamera>().rollz = playerStats.roll * playerStats.rollSpeed * 0.8f * Time.deltaTime;

        }
    }

    public float ViperRoll(PlayerControls playerControls, PlayerShipStats playerStats, Rigidbody shipRigidBody)
    {
        if (Input.GetKey(KeyCode.E)) { playerStats.roll = -1; }
        else if (Input.GetKey(KeyCode.Q))
        { playerStats.roll = 1; }
        else { playerStats.roll = 0; }
        // roll feels really sluggish, x10 the rollSpeed so the number is visually easier to work with
        return playerStats.roll * playerStats.rollSpeed * 10 * playerStats.rollMod * Time.deltaTime;


    }


    public Vector3 ViperVelocityStrafe(PlayerControls playerControls,PlayerShipStats playerStats, Rigidbody shipRigidBody)
    {
        Vector3 newvel = Vector3.zero;

        float lift = 0;
        float hort = 0;
        float vert = 0;
        float curdecceleration = playerStats.decceleration;

        if (Input.GetAxis("Horizontal") != 0 && playerControls.UseStamina(playerStats.engineStaminaCost * Time.deltaTime) == true)
        { hort = Input.GetAxis("Horizontal"); }
        else { hort = 0; }

        if (Input.GetAxis("Vertical") != 0 && playerControls.UseStamina(playerStats.engineStaminaCost * Time.deltaTime) == true)
        { lift = Input.GetAxis("Vertical"); }
        else { lift = 0; }


        Vector3 tempvel = shipRigidBody.transform.right * playerStats.strafeSpeed * hort; ;
        Vector3 tempvel3 =  transform.up * playerStats.liftSpeed * lift;

        return tempvel + tempvel3;
         
    }

    public Vector3 ViperVelocityForward(PlayerControls playerControls, PlayerShipStats playerStats, Rigidbody shipRigidBody)
    {
        
        float vert = 0;
        float curdecceleration = playerStats.decceleration;



        //Throttle engine, forward/backward movement, hold second key for reverse otherwise go forward
        if ((Input.GetKey(KeyCode.Space) || Input.GetAxis("9thAxis") > 0) && playerControls.UseStamina(playerStats.engineStaminaCost * Time.deltaTime) == true)
        {

            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton8)))

            {
                vert = -1; curdecceleration = playerStats.decceleration;
            }
            else
            {
                vert = 1; curdecceleration = playerStats.acceleration;
            }


        }
        else { vert = 0; }







        Vector3 tempvel2 = playerStats.flySpeed * vert * shipRigidBody.transform.forward;

        Vector3 targetFlyVelocity = tempvel2;


        //TODO: need an input manager, to make the buttons editable
        if (Input.GetKeyDown(KeyCode.JoystickButton8) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            playerStats.glide = !playerStats.glide; playerStats.glideIndicator.active = playerStats.glide;
        }


        //if the player disables playerStats.glide or runs out of stamina to use it: disable playerStats.glide
        if (vert == 0 )
        {
            if (playerStats.glide == true && playerControls.UseStamina(playerStats.engineStaminaCost * Time.deltaTime) == true)
            {
               return playerControls.GetForwardVelocity();

            }
            else 
            {
                playerStats.glide = false;
                return Vector3.zero;
            }
            

        }
        else
        {
            targetFlyVelocity = Vector3.Lerp(playerControls.GetForwardVelocity(), tempvel2, curdecceleration * Time.deltaTime);
            return targetFlyVelocity;
        }


    }

}
