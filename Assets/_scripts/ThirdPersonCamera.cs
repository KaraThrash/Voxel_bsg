using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class ThirdPersonCamera : MonoBehaviour
{
    public bool playerControlled;

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
    public bool lockCursor = true;
    public float rollz;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    public bool useController;
    public float xRot;
    public float yRot;
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
        
        transform.position = player.transform.position;


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


        // rollz = Mathf.Lerp(rollz, player.transform.rotation.z - transform.rotation.z, Time.deltaTime * ZSensitivity);
       // rollz = Mathf.Lerp(rollz, Input.GetAxis("3rd Axis"), Time.deltaTime * ZSensitivity);
        rollz = InputControls.RollAxis();

        if (InputControls.PreviousButton())
        {
            rollz = -1;
        }else if (InputControls.NextButton())
        {
            rollz = 1;
        }
        m_CharacterTargetRot *= Quaternion.Euler(xRot, yRot, -rollz * ZSensitivity);

        if (xRot == 0 && yRot == 0 && rollz == 0)
        {


        }
        else
        {

            idleTimer = 0;
            currentAcc = 0;

        }

        idleTimer += Time.deltaTime;

        if (idleTimer > timeToResetRotation)
        {
            currentAcc += (Time.deltaTime * idleCameraAcceleration);
            if (currentAcc > 1) { currentAcc = 1; }
           // m_CharacterTargetRot = player.transform.rotation;
        }

        Quaternion rotationDelta = Quaternion.FromToRotation(transform.forward, player.transform.forward);




        if (clampVerticalRotation) { m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot); }


        if (smooth)
        {
            if (target != null)
            {



                //lock on to a place in space based on the target velocity // [better targeting computers give more robust controls?]
                Vector3 targetpos = target.transform.position;

                if (target != player)
                {
                    //reticle.active = true;
                    //reticle.transform.position = targetpos;
                    //reticle.transform.LookAt(transform.position);

                }
                if (target.GetComponent<Rigidbody>() != null)
                {

                    //targetpos = (target.transform.position + target.GetComponent<Rigidbody>().velocity) + player.GetComponent<Rigidbody>().velocity;
                    Vector3 temp = (player.GetComponent<Rigidbody>().velocity).normalized;
                    //should the distance calculation factor in the bullet type?
                    temp *= Vector3.Distance(transform.position, target.transform.position) * 0.1f;
                    targetpos = target.transform.position + target.GetComponent<Rigidbody>().velocity - temp;

                }

                if (xRot == 0 && yRot == 0)
                {

                    if (inputbufferforlockon < 0)
                    {

                        m_CharacterTargetRot = Quaternion.LookRotation(targetpos - transform.position); inputbufferforlockon -= Time.deltaTime;
                    }
                    else { inputbufferforlockon -= Time.deltaTime; }
                }
                else
                {
                    if (inputbufferforlockon < 0.5f)
                    {
                        inputbufferforlockon += Time.deltaTime;
                    }

                }

                if (idleTimer > timeToResetRotation)
                {

                    transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation,
                    resetCameraSmooth * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, m_CharacterTargetRot,
                      smoothTime * Time.deltaTime);
                }



            }
            else
            {

                //rotate camera to catch up to player ship
                if (idleTimer > timeToResetRotation)
                {

                  //  transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation,
                    //    resetCameraSmooth * 1 * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, m_CharacterTargetRot,
                        smoothTime * Time.deltaTime);
                }


            }
        }
        else
        {
            if (target != null)
            {


                //lock on to a place in space based on the target velocity // [better targeting computers give more robust controls?]
                Vector3 targetpos = target.transform.position;
                if (target != player)
                {
                    //reticle.active = true;
                    //reticle.transform.position = targetpos;
                    //reticle.transform.LookAt(transform.position);
                }
                if (target.GetComponent<Rigidbody>() != null)
                {
                    // focus on the area the target will be if a bullet is fired now [ignoring bullet speed]
                    // targetpos = (target.transform.position + (target.GetComponent<Rigidbody>().velocity  ));
                    targetpos = (target.transform.position + (target.GetComponent<Rigidbody>().velocity * Vector3.Distance(transform.position, target.transform.position)));
                    // (target.transform.position + (target.transform.forward * 3 * (Vector3.Distance(transform.position, target.transform.position) * target.GetComponent<Rigidbody>().velocity.magnitude)));
                }


                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetpos - transform.position), lockOnSpeed * Time.deltaTime);
                m_CharacterTargetRot = transform.rotation;
            }
            else
            {
                //reticle.active = false;
                transform.rotation = m_CharacterTargetRot;

            }

            // transform.rotation = m_CameraTargetRot;
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

}
