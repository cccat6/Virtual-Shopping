using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class K26: MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        Clicked();
    }

    public void Clicked()
    {
        if (PaymentControl.shifton || (GameObject.Find("Payment Canvas/Panel/Keyboard/#26/Text").GetComponent<Text>().text[0] >= '0') && GameObject.Find("Payment Canvas/Panel/Keyboard/Backspace/Text").GetComponent<Text>().text[0] <= '9')
        {
            PaymentControl.paypassword += GameObject.Find("Payment Canvas/Panel/Keyboard/#26/Text").GetComponent<Text>().text;
            PaymentControl.switchshift();
        }
        else
        {
            PaymentControl.paypassword += GameObject.Find("Payment Canvas/Panel/Keyboard/#26/Text").GetComponent<Text>().text.ToLower();
        }
    }
}
