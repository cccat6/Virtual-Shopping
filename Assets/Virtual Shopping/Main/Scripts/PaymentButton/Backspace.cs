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
        if(PaymentControl.paypassword.Length>0)
        {
            string newpass = "";
            //PaymentControl.paypassword.Remove(PaymentControl.paypassword.Length-1);
            for(int i=0;i< PaymentControl.paypassword.Length - 1; i++)
            {
                newpass += PaymentControl.paypassword[i];
            }
            PaymentControl.paypassword = newpass;
        }
    }
}
