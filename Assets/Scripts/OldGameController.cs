using UnityEngine;
using System.Collections;

public class OldGameController : MonoBehaviour {
	public GameObject hazard;
	public int hazardCount;
	public Vector3 spawnValues;
	public float spawnWait;
	public float waveWait;
	public float startWait;

	public GUIText scoreText;
	private int score;

	private bool gameOver;
	private bool restart;

	public GUIText gameOverText;
	public GUIText restartText;

	// Use this for initialization
	void Start () {
		gameOver = false;
		restart = false;
		gameOverText.text = "";
		restartText.text = "";

		score = 0;
		UpdateScore();
		StartCoroutine (SpawnWaves());
	}

	void Update(){
		if(restart){
			if(Input.GetKeyDown (KeyCode.R)){
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds(startWait);

		while(true){
			for(int i=0;i<hazardCount;i++){
				Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x,spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				
				Instantiate(hazard,spawnPosition,spawnRotation);

				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);

			if(gameOver){
				restart = true;
				restartText.text = "Press 'R' to restart game";
				break;
			}
		}
	}

	public void AddNewScore(int newScore){
		score += newScore;
		UpdateScore();
	}

	void UpdateScore(){
		scoreText.text = "Score: " + score;
	}

	public void GameOver(){
		gameOver = true;
		gameOverText.text = "Game Over!";
	}
}
