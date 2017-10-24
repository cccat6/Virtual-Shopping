using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelDestory : MonoBehaviour {
    private GameObject bigparent;
    public int parent;//在多少层级以上是父对象

	// Use this for initialization
	void Start () {
        bigparent = transform.gameObject;
        for (int i = 0; i < parent; i++)
            bigparent = bigparent.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        Destroy(bigparent);
    }
}
