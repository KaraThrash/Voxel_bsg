using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FleetManager : MonoBehaviour
{
  public GameManager gamemanager;
  public Fleetship galactica;
  public Transform fleetShips,fleetShipButtonParent;
  public Button fleetShipButton; //button for each specific ship to show its stats/info
  public Text shipStatText,fleetStatText;
    // Start is called before the first frame update
    void Start()
    {
      GetFleetStats();
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetMouseButton(1))
      {
          GetFleetStats();
      }
    }
    public void UpdateInfo()
    {
        GetFleetStats();

    }

    public void GetFleetStats()
    {
      fleetShipButton.transform.GetChild(0).GetComponent<Text>().text = galactica.name;
      int count = 0;
      //go through fleet ships, and make a button for each one to display their stats
      while (count < fleetShips.childCount)
      {
            //check if the button exists, otherwise make one
            if(fleetShipButtonParent.childCount > count + 1)
            {

            }
            else
            {
              Button newbutton = Instantiate(fleetShipButton,new Vector3(fleetShipButton.transform.position.x,fleetShipButton.transform.position.y - ((count + 1) * fleetShipButton.GetComponent<RectTransform>().rect.height),0),fleetShipButton.transform.rotation);
              newbutton.transform.GetChild(0).GetComponent<Text>().text = fleetShips.GetChild(count).GetComponent<Fleetship>().name;
              int hptextlength =  fleetShips.GetChild(count).GetComponent<Fleetship>().totalsubsystems;
              string hptext = "";
              while(hptextlength > 0)
              {hptext += "I";
              hptextlength--;
              }
              newbutton.transform.GetChild(1).GetComponent<Text>().text = hptext;
              newbutton.transform.parent = fleetShipButton.transform.parent;
              int tempint = count ;
              newbutton.onClick.AddListener(delegate{DisplayShipStats(tempint);});
            }

            count ++;
      }
      // inventoryButtons.GetChild(count).GetComponent<Button>().onClick.AddListener(delegate{ToggleEquip(tempint);});


    }

    public void DisplayShipStats(int whichship)
    {
        if(fleetShips.childCount > whichship)
        {
          //TODO: specific stat info
          shipStatText.text = fleetShips.GetChild(whichship).GetComponent<Fleetship>().name;

        }
    }
}
