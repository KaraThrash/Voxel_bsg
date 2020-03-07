using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoDisplay : MonoBehaviour
{
  public GameManager gameManager;
  public Text display;
  public float msgTimer,msgTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(msgTimer > 0)
        {

          msgTimer -= Time.deltaTime;
            if(msgTimer <= 0)
            {
              display.text = "";
            }
        }
    }

    public void NewMessage(string msg)
    {
      display.text = msg;
      msgTimer = msgTime;
    }
}
