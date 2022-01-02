using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShipBattle : MonoBehaviour
{
    public Transform target,gunbatteryparent;
    public float rotspeed, speed, verticalOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CircleTarget();
    }
    public void CircleTarget()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        Vector3 targetpos = new Vector3(target.transform.position.x, target.transform.position.y + verticalOffset, target.transform.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetpos - transform.position), rotspeed * Time.deltaTime);
        if (gunbatteryparent == null) { return; }
        foreach (Transform el in gunbatteryparent)
        {
            el.transform.rotation = Quaternion.Slerp(el.transform.rotation, Quaternion.LookRotation(target.transform.position - el.transform.position), rotspeed * 10 * Time.deltaTime);
        }
    }

}
