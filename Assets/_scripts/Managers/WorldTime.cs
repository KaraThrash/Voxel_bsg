using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorldTime : MonoBehaviour
{
  public GameManager gameManager;
  public Text clockText;
  public int totalTimePassed,currentMinutes;
  public float timeUntilAttack;
  public bool trackTime;//time stands still in menus
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

      if(trackTime == true)
      {
        TrackTime();

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
      timeUntilAttack -= Time.deltaTime;

        currentMinutes = (int)timeUntilAttack / 60;
          if(gameManager.inMenu == false){clockText.text = currentMinutes.ToString();}



      if(timeUntilAttack <= 0){}
    }
}
