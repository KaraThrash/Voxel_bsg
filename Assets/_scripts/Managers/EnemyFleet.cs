using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyFleet : MonoBehaviour
{
  public BaseStar baseStar;
  public Text enemyFleetStrengthText;
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
}
