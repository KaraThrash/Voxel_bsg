using System.Collections;
using System.Collections.Generic;
using System.IO;
 using System;
using UnityEngine;
using UnityEngine.UI;


public struct Item {
  public string name;
  //0 = weapon //1 = hul // 2 = engine // 3 = usable // 4 = ammo

  public int type;
  public int placeInMasterList;
  public string stats;
  private int playerHeldCount;
    public int armor;
    public int damage;
    public int speed;
    public int subtype;
    public bool defaultItem;
  public int getPlayerHeld ( )
         {
          return playerHeldCount;
        }
  public void setPlayerHeld (int value)
         {
           playerHeldCount = value + playerHeldCount;
         }
}


public class ItemManager : MonoBehaviour {

 
}
