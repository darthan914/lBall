using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	private GameObject main;
	private MainController mc;

	private GameObject currentObject;

    // Use this for initialization
    void Start () {
		main = GameObject.FindWithTag ("MainCamera");
		mc = main.GetComponent<MainController> ();
	}

    private void Update()
    {
        if (currentObject != null && currentObject.gameObject.tag == "Player")
        {
            mc.complete = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (currentObject == null && other.gameObject.GetComponent<Movement>() != null)
        {
            currentObject = other.gameObject;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (currentObject == null && other.gameObject.GetComponent<Movement>() != null)
        {
            currentObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetInstanceID() == currentObject.GetInstanceID())
        {
            ClearObject();
            mc.complete = false;
        }
    }

    void ClearObject()
    {
        currentObject = null;
    }

}
