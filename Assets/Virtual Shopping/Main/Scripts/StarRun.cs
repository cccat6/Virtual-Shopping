using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRun : MonoBehaviour {

    float x, y, z;
    float p,s,xx,yy,yy2,zz;
    // Use this for initialization
    void Start () {
        p = Random.Range(0f, 3.14f);
        s = Random.Range(0.003f, 0.01f);
        xx = Random.Range(30f, 150f);
        zz = Random.Range(30f, 150f);
        yy = Random.Range(0f, 3.14f);
        yy2 = Random.Range(8f, 200f);
    }
	// Update is called once per frame
	void Update () {
        p -= s;
        x = Mathf.Sin(p) * xx;
        y = Mathf.Sin(p + yy) * 3f + yy2;
        z = Mathf.Cos(p) * zz;
        transform.position = new Vector3(x, y, z);
        transform.rotation = Quaternion.Euler(0f, p / 3.14f * 180f, 0f);
	}
}
