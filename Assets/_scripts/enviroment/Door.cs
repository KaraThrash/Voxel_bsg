using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Actor
{



    public float speed;
    public float openWidth;
    public float height;

    public float regenSpeed;


    private float tracker_Aperture;
    private float tracker_Regen;

    public bool open;
    public bool locked;

    public Transform topDoor;
    public Transform bottomDoor;
    public Transform healthindicator;

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            MoveDoors();

            if (open)
            {
                if (Hitpoints() < STAT_MaxHP())
                {
                    tracker_Regen += Time.deltaTime * regenSpeed;


                    if (tracker_Regen > 1)
                    {
                        Hitpoints(1);


                        if (healthindicator)
                        {
                            healthindicator.localScale = new Vector3(1, PercentHealth(), 1);
                            healthindicator.gameObject.SetActive(true);
                        }


                        tracker_Regen = 0;

                        if (Hitpoints() >= STAT_MaxHP())
                        {
                            SetHitpoints(STAT_MaxHP());
                            open = false;
                        }
                    }
                    else
                    {

                        if (healthindicator)
                        {
                            healthindicator.localScale = new Vector3(1, PercentHealth() + ( 0.1f * tracker_Regen), 1);
                            
                        }
                    }


                    
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1,null);
        }

    }




    public override void TakeDamage(int _dmg, Bullet _bullet)
    {
        //TODO: check if locked, and if this is the correct bullet type

        Hitpoints(-_dmg);

        if (!locked )
        {

            if (Hitpoints() <= 0)
            {
                open = true;
                if (healthindicator)
                {
                    healthindicator.gameObject.SetActive(false);
                }
            }
            else
            {
                if (healthindicator)
                {
                    healthindicator.localScale = new Vector3(1, PercentHealth(), 1);
                }
            }
        }
        
    }


    public void MoveDoors()
    {
        if (open)
        {
            if (tracker_Aperture < openWidth)
            {
                float move = Time.deltaTime * speed;

                if (move + tracker_Aperture > openWidth)
                { move = openWidth - tracker_Aperture; }
              

                tracker_Aperture += move;

                if (topDoor)
                {
                    topDoor.transform.localPosition = Vector3.MoveTowards(topDoor.transform.localPosition,new Vector3(0,height ,0), move);
                }
                if (bottomDoor)
                {
                    bottomDoor.transform.localPosition = Vector3.MoveTowards(bottomDoor.transform.localPosition, new Vector3(0, 2, 0), move);
                }

            }


        }
        else
        {
            if (tracker_Aperture > 0)
            {
                float move = Time.deltaTime * speed;

                if ( tracker_Aperture - move < 0)
                { move = tracker_Aperture; }

                tracker_Aperture -= move;

                if (topDoor)
                {
                    topDoor.transform.localPosition = Vector3.MoveTowards(topDoor.transform.localPosition, new Vector3(0, height * 0.5f, 0), move);
                }
                if (bottomDoor)
                {
                    bottomDoor.transform.localPosition = Vector3.MoveTowards(bottomDoor.transform.localPosition, new Vector3(0, height * 0.5f, 0), move);
                }

            }

        }

    }


    public override void ProcessCollisionEnter(Collision collision)
    {

    }

    public override void ProcessTriggerEnter(Collider collision)
    {

    }







}
