using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

    public float speedFade = 1f;

    private Color currentColor;
    private float alpha = 1f;

    private bool startFadeIn;
    

    void Awake() {
        currentColor = GetComponent<Image>().color;
    }
	
	void Update () {
        if(!startFadeIn)
        {
            if (alpha > 0)
            {
                GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Max(alpha, 0f));
                alpha = alpha - (Time.deltaTime * speedFade);
            }

            if (alpha <= 0) gameObject.SetActive(false);
        }
        else if (startFadeIn)
        {
            gameObject.SetActive(true);

            if (alpha <= 1)
            {
                alpha = alpha + (Time.deltaTime * (speedFade));

                GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Min(alpha, 1f));
                
            }
        }
        
    }

    public void FadeIn()
    {
        startFadeIn = true;
    }
}
