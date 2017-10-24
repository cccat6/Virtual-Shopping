using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenIcon : MonoBehaviour {
    public GameObject inobj, outobj;
    static float listening;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(
            Mathf.Sin((ControlCenter.CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * 1.2f,
            Mathf.Sin((-ControlCenter.CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f > 1.5f ? Mathf.Sin((-ControlCenter.CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f : 1.5f,
            Mathf.Cos((ControlCenter.CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * 1.2f);
        if (Microphone.IsRecording(null))//正在监听
        {
            inobj.transform.localPosition = new Vector3(0f, Mathf.Sin(listening += 0.08f) * 50f, 0f);
            inobj.transform.localRotation = Quaternion.Euler(0f, Mathf.Cos(listening), 0f);
            outobj.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        else if (!Microphone.IsRecording(null))//正在转换
        {
            outobj.transform.localPosition = new Vector3(0f, Mathf.Sin(listening += 0.08f) * 50f, 0f);
            inobj.transform.localRotation = Quaternion.Euler(0f, Mathf.Cos(listening), 0f);
            inobj.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
}
