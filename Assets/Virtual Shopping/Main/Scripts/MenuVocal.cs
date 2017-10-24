using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVocal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        ControlCenter.startListen();
        try { Destroy(GameObject.Find("menu_clone")); } catch { }
    }
}
