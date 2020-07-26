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

    public float avoidCollisionClock, seenTimer,distanceToLastCollisionObj;

    public bool destroyed, canShoot;
    public bool flyaway, flypast;

    public GameObject debugWhereToObj, debugLastSeen, patrolparent, patroltarget, currentTarget,objToAvoid;
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

            //Attack(target);
        }
        else
        {
            //currentAttackPlan = 0;
            seenTimer = 1;
            if (patroltarget != null) { Patrol(); }
            else
            {
                if (myEnemy.patrolparent != null)
                {

                    patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject;
                }
            }

            //if (gunCooldown <= 0)
            //{
            //    gunCooldown = gunCost;

            //    if (myEnemy.inBattle == true)
            //    { myEnemy.FindTarget(); }
            //    else { myEnemy.CheckToNoticePlayer(); }


            //}



        }
        // if (gunCooldown > 0)
        // { gunCooldown -= Time.deltaTime; }
        gunCooldown -= Time.deltaTime;

    }



    public void Attack(GameObject target)
    {
        //CheckForward(target);

        if (target != null)
        {
            Chase(target);
        }
        //else
        //{
        //    AvoidCollision();
        //}

    }



    public bool RaycastAtSpace(Vector3 currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, (currenttarget - transform.position).normalized, out hit, distanceToCheck))
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

    public RaycastHit GetHitRaycastAtTarget(Transform currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;
        Physics.Raycast(transform.position, currenttarget.position - transform.position, out hit, distanceToCheck);
        return hit;

    }



    public void Chase(GameObject targetship)
    {

        RaycastAtTarget(targetship.transform);

        targetLastSeen = targetship.transform.position;
        debugLastSeen.transform.position = targetLastSeen;
        targetRotation = Quaternion.LookRotation((targetship.transform.position) - transform.position);

        if (RaycastAtSpace(targetship.transform.position, 20.0f))
        {

            if (RaycastAtSpace((transform.forward * 10) + transform.position, 10.0f))
            {

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            }

            else
            {
                targetRotation = Quaternion.LookRotation((transform.position - transform.forward) - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, -rotForce * Time.deltaTime);
            }

        }
        else
        {
            if (RaycastAtSpace( (transform.forward * 10) + transform.position, 20.0f))
            {

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, -rotForce * Time.deltaTime);
            }

            else
            {
                targetRotation = Quaternion.LookRotation((transform.position - transform.forward) - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, -rotForce * Time.deltaTime);
            }


        }

        float angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

        if (avoidCollisionClock <= 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime);
        }
        else { rb.velocity = Vector3.Lerp(rb.velocity, -transform.forward * speed, Time.deltaTime); transform.Rotate(15 * Time.deltaTime, 15 * Time.deltaTime, 15 * Time.deltaTime); }


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

        if (Vector3.Distance(patroltarget.transform.position, transform.position) > 250)
        {
            currentAttackPlan = 4;
            patroltarget.active = false;
            patroltarget = myEnemy.patrolparent.transform.GetChild(0).gameObject;
            patroltarget.active = true;
        }
      
       


        switch (currentAttackPlan)
        {

            case -1:
                targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 0.2f * Time.deltaTime);

                rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * walkspeed, Time.deltaTime);
                avoidCollisionClock -= Time.deltaTime;
                if (avoidCollisionClock <= 0) { currentAttackPlan = 0; }
                break;
            case 0: //normal patrol
                if (Vector3.Distance(patroltarget.transform.position, transform.position) <= 2 || Vector3.Distance(patroltarget.transform.position, transform.position) > 150)
                {
                    //patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject; 

                    currentPatrolPoint++;
                    if (currentPatrolPoint >= myEnemy.patrolparent.transform.childCount)
                    { currentPatrolPoint = 0; }
                    patroltarget.active = false;
                    patroltarget = myEnemy.patrolparent.transform.GetChild(currentPatrolPoint).gameObject;
                    patroltarget.active = true;
                    
                }


                targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);
                float angle = Vector3.Angle(patroltarget.transform.position - transform.position, transform.forward);
                RaycastHit hit;

                if (angle <= accuracy) { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime); }
                else { rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime); }
                if (Physics.SphereCast(transform.position, 4, transform.forward, out hit, Mathf.Min(Vector3.Distance(transform.position, patroltarget.transform.position),10.0f)))
                {
                    objToAvoid = hit.transform.gameObject;
                    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Inverse(targetRotation), rotForce * 2.2f * Time.deltaTime);
                    //rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 2 * Time.deltaTime);
                    currentAttackPlan = 1;
                    distanceToLastCollisionObj = Vector3.Distance(transform.position, hit.point);
                }
                else { 

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce *  Time.deltaTime); 
                
                }
                

                break;
            case 1: //something in the way, move around it

                Vector3 newdir = RayCastToFindOpening();
                openSpotToAvoidCollision = transform.position + (newdir );

                currentAttackPlan = 2;
                break;
            case 2: //clean the obstruction
                //if (avoidCollisionClock > 0) { currentAttackPlan = 3; }
                //targetRotation = Quaternion.LookRotation(openSpotToAvoidCollision - transform.position);
                //float angle2 = Vector3.Angle(openSpotToAvoidCollision - transform.position, transform.forward);

                
                ////targetRotation = Quaternion.LookRotation(transform.position - objToAvoid.transform.position);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

                if (Physics.SphereCast(transform.position, 3, transform.forward, out hit, distanceToLastCollisionObj * 1.2f))
                {
                    rb.velocity = Vector3.Lerp(rb.velocity, -transform.forward , Time.deltaTime);
                    targetRotation = Quaternion.LookRotation(transform.position - hit.point);


                    //targetRotation = Quaternion.LookRotation(transform.position - objToAvoid.transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

                }
                else { currentAttackPlan = -1; avoidCollisionClock = 3; }

                    //if (angle2 <= accuracy)
                    //{ rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * walkspeed, Time.deltaTime); }
                    //else { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * -walkspeed * 0.1f, Time.deltaTime); }
                    //if (Vector3.Distance(openSpotToAvoidCollision, transform.position) < 2 || Vector3.Distance(openSpotToAvoidCollision, transform.position) > 40)
                    //{ currentAttackPlan = 0; }

                    break;
            case 3:
                //targetRotation = Quaternion.LookRotation((transform.position - transform.forward) - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 0.2f * Time.deltaTime);
                rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * -walkspeed, Time.deltaTime);
                avoidCollisionClock -= Time.deltaTime;
                if (avoidCollisionClock <= 0) { currentAttackPlan = 0; }
                break;

            case 4:
                targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, patroltarget.transform.position, walkspeed * Time.deltaTime);
                if (Vector3.Distance(patroltarget.transform.position, transform.position) < 50)
                {
                    currentAttackPlan = 0;
                    patroltarget.active = false;
                    patroltarget = myEnemy.patrolparent.transform.GetChild(1).gameObject;
                    patroltarget.active = true;
                }
                break;

            default:
                break;
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
                //avoidCollisionClock = 0;
                //openSpotToAvoidCollision = Vector3.zero;
            }
            else
            {
                // if (Vector3.Distance(transform.position,hit.point) < plusminus)
                // {rb.AddForce(transform.forward * speed * -Time.deltaTime);}

                //openSpotToAvoidCollision = (RayCastToFindOpening() * Vector3.Distance(hit.point, transform.position)) + transform.position;
                //if (avoidCollisionClock <= 0) { avoidCollisionClock = 0.2f; }
                //else { if (avoidCollisionClock < 1) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }

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
    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Enviroment")
        {
            currentAttackPlan = 3;
            avoidCollisionClock = 1;
            targetRotation = Quaternion.LookRotation(other.transform.position  - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Inverse(targetRotation), rotForce * 2 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position - other.transform.position, speed * Time.deltaTime);
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

    public void AvoidCollision()
    {
        //TODO: scan around to find the open space rather than always rotating away 180
        //RayCastToFindOpening();
        //if (avoidingCollisionColor != null && myRenderer != null)
        //{ myRenderer.material = avoidingCollisionColor; }


        //targetRotation = Quaternion.LookRotation(transform.position - (openSpotToAvoidCollision));
        //// targetRotation = Quaternion.LookRotation(  (transform.position  + (transform.up * 50) - (transform.forward * 50) ) - transform.position  );
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 0.3f * Time.deltaTime);
        ////either move forwad to avoid the obstacle of slow down to not collide
        //if (avoidCollisionClock < 1) { rb.AddForce(transform.forward * walkspeed * Time.deltaTime, ForceMode.Impulse); }
        //else { rb.AddForce(-transform.forward * walkspeed * Time.deltaTime, ForceMode.Impulse); }
        //// rb.velocity = Vector3.Lerp(rb.velocity,transform.forward * walkspeed,Time.deltaTime * speed );

    }
}