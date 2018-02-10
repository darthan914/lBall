using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Key : MonoBehaviour {

    public GameObject blueObject;
    public GameObject redObject;
    public GameObject blueWall;
    public GameObject redWall;

    private GameObject currentObject;
	private Movement movement;

	private bool isTouch = false;

	private int blueCount;
	private int redCount;

	private GameObject main;
	private MainController mc;


	// Use this for initialization
	void Start () {

		main = GameObject.FindWithTag ("MainCamera");
		mc = main.GetComponent<MainController> ();

		if (blueObject == null && GameObject.Find ("Blue Object") != null) {
            blueObject = GameObject.Find ("Blue Object");
			blueCount = blueObject.transform.childCount;
		}

		if (redObject == null && GameObject.Find ("Red Object") != null) {
            redObject = GameObject.Find ("Red Object");
			redCount = redObject.transform.childCount;
		}

        if (blueWall == null && GameObject.Find("Blue Wall") != null)
        {
            blueWall = GameObject.Find("Blue Wall");
        }

        if (redWall == null && GameObject.Find("Red Wall") != null)
        {
            redWall = GameObject.Find("Red Wall");
        }

    }
		
	
	// Update is called once per frame
	void Update () {
		if (movement != null && !isTouch) {
			if ((movement.lastMove == "Up" && currentObject.transform.position.y > transform.position.y) ||
				(movement.lastMove == "Down" && currentObject.transform.position.y < transform.position.y) ||
				(movement.lastMove == "Left" && currentObject.transform.position.x < transform.position.x) ||
				(movement.lastMove == "Right" && currentObject.transform.position.x > transform.position.x))
			{
				

                mc.activeGate = !mc.activeGate;
				isTouch = true;
			}
		}

        if (blueObject != null)
        {
            for (int i = 0; i < blueCount; i++)
            {
                Color currentColor = blueObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color;
                blueObject.transform.GetChild(i).GetComponent<Collider2D>().enabled = !mc.activeGate;
                blueObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, transparencyActive(!mc.activeGate));
            }
        }

        if (blueWall != null)
        {
            Color currentColor = blueWall.gameObject.GetComponent<Tilemap>().color;
            blueWall.gameObject.GetComponent<Collider2D>().enabled = !mc.activeGate;
            blueWall.gameObject.GetComponent<Tilemap>().color = new Color(currentColor.r, currentColor.g, currentColor.b, transparencyActive(!mc.activeGate));
        }

        if (redObject != null)
        {
            for (int i = 0; i < redCount; i++)
            {
                Color currentColor = redObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color;
                redObject.transform.GetChild(i).GetComponent<Collider2D>().enabled = mc.activeGate;
                redObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, transparencyActive(mc.activeGate));
            }
        }

        if (redWall != null)
        {
            Color currentColor = redWall.gameObject.GetComponent<Tilemap>().color;
            redWall.gameObject.GetComponent<Collider2D>().enabled = mc.activeGate;
            redWall.gameObject.GetComponent<Tilemap>().color = new Color(currentColor.r, currentColor.g, currentColor.b, transparencyActive(mc.activeGate));
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

	float transparencyActive(bool statActive)
	{
		if(statActive)
		{
			return 1.0f;
		}
		else
		{
			return .5f;
		}
	}
}
