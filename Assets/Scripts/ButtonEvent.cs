using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour {

    public AudioClip select;

    private GameObject main;
    private MainController mc;
    private AudioSource source;
    private GameObject uiTranslation;

    private void Awake()
    {
        main = GameObject.FindWithTag("MainCamera");
        if(main) mc = main.GetComponent<MainController>();
        source = GetComponent<AudioSource>();

        uiTranslation = GameObject.Find("UI Translation");
    }

    public void Play()
    {
        if (uiTranslation) uiTranslation.GetComponent<FadeInOut>().FadeIn();
        StartCoroutine(DelayedLoad(1));
    }

    public void Restart()
    {
        if (uiTranslation) uiTranslation.GetComponent<FadeInOut>().FadeIn();
        StartCoroutine(DelayedLoad(SceneManager.GetActiveScene().buildIndex));
    }

    public void Undo()
    {
        if(mc.GetNumberMove() > 0)
        {
            mc.UndoRedo(mc.GetNumberMove() - 1);
            mc.complete = false;
            mc.gameOver = false;
            mc.HideMessage();
            SoundEffect();
        }
    }

    public void Redo()
    {
        if (mc.GetNumberMove() < mc.GetMaxNumberMove())
        {
            mc.UndoRedo(mc.GetNumberMove() + 1);
            mc.complete = false;
            mc.gameOver = false;
            mc.HideMessage();
            SoundEffect();
        }
    }

    public void Next()
    {
        string thisScenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        string nextScenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);

        string[] thisPackName = thisScenePath.Split('/');
        string[] nextPackName = nextScenePath != "" ? nextScenePath.Split('/') : null;

        if (uiTranslation) uiTranslation.GetComponent<FadeInOut>().FadeIn();

        if (nextPackName != null && thisPackName[3] == nextPackName[3])
        {
            StartCoroutine(DelayedLoad(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else
        {
            StartCoroutine(DelayedLoad("Assets/Scenes/Level Pack/"+ thisPackName[3] + ".unity"));
        }
    }

    public void Menu()
    {
        string thisScenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        string[] thisPackName = thisScenePath.Split('/');

        if (uiTranslation) uiTranslation.GetComponent<FadeInOut>().FadeIn();

        StartCoroutine(DelayedLoad("Assets/Scenes/Level Pack/" + thisPackName[3] + ".unity"));
    }

    void SoundEffect()
    {
        source.PlayOneShot(select);
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
