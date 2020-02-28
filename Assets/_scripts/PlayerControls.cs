using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
  public GameObject playerShip,viperShip,raptorShip;
  public GameObject camera;
  public GameObject camerasphere;
  public bool inMenu;

  private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
              rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Y))
      {ChangeShip();}
    }

    public void ControlShip()
    {
          if(playerShip.GetComponent<ViperControls>() != null)
          {
            playerShip.GetComponent<ViperControls>().Fly(rb);
            playerShip.GetComponent<ViperControls>().WeaponSystems();
            playerShip.GetComponent<ViperControls>().ControlCamera(camerasphere,this.gameObject);
          }else
          {
            if(playerShip.GetComponent<RaptorControls>() != null)
            {
              playerShip.GetComponent<RaptorControls>().Fly(rb);
              playerShip.GetComponent<RaptorControls>().WeaponSystems();
              playerShip.GetComponent<RaptorControls>().ControlCamera(camerasphere,this.gameObject);
            }
          }
    }

    public void ChangeShip()
    {
          if(viperShip.active == true )
          {
            viperShip.active = false;
            playerShip = raptorShip;
            raptorShip.active = true;


          }else
          {
              raptorShip.active = false;
            viperShip.active = true;
            playerShip = viperShip;

          }
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
        }
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
        }
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
        }
      }


    }
}
