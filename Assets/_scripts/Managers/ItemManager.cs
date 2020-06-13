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

    public int money, fuel;
    public Text fueltext, moneytext,itemstatdisplay,pickUpText,playerShipStatsDisplay;
    [Header("0:weapon||1:hull||2:engine||3:usable||4:ammo")]
    public GameManager gameManager;

    public  List<Item> MasterItemList;
    //public List<GameObject> InInventory; // items available to the player, but not equiped or a recent pick up while on mission
    public List<Vector2> equipedItems; //slot,item
    public List<Vector2> pickedUpItems; //picked up during a mission
    public List<Vector2> playerInventory;

    public Transform inventoryButtons,inventoryStatText,equipButtons,bulletbuttons,consumablebuttons;
      public GameObject itemDrop,nextButton;
    public int typedisplayed,placeinlist,equipSlot;//placeinlist for scrolling through items, show: 0-9, 10-19 etc //TODO: sort options


    void Start () {
        //DisplayInventory();
        equipedItems = new List<Vector2>();
        MasterItemList = new List<Item>();
        SetDefaultItemList();
        MakeITemListFromFile();
    }



  public void SetDefaultItemList()
  {

          Item newItem = new Item
         {
                name = "DefaultGun",
                type = 0,
                placeInMasterList = 0,
                defaultItem = true
          };
          newItem.setPlayerHeld(1);

          MasterItemList.Add(newItem);
           newItem = new Item
         {
                name = "DefaultHull",
                type = 1,
                placeInMasterList = 1,
                defaultItem = true

          };
            newItem.setPlayerHeld(1);

            MasterItemList.Add(newItem);

            newItem = new Item
          {
                 name = "DefaultEngine",
                 type = 2,
                 placeInMasterList = 2,
                 defaultItem = true
           };
             newItem.setPlayerHeld(1);

             MasterItemList.Add(newItem);
             newItem = new Item
           {
                  name = "DefaultConsumable",
                  type = 3,
                  placeInMasterList = 3,
                  defaultItem = true
            };
              newItem.setPlayerHeld(1);

              MasterItemList.Add(newItem);

              newItem = new Item
            {
                   name = "DefaultBullet",
                   type = 4,
                   placeInMasterList = 4,
                   defaultItem = true
             };
               newItem.setPlayerHeld(1);
               MasterItemList.Add(newItem);

              playerInventory.Add(new Vector2(0,1));
              playerInventory.Add(new Vector2(1,1));
              playerInventory.Add(new Vector2(2,1));
              playerInventory.Add(new Vector2(3,1));
              playerInventory.Add(new Vector2(4,1));
              equipedItems.Add(new Vector2(0,0));
              equipedItems.Add(new Vector2(0,0));
              equipedItems.Add(new Vector2(0,0));
              equipedItems.Add(new Vector2(0,0));
              equipedItems.Add(new Vector2(0,0));
              equipedItems.Add(new Vector2(0,0));
  }
    public void MakeITemListFromFile()
     {
         // string text = File.ReadAllText("./Resources/Items/MasterItemFile.txt");
         //Load a text file (Assets/Resources/Text/textFile01.txt)
                 string text = Resources.Load<TextAsset>("Items/MasterItemSheetcsv").ToString();
         string[] strValues = text.Split('\n');
         print(strValues[0]);
         print(strValues[1]);
         int count = 1; //0 is the header
         while(count < strValues.Length)
         {
                string[] tempstring = strValues[count].Split(',');

                  Item newitem = new Item
                 {
                        name = tempstring[0],
                        type = 5,
                        placeInMasterList = MasterItemList.Count
                  };
                  //NOTE: set to 1 to have all items
                    newitem.setPlayerHeld(2);

              if(tempstring.Length > 1){
//0 = weapon //1 = hul // 2 = engine // 3 = usable // 4 = ammo
                      if(tempstring[1].Trim() == "weapon"){newitem.type = 0;}
                     else if(tempstring[1].Trim() == "hull"){newitem.type = 1;}
                       else if(tempstring[1].Trim() == "engine"){newitem.type = 2;}
                         else if(tempstring[1].Trim() == "usable"){newitem.type = 3;}
                         else if(tempstring[1].Trim() == "ammo"){newitem.type = 4;}
                         else{}
               }

               if(tempstring.Length >= 3){
                  if(tempstring[2].Trim() != "x"){newitem.armor = Convert.ToInt32(tempstring[2]);}
                  }

                  if(tempstring.Length >= 4){
                     if(tempstring[3].Trim() != "x"){newitem.damage = Convert.ToInt32(tempstring[3]);}
                     }

                     if(tempstring.Length >= 5){
                        if(tempstring[4].Trim() != "x"){newitem.speed = Convert.ToInt32(tempstring[4]);}
                        }
                      //
                      //  if(tempstring[2].Trim() == "stats"){newitem.stats = tempstring[3];
                      //     // print("stats: " + newitem.stats);
                      //   }
                      // else if(tempstring[2].Trim() == "effect"){}
                      //
                      //     else{}


               MasterItemList.Add(newitem);


                  // myButton.onClick.AddListener(delegate{ToggleEquip(1);});

                 count++;

               }


     }

     //for pulling prefabs in the resource folder
     public GameObject LoadItemFromResource(string itemtoloadname)
     {
       string tempstring = "Items/" + itemtoloadname;
       GameObject loadedItem = Resources.Load<GameObject>(tempstring);
       //TODO: add a placeholder item as a error catch for items that dont exist for any raisin
       return loadedItem;

     }
	// Update is called once per frame
	void Update () {

	}
