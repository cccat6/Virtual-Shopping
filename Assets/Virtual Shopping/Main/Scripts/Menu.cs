using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject b1, b2, b3, b4;
    float y = -0.3f;
    float p1, p2, p3, p4;
	// Use this for initialization
	void Start () {
        if (ControlCenter.inied)
            return;
        p1 = Random.Range(0f, 3.14f);
        p2 = Random.Range(0f, 3.14f);
        p3 = Random.Range(0f, 3.14f);
        p4 = Random.Range(0f, 3.14f);
        transform.position = new Vector3(transform.position.x, -0.2f, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (y < 1f)
            transform.position = new Vector3(transform.position.x, y += 0.05f, transform.position.z);

        b1.transform.localPosition = new Vector3(
            b1.transform.localPosition.x,
            Mathf.Sin(p1) * 10f + 55f,//上下幅度
            b1.transform.localPosition.z);
        b1.transform.localRotation = Quaternion.Euler(
            (Mathf.Sin(p1) + Mathf.Cos(p1 * 2f)),
            b1.transform.localRotation.eulerAngles.y,
            b1.transform.localRotation.eulerAngles.z);
        p1 += 0.03f;

        b2.transform.localPosition = new Vector3(
            b2.transform.localPosition.x,
            Mathf.Sin(p2) * 10f + 55f,//上下幅度
            b2.transform.localPosition.z);
        b2.transform.localRotation = Quaternion.Euler(
            (Mathf.Sin(p2) + Mathf.Cos(p2 * 2f)),
            b2.transform.localRotation.eulerAngles.y,
            b2.transform.localRotation.eulerAngles.z);
        p2 += 0.03f;

        b3.transform.localPosition = new Vector3(
            b3.transform.localPosition.x,
            Mathf.Sin(p3) * 10f + 55f,//上下幅度
            b3.transform.localPosition.z);
        b3.transform.localRotation = Quaternion.Euler(
            (Mathf.Sin(p3) + Mathf.Cos(p3 * 2f)),
            b3.transform.localRotation.eulerAngles.y,
            b3.transform.localRotation.eulerAngles.z);
        p3 += 0.03f;

        b4.transform.localPosition = new Vector3(
            b4.transform.localPosition.x,
            Mathf.Sin(p4) * 10f + 55f,//上下幅度
            b4.transform.localPosition.z);
        b4.transform.localRotation = Quaternion.Euler(
            (Mathf.Sin(p3) + Mathf.Cos(p4 * 2f)),
            b4.transform.localRotation.eulerAngles.y,
            b4.transform.localRotation.eulerAngles.z);
        p4 += 0.03f;
    }
}
