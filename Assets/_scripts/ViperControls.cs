using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViperControls : MonoBehaviour {
    public Vector3 newvel;
    public GameObject myplayer;
    public GameObject camera;
    public GameObject camerasphere;
    public GameObject fwd;
    public GameObject up;
    public GameObject rgt;
    public GameObject behind;
    public GameObject camforward;
    public Quaternion targetRotation;


    private Rigidbody rb;
    public int turnSpeed;
    public float flySpeed;
    public float  strafeSpeed;
    public float liftSpeed;
    public float lift;
    public float hort;
    public float vert;
    public float roll;
    public float rollSpeed;
    public float rollMod;
    public float mouseX;
    public float mouseY;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet;
    public float guncooldown;

    public float cameraspeed;
    public float step;

    public float distspeed;
    public int heldresource;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        cameraspeed = 15;
        distspeed = 12;
    }
	
	// Update is called once per frame
	void Update () {
       //KeyboardFlightControls();
        thirdpersonflightcontrols();


    }
    public void thirdpersonflightcontrols()
    {
        hort = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        if (Input.GetMouseButton(1))
        {
            camerasphere.transform.position = transform.position;
            targetRotation = Quaternion.LookRotation(camforward.transform.position - transform.position);
            step = Mathf.Min(4 * Time.deltaTime, 1.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, camerasphere.transform.rotation, step);
            
            camerasphere.GetComponent<ThirdPersonCamera>().rollz = roll * 20 * Time.deltaTime ;
        }
        else {

            camerasphere.GetComponent<ThirdPersonCamera>().rollz = 0;
        }
        if (Input.GetMouseButton(0))
        {

            if (guncooldown <= 0)
            {

                RaycastShootGuns();

                guncooldown = 0.2f;
            }
            guncooldown -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftBracket)) { lift = -2; }
        else if
            (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightBracket))
        { lift = 2; }
        else { lift = 0; }

       
     //   if (Input.GetKeyDown(KeyCode.E)) { if (rollMod == -15) { rollMod = 0; } else { rollMod = -15; } }
     //   if (Input.GetKeyDown(KeyCode.Q)) { if (rollMod == 15) { rollMod = 0; } else { rollMod = 15; } }

        //if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.U)) { roll = (rollSpeed + rollMod); } else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.O)) { roll = -(rollSpeed + rollMod); } else { roll = 0; }
      

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // ToggleGlide();
        }

        if (Input.GetKey(KeyCode.E)) { roll = -1; }
        else if (Input.GetKey(KeyCode.Q))
        { roll = 1; }
        else { roll = 0; }
        // if (roll != 0) { rb.AddTorque(transform.forward * roll * Time.deltaTime, ForceMode.Impulse); }
        transform.Rotate(0, 0, roll * rollSpeed * Time.deltaTime);


        //rb.AddTorque(transform.forward * roll * 15.0f * Time.deltaTime, ForceMode.Impulse);
        // GetComponent<PhotonView>().RPC("flightControls", PhotonTargets.AllViaServer, vert, hort, roll, mouseX, mouseY, exit, lift);

        Vector3 tempvel = transform.position - rgt.transform.position;
        tempvel *= strafeSpeed * -hort;
        Vector3 tempvel2 = transform.position - fwd.transform.position;
        tempvel2 *= flySpeed * -vert;
        Vector3 tempvel3 = transform.position - up.transform.position;
        tempvel3 *= flySpeed * lift;
       
        newvel = tempvel + tempvel2 + tempvel3 ;
        rb.velocity = Vector3.Lerp(rb.velocity,newvel,5.0f * Time.deltaTime);
       // flightControls(vert, hort, roll, 0, 0, lift);
    }
    public void KeyboardFlightControls()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            if (guncooldown <= 0)
            {
               
               RaycastShootGuns();

                guncooldown = 0.2f;
            }
            guncooldown -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.LeftBracket)) { lift = liftSpeed; }
        else if 
            (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.RightBracket))
        { lift = -liftSpeed; }
        else { lift = 0; }

        hort = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.E)) { if (rollMod == -15) { rollMod = 0; } else { rollMod = -15; } }
        if (Input.GetKeyDown(KeyCode.Q)) { if (rollMod == 15) { rollMod = 0; } else { rollMod = 15; } }

        if (Input.GetKey(KeyCode.Keypad7) || Input.GetKey(KeyCode.U)) { roll = (rollSpeed + rollMod); } else if (Input.GetKey(KeyCode.Keypad9) || Input.GetKey(KeyCode.O)) { roll = -(rollSpeed + rollMod); } else { roll = 0; }
        if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.J)) { mouseX = -(turnSpeed + rollMod); } else if (Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.L)) { mouseX = (turnSpeed + rollMod); } else { mouseX = 0; }
        if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.K)) { mouseY = -(turnSpeed + rollMod); } else if (Input.GetKey(KeyCode.Keypad8) || Input.GetKey(KeyCode.I)) { mouseY = (turnSpeed + rollMod); } else { mouseY = 0; }

        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
           // ToggleGlide();
        }

        // GetComponent<PhotonView>().RPC("flightControls", PhotonTargets.AllViaServer, vert, hort, roll, mouseX, mouseY, exit, lift);
        flightControls(vert, hort, roll, mouseX, mouseY, lift);
    }
    public void RaycastShootGuns()
    {
        Instantiate(bullet,gun1.transform.position,gun1.transform.rotation);
        Instantiate(bullet, gun2.transform.position, gun2.transform.rotation);
    }
    public void flightControls(float newvert, float newhort, float roll, float rollX, float rollY, float lift)
    {
        if (vert != 0)
        {
            rb.AddForce(transform.forward * ((vert * flySpeed * 10 )) * Time.deltaTime);
            
        }
        if (hort != 0)
        {

            rb.AddForce(transform.right * (hort * strafeSpeed) * Time.deltaTime, ForceMode.Impulse);
        }
      
        if (roll != 0) { rb.AddTorque(transform.forward * roll * Time.deltaTime, ForceMode.Impulse); }
        if (rollX != 0) { rb.AddTorque(transform.up * rollX * Time.deltaTime, ForceMode.Impulse); }
        if (rollY != 0) { rb.AddTorque(transform.right * rollY * Time.deltaTime, ForceMode.Impulse); }
        //if (roll == 0 && rollX == 0 && rollY == 0)
        //{ rb.angularDrag = 1.0f; rb.drag = 1.0f; }
        //else { rb.angularDrag = 0; rb.drag = 0; }

        if (lift != 0) { rb.AddForce(transform.up * lift * 30 * Time.deltaTime); }

    }


    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "BulletEnemy")
        {
            myplayer.GetComponent<Player>().vehicletakingdamage(1);

        }

        if (col.gameObject.tag == "Resource" && heldresource == 0)
        {
            heldresource = col.gameObject.GetComponent<PickUp>().type;
            Destroy(col.gameObject);

        }
    }

}
