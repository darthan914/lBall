using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSoundEffect : MonoBehaviour {

    public AudioClip collide;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Movement>())
        {
            source.PlayOneShot(collide);
        }
    }
}
