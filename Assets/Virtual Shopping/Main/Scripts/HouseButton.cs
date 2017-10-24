using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseButton : MonoBehaviour {
    public int function;//1全景，2购买，3购物车，4朗读

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        if (function == 1)
            transform.parent.GetComponent<HouseDetail>().view();
        else if (function == 2)
            transform.parent.GetComponent<HouseDetail>().buyThis();
        else if (function == 3)
            transform.parent.GetComponent<HouseDetail>().addCart();
        else if (function == 4)
            transform.parent.GetComponent<HouseDetail>().read();
    }
}
