using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float speed = 10f;
	public bool isMove = false;

    public AudioClip rolling;
    public AudioClip collideWall;
    public AudioClip collideBall;
    public AudioClip pop;

    [HideInInspector] public string lastMove = "None";
    [HideInInspector] public Vector3 centerPosition;
    [HideInInspector] public bool oneWay;
    [HideInInspector] public bool warped;

    private float t;
	private int count;

    private GameObject main;
    private MainController mc;

    private Rigidbody2D rb2d;
    private AudioSource source;

    private bool recorded;
    private Color currentColor;

    private Vector2 touchOrigin = -Vector2.one;
    private float swipeSensitive = 30f;


    void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
        source = GetComponent<AudioSource> ();

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
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

            if (Input.GetKeyDown (KeyCode.UpArrow)) {
				Rolling("Up");
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				Rolling("Down");
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				Rolling("Left");
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				Rolling("Right");
			}
#endif

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            if (Input.touchCount > 0)
            {
                Touch myTouch = Input.touches[0];
                
                if (myTouch.phase == TouchPhase.Began)
                {
                    touchOrigin = myTouch.position;
                }
                
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    Vector2 touchEnd = myTouch.position;
                    
                    float x = touchEnd.x - touchOrigin.x;
                    float y = touchEnd.y - touchOrigin.y;

                    touchOrigin.x = -1;

                    if (Mathf.Abs(x) > swipeSensitive || Mathf.Abs(y) > swipeSensitive)
                    {
                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            if (x > 0) Rolling("Right");
                            else Rolling("Left");
                        }
                        else
                        {
                            if (y > 0) Rolling("Up");
                            else Rolling("Down");
                        }
                    }
                        
                }
            }
#endif
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

        if(coll.gameObject.name == "Wall" || coll.gameObject.name =="Blue Wall" || coll.gameObject.name == "Red Wall")
        {
            source.PlayOneShot(collideWall);
        }

        if (isMove)
		{
			Recenter ();
		}

		if (coll.gameObject.tag == "Player" && gameObject.tag == "Needle") {
			coll.gameObject.GetComponent<Movement> ().PopBall ();
            
        }

		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Needle" || coll.gameObject.tag == "Stone") {
			rb2d.velocity = Vector2.zero;
            source.PlayOneShot(collideBall);
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
        source.PlayOneShot(pop);
        gameObject.GetComponent<Collider2D>().enabled = false;
        mc.gameOver = true;
        Recenter();
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
            source.PlayOneShot(rolling, 0.5f);
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
