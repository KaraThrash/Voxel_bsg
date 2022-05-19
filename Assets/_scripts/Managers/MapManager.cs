﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    //for switching maps and loading/unloading them

    public Map currentMap;

    public Map GetMap()
    {
        if (currentMap == null)
        {
            currentMap = FindObjectOfType<Map>();
        }
        return currentMap;
    }


    
}
