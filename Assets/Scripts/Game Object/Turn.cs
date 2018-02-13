using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour {

	public enum Turning{
		Left, Right
	}

	public Turning turning;
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
	void FixedUpdate () {
		if (mc.allowMove) {
			ClearObject ();
		}

		if (movement != null && isTouch) {
            
            if (movement.lastMove == "Up" && currentObject.transform.position.y > transform.position.y) {
				movement.transform.position = transform.position;
				if (turning == Turning.Left) {
					movement.Rolling("Left");
				} else if (turning == Turning.Right) {
					movement.Rolling("Right");
				}
                source.PlayOneShot(soundEffect);
                ClearObject ();
			}
			else if (movement.lastMove == "Down" && currentObject.transform.position.y < transform.position.y) {
				movement.transform.position = transform.position;
				if (turning == Turning.Left) {
					movement.Rolling("Right");
				} else if (turning == Turning.Right) {
					movement.Rolling("Left");
				}
                source.PlayOneShot(soundEffect);
                ClearObject ();
			}
			else if (movement.lastMove == "Left" && currentObject.transform.position.x < transform.position.x) {
				movement.transform.position = transform.position;
				if (turning == Turning.Left) {
					movement.Rolling("Down");
				} else if (turning == Turning.Right) {
					movement.Rolling("Up");
				}
                source.PlayOneShot(soundEffect);
                ClearObject ();
			}
			else if (movement.lastMove == "Right" && currentObject.transform.position.x > transform.position.x) {
				movement.transform.position = transform.position;
				if (turning == Turning.Left) {
					movement.Rolling("Up");
				} else if (turning == Turning.Right) {
					movement.Rolling("Down");
				}
                source.PlayOneShot(soundEffect);
                ClearObject ();
			}


		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if (other.gameObject.GetComponent<Movement> () != null && !isTouch) {
			
			currentObject = other.gameObject;
			movement = currentObject.GetComponent<Movement> ();
			isTouch = true;
		}
	}

	void ClearObject()
	{
        
        currentObject = null;
		movement = null;
		isTouch = false;
	}

}
