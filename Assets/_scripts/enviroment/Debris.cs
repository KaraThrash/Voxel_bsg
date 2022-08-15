using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetDebrisVelocity(transform.position +  transform.up ,5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetDebrisVelocity(Vector3 _pointFrom,float _speed)
    {
        foreach (Transform el in transform)
        {
            if (el.GetComponent<Rigidbody>())
            {
                el.GetComponent<Rigidbody>().velocity = (el.position - _pointFrom).normalized * _speed;
                el.GetComponent<Rigidbody>().angularVelocity = (el.position - _pointFrom).normalized * _speed * Mathf.Sign(Random.Range(-1.0f,1.0f));
            }
        }

    }





}
