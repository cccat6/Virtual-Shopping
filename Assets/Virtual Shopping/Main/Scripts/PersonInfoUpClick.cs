using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonInfoUpClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Clicked()
    {
        //int a = 0;
        //transform.GetChild(0).GetComponent<Text>().text = a.ToString();
        RefreshPersonInfo.refreshPersonalInfo.UpScroll();
        Debug.Log("PersonInfoUpClick");
    }
}
