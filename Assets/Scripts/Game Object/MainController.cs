using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

	public GameObject ball;
    public GameObject obj;
    public GameObject blueObject;
    public GameObject redObject;
    public GameObject blueWall;
    public GameObject redWall;
    public GameObject canvasUI;

    private GameObject panelButton;
    private GameObject panelStatus;
    private GameObject panelMessage;

    private Text textMove;
    private Text textTime;

    private Button buttonUndo;
    private Button buttonRedo;
    private Button buttonNext;

    private float time;
    private float delay;

    [HideInInspector] public int countBall;
    [HideInInspector] public bool allowMove;
    [HideInInspector] public bool start = false;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public bool complete = false;
    [HideInInspector] public bool activeGate = false;

    private int numberMove;
    private int maxNumberMove;
    private List<ObjectRecord> listObjectRecord = new List<ObjectRecord>();
    private bool recorded;
    
    private int temp1, temp2;
    private GameObject temp1go, temp2go;


    // Use this for initialization
    void Start () {
        FillGameObject();

        canvasUI   = canvasUI ? canvasUI : GameObject.Find("Canvas");

        countBall = Mathf.Min(ball.transform.childCount, 6);
        
        // Create UI in game
        if (canvasUI)
        {
            GameObject instantiate = Instantiate(canvasUI);
            instantiate.name = "Canvas";

            panelButton  = instantiate.transform.Find("Button Panel").gameObject;
            panelStatus  = instantiate.transform.Find("Status Panel").gameObject;
            panelMessage = instantiate.transform.Find("Message Panel").gameObject;

            buttonRedo = panelButton.transform.Find("Redo").GetComponent<Button>();
            buttonUndo = panelButton.transform.Find("Undo").GetComponent<Button>();
            buttonNext = panelButton.transform.Find("Next").GetComponent<Button>();

            textMove = panelStatus.transform.Find("Move").GetComponent<Text>();
            textTime = panelStatus.transform.Find("Time").GetComponent<Text>();

            panelMessage.GetComponent<Image>().color = Color.green;
            panelMessage.GetComponentInChildren<Text>().text = "GO";
            panelMessage.GetComponentInChildren<Text>().color = Color.green;

            panelMessage.GetComponent<Image>().CrossFadeAlpha(0f, 2.0f, false);
            panelMessage.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 2.0f, false);
        }

        AutomatedTeleport(obj);
        AutomatedTeleport(blueObject);
        AutomatedTeleport(redObject);

        buttonUndo.interactable = buttonRedo.interactable = false;

        SaveCurrentMove();
    }

	// Update is called once per frame
	void Update () {
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

        textMove.text = "Move : " + (numberMove);
        textTime.text = "Time : " + TextTime(time);

        for (int i = 0; i < countBall; i++)
		{
			if (ball.transform.GetChild(i).GetComponent<Movement> () && ball.transform.GetChild(i).GetComponent<Movement> ().isMove) {
				allowMove = false;
                recorded = false;
            }
		}

        if(allowMove && !recorded)
        {
            listObjectRecord.RemoveAll(obj => obj.NumberMove > numberMove);
            numberMove++;
            maxNumberMove = numberMove;
            SaveCurrentMove();
            buttonUndo.interactable = true;
            buttonRedo.interactable = false;
        }
	}

    public void FillGameObject()
    {
        ball       = ball ? ball : GameObject.Find("Ball");
        obj        = obj ? obj : GameObject.Find("Object");
        blueObject = blueObject ? blueObject : GameObject.Find("Blue Object");
        redObject  = redObject ? redObject : GameObject.Find("Red Object");
        blueWall   = blueWall ? blueWall : GameObject.Find("Blue Wall");
        redWall    = redWall ? redWall : GameObject.Find("Red Wall");
    }

    public void UndoRedo(int index)
    {
        List<ObjectRecord> list = listObjectRecord.FindAll(obj => obj.NumberMove == index);

        for(int i = 0; i < list.Count; i++)
        {
            numberMove = index;

            if (gameObject.GetInstanceID() == list[i].Id)
            {
                activeGate = list[i].Enabled;
            }

            for (int j = 0; j < ball.transform.childCount; j++)
            {
                if(ball.transform.GetChild(j).gameObject.GetInstanceID() == list[i].Id)
                {
                    ball.transform.GetChild(j).transform.position = list[i].Position;
                    ball.transform.GetChild(j).gameObject.GetComponent<Movement>().centerPosition = list[i].Position;
                    ball.transform.GetChild(j).gameObject.GetComponent<Collider2D>().enabled = list[i].Enabled;
                    break;
                }
            }

            for (int j = 0; j < obj.transform.childCount; j++)
            {
                if (obj.transform.GetChild(j).gameObject.GetInstanceID() == list[i].Id)
                {
                    obj.transform.GetChild(j).transform.position = list[i].Position;
                    obj.transform.GetChild(j).transform.eulerAngles = list[i].Rotation;
                    obj.transform.GetChild(j).gameObject.GetComponent<Collider2D>().enabled = list[i].Enabled;
                    break;
                }
            }

            for (int j = 0; j < blueObject.transform.childCount; j++)
            {
                if (blueObject.transform.GetChild(j).gameObject.GetInstanceID() == list[i].Id)
                {
                    blueObject.transform.GetChild(j).transform.position = list[i].Position;
                    blueObject.transform.GetChild(j).transform.eulerAngles = list[i].Rotation;
                    blueObject.transform.GetChild(j).gameObject.GetComponent<Collider2D>().enabled = list[i].Enabled;
                    break;
                }
            }

            for (int j = 0; j < redObject.transform.childCount; j++)
            {
                if (redObject.transform.GetChild(j).gameObject.GetInstanceID() == list[i].Id)
                {
                    redObject.transform.GetChild(j).transform.position = list[i].Position;
                    redObject.transform.GetChild(j).transform.eulerAngles = list[i].Rotation;
                    redObject.transform.GetChild(j).gameObject.GetComponent<Collider2D>().enabled = list[i].Enabled;
                    break;
                }
            }

            if (blueWall.gameObject.GetInstanceID() == list[i].Id)
            {
                blueWall.gameObject.GetComponent<Collider2D>().enabled = list[i].Enabled;
            }

            if (redWall.gameObject.GetInstanceID() == list[i].Id)
            {
                redWall.gameObject.GetComponent<Collider2D>().enabled = list[i].Enabled;
            }
        }

        if(numberMove > 0) buttonUndo.interactable = true;
        else buttonUndo.interactable = false;

        if (numberMove < maxNumberMove) buttonRedo.interactable = true;
        else buttonRedo.interactable = false;


    }

    public void TriggerKey()
    {
        if (blueObject != null)
        {
            for (int i = 0; i < blueObject.transform.childCount; i++)
            {
                Color currentColor = blueObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color;
                blueObject.transform.GetChild(i).GetComponent<Collider2D>().enabled = !activeGate;
                blueObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, TransparencyActive(!activeGate));
            }
        }

        if (blueWall != null)
        {
            Color currentColor = blueWall.gameObject.GetComponent<Tilemap>().color;
            blueWall.gameObject.GetComponent<Collider2D>().enabled = !activeGate;
            blueWall.gameObject.GetComponent<Tilemap>().color = new Color(currentColor.r, currentColor.g, currentColor.b, TransparencyActive(!activeGate));
        }

        if (redObject != null)
        {
            for (int i = 0; i < redObject.transform.childCount; i++)
            {
                Color currentColor = redObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color;
                redObject.transform.GetChild(i).GetComponent<Collider2D>().enabled = activeGate;
                redObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, TransparencyActive(activeGate));
            }
        }

        if (redWall != null)
        {
            Color currentColor = redWall.gameObject.GetComponent<Tilemap>().color;
            redWall.gameObject.GetComponent<Collider2D>().enabled = activeGate;
            redWall.gameObject.GetComponent<Tilemap>().color = new Color(currentColor.r, currentColor.g, currentColor.b, TransparencyActive(activeGate));
        }

        activeGate = !activeGate;
    }

    public void Complete()
    {
        panelMessage.GetComponent<Image>().color = Color.yellow;
        panelMessage.GetComponentInChildren<Text>().text = "Complete";
        panelMessage.GetComponentInChildren<Text>().color = Color.yellow;

        panelMessage.GetComponent<Image>().CrossFadeAlpha(1f, .1f, false);
        panelMessage.GetComponentInChildren<Text>().CrossFadeAlpha(1f, .1f, false);

        buttonUndo.interactable = false;
        buttonRedo.interactable = false;
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

    public void SetInteractableUndo(bool interactable)
    {
        buttonUndo.interactable = interactable;
    }

    public void SetInteractableRedo(bool interactable)
    {
        buttonRedo.interactable = interactable;
    }

    public int GetNumberMove()
    {
        return numberMove;
    }

    public int GetMaxNumberMove()
    {
        return maxNumberMove;
    }

    void Record(ObjectRecord objectRecord)
    {
        if(!gameOver) listObjectRecord.Add(objectRecord);
    }

    void SaveCurrentMove()
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

        for (int i = 0; i < blueObject.transform.childCount; i++)
        {
            Record(new ObjectRecord()
            {
                Id = blueObject.transform.GetChild(i).gameObject.GetInstanceID(),
                Name = blueObject.transform.GetChild(i).gameObject.name,
                NumberMove = numberMove,
                Position = blueObject.transform.GetChild(i).transform.position,
                Rotation = blueObject.transform.GetChild(i).transform.eulerAngles,
                Enabled = blueObject.transform.GetChild(i).GetComponent<Collider2D>().enabled
            });
        }

        for (int i = 0; i < redObject.transform.childCount; i++)
        {
            Record(new ObjectRecord()
            {
                Id = redObject.transform.GetChild(i).gameObject.GetInstanceID(),
                Name = redObject.transform.GetChild(i).gameObject.name,
                NumberMove = numberMove,
                Position = redObject.transform.GetChild(i).transform.position,
                Rotation = redObject.transform.GetChild(i).transform.eulerAngles,
                Enabled = redObject.transform.GetChild(i).GetComponent<Collider2D>().enabled
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

    void AutomatedTeleport(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject currentObject = obj.transform.GetChild(i).gameObject;
            if (currentObject.tag == "Teleport")
            {
                if (currentObject.GetComponent<Teleport>() && currentObject.GetComponent<Teleport>().teleportLocation == null)
                {
                    if (temp1go)
                    {
                        temp2 = i;
                        temp2go = currentObject;
                    }
                    else
                    {
                        temp1 = i;
                        temp1go = currentObject;
                    }

                }

                if (temp1go && temp2go)
                {
                    obj.transform.GetChild(temp1).GetComponent<Teleport>().teleportLocation = temp2go;
                    obj.transform.GetChild(temp2).GetComponent<Teleport>().teleportLocation = temp1go;
                    temp1 = temp2 = 0;
                    temp1go = temp2go = null;
                }
            }
        }

        if (temp1go || temp2go)
        {
            if (temp1go) obj.transform.GetChild(temp1).gameObject.SetActive(false);
            if (temp2go) obj.transform.GetChild(temp2).gameObject.SetActive(false);
            temp1 = temp2 = 0;
            temp1go = temp2go = null;
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
            return .5f;
        }
    }

    string TextTime(float t)
    {
        if (t < 60f)
        {
            return Mathf.Round(t).ToString();
        }
        else
        {
            return Mathf.Floor(t / 60f).ToString() + "." + Mathf.Round(t % 60).ToString();
        }
    }
}
