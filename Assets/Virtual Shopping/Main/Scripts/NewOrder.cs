using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NewOrder : MonoBehaviour {
    private GameObject showpic, goodname, price, quantity, address, add1, add2, add3;
    public string goodid, goodnames, quanlities, prices, model;//new这个类的时候一定要填写这几个参数，在克隆还没有可用的时候就要填写
    private string[] addresses = new string[0];
    private bool needLoadAddress = true;
    private int page = 0;
    private bool canNextPage = false, canLastPage = false;
    private string result = null;
    private bool sending = false;
    private GameObject msgbox;

	// Use this for initialization
	void Start () {
        if (ControlCenter.inied)
            return;
        string id = ControlCenter.GetString("id");
        if (id == "-1" || string.IsNullOrEmpty(id))
        {
            ControlCenter.ShowMessage(Language.lang.unlogin);
            Destroy(transform.gameObject);
            return;
        }

        showpic = transform.Find("Panel").Find("Item").Find("Show").gameObject;
        goodname = transform.Find("Panel").Find("Item").Find("Name").gameObject;
        price = transform.Find("Panel").Find("Item").Find("Price").gameObject;
        quantity = transform.Find("Panel").Find("Item").Find("Amount").gameObject;
        address = transform.Find("Panel").Find("Address").gameObject;
        add1 = transform.Find("Panel").Find("Panel").Find("Address1").gameObject;
        add2 = transform.Find("Panel").Find("Panel").Find("Address2").gameObject;
        add3 = transform.Find("Panel").Find("Panel").Find("Address3").gameObject;

        StartCoroutine(ControlCenter.UpdateImg("http://model.holoworld.win/GetModel.ashx?type=0&id=" + goodid, showpic));
        goodname.GetComponent<Text>().text = goodnames;
        price.GetComponent<Text>().text = prices;
        quantity.GetComponent<Text>().text = quanlities;
        address.GetComponent<Text>().text = Language.lang.needselectaddress;
        StartCoroutine(loadAddress());
        add1.SetActive(false);
        add2.SetActive(false);
        add3.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if(needLoadAddress)
        {
            try
            {
                add2.SetActive(false);
                add3.SetActive(false);
                add1.transform.GetChild(0).gameObject.GetComponent<Text>().text = addresses[3 * page];
                add1.SetActive(true);
                add2.transform.GetChild(0).gameObject.GetComponent<Text>().text = addresses[3 * page + 1];
                add2.SetActive(true);
                add3.transform.GetChild(0).gameObject.GetComponent<Text>().text = addresses[3 * page + 2];
                add3.SetActive(true);
                if (page != 0)
                    canLastPage = true;
            }
            catch { canLastPage = false; }
            needLoadAddress = false;
        }
        if (result != null && sending)
        {
            if (result != "0" && result != "-1")
            {
                Destroy(msgbox);
                ControlCenter.pay(result);
                Destroy(transform.gameObject);
            }
            else
            {
                msgbox.GetComponent<Text>().text = result;
                Destroy(transform.gameObject);
            }
        }
	}
    
    public void sendNew()
    {
        if(address.GetComponent<Text>().text == Language.lang.needselectaddress)
        {
            ControlCenter.ShowMessage(Language.lang.needselectaddress);
            return;
        }
        if (sending)
            return;
        string url = "http://central.holoworld.win/NewOrder.ashx?langNum=" + Language.lang.langNum + "&id=" + ControlCenter.GetString("id") + "&good=" + goodid + "&quantity=" + quanlities + "&model=" + UrlEncode(model) + "&address=" + UrlEncode(address.GetComponent<Text>().text);
        StartCoroutine(postOrder(url));
        sending = true;
        msgbox = ControlCenter.ShowMessage(Language.lang.creatingorder);
    }
    IEnumerator postOrder(string url)
    {
        WWW postData = new WWW(url, new byte[] { 0 });
        string er = postData.error;
        while (!postData.isDone)
            yield return new WaitForSeconds(0.1f);
        if (!string.IsNullOrEmpty(er))
            Debug.LogError(er);
        result = postData.text;
    }
    IEnumerator loadAddress()
    {
        Debug.Log("http://central.holoworld.win/ReceiveLocation.ashx?langNum=" + Language.lang.langNum + "&id=" + ControlCenter.GetString("id"));
        WWW www = new WWW("http://central.holoworld.win/ReceiveLocation.ashx?langNum="+Language.lang.langNum+"&id=" + ControlCenter.GetString("id"));
        yield return www;
        string result = www.text;
        //["北京市北京市崇文区建平镇321 蒋亦涛 15656162236","天津市天津市河西区建平镇2321 蒋亦稻民 16578302032","天津市天津市河北区建平镇231 蒋一涛 18840302034"]
        if (result.Contains("exsist"))
        {
            ControlCenter.ShowMessage(Language.lang.usernotexist);
            Destroy(transform.gameObject);
        }
        if (result.Length < 10)
        {
            ControlCenter.ShowMessage(Language.lang.havenotaddress);
            Destroy(transform.gameObject);
        }
        addresses = result.Remove(result.Length - 1, 1).Remove(0, 1).Replace("\"", "").Split(',');
        foreach(string now in addresses)
            Debug.Log(now);
        if (addresses.Length < 3)
            canNextPage = false;
        needLoadAddress = true;
    }
    public void up()
    {
        if (canLastPage)
        {
            page--;
            needLoadAddress = true;
        }
    }
    public void down()
    {
        if (canLastPage)
        {
            page++;
            needLoadAddress = true;
        }
    }

    public static string UrlEncode(string str)
    {
        StringBuilder sb = new StringBuilder();
        byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
        for (int i = 0; i < byStr.Length; i++)
        {
            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
        }

        return (sb.ToString());
    }
}
