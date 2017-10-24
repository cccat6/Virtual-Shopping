using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Select()
    {
        ControlCenter.canCircle = false;
        return;
    }
}
