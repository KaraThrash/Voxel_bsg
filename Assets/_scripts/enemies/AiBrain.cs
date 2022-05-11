using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AiBrain 
{

    //public static RelativeFacing Facing(Transform _actor, Transform _target)
    //{



    //}

    public static RangeBand DetermineRange(Transform _actor, Transform _target, Stats_Enemy _stats)
    {

     
            if (Vector3.Distance(_actor.position, _target.position) >= _stats.farRange * 2)
            {
                return RangeBand.extreme;
            }
            else if (Vector3.Distance(_actor.position, _target.position) < _stats.closeRange * 0.5f)
            {
                return RangeBand.melee;
            }
            else if (Vector3.Distance(_actor.position, _target.position) <= _stats.midRange)
            {
                return RangeBand.close;
            }
            else if (Vector3.Distance(_actor.position, _target.position) > _stats.closeRange)
            {
                return RangeBand.mid;
            }

            return RangeBand.unknown;
        

 
    }

    public static RelativeFacing Facing(Transform _actor,Transform _target)
    {
        //determine the relative orientation of the actor to the target

        bool targetFacingActor = false;

        if (Vector3.Distance(_target.position + _target.forward, _actor.position) < Vector3.Distance(_actor.position, _target.position))
        {
            //target is facing the actor
            targetFacingActor = true;

        }
   

        if (Vector3.Distance(_actor.position + _actor.forward, _target.position) < Vector3.Distance(_actor.position, _target.position))
        {
            //actor is facing the target
            if (targetFacingActor)
            {
                return RelativeFacing.chicken;
            }
            else
            {
                return RelativeFacing.behind;
            }

        }
        else
        {
            //actor is facing away from the target
           
                //actor is facing the target
                if (targetFacingActor)
                {
                    return RelativeFacing.away;
                }
                else
                {
                    return RelativeFacing.backToBack;
                }

            
        }

        
    }
}
