using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieintime : MonoBehaviour
{


      public float lifetime;
    public string parentName = "";
    private Transform parentTransform;

       void Start()
       {

       }

       // Update is called once per frame
       void Update()
       {
           if (lifetime != -1)
           {
               lifetime -= Time.deltaTime;
               if (lifetime <= 0) 
            {
                Die();
                
            }
           }
       }


    public void Die()
    {
        if (parentTransform)
        {
            transform.parent = parentTransform;
            gameObject.SetActive(false);
        }
        else { Destroy(this.gameObject); }
    }

  }
