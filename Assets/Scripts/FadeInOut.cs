using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public Color colorTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    private void OnGUI()
    {
        alpha += fadeDir * (Time.deltaTime / fadeSpeed) ;

        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(colorTexture.r, colorTexture.g, colorTexture.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return ( fadeSpeed );
    }

    private void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }
}
