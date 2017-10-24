using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Clicked()
    {
        transform.parent.gameObject.GetComponent<GoodDetail>().readInfo();
    }
}
