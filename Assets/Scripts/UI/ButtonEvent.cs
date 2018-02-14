using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour {

    public AudioClip select;

    public AudioClip complete;
    public AudioClip failure;
    public AudioClip go;

    private GameObject main;
    private MainController mc;
    private AudioSource source;
    private FadeInOut fade;

    private void Awake()
    {
        main = GameObject.FindWithTag("MainCamera");
        if(main) mc = main.GetComponent<MainController>();
        source = GetComponent<AudioSource>();

        fade = GetComponent<FadeInOut>();
        fade.fadeSpeed = select.length;
    }

    public void Play()
    {
        
        StartCoroutine(DelayedLoad(1));
    }

    public void Restart()
    {
        fade.BeginFade(1);
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

        fade.BeginFade(1);

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

        fade.BeginFade(1);

        StartCoroutine(DelayedLoad("Assets/Scenes/Level Pack/" + thisPackName[3] + ".unity"));
    }

    void SoundEffect()
    {
        source.PlayOneShot(select);
    }

    public void PlayComplete()
    {
        source.PlayOneShot(complete);
    }

    public void PlayFailure()
    {
        source.PlayOneShot(failure);
    }

    public void PlayGo()
    {
        source.PlayOneShot(go);
    }

    IEnumerator DelayedLoad(int scene)
    {
        //Play the clip once
        source.PlayOneShot(select);

        float fadeTime = fade.BeginFade(1);

        //Wait until clip finish playing
        yield return new WaitForSeconds(fadeTime);

        //Load scene here
        SceneManager.LoadScene(scene);

    }

    IEnumerator DelayedLoad(string scene)
    {
        //Play the clip once
        source.PlayOneShot(select);

        float fadeTime = fade.BeginFade(1);

        //Wait until clip finish playing
        yield return new WaitForSeconds(fadeTime);

        //Load scene here
        SceneManager.LoadScene(scene);
    }
}
