using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
  public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetBattleMap()
    {
      float rnd = Random.Range(0.0f,10.0f);
      return -1;
          //determine type of attack
          //if a battle go to battle Map
          if (rnd > 8){return -3;}
          else  if (rnd > 5){return -1;}
          else  if (rnd > 2){return 0;}
          else{return -2;}
          //otherwise go to firelink
    }

    public void AutoResolve(FleetManager fleetManager,EnemyFleet enemyFleet)
    {
      //determine type of attack
      float rnd = Random.Range(0.0f,10.0f);

      if (rnd > 8){}
      else  if (rnd > 5){}
      else  if (rnd > 2){}
      else{}
      //do math for resolution
      //give resolution
    }



}
