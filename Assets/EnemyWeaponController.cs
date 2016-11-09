using UnityEngine;
using System.Collections;

public class EnemyWeaponController : MonoBehaviour {
	public GameObject enemyShot;
	public Transform shotSpawn;

	public Vector2 startWait;
	public Vector2 fireWait;


	// Use this for initialization
	void Start () {
		StartCoroutine (Fire());
	}

	IEnumerator Fire(){
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while(true){
			Instantiate(enemyShot,shotSpawn.position,shotSpawn.rotation);
			yield return new WaitForSeconds (Random.Range (fireWait.x, fireWait.y));
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
