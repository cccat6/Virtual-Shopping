using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseView : MonoBehaviour {
    public string goodid;

    // Use this for initialization
    void Start () {
        if (ControlCenter.inied)
            return;
        if (!string.IsNullOrEmpty(goodid))
        {
            StartCoroutine(ControlCenter.UpdateImg("http://central.holoworld.win/GetHouse.ashx?id=" + goodid, transform.gameObject));
        }
        else
        {
            ControlCenter.ShowMessage("What the heil!");
            Destroy(transform.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
