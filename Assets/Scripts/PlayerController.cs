using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary{
	public float xMin,xMax,zMin,zMax;
}

public class PlayerController : MonoBehaviour {

	public float speed;
	public Boundary boundary;
	public float tilt;


	public GameObject shot;
	public Transform spawn;
	public float fireRate;

	private float nextFire;


	private float camRayLength = 100.0f;
	private Quaternion lastRotation;


	void Start(){
		nextFire = 0.0f;
		lastRotation = Quaternion.identity;
	}
	
	void Update(){
		if( Input.GetButton("Fire1") && Time.time > nextFire){
			nextFire = Time.time + fireRate;
			Instantiate(shot, spawn.position, spawn.rotation);

			GetComponent<AudioSource>().Play();
		}
	}


	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3 ( moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;

		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp( GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax ),
			0.0f,
			Mathf.Clamp( GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);

		//rigidbody.rotation = Quaternion.Euler(0.0f,0.0f, rigidbody.velocity.x * -tilt);
		Turn ();
	}

	void Turn(){
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(camRay.origin, camRay.direction * 50, Color.yellow);

		RaycastHit boundaryHit;

		if(Physics.Raycast (camRay,out boundaryHit, camRayLength)){
			Debug.DrawLine(camRay.origin, boundaryHit.point);
			Vector3 playerToMouse = boundaryHit.point - transform.position;
			playerToMouse.y = 0.0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			GetComponent<Rigidbody>().MoveRotation(newRotation);
			lastRotation = newRotation;
		}
		else{
			//Debug.Log ("No Raycast");
			GetComponent<Rigidbody>().MoveRotation(lastRotation);
		}
	}
}
