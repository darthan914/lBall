using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {

    public bool root;
    public string packName;
    public Transform targetList;

    public Button button;
    private List<LevelPack> listLevelPack = new List<LevelPack>();

	// Use this for initialization
	void Start () {

        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);

            if (scenePath.Contains("Assets/Scenes/Level Pack/" + packName))
            {
                string[] pathName = scenePath.Split('/');

                Debug.Log(scenePath);

                if(pathName.Length == 4)
                {
                    StoreLevel(new LevelPack
                    {
                        Index = sceneIndex,
                        Name = pathName[3].Replace(".unity", ""),
                        Path = scenePath,
                        Type = "Pack"
                    });
                }
                else if (pathName.Length == 5)
                {
                    StoreLevel(new LevelPack
                    {
                        Index = sceneIndex,
                        Name = pathName[4].Replace(".unity", ""),
                        Path = scenePath,
                        Type = "Level"
                    });
                }
               
            }
        }

        Button currentButton = Instantiate(button);
        currentButton.transform.SetParent(targetList, false);

        if(root)
        {
            currentButton.GetComponent<Button>().onClick.AddListener(() => LoadLevelByIndex(0));
        }
        else
        {
            currentButton.GetComponent<Button>().onClick.AddListener(() => LoadLevelByIndex(1));
        }
        
        currentButton.GetComponentInChildren<Text>().text = "Back";


        for (int i = 0; i < listLevelPack.Count; i++)
        {

            int index = listLevelPack[i].Index;

            if (root && listLevelPack[i].Type == "Pack")
            { 
                currentButton = Instantiate(button);
                currentButton.transform.SetParent(targetList, false);

                currentButton.GetComponent<Button>().onClick.AddListener(() => LoadLevelByIndex(index));
                currentButton.GetComponentInChildren<Text>().text = listLevelPack[i].Name;
            }
            else if (!root && listLevelPack[i].Type == "Level")
            {
                currentButton = Instantiate(button);
                currentButton.transform.SetParent(targetList, false);

                currentButton.GetComponent<Button>().onClick.AddListener(() => LoadLevelByIndex(index));
                currentButton.GetComponentInChildren<Text>().text = listLevelPack[i].Name;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevelByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadLevelByPath (string path)
    {
        SceneManager.LoadScene(path);
    }

    public void StoreLevel(LevelPack levelPack)
    {
        if(!listLevelPack.Contains(levelPack))
        {
            listLevelPack.Add(levelPack);
        }
    }
}
