using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Sandbox : MonoBehaviour
{
    public bool act;
    public Grades grade;
    public Camera camera;
    public Transform obj,newmapObj, spawnablesToPickFrom;
    public Transform object0, object1;
    public GameObject table;
    public GameObject gameObj,obj0,obj1,obj2,obstacles;

    public List<Actor> childList;

    // Start is called before the first frame update
    void Start()
    {
        if (act)
        {
            List<Transform> rndList = new List<Transform>();
           childList = new List<Actor>();

             UniversalFunctions.GetDeepChildren(transform, rndList);

            foreach (Transform el in rndList)
            {
                if (el.GetComponent<Actor>())
                {
                    childList.Add(el.GetComponent<Actor>());
                }
                Debug.Log(el.name);
            }

        }

        Debug.Log(grade.Passing());
 

       // IterateObjects();
        //SpawnPrimitiveCuveEnviroment();
        //meth();
    }

    // Update is called once per frame
    void Update()
    {
     

      if (Input.GetKeyDown(KeyCode.T))
      {
            Debug.Log("transform.forward:  " + Vector3.Angle(transform.forward, (obj.position - transform.position).normalized));
            Debug.Log("transform.right:  " + Vector3.Angle(transform.right, (obj.position - transform.position).normalized));
            Debug.Log("transform.up:  " + Vector3.Angle(transform.up, (obj.position - transform.position).normalized));
        }
      if(Input.GetButtonDown("Leftstick"))
      {
            Debug.Log("Leftstick ");
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
            //Debug.Log("InputControls.RightTrigger() <>" + InputControls.RightTrigger());

        }
        if (InputControls.LeftTrigger() != 0)
        {
          //  Debug.Log("InputControls.LeftTrigger() <>" + InputControls.LeftTrigger());

        }
    }



    public void IterateObjects()
    {
        int count = 0;

        while (count < 20)
        {
            int count2 = 0;
            while (count2 < 20)
            {
                GameObject clone = Instantiate(obj0.gameObject, object0.position +  new Vector3(count * 15, 25, count2 * 15), transform.rotation);
                clone.transform.parent = object0;
                if (count % 2 == 0)
                { count2++; }
                else { count2 += 3; }
            }
            count++;
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
