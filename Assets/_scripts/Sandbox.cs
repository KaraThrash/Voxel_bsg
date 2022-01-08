using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbox : MonoBehaviour
{
    public Camera camera;
    public Transform obj,newmapObj, spawnablesToPickFrom;
    public Transform object0, object1;
    public GameObject table;
    public GameObject gameObj,obj0,obj1,obj2,obstacles;
    // Start is called before the first frame update
    void Start()
    {

        //SpawnPrimitiveCuveEnviroment();
        //meth();
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.T))
      {
            
      }
      if(Input.GetKeyDown(KeyCode.Y))
      {
      
      }
      if(Input.GetKeyDown(KeyCode.U))
      {
       
      }

        //Debug.Log("InputControls.DpadHort() <>" + InputControls.DpadHort());
        //Debug.Log("InputControls.DpadVert() <>" + InputControls.DpadVert());
        //Debug.Log("InputControls.VerticalAxis() <>" + InputControls.VerticalAxis());
        //Debug.Log("InputControls.HorizontalAxis() <>" + InputControls.HorizontalAxis());
        //Debug.Log("InputControls.PreviousButton() <>" + InputControls.PreviousButton());
        //Debug.Log("InputControls.NextButton() <>" + InputControls.NextButton());
        //Debug.Log("InputControls.PickUpButton() <>" + InputControls.PickUpButton());
        //Debug.Log("InputControls.MenuButton() <>" + InputControls.MenuButton());
        //Debug.Log("InputControls.ActionButton() <>" + InputControls.ActionButton());
        if (InputControls.RightTrigger() != 0)
        { 
            Debug.Log("InputControls.RightTrigger() <>" + InputControls.RightTrigger());

        }
        if (InputControls.LeftTrigger() != 0)
        {
            Debug.Log("InputControls.LeftTrigger() <>" + InputControls.LeftTrigger());

        }
    }

    public void UnParent(Transform tempobj )
    {
        foreach(Transform el in tempobj)
        {
            if (el.childCount > 0) { UnParent(el); }
            else { el.parent = object1; }
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
