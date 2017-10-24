using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Backspace : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Clicked()
    {
        transform.parent.parent.parent.gameObject.GetComponent<PaymentControl>().backspace();
    }
}
