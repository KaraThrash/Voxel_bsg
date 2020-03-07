using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleetShip : MonoBehaviour
{
  public NpcManager npcManager;
  public EnemyFleet enemyFleetManager;
  public GameObject target;
  public GameObject cannon,bullet,hangar,missilebay,missile,rubble;
  public int enemyType,maxHp,currentHp; // enemy to tell the npc manager to spawn
  public Transform cooldownIndicator;
  public float rotForce = 0.1f,accuracy = 0; //
  public float attackCost,attackTimer,dieclock;
    // Start is called before the first frame update
    void Start()
    {
      if(maxHp == 0){maxHp = 10;currentHp = 10;}
    }

    // Update is called once per frame
    void Update()
    {


      if (dieclock != -1)
      {

        dieclock -= Time.deltaTime;
          if (dieclock <= 0)
          { ShipDestroyed(); }


      }else{InBattle();}
    }

    public void FindTarget()
    {
      if(npcManager != null){
        target = npcManager.GetClosestTarget(transform.position);

        if(target.GetComponent<Fleetship>() != null)
        {
        target =  target.GetComponent<Fleetship>().GetClosestShipPart(transform.position).gameObject;
        }

      }
    }
    public void InBattle()
    {

        if(target == null){

          FindTarget();

        }
        else{
           FaceTarget();
        }
        if(attackTimer != -1)
        {
            if(attackTimer > 0 )
            {
              //indicates how close the ship is to being ready to attackC
              // TODO: need a better color blind solution
              if(cooldownIndicator != null)
              {
                float newscale = (attackCost - attackTimer )/ attackCost;
                cooldownIndicator.localScale = new Vector3(1,newscale,newscale);
              }
              //goes slower the more damaged the ship is
              attackTimer -= (Time.deltaTime * (float)currentHp/(float)maxHp);

              float percentHealth = currentHp;
              percentHealth = percentHealth/(float)maxHp;
              attackTimer -= (Time.deltaTime * percentHealth);

            }else
            {
                  if(target != null){
                  Attack();

                    if(target.GetComponent<ShipPart>() != null && target.GetComponent<ShipPart>().destroyed == true)
                    {
                      target =  null;
                    }
                  }
            }
       }
    }
    public void Attack()
    {
          if(cannon != null && bullet != null)
          {
            float angle = Vector3.Angle(target.transform.position - transform.position, transform.forward);

            //check that the target is in front
            if (angle <= accuracy)
             {
              GameObject clone = Instantiate(bullet,bullet.transform.position,transform.rotation);
              clone.active = true;
                attackTimer = attackCost;
             }

          }
          if(hangar != null)
          {

            npcManager.SpawnOne(enemyType,hangar.transform.position,hangar.transform.rotation);
              attackTimer = attackCost;
          }
          if(missilebay != null)
          {}

    }

    public void SystemDestroyed()
    {
      currentHp--;
      if (currentHp <= 0)
      { dieclock = 10.0f;
          GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
          GetComponent<Rigidbody>().angularDrag = 0;
          GetComponent<Rigidbody>().drag = 0;
          GetComponent<Rigidbody>().velocity = Vector3.down * 5.0f;
            GetComponent<Rigidbody>().angularVelocity = (Vector3.down +  Vector3.right) * Random.Range(0.01f, 0.2f);
      }
    }
    public void ShipDestroyed()
    {
      if(rubble != null){

        GameObject clone = Instantiate(rubble,transform.position,transform.rotation);
        clone.active = true;
      }
        Destroy(this.gameObject);
    }
    public void FaceTarget()
    {

      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position ), rotForce * Time.deltaTime);
    }
}
