using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialActions : MonoBehaviour
{
  public PlayerControls playerControls;
  public string lastKeyPressed;
  public float timeForDoublePress = 0.2f;

  public string leftkey,rightkey,upkey,downkey;

    public GameObject reticle, markedTarget;
  private float keyPressedTimer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(keyPressedTimer > 0){keyPressedTimer -= Time.deltaTime;}
        if (markedTarget != null)
        {
            reticle.active = true;
            Vector3 targetpos = markedTarget.transform.position;

            if (markedTarget.GetComponent<Rigidbody>() != null)
            {
                targetpos = (markedTarget.transform.position + (markedTarget.GetComponent<Rigidbody>().velocity ));
            }
            reticle.transform.position = targetpos;
            reticle.transform.LookAt(playerControls.transform.position);
        }
        else { reticle.active = false; }

    }

    public void ListenToButtonPresses()
    {



              if(Input.GetKeyDown(leftkey))
              {CheckForDoublePress(leftkey); return;}
              if(Input.GetKeyDown(rightkey))
              {CheckForDoublePress(rightkey);return; }
              if(Input.GetKeyDown(upkey))
              {CheckForDoublePress(upkey); return;}
              if(Input.GetKeyDown(downkey))
              {CheckForDoublePress(downkey);return; }

             // if(Input.GetKeyDown("1"))
             if(Input.GetKeyDown(KeyCode.Alpha1))
             {LockOn();}
            if(Input.GetKeyDown(KeyCode.Alpha2))
             {MarkTarget();}
             // if(Input.GetKeyDown(KeyCode.JoystickButton1))
             // {DodgeRoll(vKey);}


             //controller
            //
             // if(Input.GetKeyDown(JoystickButton0))
             // {CheckForDoublePress(leftkey); return;}
             //
             // if(Input.GetKeyDown(leftkey))
             // {CheckForDoublePress(leftkey); return;}
             // if(Input.GetKeyDown(rightkey))
             // {CheckForDoublePress(rightkey);return; }
             // if(Input.GetKeyDown(upkey))
             // {CheckForDoublePress(upkey); return;}
             // if(Input.GetKeyDown(downkey))
             // {CheckForDoublePress(downkey);return; }

            if(Input.GetKeyDown(KeyCode.JoystickButton3))
            {LockOn();}



    }
    public void CheckForDoublePress(string keypressed)
    {
      if(keypressed == lastKeyPressed)
      {
        DodgeRoll(keypressed);
        keyPressedTimer = 0;
        lastKeyPressed = "xxx";

      }
        else
        {
          keyPressedTimer = timeForDoublePress;
          lastKeyPressed = keypressed;
        }
    }

    // public void ListenToButtonPresses()
    // {
    //   foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
    //          if(Input.GetKeyDown(vKey)){
    //          //your code here
    //         if(vKey == lastKeyPressed)
    //         {
    //           DodgeRoll(vKey);
    //           keyPressedTimer = 0;
    //
    //         }
    //           else
    //           {
    //             keyPressedTimer = timeForDoublePress;
    //             lastKeyPressed = vKey;
    //           }
    //
    //          // if(Input.GetKeyDown("1"))
    //          if(Input.GetKeyDown(KeyCode.Alpha1))
    //          {LockOn();}
    //          if(Input.GetKeyDown(KeyCode.JoystickButton1))
    //          {DodgeRoll(vKey);}
    //
    //          }
    //      }
    //
    // }



    public void LockOn()
    {
      if(playerControls.lockOnTarget == null)
      {
        playerControls.lockOnTarget = GetComponent<Player>().gamemanager.npcManager.GetClosestEnemy(playerControls.playerShip);
        playerControls.camerasphere.GetComponent<ThirdPersonCamera>().target = playerControls.lockOnTarget;
      }
        else{playerControls.lockOnTarget = null; playerControls.camerasphere.GetComponent<ThirdPersonCamera>().target = null;}

    }

    public void MarkTarget()
    {
        if (markedTarget == null)
        {
            markedTarget = GetComponent<Player>().gamemanager.npcManager.GetClosestEnemy(playerControls.playerShip);
        }
        else { markedTarget = null; reticle.active = false ; }

    }

    public void DodgeRoll(string direction)
    {
        playerControls.AttemptDodgeRoll();
      // if(direction == KeyCode.W){playerControls.GetComponent<Rigidbody>().velocity = playerControls.transform.forward * 350; playerControls.lockOutEngines = 0.1f;}
      // if(direction == KeyCode.S){playerControls.GetComponent<Rigidbody>().velocity = playerControls.transform.forward * -350; playerControls.lockOutEngines = 0.1f;}
      // if(direction == KeyCode.A){playerControls.GetComponent<Rigidbody>().velocity = playerControls.transform.right * -350; playerControls.lockOutEngines = 0.1f;}
      // if(direction == KeyCode.D){playerControls.GetComponent<Rigidbody>().velocity = playerControls.transform.right * 350; playerControls.lockOutEngines = 0.1f;}
      // if(direction == KeyCode.LeftShift){playerControls.GetComponent<Rigidbody>().velocity = playerControls.transform.up * -350; playerControls.lockOutEngines = 0.1f;}
      //   if(direction == KeyCode.Space){playerControls.GetComponent<Rigidbody>().velocity = playerControls.transform.up * 350; playerControls.lockOutEngines = 0.1f;}
    }

}
