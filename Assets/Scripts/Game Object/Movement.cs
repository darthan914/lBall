using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float speed = 10f;
	public bool isMove = false;

	public string lastMove = "None";
	private float t = 0f;
	private int count;

    private GameObject main;
    private MainController mc;

    private Rigidbody2D rb2d;
	public Vector3 centerPosition;

	private bool bySand;
	public bool oneWay;
	public bool warped;

    private bool recorded;
    private Color currentColor;


    void Start () {
		rb2d = GetComponent<Rigidbody2D> ();

        main = GameObject.FindWithTag("MainCamera");
        mc = main.GetComponent<MainController>();

        centerPosition = transform.position;

        currentColor = gameObject.GetComponent<SpriteRenderer>().color;

    }
	
	void FixedUpdate () {

		if (oneWay) {
			Recenter ();
			oneWay = false;
		}

		if (main.GetComponent<MainController>().allowMove) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				Rolling("Up");
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				Rolling("Down");
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				Rolling("Left");
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				Rolling("Right");
			}
			t = 0f;
			warped = false;
		}

		if(t < 10f && !isMove)
		{
			rb2d.velocity = Vector2.zero;
			t += Time.deltaTime * speed * main.GetComponent<MainController>().countBall;
			transform.position = new Vector2 (Mathf.Lerp (transform.position.x, centerPosition.x, t), Mathf.Lerp (transform.position.y, centerPosition.y, t));
		}

        if(t >= 10f && !isMove)
        {
            isMove = false;
            lastMove = "None";
        }

        gameObject.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, TransparencyActive(gameObject.GetComponent<Collider2D>().enabled));

    }

	void OnCollisionEnter2D(Collision2D coll) {

		if (isMove)
		{
			Recenter ();
		}

		if (coll.gameObject.tag == "Player" && gameObject.tag == "Needle") {
			coll.gameObject.GetComponent<Movement> ().PopBall ();
		}

		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Needle" || coll.gameObject.tag == "Stone") {
			rb2d.velocity = Vector2.zero;
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (isMove) {
			Recenter ();
		}
	}

	public void Recenter()	{
		float ModX = Mathf.Round (transform.position.x);
		float ModY = Mathf.Round (transform.position.y);

		if (ModY % 2 == 0) {
			if (lastMove == "Up") {
				ModY = ModY - 1f;
			}
			else if (lastMove == "Down") {
				ModY = ModY + 1f;
			}
		}
		if (ModX % 2 == 0) {
			if (lastMove == "Right") {
				ModX = ModX - 1f;
			}
			else if (lastMove == "Left") {
				ModX = ModX + 1f;
			}
		}
		centerPosition = new Vector2 (ModX, ModY);

		isMove = false;
		lastMove = "None";

        
    }

	public void PopBall()
	{
        gameObject.GetComponent<Collider2D>().enabled = false;
        
        mc.gameOver = true;
        mc.Failed();
	}

	public void Rolling(string direction)
	{
        if(!mc.gameOver && !mc.complete && mc.start)
        {
            if (direction == "Up")
            {
                rb2d.velocity = Vector2.up * speed;
                isMove = true;
            }
            else if (direction == "Down")
            {
                rb2d.velocity = Vector2.down * speed;
                isMove = true;
            }
            else if (direction == "Left")
            {
                rb2d.velocity = Vector2.left * speed;
                isMove = true;
            }
            else if (direction == "Right")
            {
                rb2d.velocity = Vector2.right * speed;
                isMove = true;
            }
        }

		lastMove = direction;
	}

    public void UpdatePostion(ObjectRecord obj)
    {
        transform.position = obj.Position;
        centerPosition = obj.Position;
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
