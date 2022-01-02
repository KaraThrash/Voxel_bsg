using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AI0 : MonoBehaviour
{
    public int currentAttackPlan;

    // parameters to change/set how the ship controls
    public float speed = 50, walkspeed = 10;
    public float rotForce = 6;
    public float accuracy = 1;

    public float closedistance = 10, fardistance = 40, plusminus = 5, acceleration = 0.3f;
    public float gunCost = 1, targetInSight = 0;
    public float checkForwardDistance = 100.0f;
    //

    public float gunCooldown;

    public float avoidCollisionClock;

    public bool destroyed, canShoot;
    public bool flyaway, flypast;

    public GameObject patrolparent, patroltarget;
    public Vector3 startpos, patrolpos, holdVelocity;
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
        startpos = transform.position;
        rb = GetComponent<Rigidbody>();
        myEnemy = GetComponent<Enemy>();
        patrolparent = myEnemy.patrolparent;
        patroltarget = myEnemy.patroltarget;
        if (colors.Count > 0 && myRenderer != null)
        { transform.GetChild(0).GetComponent<Renderer>().material = colors[Random.Range(0, (int)colors.Count)]; }
    }


    public void Fly(GameObject target)
    {

        myEnemy.RechargeStamina();

        if (target != null)
        {
            //how long the target is, or is not in sight
            TrackTimeTargetIsInSight(target);
            Attack(target);

        }
        else
        {
            Patrol();
            if (gunCooldown <= 0)
            {
                gunCooldown = gunCost * 3;

                if (myEnemy.inBattle == true)
                { myEnemy.FindTarget(); }
                else { myEnemy.CheckToNoticePlayer(); }


            }



        }
        // if (gunCooldown > 0)
        // { gunCooldown -= Time.deltaTime; }
        gunCooldown -= Time.deltaTime;

    }

    public void AttackPlans(GameObject target)
    {
        switch (currentAttackPlan)
        {
          
            case 0: //GetClose
                Chicken(target);

                break;
            case 1: //Fly past attacking
                FlyPast(target);

                break;
            case 2: //fly away
                GetFarAndComeBack(target);

                break;
        
            default:
                Chicken(target);
                break;
        }

    }


    public void Attack(GameObject target)
    {
        //CheckForward(target);
        if (avoidCollisionClock <= 0)
        {
            if (target != null)
            {

                AttackPlans(target);

                if (gunCooldown <= -8.0f)
                {
                    gunCooldown = 0;
                    //incase the player is chasing
                    if (currentAttackPlan == 2) { currentAttackPlan = 0; }
                }
            }
        }
        else
        {
            AvoidCollision();
        }
    }




    //how long the target is, or is not in sight
    public void TrackTimeTargetIsInSight(GameObject target)
    {
        if (myEnemy.RaycastAtTarget(target.transform) == true)
        {
            if (targetInSight < 0) { targetInSight = 0; }
            targetInSight += Time.deltaTime;
        }
        else
        {
            if (targetInSight > 0) { targetInSight = 0; }
            targetInSight -= Time.deltaTime;
        }
    }



    public void Recover(GameObject targetship)
    {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * acceleration);
        myEnemy.currentStamina += Time.deltaTime * myEnemy.staminaRechargeRate;
        if (colors.Count > 5 && myRenderer != null)
        { myRenderer.material = colors[5]; }
    }



    public void FlyPast(GameObject targetship)
    {

        if (colors.Count > 0 && myRenderer != null)
        { myRenderer.material = colors[currentAttackPlan + 1]; }

        float distancetotarget = Vector3.Distance(targetship.transform.position, transform.position);
        if (distancetotarget < fardistance)
        {
            //keep flying past target while trying to fire
            rb.velocity = Vector3.Lerp(rb.velocity, holdVelocity, Time.deltaTime * acceleration);
            float angle = Vector3.Angle((targetship.transform.position + targetship.GetComponent<Rigidbody>().velocity) - transform.position, transform.forward);

            if (angle <= accuracy)
            {

                
                if (gunCooldown <= 0)
                {

                    myEnemy.FireGuns();
                    gunCooldown = gunCost + Random.Range(0, 3.0f);

                }
            }


        //dont use stamina for this right now
                targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 2 * Time.deltaTime);
            
        }
        else
        {
            holdVelocity = Vector3.zero;
            gunCooldown = 0;
            currentAttackPlan = 2;
        }

    }




    public void Chicken(GameObject targetship)
    {
        if (colors.Count > 0 && myRenderer != null)
        { myRenderer.material = colors[currentAttackPlan + 1]; }

        float distancetotarget = Vector3.Distance(targetship.transform.position, transform.position);
        if (distancetotarget > fardistance)
        {

            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime * acceleration);


            if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
            {
                targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
                //not impulse, momentuem based

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            }
        }
        else if (distancetotarget < fardistance && distancetotarget > closedistance)
        {
            float angle = Vector3.Angle((targetship.transform.position + targetship.GetComponent<Rigidbody>().velocity) - transform.position, transform.forward);

            if (angle <= accuracy) {

                rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime * acceleration);
                if (gunCooldown <= 0 )
                {

                    myEnemy.FireGuns();
                    gunCooldown = gunCost + Random.Range(0, 3.0f);

                }
            }


            if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
            {
                targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
                //not impulse, momentuem based

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            }
        }
        else {
            holdVelocity = speed * transform.forward;
            currentAttackPlan = 1;
        }

    }
    

    public void GetFarAndComeBack(GameObject targetship)
    {
        if (colors.Count > 0 && myRenderer != null)
        { myRenderer.material = colors[currentAttackPlan + 1]; }

        float distancetotarget = Vector3.Distance(targetship.transform.position, transform.position);
        if (distancetotarget < fardistance * 2f)
        {
            //keep flying past target while trying to fire
            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime * acceleration);
         


            //dont use stamina for this right now
            targetRotation = Quaternion.LookRotation( transform.position - targetship.transform.position );

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        }
        else
        {
            holdVelocity = Vector3.zero;
            currentAttackPlan = 0;
        }
    }

    public void Patrol()
    {
        //patrol between startpos and one patrol location
        if (patrolColor != null && myRenderer != null)
        { myRenderer.material = patrolColor; }

        if (myEnemy.patrolparent != null)
        {
            if (patrolpos != Vector3.zero)
            {

                if (Vector3.Distance(patrolpos, transform.position) < 3)
                {
                    if (Vector3.Distance(startpos, transform.position) > 3)
                    {
                        patrolpos = startpos;
                    }
                    else
                    {
                        patrolpos = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).position;

                    }
                }


                targetRotation = Quaternion.LookRotation(patrolpos - transform.position);

                float angle = Vector3.Angle(patrolpos - transform.position, transform.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

                // rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
                rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * walkspeed, Time.deltaTime * acceleration);
            }
            else
            {
                patrolpos = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).position;
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
                if (Vector3.Distance(transform.position, hit.point) < plusminus)
                { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * -speed, Time.deltaTime * acceleration); }

                if (avoidCollisionClock <= 0) { avoidCollisionClock = 1.4f; }
                else { if (avoidCollisionClock < 3) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }

            }

        }
        else { avoidCollisionClock -= Time.deltaTime; canShoot = true; }
    }

    public void RayCastForward()
    {

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
