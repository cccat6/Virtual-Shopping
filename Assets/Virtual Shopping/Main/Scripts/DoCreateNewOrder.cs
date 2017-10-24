using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCreateNewOrder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        GameObject bigparent = transform.parent.parent.gameObject;
        bigparent.GetComponent<NewOrder>().sendNew();
    }
}
