using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour
{
	public Done_Boundary boundary;
	public float tilt;
	public float dodgex,dodgez,angleRange;
	public float smoothing,smoothingAngle;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	
	private float targetVx,targetVz,targetAngle,lastAngle;

	private bool foundPlayer;
	private float playerAngle;
	private bool randomTurn;

	public void FoundPlayerPosition(Vector3 playerPosition){
		foundPlayer = true;

		Vector3 toPlayer = playerPosition - transform.position;
		Vector3 forward = transform.forward;
		toPlayer.y = 0.0f;
		forward.y = 0.0f;
		float angle = Vector3.Angle(forward,toPlayer);
		Vector3 cross = Vector3.Cross(forward,toPlayer);
		
		if(cross.y < 0) angle = -angle;
		playerAngle = angle;
		Debug.Log ("PlayerAngle just found: " + playerAngle);
	}

	void Start ()
	{
		rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
		lastAngle = 0.0f;

		StartCoroutine(Evade());
		StartCoroutine(Turn());
	}


	IEnumerator Evade ()
	{
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
			targetVx = Random.Range (-dodgex, dodgex);
			targetVz = Random.Range (-dodgez, dodgez);
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetVx = targetVz = 0.0f;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}

	IEnumerator Turn ()
	{
		randomTurn = true;
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
			if(randomTurn)
				targetAngle = Random.Range (-angleRange, angleRange) + lastAngle;
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			/*
			if(randomTurn)
				targetAngle = lastAngle;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
			*/
		}
	}

	void FixedUpdate ()
	{
		if(foundPlayer){
			targetAngle = playerAngle + lastAngle;
			foundPlayer = false;
			randomTurn = false;
			//Debug.Log ("Found Player in FixedUpdate");
			Debug.Log ("Turning " + playerAngle);
		}
		float newAngle = Mathf.MoveTowards (lastAngle, targetAngle, smoothingAngle * Time.deltaTime);
		
		rigidbody.rotation = Quaternion.Euler (0, newAngle, 0);
		lastAngle = newAngle;

		if(randomTurn == false && lastAngle == targetAngle) randomTurn = true;

		float newVx = Mathf.MoveTowards (rigidbody.velocity.x, targetVx, smoothing * Time.deltaTime);
		float newVz = Mathf.MoveTowards (rigidbody.velocity.z, targetVz, smoothing * Time.deltaTime);

		rigidbody.velocity = new Vector3 (newVx, 0.0f, newVz);
		//Quaternion deltaRotation = Quaternion.Euler(new Vector3 (0.0f, angleVelocity,0.0f) * Time.deltaTime * 100);
		//Debug.Log (deltaRotation);
		//rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
		//rigidbody.rotation += deltaRotation;

		rigidbody.position = new Vector3
		(
			Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
		);

		//rigidbody.MoveRotation(Quaternion.identity);
		//rigidbody.rotation = Quaternion.Euler (0, 0, rigidbody.velocity.x * -tilt);
	}
}
