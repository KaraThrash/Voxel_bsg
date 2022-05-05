using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    Transform gravityCenter;

    public EnviromentType enviroment;
    public float gravityStrength;




    public Vector3 Gravity()
    {
        //in the absence of a relative obj direct gravity from the origin
        if (gravityCenter)
        {
            return -(gravityCenter.position).normalized * gravityStrength;
        }

        return Vector3.down * 9.81f;
    }

    public Vector3 Gravity(Transform _obj)
    {
        //only the central object of the map excerts gravity [or nothing does: e.g. space]
        if (gravityCenter)
        {
            return (_obj.position - gravityCenter.position).normalized * gravityStrength;
        }

        return Vector3.down * 9.81f;
    }


    public void Enviroment(EnviromentType _env) { enviroment = _env; }
    public EnviromentType Enviroment( ) { return enviroment ; }


}
