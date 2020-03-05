using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleetShip : MonoBehaviour
{
  public NpcManager npcManager;
  public EnemyFleet enemyFleetManager;
  public GameObject target;
  public float rotForce = 0.1f; //
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(target == null){target = npcManager.GetClosestFleetShip(transform.position);}else{ FaceTarget();}
    }

    public void FaceTarget()
    {

      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position ), rotForce * Time.deltaTime);
    }
}
