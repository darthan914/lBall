using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWay : MonoBehaviour {

	public enum Direction {
		Up, Down, Left, Right
	}

	private Direction direction;

	void Start () {
		if (Mathf.RoundToInt (transform.eulerAngles.z) == 0) {
			direction = Direction.Up;
		} else if (Mathf.RoundToInt(transform.eulerAngles.z) == 90) {
			direction = Direction.Left;
		} else if (Mathf.RoundToInt(transform.eulerAngles.z) == 180) {
			direction = Direction.Down;
		} else if (Mathf.RoundToInt(transform.eulerAngles.z) == 270) {
			direction = Direction.Right;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Movement> () != null) {
			if ((direction == Direction.Up && other.gameObject.GetComponent<Movement> ().lastMove == "Down") ||
				(direction == Direction.Down && other.gameObject.GetComponent<Movement> ().lastMove == "Up") ||
				(direction == Direction.Left && other.gameObject.GetComponent<Movement> ().lastMove == "Right") ||
				(direction == Direction.Right && other.gameObject.GetComponent<Movement> ().lastMove == "Left")) {
				other.gameObject.GetComponent<Movement> ().oneWay = true;
			}
		}
	}
}