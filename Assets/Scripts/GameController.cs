using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int asteroidCount;
	public GameObject asteroid;
	public Vector2 spawnx,spawnz;

	private int score;
	private bool gameOver;
	private bool restartEnable;
	public int currentLevel;

	public GUIText scoreText,restartText,gameOverText,levelText,wavePrompt;

	public int playerType;
	public int enemyType;
	public int enemyScore;

	public float startWait,waveWait,spawnWait,promptWait;
	public int enemyCount;
	private int enemyProduced,enemyKilled;
	public GameObject enemy;


	// Use this for initialization
	void Start () {
		score = 0;
		gameOver = false;
		restartEnable = false;
		currentLevel = 0;

		UpdateScoreText();
		gameOverText.text = "";
		restartText.text = "";
		UpdateLevelText();
		StartCoroutine(UpdateWavePrompt());


		SpawnAsteriods();
		enemyCount = 1;
		StartCoroutine (SpawnEnemyWaves());
	}

	void SpawnAsteriods(){
		for(int i=0;i<asteroidCount;i++){
			Vector3 spawnPosition = new Vector3 (Random.Range(spawnx.x,spawnx.y), 0.0f, Random.Range(spawnz.x, spawnz.y));
			Quaternion spawnRotation = Quaternion.identity;
			
			Instantiate(asteroid,spawnPosition,spawnRotation);
		}
	}

	IEnumerator SpawnEnemyWaves(){
		yield return new WaitForSeconds(startWait);
		
		while(true){
			enemyProduced = enemyKilled = 0;

			for(int i=0;i<enemyCount && !gameOver;i++){
				Vector3 spawnPosition = new Vector3 (Random.Range(spawnx.x,spawnx.y), 0.0f, Random.Range (spawnz.x,spawnz.y));
				Quaternion spawnRotation = Quaternion.identity;
				
				Instantiate(enemy,spawnPosition,spawnRotation);
				enemyProduced++;

				yield return new WaitForSeconds(spawnWait);
			}

			while(enemyKilled < enemyProduced && !gameOver){
				//Debug.Log (enemyKilled);
				//Debug.Log (enemyProduced);
				yield return new WaitForSeconds(spawnWait);
			}

			Debug.Log (enemyKilled);

			if(gameOver){
				restartEnable = true;
				restartText.text = "Press 'R' to restart game";
				break;
			}
		
			Debug.Log ("Not Game Over");

			yield return new WaitForSeconds(waveWait);

			currentLevel++;
			UpdateLevelText();
			if(enemyCount < 8)
				enemyCount *= 2;
			else
				enemyCount += enemyCount/2;

			Debug.Log (enemyCount);

			StartCoroutine(UpdateWavePrompt());
		}
	}

	void Update(){
		if(restartEnable){
			if(Input.GetKeyDown (KeyCode.R)){
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	void AddScore(int type){
		if(type == enemyType){
			score += enemyScore;
		}
		UpdateScoreText();
	}

	public void DestroyEnemy(){
		enemyKilled++;
		AddScore(enemyType);
	}

	public void EndGame(){
		gameOver = true;
		gameOverText.text = "Game Over!";
	}

	void UpdateScoreText(){
		scoreText.text = "Score: " + score;
	}

	void UpdateLevelText(){
		levelText.text = "Level: " + currentLevel;
	}

	IEnumerator UpdateWavePrompt(){
		wavePrompt.text = "Wave " + currentLevel;
		yield return new WaitForSeconds(promptWait);
		wavePrompt.text = "";
	}

	public int GetLevel(){
		return currentLevel;
	}

}
