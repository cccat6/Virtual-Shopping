using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodSearchFloat : MonoBehaviour {//用来构成搜索屏幕漂浮效果的类

    private float process = 0f;

	// Use this for initialization
	void Start () {
        if (ControlCenter.inied)
            return;
        process = Random.Range(0f, 3.14f);
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.parent.gameObject.GetComponent<GoodSearchDetail>().pauseAngle != 0f)
            return;
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Sin(process) * 0.1f + 1f,//上下幅度
            transform.position.z);
        transform.rotation = Quaternion.Euler(
            (Mathf.Sin(process*2f)+Mathf.Cos(process*2f)) * 2f,//晃动幅度
            transform.rotation.eulerAngles.y, 
            0f);
        process += 0.03f;
    }

    public void Clicked()
    {
        transform.parent.gameObject.GetComponent<GoodSearchDetail>().selected(transform.Find("goodid").GetComponent<Text>().text,
            transform.Find("isScene").GetComponent<Text>().text=="1");
    }
}
