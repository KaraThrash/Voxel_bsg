using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aibosspunch : MonoBehaviour {
    public GameObject target,mydradis;
    public GameObject lefthand, righthand;
    public GameObject leftweapon, rightweapon, backvents,ventbullet;
    public float attackangle, weapondistance,attackcooldown, rotForce, accuracy;
   // public float attackactivetimer;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        target = mydradis.GetComponent<Dradis>().target;
        if (target != null)
        {
            Attack();
        }
	}

    public void Attack()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        float angle = Vector3.Angle(target.transform.position - transform.position, transform.forward);
       
           transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
        lefthand.transform.rotation = Quaternion.Slerp(lefthand.transform.rotation, Quaternion.LookRotation(target.transform.position - lefthand.transform.position), rotForce * 2.2f * Time.deltaTime);
        righthand.transform.rotation = Quaternion.Slerp(righthand.transform.rotation, Quaternion.LookRotation(target.transform.position - righthand.transform.position), rotForce * 2.2f * Time.deltaTime);
        if (attackcooldown > 0 )
        { attackcooldown -= Time.deltaTime; }
        else
        {
            if (attackcooldown != -1) {
                attackcooldown = -1;
                rightweapon.transform.position = righthand.transform.position;
                leftweapon.transform.position = lefthand.transform.position;
                rightweapon.GetComponent<Rigidbody>().velocity = Vector3.zero;
                leftweapon.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
           // leftweapon.GetComponent<Rigidbody>().AddForce(lefthand.transform.position - leftweapon.transform.position * 100.0f * Time.deltaTime );
            //rightweapon.GetComponent<Rigidbody>().AddForce(righthand.transform.position - rightweapon.transform.position * 100.0f * Time.deltaTime );
            if (angle <= 100 && Vector3.Distance(target.transform.position,transform.position) < weapondistance)
            {
               
                if (angle <= accuracy)
                {
                    attackcooldown = 5;
                    Debug.Log("force force ");
                    leftweapon.GetComponent<Rigidbody>().velocity += lefthand.transform.forward * 30000.0f * Time.deltaTime;
                    rightweapon.GetComponent<Rigidbody>().velocity += righthand.transform.forward  * 30000.0f * Time.deltaTime;
                }
            }
            else
            {
                if (angle >= 120 && Vector3.Distance(target.transform.position, transform.position) < weapondistance)
                {
                    attackcooldown = 10;
                    Instantiate(ventbullet, backvents.transform.position, backvents.transform.rotation);
                    // leftweapon.GetComponent<Rigidbody>().AddForce(target.transform.position - leftweapon.transform.position * 10.0f * Time.deltaTime, ForceMode.Impulse);
                    //rightweapon.GetComponent<Rigidbody>().AddForce(target.transform.position - rightweapon.transform.position * 10.0f * Time.deltaTime, ForceMode.Impulse);
                }
            }

        }


    }
}
