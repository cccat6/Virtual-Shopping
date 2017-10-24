using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class ExpressDownClicked : MonoBehaviour {
  
    void Start () {
              
	}
	
	// Update is called once per frame
	void Update () {
		
	}    
    public void Clicked()
    {
        RefreshExpress.manager.DownScroll();
        Debug.Log("ExpressDownClicked");
    }
}
