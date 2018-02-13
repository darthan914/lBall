using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Key : MonoBehaviour {

    public AudioClip soundEffect;


    private AudioSource source;
    private GameObject currentObject;
	private Movement movement;

	private bool isTouch = false;

	private GameObject main;
	private MainController mc;


	// Use this for initialization
	void Awake () {
		main = GameObject.FindWithTag ("MainCamera");
		mc = main.GetComponent<MainController> ();

        source = GetComponent<AudioSource>();
    }
		
	
	// Update is called once per frame
	void Update () {
		if (movement != null && !isTouch) {
			if ((movement.lastMove == "Up" && currentObject.transform.position.y > transform.position.y) ||
				(movement.lastMove == "Down" && currentObject.transform.position.y < transform.position.y) ||
				(movement.lastMove == "Left" && currentObject.transform.position.x < transform.position.x) ||
				(movement.lastMove == "Right" && currentObject.transform.position.x > transform.position.x))
			{
                source.PlayOneShot(soundEffect);
                mc.TriggerKey();
				isTouch = true;
			}
		}

        
    }

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Movement> () != null && !isTouch) {
			currentObject = other.gameObject;
			movement = currentObject.GetComponent<Movement> ();

            if(mc.allowMove)
            {
                isTouch = true;
            }
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (currentObject == null && other.gameObject.GetComponent<Movement> () != null && !isTouch) {
			currentObject = other.gameObject;
			movement = currentObject.GetComponent<Movement> ();

            if (mc.allowMove)
            {
                isTouch = true;
            }
        }
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetInstanceID() == currentObject.GetInstanceID()) {
			currentObject = null;
			movement = null;
			isTouch = false;
		}
	}

	
}
