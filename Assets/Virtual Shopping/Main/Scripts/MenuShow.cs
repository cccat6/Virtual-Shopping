using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Clicked()
    {
        //ControlCenter.search("cover boy human", false ? GoodSearchProxy.queryOrderBy.price : false ? GoodSearchProxy.queryOrderBy.sales : GoodSearchProxy.queryOrderBy.power, true);
        ControlCenter.menu();
        //ControlCenter.Menu.SetActive(true);
    }
}
