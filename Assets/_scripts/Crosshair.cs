using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Actor actor;
    public Transform cam;

    public Transform target_Highlight;
    public Transform velocityLead_Highlight;
    public Transform isTargetingPlayer_Highlight;


    public bool isTargeted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTargeted)
        {
            if (actor && cam)
            { SetPlacement(); }
        }
    }

    public void Init(float _sensorLevel)
    {
        if (target_Highlight )
        {
            if ( _sensorLevel >= 1)
            {
                target_Highlight.gameObject.SetActive(true);
            }
            else { target_Highlight.gameObject.SetActive(false); }
        }

        if (velocityLead_Highlight)
        {
            if (_sensorLevel >= 1)
            {
                velocityLead_Highlight.gameObject.SetActive(true);
            }
            else { velocityLead_Highlight.gameObject.SetActive(false); }
        }

        if (isTargetingPlayer_Highlight)
        {
            if (_sensorLevel >= 1)
            {
                isTargetingPlayer_Highlight.gameObject.SetActive(true);
            }
            else { isTargetingPlayer_Highlight.gameObject.SetActive(false); }
        }

    }

    public void SetActor(Actor _actor)
    {
        actor = _actor;
    }

    public void SetCamera(Transform _cam)
    {
        cam = _cam;
    }



    public void SetPlacement()
    {
        if (cam == null || actor == null) { return; }

        if (target_Highlight)
        {
            target_Highlight.position = actor.MainTransform().position;
            target_Highlight.LookAt(cam.position);

        }

        if (velocityLead_Highlight)
        {
            Vector3 vel_dist = (actor.RB().velocity * 0.02f) * (Vector3.Distance(actor.MainTransform().position,cam.position) * 0.02f);
            velocityLead_Highlight.position = actor.MainTransform().position + actor.RB().velocity + vel_dist ;
            velocityLead_Highlight.LookAt(cam.position);

        }

        if (isTargetingPlayer_Highlight)
        {
            isTargetingPlayer_Highlight.position = actor.MainTransform().position;
            isTargetingPlayer_Highlight.LookAt(cam.position);
        }
        
    }


    public Vector3 GetVelocityLeadPosition()
    {
        if (velocityLead_Highlight)
        {

            if (velocityLead_Highlight.gameObject.activeSelf)
            {

                return velocityLead_Highlight.position;


            }
            else 
            {
                return actor.MainTransform().position;
            }

        }
        return actor.MainTransform().position + ((actor.RB().velocity * 0.02f) * (Vector3.Distance(actor.MainTransform().position, cam.position) * 0.02f));
    }


    public void StopTargeting()
    {
        if (target_Highlight)
        {
            target_Highlight.gameObject.SetActive(false); 
        }

        if (velocityLead_Highlight)
        {
             velocityLead_Highlight.gameObject.SetActive(false); 
        }

        if (isTargetingPlayer_Highlight)
        {
             isTargetingPlayer_Highlight.gameObject.SetActive(false); 
        }

    }

}
