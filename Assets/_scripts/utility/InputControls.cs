using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputControls 
{
    public static bool gamePad;
    //controller buttons
    public static string hortAxis = "ControllerHorizontal", vertAxis = "ControllerVertical", interactButton = "A", pickupButton = "Y", actionButton = "B", nextButton = "RB", previousButton = "LB";
    public static string leftTrigger = "LeftTrigger", rightTrigger = "RightTrigger";
    public static string menuButton = "Start";
    //controller axis as buttons
    public static string dPadVertButton = "DpadVert",dPadHortButton = "DpadHort";
    public static bool dPadVertPressed,dPadHortPressed,vertPressed,hortPressed;

    public static KeyCode interactKey = KeyCode.Space, pickupKey = KeyCode.LeftControl, actionKey = KeyCode.RightControl, nextKey = KeyCode.E, previousKey = KeyCode.Q, dPadDownKey = KeyCode.DownArrow;
    public static KeyCode menuKey = KeyCode.Return;

    private static float deadtime = 0.5f, deadClock; //holding an axis while using as a button








    static void Start()
    {
     
    }

    // Update is called once per frame
     static void Update()
    {

        //TrackAxisButtons();


        if (gamePad == false)
        {
           // KeyboardControls();
        }
        else
        {
            //GamepadControls();
        }
    }

    public static void TrackAxisButtons()
    {
        //for using the axises as a button, can only press again once they reset to 0
        if (dPadVertPressed == true && Input.GetAxis(dPadVertButton) == 0)
        { dPadVertPressed = false; }

        if (dPadHortPressed == true && Input.GetAxis(dPadHortButton) == 0)
        { dPadHortPressed = false; }

        if (hortPressed == true && Input.GetAxis(hortAxis) == 0)
        { hortPressed = false; }

        if (vertPressed == true && Input.GetAxis(vertAxis) == 0)
        { vertPressed = false; }

        if (deadClock > 0)
        {
            deadClock -= Time.deltaTime;
            if (deadClock <= 0)
            {
                dPadVertPressed = false;
                dPadHortPressed = false;
                hortPressed = false;
                vertPressed = false;
            }
        }
    }

    public static bool InteractButton()
    {
        if (Input.GetButtonDown(interactButton) || Input.GetKeyDown(interactKey))
        { return true; }
        return false;
    }

    public static bool ActionButtonDown()
    {
        if (Input.GetButtonDown(actionButton) || Input.GetKeyDown(actionKey))
        { return true; }
        return false;
    }

    public static bool ActionButtonUp()
    {
        if (Input.GetButtonUp(actionButton) || Input.GetKeyUp(actionKey))
        { return true; }
        return false;
    }

    public static bool MenuButton()
    {
        if (Input.GetButtonDown(menuButton) || Input.GetKeyDown(menuKey))
        { return true; }
        return false;
    }

    public static bool PickUpButton()
    {
        if (Input.GetButtonDown(pickupButton) || Input.GetKeyDown(pickupKey))
        { return true; }
        return false;
    }






    public static bool NextButton()
    {
        if (Input.GetButtonDown(nextButton) || Input.GetKeyDown(nextKey) )
        {  return true; }
        return false;
    }

    public static bool PreviousButton()
    {
        if (Input.GetButtonDown(previousButton) || Input.GetKeyDown(previousKey) )
        {  return true; }
        return false;
    }

    public static bool DpadHortAsButton()
    {
        if (dPadHortPressed == false && (Input.GetAxis(dPadHortButton) != 0))
        {
            deadClock = deadtime;
            dPadHortPressed = true;
            return true;
        }
        return false;
    }

    public static bool DpadVertAsButton()
    {
        if (dPadVertPressed == false && Input.GetAxis(dPadVertButton) != 0)
        {
            deadClock = deadtime;
            dPadVertPressed = true;
            return true;
        }
        return false;
    }



    public static bool VertAsButton()
    {
        if (vertPressed == false && Mathf.Abs(Input.GetAxis(vertAxis)) == 1)
        {
            deadClock = deadtime;
            vertPressed = true;
            return true;
        }
        return false;
    }

    public static bool HortAsButton()
    {
        if (hortPressed == false && Mathf.Abs(Input.GetAxis(hortAxis)) == 1)
        {
            deadClock = deadtime;
            hortPressed = true;
            return true;
        }
        return false;
    }

    public static float HorizontalAxis()
    { return Input.GetAxis(hortAxis) + Input.GetAxis("Horizontal"); }

    public static float VerticalAxis()
    { return Input.GetAxis(vertAxis) + Input.GetAxis("Vertical"); ; }

    public static float LeftTrigger()
    { return Input.GetAxis(leftTrigger) + Input.GetAxis("LeftTrigger"); }

    public static float RightTrigger()
    { return Input.GetAxis(rightTrigger) + Input.GetAxis("RightTrigger"); ; }

    public static float DpadHort()
    {

        return Input.GetAxis(dPadHortButton);
    }

    public static float DpadVert()
    {

        return Input.GetAxis(dPadVertButton);
    }



    public static void KeyboardControls()
    {
        if (Input.GetAxis(hortAxis) != 0)
        { 
            
        }
    }

    public static void GamepadControls()
    { 
    
    }

}
