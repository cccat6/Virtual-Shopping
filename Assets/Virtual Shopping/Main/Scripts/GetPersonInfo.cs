using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GetPersonInfo : MonoBehaviour
{
    public static GetPersonInfo getPersonInfo = null;
    public static PersonInfo pi = null;
    public static List<GoodIn> gi = new List<GoodIn>();
    public string id = null;

    //用于获取个人信息数据url
    public static string personalInfoUrl = "http://central.holoworld.win/PersonalInfo.ashx?langNum="+Language.lang.langNum+"&id=";
    public static string goodInfoUrl = "http://central.holoworld.win/OrderList.ashx?langNum="+Language.lang.langNum+"&id=";
    static string goodInfoPicUrl = "http://model.holoworld.win/GetModel.ashx?type=0&id={0}";

    public static string Presult = null;
    public static string Gresult = null;

    public static WWW wwwP = null;
    public static WWW wwwG = null;
    // Use this for initialization
    void Start()
    {
        if (ControlCenter.inied)
            return;
        id = ControlCenter.GetString("id");
        if (id == "-1" || string.IsNullOrEmpty(id))
        {
            ControlCenter.ShowMessage(Language.lang.unlogin);
            Destroy(transform.gameObject);
            return;
        }
        else
        {
            StartCoroutine(PersonGetstring(id));
            StartCoroutine(GoodGetstring(id));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //两个不同的分割文本，解析数据的方法
    public static PersonInfo GetPersonalInfoEntiry(string json)
    {
        PersonInfo p = new PersonInfo();
        if (json != "")
        {
            Debug.Log(json);
            string[] temp = json.Split('"', '"');
            p.UserName = temp[1];
            p.Balance = temp[3];
            p.Pic = temp[5];
        }
        else
        {
            throw new Exception("The String is null");
        }
        return p;

    }
    public static List<GoodIn> GetGoodInfoEntiry(string json)
    {
        Debug.Log(json);
        List<GoodIn> g = new List<GoodIn>();
        if (json != "")
        {
            string value = json.Substring(json.IndexOf('[') + 1, json.IndexOf(']') - json.IndexOf('['));
            string[] valueArray = value.Split('"', '"');
            int pt = 0; //指示当前的GoodInfo
            for (int i = 0; i < valueArray.Length; i++)
            {
                if (valueArray[i] == "orderid")
                {
                    g.Add(new GoodIn());
                    g[pt].orderid = valueArray[i + 2];
                }
                if (valueArray[i] == "goodid")
                {
                    g[pt].GoodPic = string.Format(goodInfoPicUrl, valueArray[i + 2]);
                }
                if (valueArray[i] == "name")
                {
                    g[pt].GoodName = valueArray[i + 2];
                }
                if (valueArray[i] == "status")
                {
                    g[pt].Status = valueArray[i + 2];
                    pt++;
                }
            }
            //g.Add(new GoodIn { GoodPic = string.Format(goodInfoPicUrl, "2"), GoodName = "123", Status = "sss",orderid= });
            return g;
        }
        else
        {
            throw new Exception("The String is null");
        }

    }
    public IEnumerator PersonGetstring(string id)
    {
        string temp = personalInfoUrl + id;
        wwwP = new WWW(temp);
        while (!wwwP.isDone)
        {
            yield return wwwP;
        }
        if (wwwP != null && string.IsNullOrEmpty(wwwP.error))
        {
            Presult = wwwP.text;
            //Debug.Log("P:" + Presult);
        }
        if (!Presult.Contains("["))
        {
            ControlCenter.ShowMessage(Language.lang.failloaddata);
            Destroy(transform.gameObject);
        }
    }
    public IEnumerator GoodGetstring(string id)
    {
        while (Presult == null)
            yield return 1;
        string temp = goodInfoUrl + id;
        Debug.Log("G_temp:" + temp);
        wwwG = new WWW(temp);
        while (!wwwG.isDone)
        {
            //Gresult = wwwG.text;
            //Debug.Log("notDone:"+wwwG.bytesDownloaded);
            yield return wwwG;
        }
        if (wwwG != null && string.IsNullOrEmpty(wwwG.error))
        {
            Gresult = wwwG.text;
            //Debug.Log("G:" + Gresult);
        }
    }

    //用于解析数据的类
    public class PersonInfo
    {
        public string Pic { get; set; }
        public string UserName { get; set; }
        public string Balance { get; set; }

    }
    public class GoodIn
    {
        public string Status { get; set; }
        public string GoodPic { get; set; }
        public string GoodName { get; set; }
        public string orderid { get; set; }
    }

    public void todo(int nowStatus,string orderid)
    {
        if(nowStatus==-1)
        {
            ControlCenter.ShowMessage(Language.lang.illegaloperate);
            return;
        }
        if (orderid == "-1")
        {
            ControlCenter.ShowMessage(Language.lang.errororder);
            return;
        }
        if (nowStatus == 1)//Pay
        {
            ControlCenter.pay(orderid);
            Destroy(transform.gameObject);
        }
        else if (nowStatus == 2)//Cancel
        {
            StartCoroutine(UpdateOrder(orderid, "0:Cancel"));
        }
        else if (nowStatus == 3)//Sign
        {
            StartCoroutine(UpdateOrder(orderid, "4:Sign"));
        }
    }
    private IEnumerator UpdateOrder(string orderid,string instruction)//更改订单状态
    {
        WWW postData = new WWW("http://central.holoworld.win/UpdateOrder.ashx?langNum="+Language.lang.langNum+"&id=" + orderid, Encoding.UTF8.GetBytes("instruction=" + instruction));
        string er = postData.error;
        while (!postData.isDone) yield return new WaitForSeconds(0.1f);
        string result = postData.text;
        ControlCenter.ShowMessage(result);
        Destroy(transform.gameObject);
    }
}
