using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Clicked()
    {
        ControlCenter.personal();
        try { Destroy(GameObject.Find("menu_clone")); } catch { }
    }
}
