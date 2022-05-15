using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class ThirdPersonCamera : MonoBehaviour
{
    public bool playerControlled;

    public Transform internalSphere;
    public GameObject realCamera,myfwdobj,reticle;
    public GameObject player,target;
    public Quaternion newrot;
    public bool inMenu,movetowards;
    public float lockOnSpeed,damping,inputbufferforlockon; 
    public float flyspeed,distanceToCatchPlayer;

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public float ZSensitivity = 2f;

    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public float angleLimit = 15f; // when reangling the camera from stand still how in line does it need to be before it goes fulling back to player control
    public bool lockCursor = true;
    public float rollz;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private Quaternion idleRot;
    private bool m_cursorIsLocked = true;

    public bool useController;
    public float xRot;
    public float yRot;

    private Vector3 targetPosition;
    private Map currentMap;



    public void Start()
    {
     //   InputControls.cameraHorizontal = "4th Axis"; InputControls.cameraVertical = "5th Axis";
        m_CharacterTargetRot = transform.rotation;

    }
    public void SetInMenu(bool menuOn)
    {
      inMenu = menuOn;
      if(inMenu == true)
      {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      }
    }

    public void ResetCameraAngle()
    {
      m_CharacterTargetRot = player.transform.rotation;
      transform.rotation = player.transform.rotation;
    }


    public float idleTimer; //how long after no camera input to change the rotation back
    public float resetCameraSmooth = 2.0f, timeToResetRotation = 1;
    public float idleCameraAcceleration = 0.2f;
    public float currentAcc;


    public void Update()

    {
        
        transform.position = player.transform.position ;


        if (!playerControlled)
        {
            ShipControlled();
        }
    }


    public void ShipControlled()
    {
       


               
                    transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation,
                        resetCameraSmooth * 1 * Time.deltaTime);
               


            
        
        

       // UpdateCursorLock();

    }

    public void PlayerControlled()
    {
        yRot = (Input.GetAxis("Mouse X") * XSensitivity) + (InputControls.CameraHorizontalAxis() * XSensitivity);
        xRot = (Input.GetAxis("Mouse Y") * -YSensitivity) + (InputControls.CameraVerticalAxis() * YSensitivity);


        rollz = InputControls.RollAxis();

        if (InputControls.PreviousButton())
        {
            rollz = -1;
        }
        else if (InputControls.NextButton())
        {
            rollz = 1;
        }
        //m_CharacterTargetRot *= Quaternion.Euler(xRot, yRot, -rollz * ZSensitivity);

        //if (xRot == 0 && yRot == 0 && rollz == 0)
        //{ }
        //else
        //{
        //    idleTimer = 0;
        //    currentAcc = 0;
        //}

        if (InputControls.CheckAxis(Axises.Thrust) == 1 || player.GetComponent<Rigidbody>().velocity.magnitude > 1)
        {
            targetPosition = player.transform.position ;
            idleTimer = -1;
            idleRot = internalSphere.rotation;
           // currentAcc = 0;
        }
        else 
        {
            idleTimer += Time.deltaTime;
        }



       


        //if (idleTimer > timeToResetRotation)
        //{
        //    currentAcc += (Time.deltaTime * idleCameraAcceleration);
        //    if (currentAcc > 1) { currentAcc = 1; }
        //}

        if (clampVerticalRotation) { m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot); }


        if (Map() && Map().OutOfBounds(transform.position ))
        {
            TurnBack();

            return;
        }


        if (smooth)
        {

            if (idleTimer == -2)
            {


                internalSphere.Rotate(new Vector3(xRot, yRot, -rollz * ZSensitivity) * Time.deltaTime * smoothTime);
            }
            else

            {
                if (Vector3.Angle(internalSphere.forward , transform.forward) < angleLimit)
                {
                    transform.Rotate(new Vector3(xRot, yRot, -rollz * ZSensitivity) * Time.deltaTime * smoothTime);
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, idleRot, Time.deltaTime * flyspeed * smoothTime);
                }

                internalSphere.localRotation = Quaternion.Lerp(internalSphere.localRotation, Quaternion.identity, Time.deltaTime * flyspeed * smoothTime);



            }

        }
        else
        {
          
            internalSphere.localRotation = Quaternion.identity;
            transform.rotation = m_CharacterTargetRot;



        }


        UpdateCursorLock();

    }


    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
          if(inMenu == false){
                  if (Input.GetKeyUp(KeyCode.Escape))
                  {
                      m_cursorIsLocked = false;
                  }
                  else if (Input.GetMouseButtonUp(0))
                  {
                      m_cursorIsLocked = true;
                  }

                  if (m_cursorIsLocked)
                  {
                      Cursor.lockState = CursorLockMode.Locked;
                      Cursor.visible = false;
                  }
                  else if (!m_cursorIsLocked)
                  {
                      Cursor.lockState = CursorLockMode.None;
                      Cursor.visible = true;
                  }
          }else{
             Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
          }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }


    public void TurnBack()
    {
        Quaternion newrot = Quaternion.LookRotation(Map().CenterOfMap() - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.deltaTime * smoothTime);
        //   RB().velocity = transform.forward *  PrimaryEngine().STAT_Power();

    }

    public Map Map()
    {
        if (currentMap == null)
        {
            currentMap = FindObjectOfType<Map>();
        }

        return currentMap;
    }
}