public void DestroyEquipOnPlayerDeath()
{
  equipedItems.Clear();
}

    //
    public void ToggleEquip(int whichitem)
    {
      if(MasterItemList[whichitem].getPlayerHeld() <= 0){return;}
      //TODO: auto equip ship with default items
      if(equipedItems.Count > equipSlot)
      {
        //default items are always available and dont need to be returned to the inventory count
          if (equipedItems[equipSlot].y > 3)
          {
            // Item tempitem = MasterItemList[(int)equipedItems[equipSlot].y];
            Item tempitem2 = MasterItemList[(int)equipedItems[equipSlot].y];//new Item{};

            if(tempitem2.defaultItem == false)
            {tempitem2.setPlayerHeld(1);}


            gameManager.playerManager.playerShipStats.UnEquipItem(tempitem2);
            MasterItemList[(int)equipedItems[equipSlot].y] = tempitem2;

          }

      }
      //structs passed by value, calling a struct gets a copy.
      //check that the item isnt a default item: default items are always available
      if(whichitem > 3)
      {

        Item tempitem = MasterItemList[whichitem];
        if(tempitem.defaultItem == false)
        {tempitem.setPlayerHeld(-1);}

        MasterItemList[whichitem] = tempitem;
        string statstring = "";
        if(tempitem.armor != 0){statstring += "armor: " + tempitem.armor.ToString() + "\n";}
        if(tempitem.damage != 0){statstring += "damage: " + tempitem.damage.ToString() + "\n";}
        if(tempitem.speed != 0){statstring += "speed: " + tempitem.speed.ToString() + "\n";}
          itemstatdisplay.text = statstring;

          gameManager.playerManager.playerShipStats.EquipItem(tempitem);
      }

      equipButtons.GetChild(equipSlot).GetChild(0).GetComponent<Text>().text = MasterItemList[whichitem].name;
      equipedItems[equipSlot] = new Vector2(equipSlot,whichitem);
      playerShipStatsDisplay.text = gameManager.playerManager.playerShipStats.GetStatsAsString();
      ResetTypeButtons();
    }

    public void ItemPickUp(GameObject whichitem)
    {
        if(whichitem.GetComponent<PickUp>() != null)
        {
          //zero is just money/points for simplicity of enemies that dont drop special items
          if(whichitem.GetComponent<PickUp>().itemnumber == 0 || whichitem.GetComponent<PickUp>().itemnumber >= MasterItemList.Count)
          {
            SpendMoney(-1);
          }else
          {
            // TODO: add item to inventory
              print(MasterItemList[whichitem.GetComponent<PickUp>().itemnumber].getPlayerHeld());
              Item tempitem2 = MasterItemList[(int)whichitem.GetComponent<PickUp>().itemnumber];//new Item{};
              if(tempitem2.defaultItem == false)
              {tempitem2.setPlayerHeld(1);}

              MasterItemList[(int)whichitem.GetComponent<PickUp>().itemnumber] = tempitem2;

            // MasterItemList[whichitem.GetComponent<PickUp>().itemnumber].setPlayerHeld(MasterItemList[whichitem.GetComponent<PickUp>().itemnumber].getPlayerHeld() + 1);
            print(MasterItemList[whichitem.GetComponent<PickUp>().itemnumber].getPlayerHeld());

            //display item pickedUp
            pickUpText.text = "Gained Item: " + MasterItemList[whichitem.GetComponent<PickUp>().itemnumber].name;
          }


        }
    }


    public void ToggleHangarDisplay(int weaponslots,int engineslots,int hullslots,int bulletslots,int consumableslots)
    {

    }



    public void ResetTypeButtons()
    {
      nextButton.active = false;
      foreach (Transform inventorybutton in inventoryButtons)
      {
        //reset text of button then disable it
        inventorybutton.GetChild(0).gameObject.GetComponent<Text>().text = "";
          inventorybutton.gameObject.active = false;

      }
      foreach (Transform statdisplay in inventoryStatText)
      {
        //reset text of button then disable it
        statdisplay.GetComponent<Text>().text = "";
          // statdisplay.gameObject.active = false;

      }
    }
    public void SetInventorySlot(int whichSlot)
    {
      equipSlot = whichSlot;
    }
    public void showtypeavailable(int whattype)
    {
        //item type of ship slot to equip
        ResetTypeButtons();
        int count = 0;
        //if the entire list has been gone through or the item type has changed, reset the place in list back to the start
        if(placeinlist >= MasterItemList.Count || (whattype != typedisplayed && whattype != -1)) //-1 for scrolling
        {
          placeinlist = 0;
        }
        if(whattype != -1){typedisplayed = whattype;}


              if(MasterItemList.Count != 0) {
                  //go through all available items of the correct type, and refernce the master list for it's info
                  while (placeinlist < MasterItemList.Count)
                  {
                    //X is the items place in the master list, y is the value held by the player
                    if( MasterItemList[placeinlist].type == typedisplayed && MasterItemList[placeinlist].getPlayerHeld() > 0)
                    {
                      inventoryButtons.GetChild(count).gameObject.active = true;
                      inventoryButtons.GetChild(count).GetChild(0).GetComponent<Text>().text = MasterItemList[placeinlist].name + " : " + MasterItemList[placeinlist].getPlayerHeld();
                      Item tempitem = MasterItemList[placeinlist];
                      inventoryStatText.GetChild(count).GetComponent<Text>().text = "Armor: " + tempitem.armor + " Damage: " + tempitem.damage + " Speed: " + tempitem.speed;
                      //so the button is not set to the variable, since we do not want the value to update
                      int tempint = placeinlist;
                      inventoryButtons.GetChild(count).GetComponent<Button>().onClick.RemoveAllListeners();
                      inventoryButtons.GetChild(count).GetComponent<Button>().onClick.AddListener(delegate{ToggleEquip(tempint);});
                      count++;
                      //display a sub set of inventory at a time
                          if(count >= inventoryButtons.childCount)
                          {
                            placeinlist++;
                            nextButton.active = true;
                            return;
                          }
                    }

                    placeinlist++;

                  }
              }


    }

    public void ItemDrop(Vector3 dropLocation,int itemtodrop)
    {
      GameObject clone = Instantiate(itemDrop,dropLocation,transform.rotation);

      if(clone.GetComponent<PickUp>() != null)
      {clone.GetComponent<PickUp>().SetWhichItem(GetComponent<ItemManager>(),UnityEngine.Random.Range(4,MasterItemList.Count));}
    }


    public bool SpendMoney(int cost)
    {

        if (money >= cost)
        {
            money -= cost;
            // moneytext.text = money.ToString();
            return true;
        }
        return false;
    }
}
