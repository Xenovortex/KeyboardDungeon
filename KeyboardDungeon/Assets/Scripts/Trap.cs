using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public KeyCode key;

	void Update () {
		if (Input.GetKeyDown (key)) {
			this.GetComponent<Renderer> ().material.color = Color.green;
		}
	}
}
