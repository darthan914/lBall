using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

	private GameObject currentObject;
	private Movement movement;

	private GameObject teleportLocation;

	private GameObject main;
	private MainController mc;

	public bool exitOnly;
	public bool stayed;

    // Use this for initialization
    void Start () {
		main = GameObject.FindWithTag ("MainCamera");
		mc = main.GetComponent<MainController> ();

		GameObject[] teleports = GameObject.FindGameObjectsWithTag("Teleport");
		GameObject[] exits = GameObject.FindGameObjectsWithTag("Exit");

        if(exits.Length == 1)
        {
            foreach (GameObject exit in exits)
            {
                if (exit.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                {
                    teleportLocation = exit.gameObject;
                }
            }
        } else if (teleports.Length == 2) {
			foreach (GameObject teleport in teleports)
			{
				if (teleport.gameObject.GetInstanceID () != gameObject.GetInstanceID ()) {
					teleportLocation = teleport.gameObject;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (mc.allowMove && movement != null) {
			ClearObject ();
			stayed = false;
            if (!exitOnly)
            {
                teleportLocation.gameObject.GetComponent<Teleport>().stayed = false;
            }
		}

		if (movement != null && !movement.warped && !stayed && !exitOnly) {
			if ((movement.lastMove == "Up" && currentObject.transform.position.y > transform.position.y) ||
				(movement.lastMove == "Down" && currentObject.transform.position.y < transform.position.y) ||
				(movement.lastMove == "Left" && currentObject.transform.position.x < transform.position.x) ||
				(movement.lastMove == "Right" && currentObject.transform.position.x > transform.position.x))
			{
				currentObject.transform.position = teleportLocation.gameObject.transform.position;
				movement.warped = true;
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

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.GetComponent<Movement> () != null && other.gameObject.GetComponent<Movement> ().lastMove == "None") {
			stayed = true;
			teleportLocation.gameObject.GetComponent<Teleport> ().stayed = true;
		}
	}

	void ClearObject()
	{
		currentObject = null;
		movement = null;
	}
}
