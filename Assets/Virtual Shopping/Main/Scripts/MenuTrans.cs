﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrans : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Clicked()
    {
        ControlCenter.Trans.SetActive(true);
        //System.Threading.Thread.Sleep(500);
        ControlCenter.Menu.SetActive(false);
    }
}