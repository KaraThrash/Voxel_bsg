﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AIusePatrolToNav : MonoBehaviour
{
    public int currentAttackPlan, currentPatrolPoint;

    // parameters to change/set how the ship controls
    public float speed = 50, walkspeed = 10;
    public float rotForce = 6, rotModifier;
    public float accuracy = 1;

    public float closedistance = 10, fardistance = 40, plusminus = 5;
    public float gunCost = 1;
    public float checkForwardDistance = 100.0f;
    //

    public float gunCooldown;

    public float avoidCollisionClock;

    public bool destroyed, canShoot;
    public bool flyaway, flypast;

    public GameObject patrolparent, patroltarget;
    public Material avoidingCollisionColor, patrolColor;
    public Renderer myRenderer;
    public List<Material> colors;

    private Vector3 straferunspot, tempTargetSpot, openSpotToAvoidCollision;
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

        if (target != null && myEnemy.inCombat == true)
        {

            Attack(target);
        }
        else
        {
            if (patroltarget != null) { Patrol(); }
            else { if (myEnemy.patrolparent != null) { patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject; } }

            if (gunCooldown <= 0)
            {
                gunCooldown = gunCost * 2;

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
        if (avoidCollisionClock <= 0)
        {
            if (target != null)
            {

                Chicken(target);


            }
        }
        else
        {
            AvoidCollision();
        }

    }

    public void Chicken(GameObject targetship)
    {

        float angle = Vector3.Angle((targetship.transform.position + targetship.GetComponent<Rigidbody>().velocity) - transform.position, transform.forward);

        if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
        if (gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

        }

        if (Vector3.Distance(transform.position, targetship.transform.position) <= fardistance  )
        {
            if (Vector3.Distance(transform.position, targetship.transform.position) >= closedistance)
            {

                if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
                {
                    if (colors.Count > 2 && myRenderer != null)
                    { myRenderer.material = colors[0]; }
                    rb.AddForce(transform.forward * speed * Time.deltaTime);
                }
            }
            else { myRenderer.material = colors[2]; }
            targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        }
        else
        {
            targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
            {
                if (colors.Count > 2 && myRenderer != null)
                { myRenderer.material = colors[1]; }

                angle = Vector3.Angle((patroltarget.transform.position ) - transform.position, transform.forward);

                if (angle <= accuracy) { rb.AddForce(transform.forward * speed * 10 * Time.deltaTime); }
   
                
            }
            if (Vector3.Distance(transform.position, patroltarget.transform.position) < 10)
            { FindClosestForwardPatrol(targetship); }
        }
       

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

        if (myEnemy.patrolparent != null)
        {
            if (patroltarget != null)
            {

                if (Vector3.Distance(myEnemy.patrolparent.transform.GetChild(currentPatrolPoint).position, transform.position) < 10)
                {
                    //patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject; 

                    currentPatrolPoint++;
                    if (currentPatrolPoint >= myEnemy.patrolparent.transform.childCount)
                    { currentPatrolPoint = 0; }

                }
                patroltarget = myEnemy.patrolparent.transform.GetChild(currentPatrolPoint).gameObject;

                targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);

                float angle = Vector3.Angle(patroltarget.transform.position - transform.position, transform.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

                // rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
                transform.position = Vector3.MoveTowards(transform.position, patroltarget.transform.position, walkspeed * Time.deltaTime);
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

            }
            else
            {
                // if (Vector3.Distance(transform.position,hit.point) < plusminus)
                // {rb.AddForce(transform.forward * speed * -Time.deltaTime);}

                if (avoidCollisionClock <= 0) { avoidCollisionClock = 1.4f; }
                else { if (avoidCollisionClock < 3) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }

            }

        }
        else { avoidCollisionClock -= Time.deltaTime; canShoot = true; }
    }

    public void RayCastToFindOpening()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, checkForwardDistance))
        {

            openSpotToAvoidCollision = (myEnemy.target.transform.position - hit.point).normalized;
            return;
        }
        else
        {

            return;

        }

    }
    public void AvoidCollision()
    {
        //TODO: scan around to find the open space rather than always rotating away 180
        RayCastToFindOpening();
        if (avoidingCollisionColor != null && myRenderer != null)
        { myRenderer.material = avoidingCollisionColor; }


        targetRotation = Quaternion.LookRotation(transform.position - (openSpotToAvoidCollision));
        // targetRotation = Quaternion.LookRotation(  (transform.position  + (transform.up * 50) - (transform.forward * 50) ) - transform.position  );
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 0.3f * Time.deltaTime);
        //either move forwad to avoid the obstacle of slow down to not collide
        if (avoidCollisionClock < 1) { rb.AddForce(transform.forward * walkspeed * Time.deltaTime, ForceMode.Impulse); }
        else { rb.AddForce(-transform.forward * walkspeed * Time.deltaTime, ForceMode.Impulse); }
        // rb.velocity = Vector3.Lerp(rb.velocity,transform.forward * walkspeed,Time.deltaTime * speed );

    }
}