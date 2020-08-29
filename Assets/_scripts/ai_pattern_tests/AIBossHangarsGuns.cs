using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossHangarsGuns : MonoBehaviour
{
    public GameManager gameManager;
    public Transform body,hangars, guns, gungun, hangargun;
    public float rotateCenter,rotateSpeed,attackCooldown,attackCooldownRange,timer;
    public GameObject shipToSpawn, bullet,target;
    public bool oppositeDirection;
    public int hp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


        if (rotateCenter > 0)
        {
            RotateBody();
        }
        else 
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Attack();
            }
        }
    }

    public void RotateBody()
    {
        float rotAmount = Time.deltaTime * rotateSpeed;
        rotateCenter -= rotAmount;
        if (rotateCenter < 0)
        { //make sure it's exactly 90
            rotAmount = rotAmount + rotateCenter ;
            rotateCenter = 0;
        }
        if (oppositeDirection == true) { rotAmount = rotAmount * -1; }
        body.Rotate(0,rotAmount,0);
        
    }

    public void Attack()
    {
        timer = attackCooldown;
        rotateCenter = 90;
        oppositeDirection = false;
        //the forward facing hangar and gun should activate if not destroyed when attacking
        Transform closest = guns.transform.GetChild(0);
        foreach (Transform el in guns)
        {
            if (Vector3.Distance(el.position, transform.position) < Vector3.Distance(closest.position, transform.position))
            { closest = el; }
        }
        if (closest.gameObject.active == true)
        {
            Instantiate(bullet, gungun.position , gungun.rotation);
            timer += Random.Range(0, attackCooldownRange);
        }
        closest = hangars.transform.GetChild(0);
        foreach (Transform el in hangars)
        {
            if (Vector3.Distance(el.position, transform.position) < Vector3.Distance(closest.position, transform.position))
            { closest = el; }
        }

        if (closest.gameObject.active == true)
        {
            GameObject clone = gameManager.npcManager.SpawnOne(3,hangargun.position, hangargun.rotation,true);
            clone.GetComponent<Enemy>().target = target;
            clone.GetComponent<Enemy>().alert = true;
            clone.GetComponent<Enemy>().inCombat = true;
            timer += Random.Range(0, attackCooldownRange);
        }
        if (Random.Range(0, 1.0f) < 0.5f) { oppositeDirection = true; }
    }
    

}
