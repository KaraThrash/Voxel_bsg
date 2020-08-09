using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public List<Transform> closest;
    // Start is called before the first frame update
    void Start()
    {
        if (closest.Count <= 0 && transform.parent != null)
        {
            FindAndSetClosest();
            FindAndSetClosest();
            FindAndSetClosest();
            FindAndSetClosest();
        }


    }

    public void FindAndSetClosest()
    {
        float dist = 100.0f;
        Transform tempclosest = transform.parent.GetChild(0);
        foreach (Transform el in transform.parent)
        {
            dist = 100;
            if (el != this.transform && closest.Contains(el) == false)
            {
                float tempdist = Vector3.Distance(transform.position, el.position);
                if (tempdist < Vector3.Distance(transform.position, tempclosest.position) && tempdist > 0)
                { tempclosest = el; dist = tempdist; }


            }
           
        }
        closest.Add(tempclosest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
