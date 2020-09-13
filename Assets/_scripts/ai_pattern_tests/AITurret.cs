using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurret : MonoBehaviour
{
    public Transform gunForward,turretHead;
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


    public bool destroyed, canShoot;


    private Quaternion targetRotation;
    private Rigidbody rb;
    private Enemy myEnemy;


    // Use this for initialization
    void Awake()
    {

        rb = GetComponent<Rigidbody>();
        myEnemy = GetComponent<Enemy>();

    }


    public void Fly(GameObject target)
    {

        myEnemy.RechargeStamina();

        if (target != null)
        {
            //how long the target is, or is not in sight
            FaceTarget(target);

        }
        else
        {
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
            case -1: 

                break;
            case 0: 
               FaceTarget(target);

                break;
           
            case 5: 
                Recover(target);

                break;
            default:
                FaceTarget(target);
                break;
        }

    }


    public void Attack(GameObject target)
    {
      
            if (target != null)
            {

                FaceTarget(target);

              
            }
        
        
    }

  


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
    }


    public void FaceTarget(GameObject targetship)
    {

        if (myEnemy.RaycastAtTarget(targetship.transform) == true)
        {

                float angle = Vector3.Angle((targetship.transform.position + targetship.GetComponent<Rigidbody>().velocity) - turretHead.position, turretHead.forward);

                if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }

                if (gunCooldown <= 0 && canShoot == true)
                {
                    myEnemy.FireGuns();
                    gunCooldown = gunCost + Random.Range(0, 3.0f);
                }

                if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
                {
                    targetRotation = Quaternion.LookRotation(targetship.transform.position - turretHead.position);

                    turretHead.rotation = Quaternion.Slerp(turretHead.rotation, targetRotation, rotForce * Time.deltaTime);
                }

        }

    }


    public void CalculateNextMove(GameObject targetship)
    {
        if (((float)myEnemy.stamina / myEnemy.currentStamina) < 0.2f)
        {
            //low stamina, need to recover
            currentAttackPlan = 5;
            return;
        }

        bool targetinfront = Vector3.Distance((transform.position + transform.forward), targetship.transform.position) < Vector3.Distance((transform.position - transform.forward), targetship.transform.position);
        bool targetfacingme = Vector3.Distance((targetship.transform.position + targetship.transform.forward), transform.position) < Vector3.Distance((targetship.transform.position - targetship.transform.forward), transform.position);
        float targetStamina = targetship.GetComponent<PlayerControls>().playerStats.GetStamina();


        if (targetfacingme == true)
        {
            
        }







        


    }

  

    public void Patrol()
    {

            if (myEnemy.inBattle == true)
            { myEnemy.FindTarget(); }
            else { myEnemy.CheckToNoticePlayer(); }

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

            }
            else
            {
                if (Vector3.Distance(transform.position, hit.point) < plusminus)
                { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * -speed, Time.deltaTime * acceleration); }

 

            }

        }
        else { canShoot = true; }
    }


   
}
