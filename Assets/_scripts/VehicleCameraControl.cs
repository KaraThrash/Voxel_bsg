using UnityEngine;
using System.Collections;

public class VehicleCameraControl : MonoBehaviour
{

	public Transform playerCar;
	private Rigidbody playerRigid;
	public float distance = 10.0f;
	public float height = 5.0f;
	private float defaultHeight = 0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public float defaultFOV = 60f;
	public float zoomMultiplier = 0.3f;

	public bool freeLook;
	public float mouseSensitivity = 3.0f,clampAngle = 30.0f;
		public float rotX,rotY;
	void Start(){

		// Early out if we don't have a target
		if (!playerCar)
			return;

			Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		playerRigid = playerCar.GetComponent<Rigidbody>();

	}

	void Update(){
		if (!playerRigid)
			return;
		if(playerRigid != playerCar.GetComponent<Rigidbody>())
			playerRigid = playerCar.GetComponent<Rigidbody>();

		// GetComponent<Camera>().fieldOfView = defaultFOV + playerRigid.velocity.magnitude * zoomMultiplier;

	}

	void FixedUpdate (){

		// Early out if we don't have a target
		if (!playerCar || !playerRigid)
			return;

			if(freeLook == true){FreeLook();}
			else{LookAtCar();}

	}
	public void ToggleFreeLook()
	{
		Vector3 rot = transform.localRotation.eulerAngles;
	rotY = rot.y;
	rotX = rot.x;
		if(freeLook == true){freeLook = false;}
		else{freeLook = true;}

	}
	public void LookAtCar()
	{
		if (!playerCar || !playerRigid)
			return;

			if(Input.GetKeyDown(KeyCode.JoystickButton6))
			{
				transform.position = playerCar.transform.position - (playerCar.transform.forward * distance);
				transform.rotation = playerCar.transform.rotation;
				return;
			}

		//calculates speed in local space. positive if going forward, negative if reversing
		float speed = (playerRigid.transform.InverseTransformDirection(playerRigid.velocity).magnitude) * 3f ;

		// Calculate the current rotation angles.
		Vector3 wantedRotationAngle = playerCar.eulerAngles;
		float wantedHeight = playerCar.position.y + height;
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;


		// if(speed < -5)
			// wantedRotationAngle.y = playerCar.eulerAngles.y + 180;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle.y, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		Vector3 targetPos = playerCar.position;
		// targetPos -= currentRotation * Vector3.forward * distance;
		targetPos -= (playerCar.transform.position - transform.position).normalized * distance;

		// Set the height of the camera
		targetPos = new Vector3(targetPos.x, currentHeight + defaultHeight, targetPos.z);

		float chaseSpeed = Time.deltaTime *  (Vector3.Distance(transform.position,targetPos) * 0.8f);
		chaseSpeed += (Vector3.Distance(transform.position,targetPos) % 5) * 0.2f;
		// if(Vector3.Distance(transform.position,targetPos) > 40){chaseSpeed *= 2;}
		transform.position = Vector3.Lerp(transform.position, targetPos, chaseSpeed);

		Quaternion targetRotation = Quaternion.LookRotation (playerCar.position - transform.position);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, 3 * Time.deltaTime);
	}
	public void FreeLook()
	{
		float speed = (playerRigid.transform.InverseTransformDirection(playerRigid.velocity).z) * 1f;
			transform.position = playerCar.transform.position;
			float mouseX = Input.GetAxis("Mouse X");
		          float mouseY = -Input.GetAxis("Mouse Y");

		          rotY += mouseX * mouseSensitivity * Time.deltaTime;
		          rotX += mouseY * mouseSensitivity * Time.deltaTime;

		          rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

		          Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		          transform.rotation = localRotation;
	}

}
