using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAddress : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        transform.parent.parent.Find("Model").GetComponent<Text>().text = transform.GetChild(0).GetComponent<Text>().text;
    }
}
