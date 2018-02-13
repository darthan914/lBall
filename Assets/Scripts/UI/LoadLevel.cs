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

    public AudioClip select;
    private AudioSource source;
    private GameObject uiTranslation;

    // Use this for initialization
    void Awake () {

        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);

            if (scenePath.Contains("Assets/Scenes/Level Pack/" + packName))
            {
                string[] pathName = scenePath.Split('/');

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

        source = GetComponent<AudioSource>();

        uiTranslation = GameObject.Find("UI Translation");
    }

    public void LoadLevelByIndex(int index)
    {
        StartCoroutine(DelayedLoad(index));
    }

    public void LoadLevelByPath (string path)
    {
        StartCoroutine(DelayedLoad(path));
    }

    public void StoreLevel(LevelPack levelPack)
    {
        if(!listLevelPack.Contains(levelPack))
        {
            listLevelPack.Add(levelPack);
        }
    }

    IEnumerator DelayedLoad(int scene)
    {
        //Play the clip once
        source.PlayOneShot(select);


        //Wait until clip finish playing
        yield return new WaitForSeconds(select.length);

        //Load scene here
        SceneManager.LoadScene(scene);

    }

    IEnumerator DelayedLoad(string scene)
    {
        //Play the clip once
        source.PlayOneShot(select);

        //Wait until clip finish playing
        yield return new WaitForSeconds(select.length);

        //Load scene here
        SceneManager.LoadScene(scene);

    }
}
