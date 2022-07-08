﻿
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menus : Manager {

    public UI_scriptable uiAssets;
    public Transform menu_Transform;
    public Transform menu_DeathChoice;
    public Transform menu_InGameUI;

    public Transform menu_equip;
    public Transform menu_FTL;

    public Transform parent_pauseMenus;

    public Transform screen_pause;
    public Transform screen_inGame;


    public Transform parent_inventoryChoiceButtons;
    public Transform parent_fleetShips;

    //the text is the only thing updated, the buttons
    //the buttons pass an int that references the saved list
    public List<Text> inventoryChoiceButtonsText;
    public List<Item> currentItemChoices;

    public Text TEXT_itemChoiceStatDisplay;
    public Text TEXT_equipmentTotals;

    public Text TEXT_fleetChoiceStatDisplay;
    public Text TEXT_fleetTotals;


    public Text TEXT_playerStamina;
    public Text TEXT_playerHitPoints;

    public Text TEXT_bulletEquipped;
    public Text TEXT_consumableEquipped;

    public Text TEXT_clock;

    public Button button_weapon;
    public Button button_chasis;
    public Button button_engine;
    public Button button_vehicle;
    public Button button_bullet;


    private GameObject currentOpenMenu;

    private int bookmark_itemList;



    public void StartInGame( )
    {
        DisableMenus();

        

        if (MenuInGameUI())
        { MenuInGameUI().gameObject.SetActive(true); }
    }


    //listener added to the player death event
    public void PlayerDeath()
    {
        
        DisableMenus();

        if (MenuDeathChoice())
        { MenuDeathChoice().gameObject.SetActive(true); }

    }

    public void DisableMenus()
    {
        if (MenuInGameUI())
        { MenuInGameUI().gameObject.SetActive(false); }

        if (MenuBase())
        { MenuBase().gameObject.SetActive(false); }

        if (MenuDeathChoice())
        { MenuDeathChoice().gameObject.SetActive(false); }

        if (menu_equip )
        { menu_equip.gameObject.SetActive(false); }

        if (screen_pause)
        { screen_pause.gameObject.SetActive(false); }

        if (parent_pauseMenus)
        {
            foreach (Transform el in parent_pauseMenus)
            {
                el.gameObject.SetActive(false);
            }
        }

    }


    public void OpenPauseMenu( )
    {
        DisableMenus();

        if (screen_pause)
        {
            if (screen_pause.gameObject.activeSelf )
            { screen_pause.gameObject.SetActive(false); }
            else { screen_pause.gameObject.SetActive(true); }
        }

        ShowFleetShips();

        TimeManager().TimeAdvance(TimeType.menuScreens,1);
    }


    public void EnableEquipMenu()
    {
        DisableMenus();

        if (menu_equip) 
        { 
            menu_equip.gameObject.SetActive(true);
        }
    }

    public void EnableFTLMenu()
    {
        DisableMenus();

        if (menu_FTL) 
        {
            menu_FTL.gameObject.SetActive(true);
        }
    }

    public void ButtonEvent_OpenMenu(GameObject _menu)
    {
       // DisableMenus();

        if (currentOpenMenu) 
        {
            currentOpenMenu.SetActive(false);
        }

        if (parent_pauseMenus)
        {
            foreach (Transform el in parent_pauseMenus)
            {
                if (el.gameObject != _menu)
                { el.gameObject.SetActive(false); }
                
            }
        }

        if (_menu)
        {
                currentOpenMenu = _menu;
                _menu.gameObject.SetActive(true);
        }
    }



    /// <summary>
    /// 
    /// Inventory
    /// 
    /// </summary>


    public void SetButtonTextToItemEquiped(Item _item)
    {
        Button tempButton = null;

        if (_item.type == ItemTypes.weapon )
        {
            tempButton = button_weapon;
        }
        if (_item.type == ItemTypes.engine)
        {
            tempButton = button_engine;
        }
        if (_item.type == ItemTypes.chasis)
        {
            tempButton = button_chasis;
        }
        if (_item.type == ItemTypes.vehicle)
        {
            tempButton = button_vehicle;
        }
        if (_item.type == ItemTypes.bullet)
        {
            tempButton = button_bullet;
        }

        if (tempButton != null)
        {
            SetButtonTextToItemEquiped(tempButton, _item.name);
        }


    }

    public void SetButtonTextToItemEquiped(Button _button, string _itemName)
    {
        if (_button.transform.childCount > 0)
        { _button.transform.GetChild(0).GetComponent<Text>().text = _itemName; }
    }



    public void ButtonEvent_EquipItem(int _item)
    {
        //sends an int that represents the list this menu manager holds
        // showing which items are currently displayed

        Debug.Log("ButtonEvent_EquipItem: " + _item);
        if (_item < CurrentItemChoices().Count && _item >= 0)
        {
            Debug.Log(CurrentItemChoices()[_item].ToString());
            if (CurrentItemChoices()[_item] != null)
            {
                Player().EquipItem(CurrentItemChoices()[_item]);
                SetButtonTextToItemEquiped(CurrentItemChoices()[_item]);

            }
        }


    }


    //note: Unity buttons need to use an int not the enum itself
    //enum ItemTypes { weapon, chasis, engine, usable, ammo,none }
    public void ButtonEvent_ShowEquipOptions(int _itemType)
    {
        Debug.Log("ButtonEvent_ShowEquipOptions: " + (ItemTypes)_itemType);
        //when the player selects an equipment slot populate the choices based on which
        //slot was clicked: The buttons are set in editor to pass an int (0..N)
        //which references the current item list this holds
        List<Item> itemList = ItemManager().GetAllByType((ItemTypes)_itemType);
        Debug.Log(itemList.Count);
        CurrentItemChoices().Clear();

        if (bookmark_itemList >= itemList.Count)
        {
            bookmark_itemList = 0;
        }

        int count = 0;

        while (count < InventoryChoiceButtonsText().Count)
        {
            if (itemList.Count > bookmark_itemList + count)
            {
                Item tempItem = itemList[bookmark_itemList + count];
                CurrentItemChoices().Add(tempItem);
                InventoryChoiceButtonsText()[count].text = tempItem.ToString();
            }
            else
            {
                InventoryChoiceButtonsText()[count].text = "---";
            }
            count++;
        }

        
    }


    public void ButtonHover_EquipItem(int _item)
    {
        //sends an int that represents the list this menu manager holds
        // showing which items are currently displayed


        if (_item < CurrentItemChoices().Count && _item >= 0)
        {
            if (CurrentItemChoices()[_item] != null)
            {
                if (TEXT_itemChoiceStatDisplay)
                {
                    TEXT_itemChoiceStatDisplay.text = CurrentItemChoices()[_item].ToString();
                }
            }
        }


    }

    public void DisplayEquipmentTotals(Equipment _equipment)
    {
        if (TEXT_equipmentTotals == null) { return; }

        string tempString = "Total:       ";



        foreach (Stats el in (Stats[])Enum.GetValues(typeof(Stats)))
        {
            if (_equipment.GetStats().ContainsKey(el) )
            {
                if (el != Stats.none && el != Stats.pointValue)
                {
                    tempString += " \n";
                    tempString += el.ToString();
                    tempString += ":   ";
                    tempString += _equipment.GetStats()[el];

                }
            }



        }

        TEXT_equipmentTotals.text = tempString;
    }



    public void ShowFleetShips()
    {
        Debug.Log("ShowFleetShips: " );

        List<FleetShip> shipList = FleetManager().Ships();
        Debug.Log(shipList.Count);


      

        int count = 0;

        while (count < shipList.Count && count < parent_fleetShips.childCount)
        {
            if (parent_fleetShips.GetChild(count).GetComponent<UiFleetshipButton>())
            {
                UiFleetshipButton btn = parent_fleetShips.GetChild(count).GetComponent<UiFleetshipButton>();
                
                if (shipList[count].GetStats()[Stats.food] != 0)
                {
                    btn.SetResourceImage(null, "Food");

                    btn.SetResourceText(
                        shipList[count].GetStats()[Stats.food],
                        shipList[count].GetStats()[Stats.producefood],
                        shipList[count].GetStats()[Stats.modifyfoodproduction]
                        );
                }
                else if (shipList[count].GetStats()[Stats.fuel] != 0)
                {
                    btn.SetResourceImage(null, "Fuel");

                    btn.SetResourceText(
                        shipList[count].GetStats()[Stats.fuel],
                        shipList[count].GetStats()[Stats.producefuel],
                        shipList[count].GetStats()[Stats.modifyfuelproduction]
                        );
                }

                btn.SetStorageText(shipList[count].GetStats()[Stats.storage]);
                btn.SetShipNameText(shipList[count].name);
            }

            

            count++;
        }

    }

    public void ButtonEvent_ExpandShipDetails(int _ship)
    {
        //sends an int that represents the list this menu manager holds
        // showing which items are currently displayed

        Debug.Log("ButtonEvent_ExpandShipDetails: " + _ship);
        //if (_ship < CurrentItemChoices().Count && _ship >= 0)
        //{
           
        //    if (CurrentItemChoices()[_ship] != null)
        //    {
        //        Player().EquipItem(CurrentItemChoices()[_ship]);
        //        SetButtonTextToItemEquiped(CurrentItemChoices()[_ship]);

        //    }
        //}


    }


    //note: Unity buttons need to use an int not the enum itself
    //enum ItemTypes { weapon, chasis, engine, usable, ammo,none }
    public void ButtonEvent_ShowFleetShips()
    {
        Debug.Log("ButtonEvent_ShowFleetShips: ");

        if (parent_fleetShips && FleetManager())
        { 
            
        }

    }





















    public void ButtonEvent_LevelSelected(int _level)
    {
        DisableMenus();
        if (screen_pause) { screen_pause.gameObject.SetActive(false); }

        GameManager().TravelFromHub(_level);
    }


    public void Set_PlayerStaminaText(float _value=0)
    {
        if (TEXT_playerStamina)
        {
            SetText(TEXT_playerStamina, TurnNumberIntoBar(_value));
        }
    }

    public void Set_PlayerHitPointsText(float _value = 0)
    {
        if (TEXT_playerHitPoints)
        {
            SetText(TEXT_playerHitPoints, TurnNumberIntoBar(_value));
        }
    }

    public void Set_PlayerBulletEquippedText(string _value)
    {
        if (TEXT_bulletEquipped)
        {
            SetText(TEXT_bulletEquipped, _value);
        }
    }

    public void Set_PlayerConsumableEquippedText(string _value )
    {
        if (TEXT_consumableEquipped)
        {
            SetText(TEXT_consumableEquipped, _value);
        }
    }


    public void SetText(Text _textObj,string _text)
    {
        _textObj.text = _text;
    }

    public string TurnNumberIntoBar(float _value)
    {
        string bars = "";
        int count = 0; //sanity check for the loop
        while (count < _value || count > 100)
        {
            bars = bars + ".";
            count++;
        }

        return bars;
    }




    public Transform MenuBase()
    { return menu_Transform; }
    public Transform MenuDeathChoice()
    { return menu_DeathChoice; }

    public Transform MenuInGameUI()
    { return menu_InGameUI; }

    public Transform ParentInventorySelectionChoiceButtons()
    { return menu_Transform; }

    public List<Item> CurrentItemChoices()
    {
        if (currentItemChoices == null )
        {
            currentItemChoices = new List<Item>();

        }
        return currentItemChoices;
    }
    public List<Text> InventoryChoiceButtonsText()
    {
        if (inventoryChoiceButtonsText == null || inventoryChoiceButtonsText.Count == 0)
        {
            inventoryChoiceButtonsText = new List<Text>();

            foreach (Transform el in parent_inventoryChoiceButtons)
            {
                if (el.childCount > 0 && el.GetChild(0).GetComponent<Text>())
                {
                    inventoryChoiceButtonsText.Add(el.GetChild(0).GetComponent<Text>());
                }
            }
        }
        return inventoryChoiceButtonsText;
    }

    public UI_scriptable GetAssets()
    {


        return uiAssets;
    }

}
