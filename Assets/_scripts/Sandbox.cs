using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbox : MonoBehaviour
{
    public Camera camera;
    public Transform obj;
    public GameObject table;
    public GameObject gameObj,obj0,obj1,obj2,obstacles;
    // Start is called before the first frame update
    void Start()
    {
      meth();
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
    public Vector3 spawntable(Vector3 pos)
    {
      if(Random.Range(0,4) > 2)
      {
        GameObject newclone = Instantiate(table,pos,table.transform.rotation);
        newclone.transform.parent = gameObj.transform;
        return spawntable(new Vector3(pos.x,pos.y + 1,pos.z));
      }
      else{return pos;}


    }
    public void meth()
    {
      int count = 0;
      int count2 = 0;
      GameObject clone = gameObj;
      while (count < 10)
      {

              if(count < 4){
                count2 = 0;
              while (count2 < 10)
              {
                  if (count2 < 4)
                  {
                      clone = obj0.transform.GetChild(Random.Range(0,obj0.transform.childCount)).gameObject;

                  }
                  else
                  {
                        clone = obj1.transform.GetChild(Random.Range(0,obj1.transform.childCount)).gameObject;
                  }
                  if(Random.Range(0,4) > -1)
                  {
                  GameObject newclone = Instantiate(clone,spawntable(new Vector3(count * 2,0,count2 * 2)),clone.transform.rotation);
                  newclone.transform.parent = gameObj.transform;
                }
                  count2 ++;
              }
            }else if(count < 7)
            {  count2 = 0;
              while (count2 < 10)
              {
                  if (count2 < 4)
                        {
                            clone = obj1.transform.GetChild(Random.Range(0,obj1.transform.childCount)).gameObject;

                        }
                        else
                        {
                              clone = obj2.transform.GetChild(Random.Range(0,obj2.transform.childCount)).gameObject;
                        }
                        if(Random.Range(0,4) > -1)
                        {

                      GameObject newclone = Instantiate(clone,spawntable(new Vector3(count * 2,0,count2 * 2)),clone.transform.rotation);
                        newclone.transform.parent = gameObj.transform;
                      }
                        count2 ++;
                    }
            }
              else{
                    count2 = 0;
                    while (count2 < 10)
                    {
                        if (count2 < 4){
                          clone = obj2.transform.GetChild(Random.Range(0,obj2.transform.childCount)).gameObject;

                      }
                      else
                      {
                            clone = obj0.transform.GetChild(Random.Range(0,obj0.transform.childCount)).gameObject;
                      }
                      if(Random.Range(0,4) > -1)
                      {
                      GameObject newclone = Instantiate(clone,spawntable(new Vector3(count * 2,0,count2 * 2 )) ,clone.transform.rotation);
                      newclone.transform.parent = gameObj.transform;
                    }
                    clone = obstacles.transform.GetChild(Random.Range(0,obstacles.transform.childCount)).gameObject;
                    Instantiate(clone,spawntable(new Vector3(count * 2 + 1,0,count2 * 2 + 1)) ,clone.transform.rotation);
                      count2 ++;
                  }

              }

        count ++;
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
