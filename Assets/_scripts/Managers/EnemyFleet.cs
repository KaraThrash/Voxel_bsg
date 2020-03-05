using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyFleet : MonoBehaviour
{
  public GameManager gameManager;
  public BaseStar baseStar;
  public Text enemyFleetStrengthText;
  public GameObject baseStarGunShip,baseStarHangarShip;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateInfo()
    {
      enemyFleetStrengthText.text = GetFleetStrengthText();
    }

    public string GetFleetStrengthText()
    {
      string tempstring = "Combat Strength: ";
      tempstring += baseStar.GetTurretStrength().ToString() + "\n";
      tempstring += "FTL Strength: ";
      tempstring += baseStar.GetEngineStrength().ToString() + "\n";
      tempstring += "Production Strength: ";
      tempstring += baseStar.GetHangarStrength().ToString() ;
      return tempstring;
    }

    public void StartFleetBattle()
    {
      int turretStrength = baseStar.GetTurretStrength();
      int count = 0;
      while (count < turretStrength)
      {

          Instantiate(baseStarGunShip,baseStar.transform.position + (transform.up * (500 + Random.Range(50,550))) + (transform.right * (150 * -count) * ( Mathf.Sign(0 - (count % 2) ))),transform.rotation);


        count++;

      }
      turretStrength = baseStar.GetHangarStrength();
      count = 0;
      while (count < turretStrength)
      {
        Instantiate(baseStarHangarShip,baseStar.transform.position + (transform.up * (-500 - Random.Range(50,550))) + (transform.right * (100 * -count) * (Mathf.Sign(0 - (count % 2) ))),transform.rotation);
          count++;
      }
    }
}
