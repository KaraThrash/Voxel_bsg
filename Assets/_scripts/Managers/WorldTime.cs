using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorldTime : MonoBehaviour
{
  public GameManager gameManager;
  public Text clockText,fleetJumpReadiness;
  public GameObject joinAttackButton,fleetJumpButton;
  public int totalTimePassed,currentMinutes;
  public int fleetJumpTimeIncrements,timeBetweenAttacks = 180; //FTL spools at slow/avg/fast per ship and player can risky jump || leave ships behind
  public float timeUntilAttack,timeSinceLastJump,timerate;
  public bool trackTime;//time stands still in menus
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

      //NOTE: always track time?
      //not when in battle


        TrackTime();



      // if(trackTime == true)
      // {
      //   TrackTime();
      //
      // }

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

          if(gameManager.GetGameState() == GameState.playing){clockText.text = currentMinutes.ToString();}



        if(timeUntilAttack <= 60)
        {
              joinAttackButton.active = true;

              //0 attack occurs, if the player isnt there it auto resolves
              //cant join a battle in progress [wont make it in time if the clock is already at 0]
              if(timeUntilAttack <= 0)
              {
                joinAttackButton.active = false;

              }
        }
        else
        {

            joinAttackButton.active = false;
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
