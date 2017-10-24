using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalInfoTodo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        string statusWord = transform.GetChild(0).gameObject.GetComponent<Text>().text;
        Debug.Log(statusWord + "," + transform.parent.Find("orderid").GetComponent<Text>().text);
        transform.parent.parent.parent.parent.gameObject.GetComponent<GetPersonInfo>().todo(
            statusWord == "Pay" ? 1 :
            statusWord == "Cancel" ? 2 :
            statusWord == "Sign" ? 3 :
            -1, transform.parent.Find("orderid").gameObject.GetComponent<Text>().text);
    }
}
