using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public struct Item {
  public string name;
  public int type;
  public int placeInMasterList;
  private int playerHeldCount;

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
    public Text fueltext, moneytext;
    public GameManager gameManager;
    public  List<Item> MasterItemList;
    //public List<GameObject> InInventory; // items available to the player, but not equiped or a recent pick up while on mission
    public List<Vector2> equipedItems; //slot,item
    public List<Vector2> pickedUpItems; //picked up during a mission
    public List<Vector2> gunsInInventory, hullsInInventory,enginesInInventory, bulletsInInventory, consumablesInInventory,playerInventory;

    public Transform inventoryButtons,equipButtons,bulletbuttons,consumablebuttons;
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
        name = "Default Gun",
        type = 0,
        placeInMasterList = 0
  };
  newItem.setPlayerHeld(3);
  gunsInInventory.Add(new Vector2(0,1));
  MasterItemList.Add(newItem);
   newItem = new Item
 {
        name = "Default Hull",
        type = 1,
        placeInMasterList = 1

  };
    newItem.setPlayerHeld(3);
  hullsInInventory.Add(new Vector2(1,1));
    MasterItemList.Add(newItem);

    newItem = new Item
  {
         name = "Default Engine",
         type = 2,
         placeInMasterList = 2
   };
     newItem.setPlayerHeld(3);
   enginesInInventory.Add(new Vector2(2,1));
     MasterItemList.Add(newItem);
     newItem = new Item
   {
          name = "Default Consumable",
          type = 3,
          placeInMasterList = 3
    };
      newItem.setPlayerHeld(3);
      consumablesInInventory.Add(new Vector2(3,1));
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
         string text = File.ReadAllText("MasterItemFile.txt");

         string[] strValues = text.Split(';');

         int count = 0;
         while(count < strValues.Length)
         {
                string[] tempstring = strValues[count].Split(',');

                  Item newitem = new Item
                 {
                        name = tempstring[0],
                        type = 5,
                        placeInMasterList = MasterItemList.Count
                  };
                    newitem.setPlayerHeld(3);

              if(tempstring.Length > 1){

                      if(tempstring[1].Trim() == "weapon"){newitem.type = 0;}
                     else if(tempstring[1].Trim() == "hull"){newitem.type = 1;}
                       else if(tempstring[1].Trim() == "engine"){newitem.type = 2;}
                         else if(tempstring[1].Trim() == "usable"){newitem.type = 3;}
                         else{}
               }
               MasterItemList.Add(newitem);


                  // myButton.onClick.AddListener(delegate{ToggleEquip(1);});

                 count++;

         }


     }
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
            tempitem2.setPlayerHeld(1);

            MasterItemList[(int)equipedItems[equipSlot].y] = tempitem2;

          }

      }
      //structs passed by value, calling a struct gets a copy.
      if(whichitem > 3)
      {
        Item tempitem = MasterItemList[whichitem];
        tempitem.setPlayerHeld(-1);
        MasterItemList[whichitem] = tempitem;
      }

      equipButtons.GetChild(equipSlot).GetChild(0).GetComponent<Text>().text = MasterItemList[whichitem].name;
      equipedItems[equipSlot] = new Vector2(equipSlot,whichitem);
      ResetTypeButtons();
    }

    public void ItemPickUp(GameObject whichitem)
    {

    }


    public void ToggleHangarDisplay(int weaponslots,int engineslots,int hullslots,int bulletslots,int consumableslots)
    {
    //
    //
    //     List<Button> shipslots = new List<Button>();
    //     shipslots.AddRange(gunbuttons);
    //     shipslots.AddRange(enginebuttons);
    //     shipslots.AddRange(hullbuttons);
    //     shipslots.AddRange(bulletbuttons);
    //     shipslots.AddRange(consumablebuttons);
    //     foreach (Button inventorybutton in shipslots)
    //     {
    //         inventorybutton.gameObject.active = false;
    //     }
    //     Fighter selectedFighter = gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>();
    //
    //     int count2 = 0;
    //         while (count2 < selectedFighter.weaponslots)
    //         {
    //             gunbuttons[count2].gameObject.active = true;
    //         if (selectedFighter.weaponequipped.Count > count2)
    //         {
    //             gunbuttons[count2].transform.GetChild(0).GetComponent<Text>().text = AllItems[selectedFighter.weaponequipped[count2]].name;
    //         }
    //         else { gunbuttons[count2].transform.GetChild(0).GetComponent<Text>().text = "weapon slot"; }
    //             count2++;
    //         }
    //         count2 = 0;
    //     while (count2 < selectedFighter.engineslots)
    //     {
    //         enginebuttons[count2].gameObject.active = true;
    //         if (selectedFighter.enginequipped.Count > count2)
    //         {
    //             enginebuttons[count2].transform.GetChild(0).GetComponent<Text>().text = AllItems[selectedFighter.enginequipped[count2]].name;
    //         }
    //         else { enginebuttons[count2].transform.GetChild(0).GetComponent<Text>().text = "engine slot"; }
    //         count2++;
    //     }
    //     count2 = 0;
    //     while (count2 < selectedFighter.hullslots)
    //     {
    //         hullbuttons[count2].gameObject.active = true;
    //         if (selectedFighter.hullequipped.Count > count2)
    //         {
    //             hullbuttons[count2].transform.GetChild(0).GetComponent<Text>().text = AllItems[selectedFighter.hullequipped[count2]].name;
    //         }
    //         else { hullbuttons[count2].transform.GetChild(0).GetComponent<Text>().text = "hull slot"; }
    //         count2++;
    //     }
    //     count2 = 0;
    //     while (count2 < selectedFighter.bulleteslots)
    //     {
    //         bulletbuttons[count2].gameObject.active = true;
    //         count2++;
    //     }
    //     count2 = 0;
    //     while (count2 < selectedFighter.consumableslots)
    //     {
    //         consumablebuttons[count2].gameObject.active = true;
    //         count2++;
    //     }
    //     count2 = 0;
    //
    //
    //
    }



    public void ResetTypeButtons()
    {
      foreach (Transform inventorybutton in inventoryButtons)
      {
        //reset text of button then disable it
        inventorybutton.GetChild(0).gameObject.GetComponent<Text>().text = "";
          inventorybutton.gameObject.active = false;

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
        if(placeinlist >= MasterItemList.Count || whattype != typedisplayed)
        {
          placeinlist = 0;
        }

        typedisplayed = whattype;

              if(MasterItemList.Count != 0) {
                  //go through all available items of the correct type, and refernce the master list for it's info
                  while (placeinlist < MasterItemList.Count)
                  {
                    //X is the items place in the master list, y is the value held by the player
                    if( MasterItemList[placeinlist].type == typedisplayed)
                    {
                      inventoryButtons.GetChild(count).gameObject.active = true;
                      inventoryButtons.GetChild(count).GetChild(0).GetComponent<Text>().text = MasterItemList[placeinlist].name + " : " + MasterItemList[placeinlist].getPlayerHeld();

                      //so the button is not set to the variable, since we do not want the value to update
                      int tempint = placeinlist;
                      inventoryButtons.GetChild(count).GetComponent<Button>().onClick.RemoveAllListeners();
                      inventoryButtons.GetChild(count).GetComponent<Button>().onClick.AddListener(delegate{ToggleEquip(tempint);});
                      count++;
                      //display a sub set of inventory at a time
                          if(count >= inventoryButtons.childCount)
                          {
                            placeinlist++;
                            return;
                          }
                    }

                    placeinlist++;

                  }
              }


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
