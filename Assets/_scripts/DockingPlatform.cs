using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingPlatform : MonoBehaviour
{
  public GameManager gameManager;
  public Transform spawnSpot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.T))
      {
        gameManager.Dock(this.transform,spawnSpot);


      }
    }

}
