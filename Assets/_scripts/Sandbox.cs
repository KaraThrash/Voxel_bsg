using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbox : MonoBehaviour
{
    public Camera camera;
    public Transform obj;

    public GameObject gameObj,obj0,obj1,obj2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.T))
      {
        raycastfromcam(obj0);
      }
      if(Input.GetKeyDown(KeyCode.Y))
      {
        raycastfromcam(obj1);
      }
      if(Input.GetKeyDown(KeyCode.U))
      {
        raycastfromcam(obj2);
      }


    }
    public void raycastfromcam(GameObject obj){
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {


          Instantiate(obj,hit.point,transform.rotation);
          // Vector3.Distance(hit.point,camera.transform.position)
            // Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
        }
    }
}
