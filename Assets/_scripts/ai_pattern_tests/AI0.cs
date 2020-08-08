using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AI0 : MonoBehaviour
{
    public int currentAttackPlan, currentPatrolPoint;

    // parameters to change/set how the ship controls
    public float speed = 50, walkspeed = 10;
    public float rotForce = 6, rotModifier;
    public float accuracy = 1, acceleration = 0.2f;

    public float closedistance = 10, fardistance = 40, plusminus = 5;
    public float gunCost = 1;
    public float checkForwardDistance = 100.0f;
    //

    public float gunCooldown;

    public float avoidCollisionClock, seenTimer,distanceToLastCollisionObj,timeInCurrentPosition,distanceToCountAsMoving = 10;

    public bool destroyed, canShoot;
    public bool flyaway, flypast;

    public GameObject  patrolparent, patroltarget, currentTarget,objToAvoid;
    public GameObject debug0, debug1, debug2;
    public Material avoidingCollisionColor, patrolColor;
    public Renderer myRenderer;
    public List<Material> colors;

    private Vector3 straferunspot, tempTargetSpot, openSpotToAvoidCollision, lastPosition ;
    private Quaternion targetRotation;
    private Rigidbody rb;
    private Enemy myEnemy;
    // Use this for initialization
    void Awake()
    {

        rb = GetComponent<Rigidbody>();
        myEnemy = GetComponent<Enemy>();
        patrolparent = myEnemy.patrolparent;
        patroltarget = myEnemy.patroltarget;
        if (colors.Count > 0 && myRenderer != null)
        { transform.GetChild(0).GetComponent<Renderer>().material = colors[Random.Range(0, (int)colors.Count)]; }
        if (myEnemy.patrolparent != null) { patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject; }

    }


    public void Fly(GameObject target)
    {

        myEnemy.RechargeStamina();
        //check if the ship has been in the same area for too long, indicating it is stuck
        if (Vector3.Distance(transform.position, lastPosition) > distanceToCountAsMoving)
        {
            lastPosition = transform.position;
            timeInCurrentPosition = 0;
        }
        else
        {
            timeInCurrentPosition += Time.deltaTime;
            if (timeInCurrentPosition > 10)
            {

                transform.position = transform.position - (transform.forward * 2);
                transform.Rotate(25, 25, 25);
                timeInCurrentPosition = 0;
            }
        }
        if (myEnemy.target != null && myEnemy.inCombat == true)
        {

            Attack(target);
        }
        else
        {
            seenTimer = 1;
            if (patroltarget != null) { Patrol(); }
            else
            {
                if (myEnemy.patrolparent != null)
                {

                    patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject;
                }
            }

            if (gunCooldown <= 0)
            {
                gunCooldown = gunCost;

                if (myEnemy.inBattle == true)
                { myEnemy.FindTarget(); }
                else { myEnemy.CheckToNoticePlayer(); }


            }



        }
        if (gunCooldown > 0)
        { gunCooldown -= Time.deltaTime; }

    }



    public void Attack(GameObject target)
    {
        

        if (target != null)
        {
            Chase(target);
        }
   

    }



    public bool RaycastAtSpace(Vector3 currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 3.0f,(currenttarget - transform.position).normalized, out hit, distanceToCheck))
        {

            return false;

        }
        return true;

    }


    public bool RaycastAtTarget(Transform currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, currenttarget.position - transform.position, out hit, distanceToCheck))
        {

            if (hit.transform == currenttarget)
            {
                return true;
            }
        }
        return false;

    }

    public RaycastHit GetHitRaycastAtTarget(Transform currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;
        Physics.Raycast(transform.position, currenttarget.position - transform.position, out hit, distanceToCheck);
        return hit;

    }



    public void Chase(GameObject targetship)
    {


        targetRotation = Quaternion.LookRotation((targetship.transform.position) - transform.position);

        float angle = 0;
        RaycastHit hit;

        //Vector3.Distance(transform.position, targetship.transform.position)
        if (Physics.SphereCast(transform.position, 4, transform.forward, out hit, rb.velocity.magnitude + 2))
        {
            if (hit.transform.gameObject != targetship && Physics.SphereCast(transform.position, 2, transform.forward, out hit, Mathf.Min(Vector3.Distance(transform.position, targetship.transform.position), rb.velocity.magnitude + 5)))
            {

                targetRotation = Quaternion.LookRotation((((targetship.transform.position - hit.point).normalized * 2) + transform.position) - transform.position);
                angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce *  Time.deltaTime);
                if (angle <= accuracy && Vector3.Distance(transform.position,targetship.transform.position) > fardistance) { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime); }
                else { rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime); }
                debug0.transform.position = hit.point;

            }
            else
            {//narrow forward clear just advance dont rotate

                targetRotation = Quaternion.LookRotation((((targetship.transform.position - hit.point).normalized * 2) + transform.position) - transform.position);
                angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
                if (RaycastAtTarget(targetship.transform))
                { rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime); }
                else {  rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime); }
                debug1.transform.position = hit.point;

            }


        }
        else
        {   //wide forward clear, rotate freely
            targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
            angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            if (angle <= accuracy && Vector3.Distance(transform.position, targetship.transform.position) > fardistance) { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime); }
            else { if(Vector3.Distance(transform.position, targetship.transform.position) < closedistance){ rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime); } }
            debug2.transform.position = hit.point;
        }

        

        if (angle <= accuracy && gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

        }

    }

  

    //find a point closest to this enemy that is also closer to the target
    public void FindClosestForwardPatrol(GameObject targetship)
    {
        //GameObject lastspot = currentTarget;
        //float distanceToTarget = Vector3.Distance(transform.position, patroltarget.transform.position);
        //float distanceToMe = Vector3.Distance(transform.position, patroltarget.transform.position);
        //foreach (Transform el in myEnemy.patrolparent.transform)
        //{
        //    if (el.gameObject != lastspot && Vector3.Distance(el.position, patroltarget.transform.position) < distanceToTarget && Vector3.Distance(el.position, transform.position) < distanceToMe && Vector3.Distance(el.position, transform.position) > 5)
        //    {
        //        currentTarget = el.gameObject;
        //        distanceToMe = Vector3.Distance(el.position, transform.position);
        //    }
        //}

    }

    public void MoveToAltPatrolSpot()
    {
        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.transform.position) < 3)
        {
            FindClosestForwardPatrol(currentTarget);
        }

        //targetRotation = Quaternion.LookRotation((transform.position - transform.forward) - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, -rotForce * Time.deltaTime);

    }

    public void Patrol()
    {
        float angle = 0;
        RaycastHit hit;
      
        if (Vector3.Distance(patroltarget.transform.position, transform.position) <= 2 || Vector3.Distance(patroltarget.transform.position, transform.position) > 150)
        {

            currentPatrolPoint++;
            if (currentPatrolPoint >= myEnemy.patrolparent.transform.childCount)
            { currentPatrolPoint = 0; }
            patroltarget = myEnemy.patrolparent.transform.GetChild(currentPatrolPoint).gameObject;

        }
    

        if (Physics.SphereCast(transform.position, 5, transform.forward, out hit, Mathf.Min(Vector3.Distance(transform.position, patroltarget.transform.position), rb.velocity.magnitude + 2)))
        {
            if (Physics.SphereCast(transform.position, 3, transform.forward, out hit, Mathf.Min(Vector3.Distance(transform.position, patroltarget.transform.position), rb.velocity.magnitude + 2)))
            {

                targetRotation = Quaternion.Inverse(Quaternion.LookRotation( hit.point - transform.position )) ;
                print(transform.position - (hit.point - transform.position));
                angle = Vector3.Angle(patroltarget.transform.position - hit.point, transform.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * rotModifier * Time.deltaTime);
                if (angle <= accuracy) { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime); }
                //else { rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime); }
                //debug0.transform.position = hit.point;

            }
            else 
            {//narrow forward clear just advance dont rotate
                rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime);
                //debug1.transform.position = transform.position + transform.forward;
            }


        }
        else 
        {   //wide forward clear, rotate freely
            targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);
            angle = Vector3.Angle(patroltarget.transform.position - transform.position, transform.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            if (angle <= accuracy) { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime); }
            else { rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime); }
            //debug2.transform.position = transform.position + transform.forward;
        }


       
    }


   
    public Vector3 RayCastToFindOpening()
    {
        RaycastHit hit;
        Vector3 raydir = (transform.forward + (transform.right * 0.2f));
        if (Physics.Raycast(transform.position, raydir, out hit, checkForwardDistance))
        { raydir = (transform.forward - (transform.right * 0.2f)); }
        else
        {
            return raydir;

        }
        if (Physics.Raycast(transform.position, raydir, out hit, checkForwardDistance))
        { raydir = (transform.forward - (transform.up * 0.2f)); }
        else
        {
            return raydir;

        }
        if (Physics.Raycast(transform.position, raydir, out hit, checkForwardDistance))
        { raydir = (transform.forward + (transform.up * 0.2f)); }
        else
        {
            return raydir;



        }
        if (Physics.Raycast(transform.position, raydir, out hit, checkForwardDistance))
        { return (-transform.forward); }
        else
        {

            return raydir;


        }
       
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enviroment") { if (avoidCollisionClock <= 0) { currentAttackPlan = 3; } }
    }

    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Enviroment" && currentAttackPlan == 3) {  }
    }


}