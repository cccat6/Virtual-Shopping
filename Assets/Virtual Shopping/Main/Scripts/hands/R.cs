using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hands;

public class R : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = new Vector3(position.Rx, position.Ry, position.Rz);
    }
}
