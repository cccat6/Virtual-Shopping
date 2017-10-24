using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Key : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
        Clicked();
    }

    public void Clicked()
    {
        transform.parent.parent.parent.gameObject.GetComponent<PaymentControl>().addWord(transform.Find("Text").gameObject.GetComponent<Text>().text);
    }
}
