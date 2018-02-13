using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour {

    public AudioClip flipObject;
    public AudioClip hitWall;
    private AudioSource source;

    public enum Line {
		Horizontal, Vertical
	}

	public enum Direction {
		Up, Down, Left, Right
	}

	Direction Reverse(Direction dir)
	{
		if(dir == Direction.Up)
			dir = Direction.Down;
		else if(dir == Direction.Down)
			dir = Direction.Up;
		else if(dir == Direction.Right)
			dir = Direction.Left;
		else if(dir == Direction.Left)
			dir = Direction.Right;

		return dir;
	}

    private Line line;
	private Direction direction;

	private GameObject currentObject;
	private Movement movement;
	private bool fliped;

	private GameObject main;
	private MainController mc;
    private bool recorded;

    void Awake () {
		main = GameObject.FindWithTag ("MainCamera");
		mc = main.GetComponent<MainController> ();

		if ((int)Mathf.Round (transform.eulerAngles.z) % 180 == 0) {
			line = Line.Vertical;
		} else if ((int)Mathf.Round (transform.eulerAngles.z) % 180 != 0) {
			line = Line.Horizontal;
		}

		if ((int)Mathf.Round (transform.eulerAngles.z) == 0) {
			direction = Direction.Up;
		} else if ((int)Mathf.Round (transform.eulerAngles.z) == 90) {
			direction = Direction.Left;
		} else if ((int)Mathf.Round (transform.eulerAngles.z) == 180) {
			direction = Direction.Down;
		} else if ((int)Mathf.Round (transform.eulerAngles.z) == 270) {
			direction = Direction.Right;
		}

        source = GetComponent<AudioSource>();

    }

	void Update () {
		if (mc.allowMove && movement != null) {
			fliped = false;
			ClearObject ();
		}

		if ((int)Mathf.Round (transform.eulerAngles.z) % 180 == 0) {
			line = Line.Vertical;
		} else if ((int)Mathf.Round (transform.eulerAngles.z) % 180 == 90) {
			line = Line.Horizontal;
		}

		if ((int)Mathf.Round (transform.eulerAngles.z) == 0) {
			direction = Direction.Up;
		} else if ((int)Mathf.Round (transform.eulerAngles.z) == 90) {
			direction = Direction.Left;
		} else if ((int)Mathf.Round (transform.eulerAngles.z) == 180) {
			direction = Direction.Down;
		} else if ((int)Mathf.Round (transform.eulerAngles.z) == 270) {
			direction = Direction.Right;
		}

		if (movement != null && !fliped) {
			if ((movement.lastMove == "Up" && currentObject.transform.position.y > transform.position.y) ||
				(movement.lastMove == "Down" && currentObject.transform.position.y < transform.position.y) ||
				(movement.lastMove == "Left" && currentObject.transform.position.x < transform.position.x) ||
				(movement.lastMove == "Right" && currentObject.transform.position.x > transform.position.x))
			{
                source.PlayOneShot(flipObject);
                transform.eulerAngles = new Vector3 (0,0,transform.eulerAngles.z + 180);
				direction = Reverse (direction);
				ClearObject ();
				fliped = true;
			}
		}

    }

	void OnTriggerEnter2D(Collider2D other) {
		

		if (other.gameObject.GetComponent<Movement> () != null) {
			currentObject = other.gameObject;
			movement = currentObject.GetComponent<Movement> ();

            if ((direction != Direction.Up && movement.lastMove == "Down") ||
            (direction != Direction.Down && movement.lastMove == "Up") ||
            (direction != Direction.Left && movement.lastMove == "Right") ||
            (direction != Direction.Right && movement.lastMove == "Left"))
            {
                other.gameObject.GetComponent<Movement>().oneWay = true;
                source.PlayOneShot(hitWall);
            }
        }

	}

	void OnTriggerStay2D(Collider2D other) {

		if (other.gameObject.GetComponent<Movement> () != null && movement == null) {
			currentObject = other.gameObject;
			movement = currentObject.GetComponent<Movement> ();

            if (((line != Line.Vertical && (movement.lastMove == "Down" || movement.lastMove == "Up")) ||
            (line != Line.Horizontal && (movement.lastMove == "Left" || movement.lastMove == "Right")) &&
            movement != null))
            {
                other.gameObject.GetComponent<Movement>().oneWay = true;
                source.PlayOneShot(hitWall);
            }
        }
	}

	void ClearObject()
	{
		currentObject = null;
		movement = null;
	}

    string DirectionToString(Direction dir)
    {
        if (dir == Direction.Up)
            return "Up";
        else if (dir == Direction.Down)
            return "Down";
        else if (dir == Direction.Right)
            return "Right";
        else if (dir == Direction.Left)
            return "Left";

        return "None";
    }

    public void UpdatePostion(ObjectRecord obj)
    {
        transform.position = obj.Position;
        transform.eulerAngles = obj.Rotation; 
    }
}
