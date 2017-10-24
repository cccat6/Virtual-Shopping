using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartUpDown : MonoBehaviour {

    public int UpDown;//0上，1下

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        if (UpDown == 0)
            transform.parent.gameObject.GetComponent<CartControl>().up();
        if (UpDown == 1)
            transform.parent.gameObject.GetComponent<CartControl>().down();
    }
}
