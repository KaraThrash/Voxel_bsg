using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemConstants 
{


    public static int PLAYERLAYER = 20;
    public static int ENEMYLAYER = 9;



}


public static class UniversalFunctions 
{



    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }

    public static Transform FindDeepChildByTag(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.CompareTag(aName))
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }

    public static void FindDeepChildrenByTag(this Transform aParent, List<Transform> _list, string _tag)
    {
        GetChildrenObject(aParent, _list, _tag);
    }

    public static void GetChildrenObject(Transform parent, List<Transform> _list, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                _list.Add(child);
            }
            if (child.childCount > 0)
            {
                GetChildrenObject(child, _list,_tag);
            }
        }
    }


    public static void RecursiveSetColliderTrigger(Transform _parent, bool _on)
    {
        if (_parent.GetComponent<Collider>() != null)
        {
            _parent.GetComponent<Collider>().isTrigger = _on;
        }

        foreach (Transform el in _parent)
        {
            if (el.GetComponent<Collider>() != null)
            {

                el.GetComponent<Collider>().isTrigger = _on;

            }
            if (el.childCount > 0) { RecursiveSetColliderTrigger(el, _on); }
        }
    }

    public static void RecursiveSetCollider(Transform _parent, bool _on)
    {
        if (_parent.GetComponent<Collider>() != null )
        {
            _parent.GetComponent<Collider>().enabled = _on;
        }

        foreach (Transform el in _parent)
        {
            if (el.GetComponent<Collider>() != null )
            {

                el.GetComponent<Collider>().enabled = _on;

            }
            if (el.childCount > 0) { RecursiveSetCollider(el, _on); }
        }
    }


    public static void RecursiveToggleKinematic(Transform _parent,bool _on)
    {
        if (_parent.GetComponent<Rigidbody>() != null && _parent.GetComponent<Rigidbody>().isKinematic != _on)
        {
            _parent.GetComponent<Rigidbody>().isKinematic = _on;
        }

        foreach (Transform el in _parent)
        {
            if (el.GetComponent<Rigidbody>() != null && el.GetComponent<Rigidbody>().isKinematic != _on)
            {

                el.GetComponent<Rigidbody>().isKinematic = _on;

            }
            if (el.childCount > 0) { RecursiveToggleKinematic(el, _on); }
        }
    }


    public static void RecursiveSetVelocity(Transform _parent,Vector3 _velocity)
    {
        if (_parent.GetComponent<Rigidbody>() != null )
        {
            _parent.GetComponent<Rigidbody>().velocity = _velocity;
        }

        foreach (Transform el in _parent)
        {
            if (el.GetComponent<Rigidbody>() != null && el.GetComponent<Rigidbody>().isKinematic != true)
            {

                el.GetComponent<Rigidbody>().velocity = _velocity;

            }
            if (el.childCount > 0) { RecursiveSetVelocity(el, _velocity); }
        }
    }
}
