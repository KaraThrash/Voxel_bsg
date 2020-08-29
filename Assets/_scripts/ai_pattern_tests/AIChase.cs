using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
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
           
            case 0: //closein
                CloseIn(target);

                break;
            case 1: //chicken
                Chicken(target);

                break;
            case -1: //recover
                Recover(target);

                break;
            default:
                Chicken(target);
                break;
        }

    }


    public void Attack(GameObject target)
    {
       
            if (target != null)
            {

                AttackPlans(target);

                if (gunCooldown <= -8.0f)
                {
                    gunCooldown = 0;
                }
            }
        
      
    }

    public float DistanceToTarget(GameObject target)
    { return Vector3.Distance(transform.position, target.transform.position); }

    public float DistanceToTarget(Transform target)
    { return Vector3.Distance(transform.position, target.position); }

    public float DistanceToTarget(Vector3 target)
    { return Vector3.Distance(transform.position, target); }


    //how long the target is, or is not in sight
    public void TrackTimeTargetIsInSight(GameObject target)
    {
        if (RaycastAtTarget(target.transform) == true)
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

    public void Recover(GameObject targetship)
    {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * acceleration);
        myEnemy.currentStamina += Time.deltaTime * myEnemy.staminaRechargeRate;
        if (colors.Count > 5 && myRenderer != null)
        { myRenderer.material = colors[5]; }
        if (myEnemy.currentStamina > 5) { currentAttackPlan = 0; }
    }

   

 

   

    

    public void CloseIn(GameObject targetship)
    {
        if (colors.Count > 0 && myRenderer != null)
        { myRenderer.material = colors[currentAttackPlan + 1]; }
        float angle = Vector3.Angle((targetship.transform.position + targetship.GetComponent<Rigidbody>().velocity) - transform.position, transform.forward);

        if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
        if (gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

            // CalculateNextMove(target);
        }
        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
        {
            
            //not impulse, momentuem based

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime);
        }
       
        if (Vector3.Distance(targetship.transform.position, transform.position) < closedistance)
        {
            //tempTargetSpot = targetship.transform.position + ((targetship.transform.position - transform.position).normalized * fardistance);
            tempTargetSpot = transform.forward * fardistance;
            currentAttackPlan = 1;
        }
    }

    public void Chicken(GameObject targetship)
    {
        if (colors.Count > 0 && myRenderer != null)
        { myRenderer.material = colors[currentAttackPlan + 1]; }
        float angle = Vector3.Angle((targetship.transform.position + targetship.GetComponent<Rigidbody>().velocity) - transform.position, transform.forward);

        if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
        if (gunCooldown <= 0 && canShoot == true)
        {

            myEnemy.FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

            // CalculateNextMove(target);
        }

        //targetRotation = Quaternion.LookRotation(tempTargetSpot - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
        
        rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime);
        if (Vector3.Distance(targetship.transform.position, transform.position) > fardistance)
        {
            if (myEnemy.stamina > 5) { currentAttackPlan = 0; } else { currentAttackPlan = -1; }
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

                if (Vector3.Distance(patroltarget.transform.position, transform.position) < 3)
                { patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject; }


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
                if (Vector3.Distance(transform.position, hit.point) < plusminus)
                { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * -speed, Time.deltaTime * acceleration); }

                if (avoidCollisionClock <= 0) { avoidCollisionClock = 1.4f; }
                else { if (avoidCollisionClock < 3) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }

            }

        }
        else { avoidCollisionClock -= Time.deltaTime; canShoot = true; }
    }


   
}
