using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject targetExplosion;
	public int targetType;

	//public GameObject playerExplosion;
	//public int scoreValue;
	private GameController gameController;

	private int playerType,enemyType;


	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if(gameControllerObject != null){
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if(gameController == null){
			Debug.Log ("Cannot find 'GameController' script\n");
		}
		else{
			playerType = gameController.playerType;
			enemyType = gameController.enemyType;
		}
	}

	/*
	void OnTriggerEnter(Collider other){
		if(other.tag == "Boundary") return;

		Instantiate(explosion,transform.position,transform.rotation);
		if(other.tag == "Player"){
			//Debug.Log("Player Detected\n");
			Instantiate(playerExplosion,other.transform.position,other.transform.rotation);
			if(gameController != null)
				gameController.GameOver();
		}
		if(gameController != null)
			gameController.AddNewScore(scoreValue); 
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
	*/
	void OnTriggerEnter(Collider other){

		if(other.tag == "Asteroid"){
			Destroy (gameObject);
		}
		else if(other.tag == "Enemy"){
			if(targetType == enemyType){
				//gameController.AddScore(enemyType);
				gameController.DestroyEnemy();
				Instantiate(targetExplosion, other.transform.position, other.transform.rotation);
				Destroy(other.gameObject);
				Destroy (gameObject);
			}
		}
		else if(other.tag == "Player"){
			if(targetType == playerType){
				gameController.EndGame();
				Instantiate(targetExplosion, other.transform.position, other.transform.rotation);
				Destroy(other.gameObject);
				Destroy (gameObject);
			}
		}
	}
}
