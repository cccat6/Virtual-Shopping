using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour {
    
    //public static string selectedName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Clicked()
    {
        Debug.Log(transform.parent.gameObject);
        ReceiveHand.selected = transform.parent.gameObject;
        /*if (ReceiveHand.selected = transform.parent.gameObject)
        {
            ReceiveHand.selected = null;
            //selectedName = null;
        }
        else
        {
            ReceiveHand.selected = transform.parent.gameObject;
            //selectedName = selected.name;
        }*/
    }

    public static void SetTo(Vector3 position, float rx, float ry)
    {
        ReceiveHand.selected.transform.position = position;
        ReceiveHand.selected.transform.rotation = Quaternion.Euler(rx, ry, 0);//rx俯仰，ry面向
    }
}
