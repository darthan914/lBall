using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    private Color currentColor;

    void Start()
    {
        currentColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, TransparencyActive(gameObject.GetComponent<Collider2D>().enabled));
    }

    void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "Needle")
		{
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, TransparencyActive(false));
        }
	}

    float TransparencyActive(bool statActive)
    {
        if (statActive)
        {
            return 1.0f;
        }
        else
        {
            return 0f;
        }
    }
}
