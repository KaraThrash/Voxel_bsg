
using UnityEngine;

public class Bullet : Actor
{

    public BulletType bulletType;

    public float speed, rotSpeed;
    public GameObject explosion, target;
    public bool large;

    public GameObject intialExplosion, childObject;

    public float lifetimeMax = 10.0f, lifeTime, impactForce;
    public int  damage;

    public float timeIncrement = 1.0f, incrementTimer = 1.0f, spiralRange = 40.0f;



    private int toggleValue = 1;
    public bool lance, missile, spray, twinlinked, boomerang, spiral; //toggles on instead of being a projectile
    public bool ice;

    private Vector3 direction, secondaryDirection, relativeVelocity;
    private Rigidbody rb;

    private Collider myCollider;

    private Transform bulletParent;
    private string bulletParentName = "PARENT_Bullet";

    

    public float LifeTimeMax() { return lifetimeMax; }
    public float Speed() { return speed; }
    public float RotationSpeed() { return rotSpeed; }
    public int Damage() { return damage; }
    public void Damage(int _dmg) {  damage = _dmg; }



    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.parent = BulletParent();


        if (childObject == null && transform.childCount > 0) { childObject = transform.GetChild(0).gameObject; }

    }



    public void Launch(float _power = 1)
    {
        RB().isKinematic = false;
        lifeTime = LifeTimeMax();
        //move to the bottom of the child list [bullets are selected from the first child '0']
        transform.parent = null;
        transform.parent = BulletParent();

        RB().velocity = transform.forward * _power;
        SetCollider(true);

    }

    public void Launched(GameObject newtarget = null)
    {
        if (newtarget) { target = newtarget; }

        if (bulletType == BulletType.boomerang)
        {
           // direction = (target.transform.forward + target.transform.right).normalized;
           // secondaryDirection = -target.transform.right;
        }


    }
    public void SetRelativeVelocity(Vector3 newvel)
    { relativeVelocity = newvel; }

    // Update is called once per frame
    void FixedUpdate()
    {

        lifeTime -= Time.deltaTime;

        if (bulletType == BulletType.boomerang)
        {
            transform.Rotate(transform.right * speed);
        }
        else
        {
           
        }
        RB().velocity = (transform.forward * speed) + relativeVelocity;

        if ((lifeTime <= 0 && lifeTime != -1) || (target != null && transform.position == target.transform.position)) 
        { Die(); }
    }

    public override void ProcessCollisionEnter(Collision collision)
    {

        
        if (collision.gameObject.GetComponent<Ship>() != null)
        {
            HitShip(collision.gameObject.GetComponent<Ship>(), collision.contacts[0].point);
        }
        else if (collision.gameObject.GetComponent<Actor>() != null)
        {
            HitActor(collision.gameObject.GetComponent<Actor>(), collision.contacts[0].point);
        }
        else if (collision.transform.CompareTag("Enviroment"))
        {
            HitEnviroment(collision);
        }

    }

    public override void ProcessTriggerEnter(Collider collision)
    {

        if (collision.gameObject.GetComponent<Ship>() != null)
        {
            HitShip(collision.gameObject.GetComponent<Ship>(), collision.ClosestPoint(transform.position));
        }
        else if (collision.gameObject.GetComponent<Actor>() != null)
        {
            HitActor(collision.gameObject.GetComponent<Actor>(), collision.ClosestPoint(transform.position));
        }
    }


    public virtual void HitActor(Actor _actor, Vector3 _pos)
    {

        GetComponent<Collider>().enabled = false;

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        _actor.TakeDamage(Damage());

        Die();

    }

    public virtual void HitShip(Ship _ship, Vector3 _pos)
    {

        GetComponent<Collider>().enabled = false;

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (_ship.Chasis())
        {
            _ship.Chasis().ExternalForce(damage  * (transform.position - _pos).normalized);
        }
        
        _ship.TakeDamage(Damage());




        Die();

    }




    public void HitEnviroment(Collision collision)
    {


        GetComponent<Collider>().enabled = false;

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        Die();

    }



    public Transform BulletParent()
    {
        if (bulletParent == null)
        {
            if (bulletParentName.Length < 1)
            {
                bulletParentName = "PARENT_Bullet";// + this.GetType().ToString();
            }

            GameObject findParent = GameObject.Find(bulletParentName);


            if (findParent == null)
            {
                bulletParent = new GameObject(bulletParentName).transform;
            }
            else { bulletParent = findParent.transform; }
        }

        return bulletParent;
    }

    public override void Die()
    {
        SetCollider(false);
        RB().isKinematic = true;

        lifeTime = -1;

        if (GetComponent<TrailRenderer>())
        { GetComponent<TrailRenderer>().Clear(); }

        gameObject.SetActive(false);

    }

    public Rigidbody RB()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        return rb;
    }

    public void SetCollider(bool _on)
    {
        if (GetCollider() != null)
        {
            GetCollider().enabled = _on;
        }

    }

    public Collider GetCollider()
    {
        if (myCollider == null)
        {
            myCollider = GetComponent<Collider>();
        }
        if (myCollider == null)
        {
            gameObject.AddComponent<SphereCollider>();
        }

        return myCollider;
    }

}
