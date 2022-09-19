using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldTime : Manager
{

    public TimeType primaryTimeElement; //What the game considers for advancing the 'clock' as default real seconds
                                        //but for player QoL/Fun other parameters can be used

  public Text clockText,fleetJumpReadiness;
  public GameObject joinAttackButton,fleetJumpButton;
  public int totalTimePassed,currentMinutes;
  public int fleetJumpTimeIncrements,timeBetweenAttacks = 180; //FTL spools at slow/avg/fast per ship and player can risky jump || leave ships behind
  public float timeUntilAttack,timeSinceLastJump,timerate;
  public bool trackTime;//time stands still in menus

    //Note:    public enum TimeType { time, playerInput, ftlJumps, distanceTraveled, manualControl, realtime, menuScreens }



    public TimeStamp previousTimeStamp;

    private float timer;


    public float tracker_time;
    public float tracker_menu;
    public float tracker_ftlJumps;
    public float tracker_playerInputs;
    public float tracker_distanceTraveled;







    public void EndOfTurn()
    {
        //negative events
        //followed by enemy fleet attack


        EnemyManager().Produce();


    }

    public void StartOfTurn()
    {
        //preceeded by enemy fleet attack
        //positive events

        FleetManager().Produce();
    }














    void Start()
    {
        previousTimeStamp = MakeTimeStamp();
    }


    void Update()
    {
        if (GameManager().GetGameState() == GameState.playing)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                timer = 0;
                TimeAdvance(TimeType.time,1);

                if (MenuManager().TEXT_clock)
                {
                    MenuManager().TEXT_clock.text = GetTimeElement(primaryTimeElement)  + " : " + primaryTimeElement.ToString() ;
                }

            }
        }

        TrackTime();


    }




    public void TimeAdvance(TimeType _time,float _value=1.0f)
    {

        if (_time == TimeType.time)
        { tracker_time += _value; }
        else if (_time == TimeType.playerInput)
        { tracker_playerInputs++; }
        else if (_time == TimeType.ftlJumps)
        { tracker_ftlJumps++; }
        else if (_time == TimeType.menuScreens)
        { tracker_menu++; }
        else if (_time == TimeType.distanceTraveled)
        { tracker_distanceTraveled+= _value; }


    }

    public bool CheckForAttack()
    {
        if (primaryTimeElement == TimeType.time)
        {
            if (tracker_time - previousTimeStamp.time > GameConstants.TIME_BETWEEN_ATTACKS) { return true; }
        }
        else if (primaryTimeElement == TimeType.playerInput)
        {
            if (tracker_playerInputs - previousTimeStamp.playerInputs > GameConstants.INPUT_BETWEEN_ATTACKS) { return true; }
        }
        else if (primaryTimeElement == TimeType.ftlJumps)
        {
            if (tracker_ftlJumps - previousTimeStamp.ftlJumps > GameConstants.JUMPS_BETWEEN_ATTACKS) { return true; }
        }
        else if (primaryTimeElement == TimeType.menuScreens)
        {
            if (tracker_menu - previousTimeStamp.menu > GameConstants.MENU_BETWEEN_ATTACKS) { return true; }
        }
        else if (primaryTimeElement == TimeType.distanceTraveled)
        {
            if (tracker_distanceTraveled - previousTimeStamp.distanceTraveled > GameConstants.DISTANCE_BETWEEN_ATTACKS) { return true; }
        }

        return false;
    }

    public float GetTimeElement(TimeType _time)
    {

        if (_time == TimeType.time)
        { return tracker_time; }
        else if (_time == TimeType.playerInput)
        { return tracker_playerInputs; }
        else if (_time == TimeType.ftlJumps)
        { return tracker_ftlJumps; }
        else if (_time == TimeType.menuScreens)
        { return tracker_menu; }
        else if (_time == TimeType.distanceTraveled)
        { return tracker_distanceTraveled; }

        return -1;
    }

    public float GetTimeElement(TimeStamp _stamp, TimeType _time)
    {

        if (_time == TimeType.time)
        { return tracker_time; }
        else if (_time == TimeType.playerInput)
        { return tracker_playerInputs; }
        else if (_time == TimeType.ftlJumps)
        { return tracker_ftlJumps; }
        else if (_time == TimeType.menuScreens)
        { return tracker_menu; }
        else if (_time == TimeType.distanceTraveled)
        { return tracker_distanceTraveled; }

        return -1;
    }

    public TimeStamp MakeTimeStamp()
    {
        TimeStamp stamp = new TimeStamp();

        stamp.time = tracker_time;
        stamp.menu = tracker_menu;
        stamp.ftlJumps = tracker_ftlJumps;
        stamp.playerInputs = tracker_playerInputs;
        stamp.distanceTraveled = tracker_distanceTraveled;

        return stamp;
    }





    public void ResetTheClock()
    {
      timeUntilAttack = timeBetweenAttacks;
      timeSinceLastJump = 0;
      SetClockText();
    }

    public void SetInMenu(bool isinmenu)
    {
      trackTime = !isinmenu;
      string seconds = "";
      //display 9 seconds remaining as 00:09
      if((int)timeUntilAttack % 60 < 10){seconds += "0";}
      string minutes = "";
      if((int)timeUntilAttack / 60 < 10){minutes += "0";}
      // clockText.text = minutes + ((int)timeUntilAttack / 60).ToString() + " :" + seconds + ((int)timeUntilAttack % 60).ToString();
    }
    public void SetClockText()
    {
      string seconds = "";
      //display 9 seconds remaining as 00:09
      if((int)timeUntilAttack % 60 < 10){seconds += "0";}
      string minutes = "";
      if((int)timeUntilAttack / 60 < 10){minutes += "0";}
      if(trackTime == false)
      {clockText.text = minutes + Mathf.Max(((int)timeUntilAttack / 60),0).ToString() + " :" + seconds + ((int)Mathf.Max(timeUntilAttack % 60,0)).ToString();}
        else
        {clockText.text = minutes + Mathf.Max(((int)Mathf.Ceil(timeUntilAttack / 60)),0).ToString() ;}

    }
    public void TrackTime()
    {


     
        if(timeUntilAttack > 0){  timeUntilAttack -= (Time.deltaTime * timerate);}

        currentMinutes = (int)timeUntilAttack / 60;

          if(GameManager().GetGameState() == GameState.playing){clockText.text = currentMinutes.ToString();}


        if (joinAttackButton)
        {
            if (timeUntilAttack <= 60)
            {
                joinAttackButton.active = true;

                //0 attack occurs, if the player isnt there it auto resolves
                //cant join a battle in progress [wont make it in time if the clock is already at 0]
                if (timeUntilAttack <= 0)
                {
                    joinAttackButton.active = false;

                }
            }
            else
            {

                joinAttackButton.active = false;
            }

        }




        timeSinceLastJump += (Time.deltaTime * timerate);


      //todo: make this a bar that fills, or clock that ticks?
     


      SetClockText();
    }

    public void FastForwardTime(int rate)
    {
      //buttons in menu to speed times towards the next Attack
      //NOTE: special events to interrupt? or just auto to the next battle?

      // next battle ready at minutes < 1 so no need to speed past that time
      if(timeUntilAttack / 60 > 1)
      {
        timerate = rate;
          // timeUntilAttack -= (Time.deltaTime * rate) ;
      }else{timerate = 1;}

    }





}


public class TimeStamp
{
    public int turnNumber;

    public float time;
    public float menu;
    public float ftlJumps;
    public float playerInputs;
    public float distanceTraveled;
}