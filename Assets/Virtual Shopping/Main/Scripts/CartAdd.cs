using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartAdd : MonoBehaviour {
    public int addnum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        transform.parent.parent.gameObject.GetComponent<CartControl>().addQ(addnum);
        if (addnum == 0)
            transform.parent.Find("Amount").GetComponent<Text>().text = "0";
        else
            transform.parent.Find("Amount").GetComponent<Text>().text = (int.Parse(transform.parent.Find("Amount").GetComponent<Text>().text) + addnum).ToString();
    }
}
