using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Gvr.Internal;

public class PaymentControl : MonoBehaviour {
    private static string paypassword = null;
    private Image image;
    private string price = null;
    private bool needInfo = true;
    private string buyerid = null;
    private bool paying = false;
    private string result = null;
    private GameObject msgbox;
    
    List<GameObject> K = new List<GameObject>();
    void GetKList()
    {
        K.Add(transform.Find("Panel/Keyboard/#0").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#1").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#2").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#3").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#4").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#5").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#6").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#7").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#8").gameObject);
        K.Add(transform.Find("Panel/Keyboard/#9").gameObject);
    }
    void SetKey(GameObject gameobject,int KeyNum)
    {
        switch (KeyNum)
        {
            case 1:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "1";
                break;
            case 2:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "2";
                break;
            case 3:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "3";
                break;
            case 4:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "4";
                break;
            case 5:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "5";
                break;
            case 6:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "6";
                break;
            case 7:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "7";
                break;
            case 8:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "8";
                break;
            case 9:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "9";
                break;
            case 10:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "0";
                break;
            default:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "#";
                break;
        }
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(getOrder(transform.Find("Panel").Find("Orderid").gameObject.GetComponent<Text>().text));
        GetKList();
        List<int> list = new List<int>();
        System.Random rm = new System.Random();
        while (list.Count < 10)
        {
            int num = rm.Next(1, 11);
            if (!list.Contains(num))
                list.Add(num);
        }
        //以上是为键盘生成随机排序
        for(int i=0;i<=9;i++)
        {
            SetKey(K[i], list[i]);
        }

        image = GetComponent<Image>();
    }
    
    // Update is called once per frame
    void Update () {
        if (needInfo && price != null)
        {
            needInfo = false;
            transform.Find("Panel").Find("Price").gameObject.GetComponent<Text>().text = "Total:$" + price;
        }
        if (paying && result != null)
        {
            paying = false;
            msgbox.transform.Find("Text").GetComponent<Text>().text = result;
            Destroy(transform.gameObject);
        }
    }

    IEnumerator getOrder(string orderid)
    {
        string url = "http://central.holoworld.win/Order.ashx?langNum="+Language.lang.langNum+"&id=" + orderid;
        WWW getData = new WWW(url);
        while (!getData.isDone) yield return new WaitForSeconds(0.1f);
        string result = getData.text;
        //{"id":"21d71b70-02f8-46d2-8af1-6e50626b9a20","orderid":"201704192218574102","buyerid":"4","goodid":"102","name":"cover boy human","quantity":"1","price":"11.2","operate":[{"time":"20170419221857","change":"OrderBuilt"}],"status":"1","express":null,"tracking":null,"address":"天津市天津市河北区建平镇231 蒋一涛 18840302034"}
        Debug.Log(result);
        buyerid = result.Split('\"')[11];
        price = result.Split('\"')[27];
        //price = result.Split("price\":\"".ToCharArray())[1].Split('\"')[0];
        //buyerid = result.Split("buyerid\":\"".ToCharArray())[1].Split('\"')[0];
    }

    public void addWord(string character)
    {
        if (transform.Find("Panel").Find("Password").Find("Password").GetComponent<Text>().text == "Password")
            transform.Find("Panel").Find("Password").Find("Password").GetComponent<Text>().text = "";
        transform.Find("Panel").Find("Password").Find("Password").GetComponent<Text>().text += character;
    }
    public void backspace()
    {
        if (transform.Find("Panel").Find("Password").Find("Password").GetComponent<Text>().text.Length > 0)
            transform.Find("Panel").Find("Password").Find("Password").GetComponent<Text>().text = transform.Find("Panel").Find("Password").Find("Password").gameObject.GetComponent<Text>().text.Remove(transform.Find("Panel").Find("Password").Find("Password").GetComponent<Text>().text.Length - 1, 1);
    }
    public void doPay()
    {
        if (transform.Find("Panel").Find("Password").Find("Password").gameObject.GetComponent<Text>().text != "" && buyerid != null)
        {
            string url = "http://central.holoworld.win/Pay.ashx?langNum="+Language.lang.langNum+"&id=" + buyerid 
                + "&order=" + transform.Find("Panel").Find("Orderid").gameObject.GetComponent<Text>().text 
                + "&pass=" + transform.Find("Panel").Find("Password").Find("Password").gameObject.GetComponent<Text>().text;
            Debug.Log(url);
            StartCoroutine(postPay(url));
            paying = true;
            msgbox = ControlCenter.ShowMessage(Language.lang.paying);
        }
    }
    IEnumerator postPay(string url)
    {
        WWW postData = new WWW(url, new byte[] { 0 });
        string er = postData.error;
        while (!postData.isDone) yield return new WaitForSeconds(0.1f);
        result = postData.text;
    }
}
