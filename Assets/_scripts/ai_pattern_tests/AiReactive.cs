﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiReactive : MonoBehaviour
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

    public float gunCooldown,phaseChangeTimer, phaseTime = 5.0f;

    public float avoidCollisionClock, seenTimer;

    public bool destroyed, canShoot;
    public bool flyaway, flypast;

    public GameObject debugWhereToObj, debugLastSeen, patrolparent, patroltarget;
    public Material avoidingCollisionColor, patrolColor;
    public Renderer myRenderer;
    public List<Material> colors;

    private Vector3 straferunspot, tempTargetSpot, openSpotToAvoidCollision, targetLastSeen;
    private Quaternion targetRotation;
    private Rigidbody rb;
    private Enemy myEnemy;
    // Use this for initialization
    void Awake()
    {
        debugWhereToObj.transform.parent = null;
        debugLastSeen.transform.parent = null;
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
        debugLastSeen.active = myEnemy.inCombat;
        if (myEnemy.target != null && myEnemy.inCombat == true)
        {

            Attack(target);
        }
        else
        {
            currentAttackPlan = 0;
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
        // if (gunCooldown > 0)
        // { gunCooldown -= Time.deltaTime; }
        gunCooldown -= Time.deltaTime;

    }



    public void Attack(GameObject target)
    {
        CheckForward(target);

        if (target != null)
        {

            AttackPlans(target);


        }

        //else
        //{
        //    AvoidCollision();
        //}

    }


    public void AttackPlans(GameObject target)
    {

        phaseChangeTimer += Time.deltaTime;
        if (phaseChangeTimer > phaseTime)
        { 
            phaseChangeTimer = 0;
            currentAttackPlan++;
        }

        switch (currentAttackPlan)
        {
            case -1:

                if (RaycastAtTarget(target.transform) == true)
                {
                    seenTimer += Time.deltaTime * 2;
                    if (seenTimer > 5.0f) { seenTimer = 5.0f; }
                    GetBehind(target);
                    if (Vector3.Distance(transform.position, target.transform.position) > fardistance)
                    { currentAttackPlan = 0; }
                }
                else
                { currentAttackPlan = 1; }
                break;
            case 0: //Can see player
                if (RaycastAtTarget(target.transform) == true)
                {
                    Chase(target);

                }
                //else
                //{ currentAttackPlan = 1; }


                break;
            case 1: //Had seen player, then player left sight: move to last known location
                if (RaycastAtTarget(target.transform) == true)
                {
                    float angle = Vector3.Angle(transform.position - target.transform.position, target.transform.forward);
                    if (angle <= accuracy)
                    {
                        currentAttackPlan = 2;
                        phaseChangeTimer = 0;
                    }
                    else
                    {
                        currentAttackPlan = 0;
                        phaseChangeTimer = 0;
                    }
                }
                else
                {
                    //myEnemy.inCombat = false;
                }


                break;
            case 2: //Can not see player, and last know location empty, move to the closest patrol spot in general forward direction, to investigate before leashing
                if (RaycastAtTarget(target.transform) == true)
                { CircleStrafe(target); }

                    
             
                break;
            case 3:
                if (RaycastAtTarget(target.transform) == true)
                {
                    float angle = Vector3.Angle(transform.position - target.transform.position, target.transform.forward);
                    if (angle <= accuracy)
                    {
                        currentAttackPlan = 4;
                        phaseChangeTimer = 0;
                    }
                    else
                    {
                        //currentAttackPlan = 0;
                        //phaseChangeTimer = 0;
                    }
                }
                else
                {
                    //myEnemy.inCombat = false;
                }
                break;
            case 4:
                if (RaycastAtTarget(target.transform) == true)
                { CircleStrafeUp(target); }
                break;
            case 5:
                if (target.GetComponent<PlayerControls>().GetLastAction() == "attack")
                {
                    currentAttackPlan = 6;
                    phaseChangeTimer = 0;
                }
                else
                {
                    currentAttackPlan = 0;
                    phaseChangeTimer = 0;
                }
                break;
            case 6:
                StandAndShoot(target);
                if (target.GetComponent<PlayerControls>().GetLastAction() == "attack")
                {
                    phaseChangeTimer = 0;
                }
                break;
            default:
  
                currentAttackPlan = 0;
                phaseChangeTimer = 0;
                break;
        }

    }

    public bool RaycastAtSpace(Vector3 currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, currenttarget - transform.position, out hit, 1500.0f))
        {

            return false;

        }
        return true;

    }
    public bool RaycastAtTarget(Transform currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, currenttarget.position - transform.position, out hit, 1500.0f))
        {

            if (hit.transform.gameObject.tag == "Player")
            {
                targetLastSeen = hit.point;
                return true;
            }
        }
        return false;

    }

    public void StandAndShoot(GameObject targetship)
    {
        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
        if ( gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 1.0f);

        }
        if (Vector3.Distance(transform.position, targetship.transform.position) < closedistance)
        { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * -speed, Time.deltaTime * acceleration); }
    }



        public void GetBehind(GameObject targetship)
    {

        RaycastAtTarget(targetship.transform);

        Vector3 targetloc = targetship.transform.position - (targetship.transform.forward * 10) + openSpotToAvoidCollision;
        debugWhereToObj.transform.position = targetloc;
        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        float angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);


        if (Vector3.Distance(transform.position, targetloc) >= closedistance * 0.5f)
        { rb.velocity = Vector3.Lerp(rb.velocity, (targetloc - transform.position).normalized * speed, Time.deltaTime * acceleration); }
        //else
        //{ rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime); }

        if (angle <= accuracy && gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

        }

    }

    public void Chase(GameObject targetship)
    {

        RaycastAtTarget(targetship.transform);

        targetLastSeen = targetship.transform.position;
        debugLastSeen.transform.position = targetLastSeen;
        targetRotation = Quaternion.LookRotation((targetship.transform.position ) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        float angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

        rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime * acceleration);


        if (angle <= accuracy && gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

        }

    }

    public void CircleStrafe(GameObject targetship)
    {
        debugLastSeen.transform.position = targetLastSeen;
        targetRotation = Quaternion.LookRotation((targetship.transform.position ) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        float angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);
        Vector3 newvel = transform.right;
        if (Vector3.Distance(transform.position, targetship.transform.position) < closedistance)
        { newvel = (transform.right + transform.forward).normalized; }

        
        rb.velocity = Vector3.Lerp(rb.velocity, newvel * speed, Time.deltaTime * acceleration);


        if (angle <= accuracy && gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

        }

    }

    public void CircleStrafeUp(GameObject targetship)
    {
        debugLastSeen.transform.position = targetLastSeen;
        targetRotation = Quaternion.LookRotation((targetship.transform.position) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        float angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

        Vector3 newvel = transform.up;
        if (Vector3.Distance(transform.position, targetship.transform.position) < closedistance)
        { newvel = (transform.right + transform.forward).normalized; }


        rb.velocity = Vector3.Lerp(rb.velocity, newvel * speed, Time.deltaTime * acceleration);


        if (angle <= accuracy && gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

        }

    }


    public void Chicken(GameObject targetship)
    {



        Vector3 tempTargetPos = targetship.transform.position;
        bool canseetarget = RaycastAtTarget(targetship.transform);
        if (canseetarget == false)
        {
            seenTimer -= Time.deltaTime;


        }
        else { seenTimer += Time.deltaTime * 2; targetLastSeen = targetship.transform.position; }
        if (seenTimer < -0.5f) { seenTimer = -0.5f; }
        if (seenTimer > 2.0f) { seenTimer = 2.0f; }
        float angle = Vector3.Angle(tempTargetPos - transform.position, transform.forward);


        if (seenTimer > 0)
        {
            targetLastSeen = targetship.transform.position;

            targetRotation = Quaternion.LookRotation(tempTargetPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);
            //rb.AddForce((targetship.transform.position - transform.position) * speed * Time.deltaTime);
        }
        else
        {


            if (Vector3.Distance(targetship.transform.position, targetLastSeen) > 50)
            {
                FindClosestForwardPatrol(targetship);
                targetLastSeen = patroltarget.transform.position;
            }
            if (Vector3.Distance(targetLastSeen, transform.position) < 10)
            {
                FindClosestForwardPatrol(targetship);
                targetLastSeen = patroltarget.transform.position;
            }
            targetRotation = Quaternion.LookRotation(targetLastSeen - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            angle = Vector3.Angle(targetLastSeen - transform.position, transform.forward);

        }

        if (angle <= accuracy)
        { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime); }


    }

    //find a point closest to this enemy that is also closer to the target
    public void FindClosestForwardPatrol(GameObject targetship)
    {
        GameObject lastspot = patroltarget;
        float distanceToTarget = Vector3.Distance(transform.position, targetship.transform.position);
        float distanceToMe = Vector3.Distance(transform.position, targetship.transform.position);
        foreach (Transform el in myEnemy.patrolparent.transform)
        {
            if (el.gameObject != lastspot && Vector3.Distance(el.position, targetship.transform.position) < distanceToTarget && Vector3.Distance(el.position, transform.position) < distanceToMe && Vector3.Distance(el.position, transform.position) > 5)
            {
                patroltarget = el.gameObject;
                distanceToMe = Vector3.Distance(el.position, transform.position);
            }
        }

    }



    public void Patrol()
    {


        if (patrolColor != null && myRenderer != null)
        { myRenderer.material = patrolColor; }
        if (myEnemy.target != null)
        { targetLastSeen = myEnemy.target.transform.position; }
        if (myEnemy.patrolparent != null)
        {
            if (patroltarget != null)
            {
                CheckForward(patroltarget);

                if (Vector3.Distance(myEnemy.patrolparent.transform.GetChild(currentPatrolPoint).position, transform.position) < 10)
                {
                    //patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject; 

                    currentPatrolPoint++;
                    if (currentPatrolPoint >= myEnemy.patrolparent.transform.childCount)
                    { currentPatrolPoint = 0; }

                }
                patroltarget = myEnemy.patrolparent.transform.GetChild(currentPatrolPoint).gameObject;


                float angle = Vector3.Angle(patroltarget.transform.position - transform.position, transform.forward);


                if (avoidCollisionClock > 0 && Vector3.Distance(transform.position, patroltarget.transform.position) > checkForwardDistance)
                {
                    debugWhereToObj.transform.position = openSpotToAvoidCollision;
                    //targetRotation = Quaternion.LookRotation(openSpotToAvoidCollision - transform.position);
                    targetRotation = Quaternion.LookRotation(transform.position - (patroltarget.transform.position));


                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);




                }
                else
                {
                    targetRotation = Quaternion.LookRotation((patroltarget.transform.position) - transform.position);


                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);



                }
                if (angle <= accuracy)
                { }
                // rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
                rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime);
                //transform.position = Vector3.MoveTowards(transform.position, patroltarget.transform.position, walkspeed * Time.deltaTime);
            }
            else
            {
                patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject;
            }
        }
        else
        {

            if (myEnemy.inBattle == true)
            { myEnemy.FindTarget(); }
            else { myEnemy.CheckToNoticePlayer(); }

        }



    }



    public void CheckForward(GameObject target)
    {
        // possible issue with dradis detection
        RaycastHit hit;



        if (Physics.Raycast(transform.position, transform.forward, out hit, checkForwardDistance))
        {

            if (hit.transform.gameObject == target)
            {
                canShoot = true;
                avoidCollisionClock = 0;
                openSpotToAvoidCollision = Vector3.zero;
            }
            else
            {
                // if (Vector3.Distance(transform.position,hit.point) < plusminus)
                // {rb.AddForce(transform.forward * speed * -Time.deltaTime);}

                openSpotToAvoidCollision = (RayCastToFindOpening() * Vector3.Distance(hit.point, transform.position)) + transform.position;
                if (avoidCollisionClock <= 0) { avoidCollisionClock = 1.4f; }
                else { if (avoidCollisionClock < 5) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }

            }

        }
        else
        {
            avoidCollisionClock -= Time.deltaTime;
            //openSpotToAvoidCollision = Vector3.Lerp(openSpotToAvoidCollision, Vector3.zero, Time.deltaTime);
            canShoot = true;
            //if (avoidCollisionClock <= 0)
            //{ openSpotToAvoidCollision = Vector3.zero; }
        }
    }

    public Vector3 RayCastToFindOpening()
    {
        RaycastHit hit;
        Vector3 raydir = (transform.forward + transform.right).normalized;
        if (Physics.Raycast(transform.position, raydir, out hit, checkForwardDistance))
        { raydir = (transform.forward - transform.right).normalized; }
        else
        {
            return raydir;

        }
        if (Physics.Raycast(transform.position, raydir, out hit, checkForwardDistance))
        { raydir = (transform.forward - transform.up).normalized; }
        else
        {
            return raydir;

        }
        if (Physics.Raycast(transform.position, raydir, out hit, checkForwardDistance))
        { raydir = (transform.forward + transform.up).normalized; }
        else
        {
            return raydir;



        }
        return -transform.forward;
    }


    public void AvoidCollision()
    {

        if (avoidingCollisionColor != null && myRenderer != null)
        { myRenderer.material = avoidingCollisionColor; }


        targetRotation = Quaternion.LookRotation(transform.position - (openSpotToAvoidCollision));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 0.3f * Time.deltaTime);
        //either move forwad to avoid the obstacle of slow down to not collide
        if (avoidCollisionClock < 1) { rb.AddForce(transform.forward * walkspeed * Time.deltaTime, ForceMode.Impulse); }
        else { rb.AddForce(-transform.forward * walkspeed * Time.deltaTime, ForceMode.Impulse); }

    }
}