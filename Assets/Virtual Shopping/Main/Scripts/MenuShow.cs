﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Clicked()
    {
        ControlCenter.Menu.SetActive(true);
    }
}
