using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class GetExpressDetail:MonoBehaviour //从中控服务器获取快递路由信息
{
    public static string expressUrl = "http://central.holoworld.win/Transform.ashx?langNum="+Language.lang.langNum+"&id=";
    public static WWW wwwE = null;
    public static string Eresult = null;
    static string id = null;
    void Start()
    {
        id = ControlCenter.GetString("id");
        if (id == "-1" || string.IsNullOrEmpty(id))
        {
            ControlCenter.ShowMessage(Language.lang.unlogin);
            Destroy(transform.gameObject);
            return;
        }
        else
        {
            StartCoroutine(ExpressGetstring(id));
          
        }
    }   
    //快递Get调用方法
    public IEnumerator ExpressGetstring(string id)
    {
        string temp = expressUrl + id;
       
        wwwE = new WWW(temp);
        yield return wwwE;
        if (wwwE.error != null)
        {
            yield return wwwE.error;
        }
        if (!wwwE.text.Contains("["))
        {
            ControlCenter.ShowMessage(Language.lang.failloaddata);
            Destroy(transform.gameObject);
        }
        if (wwwE.text != null)
        {
            Eresult = wwwE.text;
            wwwE.Dispose();
        }
            
        
    }
    //用于正则匹配浮点数
    public static float GetGisInfo(string s)
    {
        string sale = "";
        MatchCollection matchSet = Regex.Matches(s, "[0-9.]");
        foreach (Match aMatch in matchSet)
        {
            sale += aMatch;
        }
        return (float)Convert.ToDouble(sale);
    }

    /*public static List<Express> ExpressManager()
    {
        List<Express> gi = new List<Express>();
        gi.Add(new Express() { Orderid = "1", Nu = "1draw2ds3", Last = "s00dsd", SLatitude = 40.14f, SLongitude = 30f, ELatitude = 12f, ELongitude = 114.11f, FinishRate = 15f });
        gi.Add(new Express() { Orderid = "12", Nu = "1draw2ds3", Last = "sd00sd", SLatitude = 55f, SLongitude = 30f, ELatitude = 11f, ELongitude = 114.25f, FinishRate = 95f });
        gi.Add(new Express() { Orderid = "13", Nu = "1draw223", Last = "sdsd00", SLatitude = 15f, SLongitude = 30f, ELatitude = 25f, ELongitude = 52f, FinishRate = 85f });
        gi.Add(new Express() { Orderid = "11", Nu = "1draw232", Last = "sds00d", SLatitude = 55f, SLongitude = 30f, ELatitude = 33f, ELongitude = 25f, FinishRate = 55f });
        gi.Add(new Express() { Orderid = "1132", Nu = "1dra2w23", Last = "sd00sd", SLatitude = 66f, SLongitude = 30f, ELatitude = 30f, ELongitude = 2f, FinishRate = 45f });
        gi.Add(new Express() { Orderid = "11", Nu = "1draw223", Last = "sd00sd", SLatitude = 77.14f, SLongitude = 30f, ELatitude = 0f, ELongitude = 0f, FinishRate = 25f });
        gi.Add(new Express() { Orderid = "31", Nu = "1dra2w23", Last = "sd00sd", SLatitude = 25.14f, SLongitude = 30f, ELatitude = 0f, ELongitude = 0f, FinishRate = 25f });
        gi.Add(new Express() { Orderid = "14", Nu = "1dr2aw23", Last = "sd000sd", SLatitude = 89.14f, SLongitude = 30f, ELatitude = 11f, ELongitude = 122.37f, FinishRate = 55f });
        gi.Add(new Express() { Orderid = "15", Nu = "1d2raw23", Last = "sds02d", SLatitude = 70.14f, SLongitude = 30f, ELatitude = 11f, ELongitude = 122.37f, FinishRate = 15f });
        gi.Add(new Express() { Orderid = "17", Nu = "1draw23", Last = "sds32d", SLatitude = 15.14f, SLongitude = 30f, ELatitude = 28, ELongitude = 122.37f, FinishRate = 5f });
        return gi;
    }*/

    /// <summary>
    /// 将返回的Json数据转为强类型的Good类型
    /// </summary>
    /// <param name="json">Get返回的文本</param>
    /// <returns>一个List<Express>实例</returns>
    public static List<Express> GetExpressEntiry(string json)
    {
        List<Express> g = new List<Express>();
        if (json != "")
        {
            //string json1 = "[{\"orderid\":\"201703181243\",\"goodid\":\"2\",\"quantity\":null,\"price\":\"9.9\",\"status\":\"test status\",\"express\":\"未知快递\",\"tracking\":\"123\",\"address\":\"深圳\"},{\"orderid\":\"201703181243\",\"goodid\":\"1\",\"quantity\":null,\"price\":\"9.9\",\"status\":\"test status\",\"express\":\"未知快递\",\"tracking\":\"123\",\"address\":\"广州\"},{\"orderid\":\"201703181243\",\"goodid\":\"1\",\"quantity\":null,\"price\":\"9.9\",\"status\":\"test status\",\"express\":\"未知快递\",\"tracking\":\"123\",\"address\":null}]";
            //string json = "[{\"orderid\":\"广州\",\"nu\":\"201703181243\",\"last\":null,\"sLatitude\":0.0,\"sLongitude\":0.0,\"eLatitude\":0.0,\"eLongitude\":0.0,\"finishrate\":0.0},{\"orderid\":null,\"nu\":\"201703181243\",\"last\":null,\"sLatitude\":0.0,\"sLongitude\":0.0,\"eLatitude\":0.0,\"eLongitude\":0.0,\"finishrate\":0.0}]";
            //string s = "[\"潘峰\",\"0.0000\",\"http://holoworld.win/morning/upload/icon/20170228/1488259695513.jpg\"]";
            //string[] aaa = s.Split('"', '"');
            //Test Json:string json = "[{\"orderid\":\"201703181243\",\"nu\":\"123\",\"last\":null,\"sLatitude\":0.0,\"sLongitude\":0.0,\"eLatitude\":0.0,\"eLongitude\":0.0,\"finishrate\":0.0},{\"orderid\":\"201703181243\",\"nu\":\"123\",\"last\":\"汕头\",\"sLatitude\":0.0,\"sLongitude\":0.0,\"eLatitude\":0.0,\"eLongitude\":0.0,\"finishrate\":0.0},{\"orderid\":\"201703181243\",\"nu\":\"123\",\"last\":null,\"sLatitude\":0.0,\"sLongitude\":0.0,\"eLatitude\":0.0,\"eLongitude\":0.0,\"finishrate\":0.0}]";
            string value = json.Substring(json.IndexOf('[') + 1, json.IndexOf(']') - json.IndexOf('['));
            string[] valueArray = value.Split('"', '"');
            int pt = 0; //指示当前的GoodInfo
            for (int i = 0; i < valueArray.Length; i++)
            {
                if (valueArray[i] == "orderid")
                {
                    g.Add(new Express());
                    g[pt].Orderid = valueArray[i + 2];
                }
                if (valueArray[i] == "nu")
                {
                    g[pt].Nu = valueArray[i + 2];
                }
                if (valueArray[i] == "last")
                {
                    if (valueArray[i + 1].Contains("null"))
                    {
                        g[pt].Last = null;
                    }
                    else
                    {
                        g[pt].Last = valueArray[i + 2];
                    }
                }
                //分割前面部分
                if (valueArray[i] == "slatitude")
                {

                    g[pt].SLatitude = GetGisInfo(valueArray[i + 1]);

                }
                if (valueArray[i] == "slongitude")
                {
                    g[pt].SLongitude = GetGisInfo(valueArray[i + 1]);
                }
                if (valueArray[i] == "elatitude")
                {
                    g[pt].ELatitude = GetGisInfo(valueArray[i + 1]);
                }
                if (valueArray[i] == "elongitude")
                {
                    g[pt].ELongitude = GetGisInfo(valueArray[i + 1]);
                }
                if (valueArray[i] == "finishrate")
                {
                    g[pt].FinishRate = GetGisInfo(valueArray[i + 1]);
                    pt++;
                }
                // string s = "{  \"@odata.context\": \"https://holoworld.search.windows.net/indexes('goods')/$metadata#docs(id,name,price,sales)",  "value": [    {      "@search.score": 1.0,      \"orderid": "10",      "nu": "First",      "last": "23",      "slatitude": 11.11,      "slongitude": 22.22,      "elatitude": 111.11,      "elongitude": 222.22    },    {      "@search.score": 1.0,      "orderid": "12",      "nu": "Second",      "last": "45",      "slatitude": 22.22,      "slongitude": 35.35,      "elatitude":";
            }
            return g;
        }
        else
        {
            throw new Exception("The String is null");
        }

    }
}

public class Express
{
    public string Orderid { get; set; }
    public string Nu { get; set; }
    public string Last { get; set; }
    public float SLatitude { get; set; }
    public float SLongitude { get; set; }
    public float ELatitude { get; set; }
    public float ELongitude { get; set; }
    public float FinishRate { get; set; }
}



