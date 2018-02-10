using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour {

	private GameObject currentObject;
	private Movement movement;

	private GameObject main;
	private MainController mc;

	// Use this for initialization
	void Start () {
		main = GameObject.FindWithTag ("MainCamera");
		mc = main.GetComponent<MainController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (mc.allowMove) {
			ClearObject ();
		}

		if (movement != null) {
			if ((movement.lastMove == "Up" && currentObject.transform.position.y > transform.position.y) ||
				(movement.lastMove == "Down" && currentObject.transform.position.y < transform.position.y) ||
				(movement.lastMove == "Left" && currentObject.transform.position.x < transform.position.x) ||
				(movement.lastMove == "Right" && currentObject.transform.position.x > transform.position.x))
			{
				movement.Recenter ();
				movement = null;
				currentObject = null;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Movement> () != null) {
			currentObject = other.gameObject;
			movement = currentObject.GetComponent<Movement> ();
		}
	}

	void ClearObject()
	{
		currentObject = null;
		movement = null;
	}
}
