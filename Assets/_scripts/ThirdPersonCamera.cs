using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject myfwdobj;
    public GameObject player,target;
    public Quaternion newrot;
    public bool inMenu,movetowards;
    public float lockOnSpeed,damping;
    public float flyspeed;
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
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

    public void Update()

    {
        transform.position = player.transform.position;
        if (useController == false)
        {

             yRot = (Input.GetAxis("Mouse X") * XSensitivity) + ((Input.GetAxis("4th Axis") * ( XSensitivity)))  ;
             xRot = (Input.GetAxis("Mouse Y") * YSensitivity) +  ((Input.GetAxis("5th Axis") * -( YSensitivity)))  ;
        }
        else
        {
             yRot = (Input.GetAxis("Mouse X") * XSensitivity) +  Input.GetAxis("4th Axis") * (1 + XSensitivity);
             xRot = (Input.GetAxis("Mouse Y") * YSensitivity) + Input.GetAxis("5th Axis") * -(1 + YSensitivity);
        }

        m_CharacterTargetRot *= Quaternion.Euler(-xRot, yRot, rollz);
        //   m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

          Quaternion rotationDelta = Quaternion.FromToRotation(transform.forward, player.transform.forward);




        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
          if(target != null){


            //lock on to a place in space based on the target velocity // [better targeting computers give more robust controls?]
            Vector3 targetpos = target.transform.position;
            if(target.GetComponent<Rigidbody>() != null)
            {  targetpos = (target.transform.position + target.GetComponent<Rigidbody>().velocity);}


            m_CharacterTargetRot = Quaternion.LookRotation ( targetpos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);


            }
            else{
              //rotate camera to catch up to player ship
              transform.rotation = Quaternion.Slerp(transform.rotation, m_CharacterTargetRot,
                  smoothTime * Time.deltaTime);

                  //rotate ship to catch up to camera
              // player.transform.rotation = Quaternion.Slerp(player.transform.rotation, m_CharacterTargetRot,
              //     smoothTime * 18 * Time.deltaTime);


            }
        }
        else
        {
          if(target != null){


            //lock on to a place in space based on the target velocity // [better targeting computers give more robust controls?]
            Vector3 targetpos = target.transform.position;
            if(target.GetComponent<Rigidbody>() != null)
            {
              // focus on the area the target will be if a bullet is fired now [ignoring bullet speed]
                // targetpos = (target.transform.position + (target.GetComponent<Rigidbody>().velocity  ));
                targetpos = (target.transform.position + (target.GetComponent<Rigidbody>().velocity * Vector3.Distance(transform.position, target.transform.position) ));
                // (target.transform.position + (target.transform.forward * 3 * (Vector3.Distance(transform.position, target.transform.position) * target.GetComponent<Rigidbody>().velocity.magnitude)));
            }


              transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation ( targetpos - transform.position), lockOnSpeed * Time.deltaTime);
              m_CharacterTargetRot = transform.rotation;
          }
          else{

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
