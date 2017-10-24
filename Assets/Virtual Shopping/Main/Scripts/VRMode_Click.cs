using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMode_Click : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Clicked()
    {
        GameObject.Find("GvrViewerMain").GetComponent<GvrViewer>().ChangeVRMode();
    }
}
