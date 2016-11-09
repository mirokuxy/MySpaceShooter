using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {
	private GameController gameController;

	
	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if(gameControllerObject != null){
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if(gameController == null){
			Debug.Log ("Cannot find 'GameController' script in DestroyByBoundary\n");
		}
	}


	void OnTriggerExit(Collider other){
		Destroy(other.gameObject);
		if(other.tag == "Enemy"){
			gameController.DestroyEnemy();
			string debug = other.name + " Destroyed by boundary" + " at " + other.transform.position;
			Debug.Log(debug);
		}
	}
}
