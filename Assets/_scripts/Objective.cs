using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectiveEvent : UnityEvent<InGameEvent>
{

}

public class Objective : Actor
{
    // Start is called before the first frame update
    void Start()
    {
        if (Map())
        { transform.parent = Map().transform; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Hitpoints() != -1 && Hitpoints() <= 0)
        { Die(); }
    }


}
