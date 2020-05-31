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
  public int gunshipNumber,hangarShipNumber;
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
      //attacks the fleet ships 
      string tempstring = "Combat Strength: ";
      tempstring += baseStar.GetTurretStrength().ToString() + "\n";
      //chase potential // time to next attack
      tempstring += "FTL Strength: ";
      tempstring += baseStar.GetEngineStrength().ToString() + "\n";
      // produces fighters that attack the player
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


        gameManager.npcManager.SpawnOne(gunshipNumber,baseStar.transform.position + ( (transform.forward * -(500 + Random.Range(250,750)))) + (transform.up * (500 + Random.Range(250,750))) + (transform.right * (350 * -count) * ( Mathf.Sign(0 - (count % 2) ))),transform.rotation);

        count++;

      }
      turretStrength = baseStar.GetHangarStrength();
      count = 0;
      while (count < turretStrength)
      {
        gameManager.npcManager.SpawnOne(hangarShipNumber,baseStar.transform.position + ( (transform.forward * -(500 + Random.Range(250,750)))) + (transform.up * (-500 - Random.Range(250,750))) + (transform.right * (500 * -count) * (Mathf.Sign(0 - (count % 2) ))),transform.rotation);

          count++;
      }
    }
}
