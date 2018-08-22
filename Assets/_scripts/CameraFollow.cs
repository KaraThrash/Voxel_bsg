using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject target;
    public Quaternion newrot;
    public bool movetowards;
    public float damping;
    public float flyspeed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate() {

       


        if (Vector3.Distance(transform.position, target.transform.position) > 55)
        {
            movetowards = true;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 25)
        {
            movetowards = false;
        }
        if (movetowards == true)
        {
           transform.position = Vector3.MoveTowards(transform.position, target.transform.position, flyspeed * Time.deltaTime);
        }
        newrot = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.deltaTime * damping);
    }
    //void LateUpdate()
    //{
    //    newrot = Quaternion.LookRotation(target.transform.position - transform.position);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.deltaTime * damping);



        //float wantedRotationAngle = target.transform.eulerAngles.y;
        //float wantedHeight = target.transform.position.y + 5.0f;

        //float currentRotationAngle = transform.eulerAngles.y;
        //float currentHeight = transform.position.y;

        //// Damp the rotation around the y-axis
        //currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, 3.0f * Time.deltaTime);

        //// Damp the height
        //currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 2.0f * Time.deltaTime);

        //// Convert the angle into a rotation
        //var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        //// Set the position of the camera on the x-z plane to:
        //// distance meters behind the target
        ////transform.position = target.transform.position;
        //transform.position -= currentRotation * Vector3.forward * 10;

        //// Set the height of the camera
        //transform.position = new Vector3(transform.position.x,currentHeight,transform.position.z);


    //}
}
