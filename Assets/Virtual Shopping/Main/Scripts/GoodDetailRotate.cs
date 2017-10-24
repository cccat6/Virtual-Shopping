using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodDetailRotate : MonoBehaviour {//商品详细信息那四个旋转按钮的控制
    public int mode;//1是左，2是右，3是上，4是下

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Select()
    {
        transform.parent.parent.GetComponent<GoodDetail>().rotate(mode);
    }
}
