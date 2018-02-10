using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour {

    private GameObject main;
    private MainController mc;

    private void Start()
    {
        main = GameObject.FindWithTag("MainCamera");
        mc = main.GetComponent<MainController>();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Undo()
    {
        if(mc.numberMove > 0)
        {
            mc.UndoRedo(mc.numberMove - 1);
            mc.complete = false;
            mc.gameOver = false;
            mc.HideMessage();
        }
    }

    public void Redo()
    {
        List<ObjectRecord> find = mc.listObjectRecord.FindAll(obj => obj.NumberMove > mc.numberMove);
        if (find.Count != 0)
        {
            mc.UndoRedo(mc.numberMove + 1);
            mc.complete = false;
            mc.gameOver = false;
            mc.HideMessage();
        }
    }

    public void Next()
    {
        string thisScenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        string nextScenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);

        Debug.Log(nextScenePath);

        string[] thisPackName = thisScenePath.Split('/');
        string[] nextPackName = nextScenePath != "" ? nextScenePath.Split('/') : null;

        if(nextPackName != null && thisPackName[3] == nextPackName[3])
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene("Assets/Scenes/Level Pack/"+ thisPackName[3] + ".unity");
        }
    }

    public void Menu()
    {
        string thisScenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        string[] thisPackName = thisScenePath.Split('/');

        SceneManager.LoadScene("Assets/Scenes/Level Pack/" + thisPackName[3] + ".unity");
    }
}
