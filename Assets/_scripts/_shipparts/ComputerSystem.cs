using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerSystem : ShipSystem
{
    public Transform cam;
    public Transform dradisSitSpot;

    public Actor currentTarget;
    public Transform dradisCenter;

    public override void PlayerInput()
    {
        if (on && (ship && ship.CanAct()))
        {

            if (PositiveButtonDown())
            {
                FindClosest(ActorType.enemy);
            }

            if (NegativeButtonDown())
            {
                FindClosest(ActorType.objective);
             //   ClearTarget();
            }
        }
    }

    public override void Act()
    {
        if (on && (ship && ship.CanAct()))
        {
            
            

        }
    }


    public override void Act_Fixed()
    {
        if (on && (ship && ship.CanAct()))
        {
            if (currentTarget && currentTarget.GetCrossHair())
            {
                currentTarget.GetCrossHair().SetPlacement();
            }



        }
    }





    public void DradisPing()
    {

        List<Enemy> actorList = GameManager().EnemyManager().Enemies();

        foreach (Enemy el in actorList)
        {
            if (el.canAct && el.dradisIcon)
            {
                el.dradisIcon.gameObject.SetActive(true);

                if (el.dradisIcon.parent != null)
                { el.dradisIcon.parent = dradisCenter; }

                el.dradisIcon.position = dradisCenter.position + (el.MainTransform().position - cam.position) * 0.001f;
                el.dradisIcon.rotation = el.MainTransform().rotation ;


            }
            else
            {
              
            }


        }

    }















    public Actor GetTarget()
    {
        return currentTarget;
    }


    public void SetTarget(Actor _newTarget)
    {

        ClearTarget();

        currentTarget = _newTarget;

        if (currentTarget && currentTarget.GetCrossHair())
        {
            currentTarget.GetCrossHair().SetCamera(cam);
            currentTarget.GetCrossHair().SetActor(currentTarget);

            float sensorLvl = 2;
            if (ship.GetEquipment())
            {
              //  sensorLvl = ship.GetEquipment().GetStats()[Stats.sensor];
            }
            currentTarget.GetCrossHair().Init(sensorLvl);
        }

    }
    public void ClearTarget()
    {
        if (currentTarget && currentTarget.GetCrossHair())
        {
            currentTarget.GetCrossHair().StopTargeting();

        }
        currentTarget = null;
    }


    public void FindClosest(ActorType _actorType)
    {


        // Actor[] actorList = FindObjectsOfType<Actor>();


        if (_actorType == ActorType.enemy)
        {
            if (currentTarget )
            {
                if (currentTarget.GetComponent<Enemy>())
                {
                    FindNextClosestEnemy();

                }
                else
                {
                    ClearTarget();
                    FindClosestEnemy();
                }

            }
            else
            {
                FindClosestEnemy();
            }

        }
        else if (_actorType == ActorType.objective)
        {
            if (currentTarget )
            {
                if (currentTarget && currentTarget.GetComponent<Map_POI>())
                {
                    FindNextClosestObjective();

                }
                else
                {
                    ClearTarget();
                    FindClosestObjective();
                }

            }
            else 
            {
                FindClosestObjective();
            }
        }




    }

    public void FindNextClosest(ActorType _actorType)
    {
        

        // Actor[] actorList = FindObjectsOfType<Actor>();
        

        if (_actorType == ActorType.enemy)
        {
            FindNextClosestEnemy();

        }
        else if (_actorType == ActorType.objective)
        {
            FindNextClosestObjective();
        }


        

    }





    public void FindClosestEnemy()
    {
        float dist = 100;


        Actor newTarget = currentTarget;

        List<Enemy> actorList = GameManager().EnemyManager().Enemies();

        foreach (Enemy el in actorList)
        {
            //confirm this is a viable transform
            if (el == null || el.MainTransform() == null) { }
            else 
            {
                if (newTarget == null)
                { newTarget = el; }
                else
                {
                    float newDist = Vector3.Distance(el.MainTransform().position, ship.transform.position);

                    if (newDist <= dist)
                    {
                        dist = newDist;
                        newTarget = el;
                    }
                }
            }

           
            

        }

      
            SetTarget(newTarget);

        

    }


    public void FindNextClosestEnemy()
    {
        float dist = 100;
        float mindist = 0;

        if (currentTarget)
        {
            mindist = Vector3.Distance(currentTarget.MainTransform().position, ship.transform.position);
        }
        dist = mindist * 100;

        Actor newTarget = null;

        List<Enemy> actorList = GameManager().EnemyManager().Enemies();

        foreach (Enemy el in actorList)
        {
            //confirm this is a viable transform
            if (el == null || el.MainTransform() == null) { }
            else
            {
                if (newTarget == null)
                {
                    if (Vector3.Distance(el.MainTransform().position, ship.transform.position) >= mindist)
                    {
                        newTarget = el;
                        dist = Vector3.Distance(el.MainTransform().position, ship.transform.position);
                    }
                }
                else
                {
                    float newDist = Vector3.Distance(el.MainTransform().position, ship.transform.position);

                    if (newDist > mindist && newDist <= dist)
                    {
                        dist = newDist;
                        newTarget = el;
                    }
                }

            }

        }

        if (newTarget == currentTarget || newTarget == null)
        {
            ClearTarget();
        }
        else
        {
            SetTarget(newTarget);

        }
    }



    public void FindClosestObjective()
    {
        float dist = 0;


        Actor newTarget = null;

        List<Map_POI> actorList = GameManager().MapManager().GetMap().GetPOIList();

        foreach (Map_POI el in actorList)
        {
            if (newTarget == null)
            {
                if (el.isObjective)
                {
                    newTarget = el;
                    dist = Vector3.Distance(el.MainTransform().position, ship.transform.position);
                }
            }
            else
            {
                float newDist = Vector3.Distance(el.MainTransform().position, ship.transform.position);

                if (el.isObjective &&  newDist < dist)
                {
                    dist = newDist;
                    newTarget = el;
                }
            }

            

        }

        if (newTarget == currentTarget)
        {
           // ClearTarget();
        }
        else
        {
            

        }
        SetTarget(newTarget);
    }


    public void FindNextClosestObjective()
    {
        float dist = 100;
        float mindist = 0;

        if (currentTarget)
        {
            mindist = Vector3.Distance(currentTarget.MainTransform().position, ship.transform.position);
        }
        dist = mindist * 100;

        Actor newTarget = null;

        List<Map_POI> actorList = GameManager().MapManager().GetMap().GetPOIList();

        foreach (Map_POI el in actorList)
        {

            if (newTarget == null)
            {
                if (el.isObjective && Vector3.Distance(el.MainTransform().position, ship.transform.position) >= mindist)
                {
                    newTarget = el;
                    dist = Vector3.Distance(el.MainTransform().position, ship.transform.position);
                }
            }
            else 
            {
                float newDist = Vector3.Distance(el.MainTransform().position, ship.transform.position);

                if (el.isObjective && newDist > mindist && newDist <= dist)
                {
                    dist = newDist;
                    newTarget = el;
                }
            }

            

        }

        if (newTarget == currentTarget || newTarget == null)
        {
            ClearTarget();
        }
        else
        {
            SetTarget(newTarget);

        }
    }

}
