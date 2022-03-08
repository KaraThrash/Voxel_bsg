using UnityEngine;


[RequireComponent(typeof(ShipController))]
public class ShipPhysics : EngineBase
{
    #region Fields =================================================================================================

    [Tooltip("Affects how strong and, thus, how fast rotations are applied to the spaceship.")]
    [SerializeField]
    private float torque = 25.0f;

    [Tooltip("Defines how much a translation force is applied to the spaceship.")]
    [SerializeField]
    private float speed = 1000.0f;

    [Tooltip("The 'Spaceship Controller' component that is used to calculate force and rotation values.")]
    [SerializeField]
    private ShipController spaceshipController;

    private Vector3 eulerAngleVelocity;
    private Vector3 translation;
    private Rigidbody body;

    #endregion // Fields

    #region Properties =============================================================================================

    /// <summary>
    /// Affects how strong and, thus, how fast rotations are applied to the spaceship.
    /// </summary>
    public float Torque
    {
        get { return torque; }
        set { torque = value; }
    }

    //--------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Defines how much a translation force is applied to the spaceship.
    /// </summary>
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    //--------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// The <see cref="Package.SpaceshipController"/> that is used to calculate force and rotation values.
    /// </summary>
    public ShipController SpaceshipController
    {
        get { return spaceshipController; }
        set { spaceshipController = value; }
    }

    #endregion // Properties

    #region Methods ================================================================================================

    private void Start()
    {
        SpaceshipController = GetComponent<ShipController>();
        body = SpaceshipController.Body;
         body.maxAngularVelocity = 4;
    }

    //--------------------------------------------------------------------------------------------------------------

   

    public override void Act()
    {
        if (GetSystemState() == SystemState.on)
        {

            eulerAngleVelocity = new Vector3(-SpaceshipController.Pitch,
                                                   SpaceshipController.Yaw,
                                                   SpaceshipController.Roll);
            translation = transform.right * SpaceshipController.Force.x
                        + transform.up * SpaceshipController.Force.y
                        + transform.forward * SpaceshipController.Force.z;

            SpaceshipController.Body.AddRelativeTorque(eulerAngleVelocity * Torque * body.mass);
            SpaceshipController.Body.AddForce(translation * Speed * body.mass);
        }
        
    }



    #endregion // Methods
} // class SpaceshipPhysics

