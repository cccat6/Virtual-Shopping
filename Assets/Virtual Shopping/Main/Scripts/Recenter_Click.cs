using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recenter_Click : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Clicked()
    {
        GvrViewer todo = new GvrViewer();
        todo.Recenter();
    }
}
