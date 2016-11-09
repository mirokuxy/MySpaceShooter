using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

	private int detectCount = 0;

	private GameController gameController;
	private EnemyMover enemyMover;
	private GameObject player;

	private int scaleUp;

	public Vector2 startWait,detectWait;
	
	int CalScaleUp(int level){
		int ans=1;
		for(int i=0;i<level && i<2;i++)
			ans *= 2;
		for(int i=2;i<level;i++)
			ans += ans/2;

		if(ans > 60) ans = 60;
		return ans;
	}

	void Start(){

		//Debug.Log (transform.localScale);
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if(gameControllerObject != null){
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if(gameController == null){
			Debug.Log ("Cannot find 'GameController' script\n");

			scaleUp = 1;
		}
		else
			scaleUp = CalScaleUp(gameController.GetLevel());

		transform.localScale *= scaleUp;

		enemyMover = gameObject.GetComponentInParent <EnemyMover>();
		if(enemyMover == null) Debug.Log ("Cannot find 'EnemyMover' Script");
		//else Debug.Log ("Found 'EnemyMover' Script");

		player = GameObject.FindWithTag ("Player");

		StartCoroutine(Detect ());
	}

	IEnumerator Detect ()
	{
		float DetectArea = 0.5f * scaleUp;

		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
			if(player == null || player.gameObject == null || player.GetComponent<Rigidbody>() == null) yield break;
			if(Vector3.Distance(transform.position, player.transform.position) < DetectArea){
				Debug.Log ("Found Player " + detectCount++ + " in Detect()");
				enemyMover.FoundPlayerPosition(player.GetComponent<Rigidbody>().position);
			}
			yield return new WaitForSeconds (Random.Range (detectWait.x, detectWait.y));
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			Debug.Log ("Found Player" + detectCount++ );

			enemyMover.FoundPlayerPosition(other.GetComponent<Rigidbody>().position);

			/*
			Vector3 toPlayer = other.transform.position - transform.position;
			Vector3 forward = transform.forward;
			toPlayer.y = 0.0f;
			forward.y = 0.0f;
			float angle = Vector3.Angle(forward,toPlayer);
			Vector3 cross = Vector3.Cross(forward,toPlayer);

			if(cross.y < 0) angle = -angle;
			Debug.Log (angle);
			*/

			//float angle2 = Mathf.Rad2Deg(Mathf.Acos (Vector2.Dot (toPlayer2D,fo)))
			/*
			Vector2.magnitude
			vector2.Dot

			Debug.Log (angle);
			Mathf.Rad2Deg
			*/
			//transform.localScale *= 2;
			//Debug.Log (transform.localScale);
		}
	}

}
