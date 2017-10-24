using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressUpClicked : MonoBehaviour {

   

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Clicked()
    {
       
        RefreshExpress.manager.UpScroll();
        Debug.Log("ExpressUpClicked");
    }
}
