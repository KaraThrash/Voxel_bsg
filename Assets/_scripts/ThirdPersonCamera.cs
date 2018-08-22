using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject myfwdobj;
    public GameObject target;
    public Quaternion newrot;
    public bool movetowards;
    public float damping;
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

    public void Start()
    {
        m_CharacterTargetRot = transform.rotation;

    }
    public void Update()

    {
        transform.position = target.transform.position;
        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

       
        m_CharacterTargetRot *= Quaternion.Euler(-xRot, yRot, rollz);
        //   m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);
        Quaternion rotationDelta = Quaternion.FromToRotation(transform.forward, target.transform.forward);


        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            transform.rotation = m_CharacterTargetRot;
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
