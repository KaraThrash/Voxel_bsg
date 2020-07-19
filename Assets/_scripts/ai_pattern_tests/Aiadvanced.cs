using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Aiadvanced : MonoBehaviour
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

    public float avoidCollisionClock,seenTimer;

    public bool destroyed, canShoot;
    public bool flyaway, flypast;

    public GameObject patrolparent, patroltarget;
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
       
        if (target != null)
        {

            Chicken(target);


        }
        
        //else
        //{
        //    AvoidCollision();
        //}

    }


    public void AttackPlans(GameObject target)
    {
        switch (currentAttackPlan)
        {
            case -1: 

                break;
            case 0: //chicken
                Chicken(target);

                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            default:
               
                break;
        }

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



    public void Chicken(GameObject targetship)
    {

        

        Vector3 tempTargetPos = targetship.transform.position ;
        bool canseetarget = RaycastAtTarget(targetship.transform);
        if (canseetarget == false)
        {
            seenTimer -= Time.deltaTime;
            //if the ship reaches the last know location and still can't see the target
            //if (Vector3.Distance(transform.position, targetLastSeen) < 10 || targetLastSeen == Vector3.zero )
            //{
            //    FindClosestForwardPatrol(targetship);
            //    targetLastSeen = patroltarget.transform.position;
            //}
            //tempTargetPos = targetLastSeen;

        }
        else { seenTimer += Time.deltaTime * 2; targetLastSeen = targetship.transform.position; }
        if (seenTimer < -0.5f) { seenTimer = 0.5f; }
        if (seenTimer > 2.0f) { seenTimer = 2.0f; }
        float angle = Vector3.Angle(tempTargetPos - transform.position, transform.forward);

       
        //if (gunCooldown <= 0 && canShoot == true && canseetarget)
        //{

        //    myEnemy.FireGuns();
        //    gunCooldown = gunCost + Random.Range(0, 3.0f);

        //}

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
        { rb.velocity = Vector3.Lerp(rb.velocity,transform.forward * speed,Time.deltaTime); }


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
        if(myEnemy.target != null )
        { targetLastSeen = myEnemy.target.transform.position; }
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