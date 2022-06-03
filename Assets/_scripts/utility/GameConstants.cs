using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants 
{
    //default time that controls are locked out from events like collisions with the enviroment or taking damage
    public static float systemStun = 0.2f;

    public static float playerFocalLength = 15.0f;
    public static bool typeA = false;// manually control the ship rotation, the camera focuses in front of the ship

}
