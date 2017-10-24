using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewOrderUpDown : MonoBehaviour {
    public bool updown;//true为上，false为下
    public GameObject bigparent;//新建订单主类所在的游戏物体
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Clicked()
    {
        if (updown)
            bigparent.GetComponent<NewOrder>().up();
        else
            bigparent.GetComponent<NewOrder>().down();
    }
}
