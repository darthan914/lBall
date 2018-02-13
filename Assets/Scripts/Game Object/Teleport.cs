using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public AudioClip soundEffect;
    private AudioSource source;

    private GameObject currentObject;
	private Movement movement;

	private GameObject main;
	private MainController mc;
	
    public GameObject teleportLocation;
    public bool exitOnly;

    public bool stayed;

    // Use this for initialization
    void Awake () {
		main = GameObject.FindWithTag ("MainCamera");
		mc = main.GetComponent<MainController> ();

        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (mc.allowMove && movement) {
			ClearObject ();
			stayed = false;
            if (!exitOnly) teleportLocation.gameObject.GetComponent<Teleport>().stayed = false;
		}

		if (teleportLocation && movement && !movement.warped && !stayed && !exitOnly) {
			if ((movement.lastMove == "Up" && currentObject.transform.position.y > transform.position.y) ||
				(movement.lastMove == "Down" && currentObject.transform.position.y < transform.position.y) ||
				(movement.lastMove == "Left" && currentObject.transform.position.x < transform.position.x) ||
				(movement.lastMove == "Right" && currentObject.transform.position.x > transform.position.x))
			{
                source.PlayOneShot(soundEffect);
                currentObject.transform.position = teleportLocation.gameObject.transform.position;
				movement.warped = true;
                ClearObject();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Movement> ()) {
			currentObject = other.gameObject;
			movement = currentObject.GetComponent<Movement> ();
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.GetComponent<Movement> () && other.gameObject.GetComponent<Movement> ().lastMove == "None") {
			stayed = true;
			teleportLocation.gameObject.GetComponent<Teleport> ().stayed = true;
		}
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Movement>())
        {
            stayed = false;
            teleportLocation.gameObject.GetComponent<Teleport>().stayed = false;
        }
    }

    void ClearObject()
	{
		currentObject = null;
		movement = null;
	}
}
