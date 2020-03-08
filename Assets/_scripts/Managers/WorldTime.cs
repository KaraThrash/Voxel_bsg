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
        TrackTime();
      if(trackTime == true)
      {


      }

    }
    public void SetInMenu(bool isinmenu)
    {
      trackTime = !isinmenu;
      string seconds = "";
      //display 9 seconds remaining as 00:09
      if((int)timeUntilAttack % 60 < 10){seconds += "0";}
      string minutes = "";
      if((int)timeUntilAttack / 60 < 10){minutes += "0";}
      clockText.text = minutes + ((int)timeUntilAttack / 60).ToString() + " :" + seconds + ((int)timeUntilAttack % 60).ToString();
    }
    public void TrackTime()
    {

      timeUntilAttack -= (Time.deltaTime * timerate);
      timeSinceLastJump += (Time.deltaTime * timerate);

      if(gameManager.fleetManager.engineStrength < timeSinceLastJump ){fleetJumpButton.active = true;}
      else{fleetJumpButton.active = false;}
      fleetJumpReadiness.text = (int)timeSinceLastJump + " / " + gameManager.fleetManager.engineStrength;

        currentMinutes = (int)timeUntilAttack / 60;
          if(gameManager.inMenu == false){clockText.text = currentMinutes.ToString();}



      if(timeUntilAttack <= 1)
      {
        joinAttackButton.active = true;
      }else{  joinAttackButton.active = false;}

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
