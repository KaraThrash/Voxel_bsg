using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerSystem : ShipSystem
{
    public Transform cam;

    public Actor currentTarget;


    public override void PlayerInput()
    {
        if (on && (ship && ship.CanAct()))
        {

            if (PositiveButtonDown())
            {
                FindNextClosest(ActorType.enemy);
            }

            if (NegativeButtonDown())
            {
                ClearTarget();
            }
        }
    }
    public override void Act()
    {
        if (on && (ship && ship.CanAct()))
        {
            if (currentTarget && currentTarget.GetCrossHair())
            {
                currentTarget.GetCrossHair().SetPlacement();
            }

   
        }
    }


    public void SetTarget(Actor _newTarget)
    {

        ClearTarget();

        currentTarget = _newTarget;

        if (currentTarget && currentTarget.GetCrossHair())
        {
            currentTarget.GetCrossHair().SetCamera(cam);
            currentTarget.GetCrossHair().SetActor(currentTarget);
            currentTarget.GetCrossHair().Init(2);
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



    public void FindNextClosest(ActorType _actorType)
    {
        float dist = 0;
        float mindist = 0;
        if (currentTarget) { mindist = Vector3.Distance(currentTarget.MainTransform().position, ship.transform.position); }
        dist = mindist;

        Actor newTarget = currentTarget;

        // Actor[] actorList = FindObjectsOfType<Actor>();
        

        if (_actorType == ActorType.enemy)
        {
            List<Enemy> actorList = GameManager().EnemyManager().Enemies();

            foreach (Enemy el in actorList)
            {

                float newDist = Vector3.Distance(el.MainTransform().position, ship.transform.position);

                if (newTarget == null || (newDist > mindist && newDist <= dist))
                {
                    dist = newDist;
                    newTarget = el;
                }

            }

            if (newTarget == currentTarget)
            {
                ClearTarget();
            }
            else
            {
                SetTarget(newTarget);

            }

        }
        else if (_actorType == ActorType.objective)
        {
            
        }


        

    }




}
