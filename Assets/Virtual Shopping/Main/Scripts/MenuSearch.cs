using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSearch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        ControlCenter.startListen();
        //ControlCenter.Voice.SetActive(true);
        //System.Threading.Thread.Sleep(500);
        ControlCenter.Menu.SetActive(false);
    }
}
