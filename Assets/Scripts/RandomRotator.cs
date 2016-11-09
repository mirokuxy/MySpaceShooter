using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {

	public float tumble;
	private Vector3 rotation;

	void Start(){
		rotation = Random.insideUnitSphere * 90;
	}

	// Use this for initialization
	void Update () {
		transform.Rotate(rotation * Time.deltaTime);
	}

}
