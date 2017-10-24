using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseDetail : MonoBehaviour {
    public string goodid;
    private string name = "null", price = "null", info = "null";
    private bool reflush = false;

    // Use this for initialization
    void Start (){
        if (ControlCenter.inied)
            return;
        StartCoroutine(getDetail(goodid));
    }
	
	// Update is called once per frame
	void Update () {
        if (reflush)
        {
            reflush = false;
            transform.Find("Detail Panel").Find("Goodname").GetComponent<Text>().text = name;
            transform.Find("Detail Panel").Find("Price").GetComponent<Text>().text = price;
            transform.Find("Detail Panel").Find("Goodinfo").GetComponent<Text>().text = info;
        }
    }

    public IEnumerator getDetail(string id)
    {
        WWW www = new WWW("http://central.holoworld.win/GoodDetail.ashx?langNum=" + Language.lang.langNum + "&id=" + id);
        while (!www.isDone)
        {
            yield return 1;
        }
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            string[] result = www.text.Split('翐');
            name = result[0];
            price = "$" + result[1];
            info = result[2];
            reflush = true;
        }
    }

    public void view()
    {
        ControlCenter.house(goodid);
    }
    public void buyThis()
    {
        ControlCenter.confirmModel(goodid, name, "1", price);//这里有点小bug，购买数量不能设置，只能为1
        try { Destroy(GameObject.Find("search_clone")); } catch { }
        Destroy(transform.gameObject);
    }
    public void addCart()
    {
        WWW postData = new WWW("http://central.holoworld.win/AddCart.ashx?id=" + ControlCenter.GetString("id") + "&good=" + goodid + "&quantity=1", new byte[] { 0 });
        Destroy(transform.gameObject);
    }
    public void read()
    {
        SoundsControl.playText(info);
    }
}
