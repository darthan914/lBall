﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBackground : MonoBehaviour {

	void Awake () {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicBackground");
        if (objs.Length > 1) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
	}
}
