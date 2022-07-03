using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axises { Horizontal, Vertical,rightStickHorizontal,rightStickVertical,Thrust }
public enum Buttons { none,A,B,X,Y,LB,RB,Leftstick,Rightstick,leftTrigger,rightTrigger }


public static class InputControls 
{
    public static bool gamePad;
    //controller buttons
    public static string hortAxis = "ControllerHorizontal", vertAxis = "ControllerVertical", interactButton = "A", pickupButton = "Y", actionButton = "B", nextButton = "RB", previousButton = "LB";
    public static string aButton = "A", yButton = "Y", bButton = "B", xButton="X",rbButton = "RB", lbButton = "LB",leftstickButton="Leftstick", rightstickButton = "Rightstick";

    public static string leftTrigger = "LeftTrigger", rightTrigger = "RightTrigger";
    public static string primaryEngineAxis = "RightTrigger", rollAxis = "RollAxis";

    public static string thrustAxis = "3rd Axis";


    public static string menuButton = "Start";
    //controller axis as buttons
    public static string dPadVertButton = "DpadVert",dPadHortButton = "DpadHort";
    public static string cameraHorizontal = "4th Axis",cameraVertical = "5th Axis";


    public static bool dPadVertPressed,dPadHortPressed,vertPressed,hortPressed;

    public static KeyCode interactKey = KeyCode.Space, pickupKey = KeyCode.LeftControl, actionKey = KeyCode.RightControl, nextKey = KeyCode.E, previousKey = KeyCode.Q, dPadDownKey = KeyCode.DownArrow;
    public static KeyCode menuKey = KeyCode.Return;

    private static float tracker_lastTimeDPadHortPressed;
    private static float tracker_lastTimeDPadVertPressed;

    private static float tracker_lastTimeLeftTriggerPressed;
    private static float tracker_lastTimeRightTriggerPressed;

    private static float deadtime = 0.5f, deadClock; //holding an axis while using as a button

    private static bool rightTriggerPressed, leftTriggerPressed;


    public static void ResetAxisAsButton()
    {
        if (leftTriggerPressed == true && Input.GetAxis(leftTrigger) == 0)
        {
            leftTriggerPressed = false;
        }

        if (rightTriggerPressed == true && Input.GetAxis(rightTrigger) == 0)
        {
            rightTriggerPressed = false;
        }
        
    }

    public static bool CheckButtonPressed(Buttons _button)
    {
        ResetAxisAsButton();

        if (_button == Buttons.A) { return Input.GetButtonDown(aButton); }
        else if (_button == Buttons.B) { return Input.GetButtonDown(bButton); }
        else if (_button == Buttons.Y) { return Input.GetButtonDown(yButton); }
        else if (_button == Buttons.X) { return Input.GetButtonDown(xButton); }
        else if (_button == Buttons.LB) { return Input.GetButtonDown(lbButton); }
        else if (_button == Buttons.RB) { return Input.GetButtonDown(rbButton); }
        else if (_button == Buttons.Leftstick) { return Input.GetButtonDown(leftstickButton); }
        else if (_button == Buttons.Rightstick) { return Input.GetButtonDown(rightstickButton); }
        else if (_button == Buttons.leftTrigger && leftTriggerPressed == false)
        {
            if (Input.GetAxis(leftTrigger) == 1)
            {
                leftTriggerPressed = true;
                return Input.GetButtonUp(rightstickButton);
            }
        }

        return false;
    }

    public static bool CheckButtonReleased(Buttons _button)
    {
        if (_button == Buttons.A) { return Input.GetButtonUp(aButton); }
        else if (_button == Buttons.B) { return Input.GetButtonUp(bButton); }
        else if (_button == Buttons.Y) { return Input.GetButtonUp(yButton); }
        else if (_button == Buttons.X) { return Input.GetButtonUp(xButton); }
        else if (_button == Buttons.LB) { return Input.GetButtonUp(lbButton); }
        else if (_button == Buttons.RB) { return Input.GetButtonUp(rbButton); }
        else if (_button == Buttons.Leftstick) { return Input.GetButtonUp(leftstickButton); }
        else if (_button == Buttons.Rightstick) { return Input.GetButtonUp(rightstickButton); }
        else if (_button == Buttons.leftTrigger) { return Input.GetButtonUp(rightstickButton); }

        return false;
    }


    public static bool CheckButton(Buttons _button)
    {
        if (_button == Buttons.A) { return Input.GetButton(aButton); }
        else if (_button == Buttons.B) { return Input.GetButton(bButton); }
        else if (_button == Buttons.Y) { return Input.GetButton(yButton); }
        else if (_button == Buttons.X) { return Input.GetButton(xButton); }
        else if (_button == Buttons.LB) { return Input.GetButton(lbButton); }
        else if (_button == Buttons.RB) { return Input.GetButton(rbButton); }
        else if (_button == Buttons.Leftstick) { return Input.GetButton(leftstickButton); }
        else if (_button == Buttons.Rightstick) { return Input.GetButton(rightstickButton); }
        else if (_button == Buttons.leftTrigger )
        {
            
                return Input.GetAxis(leftTrigger) == 1;
            
        }

        return false;
    }


    public static float CheckAxis(Axises _axis)
    {
        if (_axis == Axises.Thrust) { return Input.GetAxis(thrustAxis); }
        else { return 0; }
       

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
        if (Input.GetButton(nextButton) || Input.GetKey(nextKey) )
        {  return true; }
        return false;
    }

    public static bool PreviousButton()
    {
        if (Input.GetButton(previousButton) || Input.GetKey(previousKey) )
        {  return true; }
        return false;
    }

    public static bool DpadHortAsButton()
    {
        if (Input.GetAxis(dPadHortButton) != 0)
        {
            if (Time.time - tracker_lastTimeDPadHortPressed > deadtime)
            {
                tracker_lastTimeDPadHortPressed = Time.time;
                return true;
            }
        }
        return false;
    }

    public static bool DpadVertAsButton()
    {
        if (Input.GetAxis(dPadVertButton) != 0)
        {
            if (Time.time - tracker_lastTimeDPadVertPressed > deadtime)
            {
                tracker_lastTimeDPadVertPressed = Time.time;
                return true;
            }
            
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

    public static float RollAxis()
    { return Input.GetAxis(rollAxis); }

    public static float PrimaryEngineAxis()
    { return Input.GetAxis(primaryEngineAxis); }

    public static float HorizontalAxis()
    { return Input.GetAxis(hortAxis) + Input.GetAxis("Horizontal"); }

    public static float VerticalAxis()
    { return Input.GetAxis(vertAxis) + Input.GetAxis("Vertical"); ; }

    public static float CameraHorizontalAxis()
    { return Input.GetAxis(cameraHorizontal) ; }

    public static float CameraVerticalAxis()
    { return Input.GetAxis(cameraVertical) ;  }

    public static float LeftTrigger()
    { return Input.GetAxis(leftTrigger) ; }

    public static float RightTrigger()
    { return Input.GetAxis(rightTrigger); }

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
