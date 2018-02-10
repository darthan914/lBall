using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

	private GameObject ball;
	private GameObject obj;
	private GameObject blueWall;
	private GameObject redWall;

    private GameObject[] teleports;
	private GameObject[] exits;

    private Text textMove;
    private Text textTime;
    private GameObject panelMessage;

    private Button buttonUndo;
    private Button buttonRedo;
    private Button buttonNext;

    private float time;
    private float delay;

    public GameObject canvasUI;
    public int countBall;

    public bool allowMove;
	public bool start = false;
	public bool gameOver = false;
    public bool complete = false;
	public bool activeGate = false;

    public int numberMove;
    public List<ObjectRecord> listObjectRecord = new List<ObjectRecord>();
    public bool recorded;


    // Use this for initialization
    void Start () {
        ball = GameObject.Find("Ball");
        obj = GameObject.Find("Object");
        blueWall = GameObject.Find("Blue Wall");
        redWall = GameObject.Find("Red Wall");

        teleports = GameObject.FindGameObjectsWithTag("Teleport");
		exits = GameObject.FindGameObjectsWithTag("Exit");

 		countBall = GameObject.Find("Ball") != null ? ball.transform.childCount : -1;

        if (exits.Length != 1)
        {
            foreach (GameObject exit in exits)
            {
                exit.gameObject.SetActive(false);
            }
        }

        if (exits.Length != 1 && teleports.Length != 2)
        {
            foreach (GameObject teleport in teleports)
            {
                teleport.gameObject.SetActive(false);
            }
        }

        if (canvasUI != null && GameObject.Find("Canvas") == null)
        {
            GameObject clone = Instantiate(canvasUI);
            clone.name = "Canvas";
            textMove = GameObject.Find("Move").GetComponent<Text>();
            textTime = GameObject.Find("Time").GetComponent<Text>();
            panelMessage = GameObject.Find("Message Panel");

            buttonRedo = GameObject.Find("Redo").GetComponent<Button>();
            buttonUndo = GameObject.Find("Undo").GetComponent<Button>();
            buttonNext = GameObject.Find("Next").GetComponent<Button>();

            panelMessage.GetComponent<Image>().color = Color.green;
            panelMessage.GetComponentInChildren<Text>().text = "GO";
            panelMessage.GetComponentInChildren<Text>().color = Color.green;

            panelMessage.GetComponent<Image>().CrossFadeAlpha(0f, 2.0f, false);
            panelMessage.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 2.0f, false);

        }

    }

	// Update is called once per frame
	void Update () {

        if (complete)
        {
            buttonUndo.interactable = false;
            buttonRedo.interactable = false;
        }

        if (!gameOver && !complete)
        {
            if (delay < 1)
            {
                delay += Time.deltaTime;
            }
            else
            {
                time += Time.deltaTime;
                start = true;
            }
        }

		allowMove = true;

        textMove.text = "Move : " + (numberMove - 1);
        textTime.text = "Time : " + TextTime(time);

        for (int i = 0; i < countBall; i++)
		{
			if (ball.transform.GetChild(i).GetComponent<Movement> () != null && ball.transform.GetChild(i).GetComponent<Movement> ().isMove) {
				allowMove = false;
                recorded = false;
            }
		}

        if(allowMove && !recorded)
        {
            listObjectRecord.RemoveAll(obj => obj.NumberMove > numberMove);
            numberMove++;
            SaveRecord();
        }

	}

    public void Record(ObjectRecord objectRecord)
    {
        listObjectRecord.Add(objectRecord);
    }

    void SaveRecord()
    {
        Record(new ObjectRecord()
        {
            Id = gameObject.GetInstanceID(),
            Name = gameObject.name,
            NumberMove = numberMove,
            Position = transform.position,
            Rotation = transform.eulerAngles,
            Enabled = activeGate
        });

        for (int i = 0; i < ball.transform.childCount; i++)
        {
            Record(new ObjectRecord()
            {
                Id = ball.transform.GetChild(i).gameObject.GetInstanceID(),
                Name = ball.transform.GetChild(i).gameObject.name,
                NumberMove = numberMove,
                Position = numberMove == 0 ? ball.transform.GetChild(i).transform.position : ball.transform.GetChild(i).GetComponent<Movement>().centerPosition,
                Rotation = ball.transform.GetChild(i).transform.eulerAngles,
                Enabled = ball.transform.GetChild(i).GetComponent<Collider2D>().enabled
            });
        }

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Record(new ObjectRecord()
            {
                Id = obj.transform.GetChild(i).gameObject.GetInstanceID(),
                Name = obj.transform.GetChild(i).gameObject.name,
                NumberMove = numberMove,
                Position = obj.transform.GetChild(i).transform.position,
                Rotation = obj.transform.GetChild(i).transform.eulerAngles,
                Enabled = obj.transform.GetChild(i).GetComponent<Collider2D>().enabled
            });
        }

        Record(new ObjectRecord()
        {
            Id = blueWall.gameObject.GetInstanceID(),
            Name = blueWall.gameObject.name,
            NumberMove = numberMove,
            Position = blueWall.transform.position,
            Rotation = blueWall.transform.eulerAngles,
            Enabled = blueWall.GetComponent<Collider2D>().enabled
        });

        Record(new ObjectRecord()
        {
            Id = redWall.gameObject.GetInstanceID(),
            Name = redWall.gameObject.name,
            NumberMove = numberMove,
            Position = redWall.transform.position,
            Rotation = redWall.transform.eulerAngles,
            Enabled = redWall.GetComponent<Collider2D>().enabled
        });

        recorded = true;
    }

    public void UndoRedo(int index)
    {
        List<ObjectRecord> or = listObjectRecord.FindAll(obj => obj.NumberMove == index);

        for(int i = 0; i < or.Count; i++)
        {
            numberMove = index;

            if (gameObject.GetInstanceID() == or[i].Id)
            {
                activeGate = or[i].Enabled;
            }

            for (int j = 0; j < ball.transform.childCount; j++)
            {
                if(ball.transform.GetChild(j).gameObject.GetInstanceID() == or[i].Id)
                {
                    ball.transform.GetChild(j).transform.position = or[i].Position;
                    ball.transform.GetChild(j).gameObject.GetComponent<Movement>().centerPosition = or[i].Position;
                    ball.transform.GetChild(j).gameObject.GetComponent<Collider2D>().enabled = or[i].Enabled;
                    break;
                }
            }

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (obj.transform.GetChild(j).gameObject.GetInstanceID() == or[i].Id)
                {
                    obj.transform.GetChild(j).transform.position = or[i].Position;
                    obj.transform.GetChild(j).transform.eulerAngles = or[i].Rotation;
                    obj.transform.GetChild(j).gameObject.GetComponent<Collider2D>().enabled = or[i].Enabled;
                    break;
                }
            }

            if (blueWall.gameObject.GetInstanceID() == or[i].Id)
            {
                blueWall.gameObject.GetComponent<Collider2D>().enabled = or[i].Enabled;
            }

            if (redWall.gameObject.GetInstanceID() == or[i].Id)
            {
                redWall.gameObject.GetComponent<Collider2D>().enabled = or[i].Enabled;
            }
        }
        
    }

    string TextTime(float t)
    {
        if(t < 60f)
        {
            return Mathf.Round(t).ToString();
        }
        else
        {
            return Mathf.Floor(t / 60f).ToString() + "." + Mathf.Round(t % 60).ToString();
        }
    }

    public void Complete()
    {
        panelMessage.GetComponent<Image>().color = Color.yellow;
        panelMessage.GetComponentInChildren<Text>().text = "Complete";
        panelMessage.GetComponentInChildren<Text>().color = Color.yellow;

        panelMessage.GetComponent<Image>().CrossFadeAlpha(1f, .1f, false);
        panelMessage.GetComponentInChildren<Text>().CrossFadeAlpha(1f, .1f, false);
    }

    public void Failed()
    {
        panelMessage.GetComponent<Image>().color = Color.red;
        panelMessage.GetComponentInChildren<Text>().text = "Failure";
        panelMessage.GetComponentInChildren<Text>().color = Color.red;

        panelMessage.GetComponent<Image>().CrossFadeAlpha(1f, .1f, false);
        panelMessage.GetComponentInChildren<Text>().CrossFadeAlpha(1f, .1f, false);
    }

    public void HideMessage()
    {
        panelMessage.GetComponent<Image>().CrossFadeAlpha(0f, 0f, false);
        panelMessage.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0f, false);
    }
}
