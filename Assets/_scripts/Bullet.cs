
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Actor
{

    public Bullet_Type bulletType;

    public float speed, rotSpeed;
    private float currentRotSpeed;

    private float acceleration;
    private float current_acceleration;

    public GameObject explosion, target;
    public bool large;

    public GameObject intialExplosion, childObject;
    [Min(1)]
    public float lifetimeMax = 10.0f;
    public float  lifeTime;

    private float  eventTimestamp = -1;
    private float  eventIncrement = -1;

    private float tracker_DirectionChanges = 0;
    private float stat_DirectionChanges = 0;




    public float  impactForce;

    public int  damage;

    public float timeIncrement = 1.0f, incrementTimer = 1.0f, spiralRange = 40.0f;



    private int toggleValue = 1;


    public bool ice;

    private Vector3 direction, secondaryDirection, relativeVelocity;
    private Vector3 rotationDirection;


    private Rigidbody anchorPoint; //for attacks that are part of the attacker [e.g. lance]

    private Collider myCollider;

    private Transform bulletParent;
    private string bulletParentName = "PARENT_Bullet";
    private Transform explosionParent;
    private string explosionParentName = "PARENT_Explosion";

    public GameObject missileObj;
    public MeshRenderer renderer;


    public float LifeTimeMax() { return lifetimeMax; }
    public float Speed() { return speed; }
    public float RotationSpeed() { return rotSpeed; }
    public int Damage() { return damage; }
    public void Damage(int _dmg) {  damage = _dmg; }
    public Bullet_Type BulletType() { return bulletType; }
    public void BulletType(Bullet_Type _type) { bulletType = _type; }


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.parent = BulletParent();


        if (childObject == null && transform.childCount > 0) { childObject = transform.GetChild(0).gameObject; }

    }


    public void Init()
    {
        if (missileObj) { missileObj.SetActive(false); }
        if (renderer) { renderer.enabled = true ; }

        SetCollider(false);
        RB().isKinematic = false;
        lifeTime = LifeTimeMax();
        //move to the bottom of the child list [bullets are selected from the first child '0']
        transform.parent = null;
        transform.parent = BulletParent();

        rotationDirection = Vector3.zero;
        currentRotSpeed = 0;
        current_acceleration = 0;
    }

    public void Launch(Ship _ship)
    {
        

        float pwr = 1;
        float acc = 0.1f;
        float dmg = 0;

        float newLifeTime = LifeTimeMax();

        currentRotSpeed = 0;
        current_acceleration = 0;


        if (_ship == null || _ship.GetEquipment() == null || _ship.GetEquipment().GetBullet() == null)
        {
           
            rotSpeed = 1;
            pwr = GameConstants.BULLET_SPEED;
            dmg = GameConstants.BULLET_DAMAGE;
            BulletType(Bullet_Type.basic);
        }
        else
        {
            Dictionary<Stats, float> stats = _ship.GetEquipment().GetStats();
            Item bulletStats = _ship.GetEquipment().GetBullet();
            pwr = stats[Stats.projectileSpeed];
            pwr = (pwr / 100) * bulletStats.GetStats()[Stats.projectileSpeed];
            pwr += bulletStats.GetStats()[Stats.projectileSpeed];


             dmg = stats[Stats.damage];
            dmg = (dmg / 100) * bulletStats.GetStats()[Stats.damage];
            dmg += bulletStats.GetStats()[Stats.damage];

             newLifeTime = stats[Stats.bulletlifetime];
            newLifeTime = (newLifeTime / 100) * bulletStats.GetStats()[Stats.bulletlifetime];
            newLifeTime += bulletStats.GetStats()[Stats.bulletlifetime];

            rotSpeed = bulletStats.GetStats()[Stats.mobility];

            
            acc = bulletStats.GetStats()[Stats.speed];

            BulletType((Bullet_Type)bulletStats.subtype);

            if (BulletType() == Bullet_Type.missile)
            {
                transform.position += _ship.DeploySpot().localPosition;

            }


            if (_ship.CPU() && _ship.CPU().GetTarget())
            { 
                SetTarget(_ship.CPU().GetTarget().gameObject);

            }





        }

        lifeTime = newLifeTime;
        eventTimestamp = -1;


        if (lifeTime < 1) { lifeTime = 1; }

        speed = pwr;

   

        Damage(Mathf.FloorToInt(dmg));

        rotationDirection = Vector3.zero;
        direction = transform.forward;

        if (BulletType() == Bullet_Type.basic)
        {
            RB().velocity = transform.forward * pwr;
        }
        else if (BulletType() == Bullet_Type.boomerang)
        {
            direction = transform.right;
            rotationDirection = _ship.MainTransform().forward ;

            eventTimestamp = newLifeTime * 0.7f;
            target = _ship.MainTransform().gameObject;

        }
        else if (BulletType() == Bullet_Type.missile)
        {
            if (missileObj) { missileObj.SetActive(true); }
            if (renderer) { renderer.enabled = false; }
            SetCollider(false);
            current_acceleration = 0.1f;
            if (acc > 0)
            {
                //projectiles with no acceleration have a stable constant speec [i.e. current acceleration is 1]
                acceleration = acc;
                current_acceleration = 0.1f;
            }
        }
        else if (BulletType() == Bullet_Type.spread)
        {

        }
        else if (BulletType() == Bullet_Type.lance)
        {

        }
        else if (BulletType() == Bullet_Type.large)
        {
            transform.localScale = new Vector3(1,1,1); ;
        }
        else if (BulletType() == Bullet_Type.spiral)
        {
            rotationDirection = -_ship.MainTransform().right - _ship.MainTransform().up;
        }
        else if (BulletType() == Bullet_Type.zigzag)
        {

            direction = transform.forward + transform.right;
            rotationDirection = transform.forward - transform.right;

            eventIncrement = lifeTime * 0.1f;
            eventTimestamp = lifeTime - (eventIncrement * 0.5f);

        }
        else
        {
            RB().velocity = transform.forward * pwr;
        }

        gameObject.name = "Bullet_" + bulletType;

        SetCollider(true);
    }

    public void Launch(Stats_Enemy _stats)
    {


        currentRotSpeed = 0;
        current_acceleration = 0.1f;



        lifeTime = _stats.bulletLifeTime;
        eventTimestamp = -1;


        if (lifeTime < 1) { lifeTime = 1; }

        speed = _stats.bulletSpeed;



        Damage(Mathf.FloorToInt(_stats.gunDamage));

        rotationDirection = Vector3.zero;
        direction = transform.forward;

        if (BulletType() == Bullet_Type.basic)
        {
            RB().velocity = transform.forward * speed;
        }
        else if (BulletType() == Bullet_Type.boomerang)
        {


        }
        else if (BulletType() == Bullet_Type.missile)
        {

        }
        else if (BulletType() == Bullet_Type.spread)
        {

        }
        else if (BulletType() == Bullet_Type.lance)
        {

        }
        else if (BulletType() == Bullet_Type.large)
        {
            transform.localScale = new Vector3(1, 1, 1); ;
        }
        else if (BulletType() == Bullet_Type.spiral)
        {

        }
        else if (BulletType() == Bullet_Type.zigzag)
        {

            direction = transform.forward + transform.right;
            rotationDirection = transform.forward - transform.right;

            eventIncrement = lifeTime * 0.1f;
            eventTimestamp = lifeTime - (eventIncrement * 0.5f);

        }
        else
        {
            RB().velocity = transform.forward * speed;
        }

        gameObject.name = "Bullet_" + bulletType;

        SetCollider(true);
    }

    public void SetTarget(GameObject newtarget = null)
    {
        if (newtarget) { target = newtarget; }


    }

    public void SetRelativeVelocity(Vector3 newvel)
    { relativeVelocity = newvel; }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lifeTime != -1 )
        { lifeTime -= Time.deltaTime; }
        

        if (BulletType() == Bullet_Type.boomerang)
        {
            Boomerang();
        }
        else if (BulletType() == Bullet_Type.missile)
        {
            Missile();
        }
        else if (BulletType() == Bullet_Type.bounce)
        {
            
        }
        else if (BulletType() == Bullet_Type.spiral)
        {
            Spiral();
        }
        else if (BulletType() == Bullet_Type.zigzag)
        {
            ZigZag();
        }
        else
        {
            Basic();
        }

        

        if ((lifeTime <= 0 && lifeTime != -1) || (target != null && transform.position == target.transform.position)) 
        { Die(); }
    }

    /// <summary>
    /// Basic
    /// </summary>

    public void Basic()
    {
        RB().velocity = (transform.forward * speed) + relativeVelocity;
    }

    public void Basic_Collision(Collision collision)
    {

    }

    public void Basic_Trigger(Collider collision)
    {

    }

    /// <summary>
    /// Boomerang
    /// </summary>

    public void Boomerang()
    {
        RB().velocity = (transform.forward * speed);// + relativeVelocity;

        if (target && eventTimestamp > lifeTime)
        {
            if (currentRotSpeed < rotSpeed)
            {
                currentRotSpeed += Time.deltaTime;
                if (currentRotSpeed > rotSpeed)
                {
                    currentRotSpeed = rotSpeed;
                }
            }
           
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, currentRotSpeed * Time.deltaTime);
        }

    }

    public void Boomerang_Collision(Collision collision)
    {
       
    }

    public void Boomerang_Trigger(Collider collision)
    {
       
    }

    /// <summary>
    /// Missile
    /// </summary>

    public void Missile()
    {
        Vector3 vel = transform.forward * speed * current_acceleration;

        if (current_acceleration < 0.2f)
        {
            vel +=  -transform.up * speed * (0.21f - current_acceleration);
        }

        
        


        RB().velocity = vel;


        if (target)
        {
            if (currentRotSpeed < rotSpeed)
            {
                currentRotSpeed += (Time.deltaTime * acceleration);
                if (currentRotSpeed > rotSpeed)
                {
                    currentRotSpeed = rotSpeed;
                }
            }

            if (current_acceleration < 1)
            {
                current_acceleration += Time.deltaTime * acceleration;

                if (current_acceleration > 1)
                {
                    currentRotSpeed = 1;
                }
            }

            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, currentRotSpeed * Time.deltaTime);
        }
       
    }

    public void Missile_Collision(Collision collision)
    {

    }

    public void Missile_Trigger(Collider collision)
    {

    }


    /// <summary>
    /// Spiral
    /// </summary>


    public void Spiral()
    {
        RB().velocity = (direction * speed) + (transform.forward * speed);// + relativeVelocity;
        RB().angularVelocity = (rotationDirection) * rotSpeed;// + relativeVelocity;
       // transform.Rotate((transform.right - transform.up) * rotSpeed * Time.deltaTime);
    }

    public void Spiral_Collision(Collision collision)
    {

    }

    public void Spiral_Trigger(Collider collision)
    {

    }

    /// <summary>
    /// zigzag
    /// </summary>

    public void ZigZag()
    {
        RB().velocity = (direction * speed);// + relativeVelocity;


        if (eventTimestamp > lifeTime)
        {
            eventTimestamp = lifeTime - eventIncrement ;

            Vector3 oldDir = direction;
            direction = rotationDirection;
            rotationDirection = oldDir;

            //tracker_DirectionChanges++;

            //if (tracker_DirectionChanges >= stat_DirectionChanges)
            //{ tracker_DirectionChanges = 0; }


            //Vector3 oldDir = direction;

            //if (tracker_DirectionChanges == 0)
            //{
            //    direction = transform.forward + transform.right - transform.up;
            //}
            //else if (tracker_DirectionChanges == 1)
            //{
            //    direction = transform.forward - transform.right;
            //}
            //else if (tracker_DirectionChanges == 2)
            //{
            //    direction = transform.forward - transform.up + transform.right;
            //}
            //else if (tracker_DirectionChanges == 3)
            //{
            //    direction = transform.forward + transform.up ;
            //}
            //else
            //{
                
            //}





        }

    }

    public void ZigZag_Collision(Collision collision)
    {

    }

    public void ZigZag_Trigger(Collider collision)
    {

    }


    public override void ProcessCollisionEnter(Collision collision)
    {

        
        //if (collision.gameObject.GetComponent<Ship>() != null)
        //{
        //    HitShip(collision.gameObject.GetComponent<Ship>(), collision.contacts[0].point);
        //}
        //else 
        if (collision.gameObject.GetComponent<Actor>() != null)
        {
            HitActor(collision.gameObject.GetComponent<Actor>(), collision.contacts[0].point);
        }
        else if (collision.transform.CompareTag("Enviroment"))
        {
            HitEnviroment();
        }
        else 
        {
            HitEnviroment();
        }

    }

    public override void ProcessTriggerEnter(Collider collision)
    {

        //if (collision.gameObject.GetComponent<Ship>() != null)
        //{
        //    HitShip(collision.gameObject.GetComponent<Ship>(), collision.ClosestPoint(transform.position));
        //}
        //else 
        
        if (collision.gameObject.GetComponent<Actor>() != null)
        {
            HitActor(collision.gameObject.GetComponent<Actor>(), collision.ClosestPoint(transform.position));
        }
        else if (collision.transform.CompareTag("Enviroment"))
        {
            HitEnviroment();
        }
        else
        {
            HitEnviroment();
        }
    }


    public virtual void HitActor(Actor _actor, Vector3 _pos)
    {

        PlaceExplosion();

        _actor.TakeDamage(Damage(),this);


        if (BulletType() == Bullet_Type.bounce)
        {

        }
        else
        {
            GetComponent<Collider>().enabled = false;

            
            Die();

        }

    }

    public virtual void HitShip(Ship _ship, Vector3 _pos)
    {
        PlaceExplosion();


        if (_ship.Chasis())
        {
            _ship.Chasis().ExternalForce(damage  * (transform.position - _pos).normalized);
        }
        
        _ship.TakeDamage(Damage());




        if (BulletType() == Bullet_Type.bounce)
        {

        }
        else
        {
            GetComponent<Collider>().enabled = false;

           
            Die();

        }

    }




    public void HitEnviroment()
    {

        PlaceExplosion();


        if (BulletType() == Bullet_Type.bounce)
        {

        }
        else
        {
            GetComponent<Collider>().enabled = false;

     
            Die();

        }

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

    public Transform ExplosionParent()
    {
        if (explosionParent == null)
        {
            if (explosionParentName.Length < 1)
            {
                explosionParentName = "PARENT_Explosion";// + this.GetType().ToString();
            }

            GameObject findParent = GameObject.Find(explosionParentName);


            if (findParent == null)
            {
                explosionParent = new GameObject(explosionParentName).transform;
            }
            else { explosionParent = findParent.transform; }
        }

        return explosionParent;
    }



    public void PlaceExplosion()
    {

        if (ExplosionParent().childCount == 0 || ExplosionParent().GetChild(0).gameObject.activeSelf)
        {
            if (explosion != null)
            {
                GameObject clone = Instantiate(explosion, transform.position, transform.rotation);
                clone.SetActive(true);
                clone.GetComponent<SfxExplosion>().Init(SFX.bulletImpact);

            }
        }
        else
        {
            Transform newExplosion = ExplosionParent().GetChild(0);
            newExplosion.gameObject.SetActive(true);
            newExplosion.GetComponent<SfxExplosion>().Init(SFX.bulletImpact);


            newExplosion.position = transform.position;
            newExplosion.rotation = transform.rotation;


        }


        if (explosion != null)
        {
           // Instantiate(explosion, transform.position, transform.rotation);
        }

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
