using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LanguageUnderstand : MonoBehaviour {

    private static string[] entity = null;
    public static GameObject ListenIcon = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (entity != null)
        {
            foreach (string now in entity)
                Debug.Log("Understand: " + now);
            if (entity[0] == "error")
                entity = null;
            else
            {
                operate(entity);
                entity = null;
            }
        }
	}

    public void go(string data)
    {
        if (data.Contains("switch") || data.Contains("Switch"))
        {
            if (data.Contains("chinese") || data.Contains("Chinese"))
                ControlCenter.SetString("lang", "1");
            else if (data.Contains("english") || data.Contains("English"))
                ControlCenter.SetString("lang", "0");
            else
                ControlCenter.SetString("lang", "0");

            Language.ini(int.Parse(ControlCenter.GetString("lang")));//语言初始化
            ControlCenter.check();//检查版本
            ListenIcon.SetActive(false);
            ListenIcon = null;
            return;
        }
        StartCoroutine(Get(data));
    }
    private IEnumerator Get(string data)
    {
        List<string> entities = new List<string>();
        WWW web = new WWW("https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/e94dcbd1-c0b3-48db-bb7a-e1e357e28aac?subscription-key=95229559c87c41a28035d63b9d3a661f&verbose=false&timezoneOffset=0.0&spellCheck=true&q=" + WWW.EscapeURL(data));
        while (!web.isDone)
            yield return web;
        string result = web.text;
        result = result.Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty);
        try
        {
            Regex reg = new Regex("intent\": \".*?\"");
            Match match = reg.Match(result);
            entities.Add(match.Groups[0].Value.Split('\"')[2]);
            reg = new Regex("entity\": \".*?\"");
            MatchCollection matches = reg.Matches(result);
            foreach (Match now in matches)
                entities.Add(now.Groups[0].Value.Split('\"')[2]);
            entity = entities.ToArray();
        }
        catch { entity = new string[] { "error" }; }
        try
        {
            ListenIcon.SetActive(false);
        }
        catch { }
        ListenIcon = null;
    }
    private bool operate(string[] entities)//会返回一个bool表示是否执行成功，但不一定有用
    {
        if (entities[0] == "search")
            return search(entities);
        else if (entities[0].Contains("log") || entities[0].Contains("sign"))
            return log(entities);
        else if (entities[0] == "window")
            return window(entities);
        ControlCenter.ShowMessage(Language.lang.error);
        return false;
    }
    private bool search(string[] entities)
    {
        string good = null;//搜索内容
        int criteria = 0, //排序规则
         sort = 0;//排序字段
        foreach(string now in entities)
        {
            if (now == "ascending")
                criteria = 1;
            else if (now == "descending")
                criteria = 2;
            else if (now == "price")
                sort = 1;
            else if (now == "seles")
                sort = 2;
            else
                good = now;//如果出错，有可能搜索内容是search，因为第一个值是search
            ControlCenter.search(good, sort == 1 ? GoodSearchProxy.queryOrderBy.price : sort == 2 ? GoodSearchProxy.queryOrderBy.sales : GoodSearchProxy.queryOrderBy.power, criteria != 2);
        }
        if (good == "search")
            return false;
        //在这里调用搜索出现的函数
        return true;
    }
    private bool log(string[] entities)
    {
        try
        {
            foreach (string now in entities)
            {
                if (now.Contains("out") || now.Contains("unsign"))
                {
                    ControlCenter.SetString("id", "-1");
                    Login.logining = false;
                    SoundsControl.playAudio(ControlCenter.MainCamera.GetComponent<PrefebCollector>().msg);
                    return true;
                }
                else if (now.Contains("in") || now.Contains("sign") || now.Contains("log"))
                {
                    Login.startLogin();
                    return true;
                }
            }
        }
        catch { return false; }
        return false;
    }
    private bool window(string[] entities)
    {
        int onoff = 0;//1开2关
        string target = null;
        foreach(string now in entities)
        {
            if (now == "open" || now == "turn on" || now == "show")
                onoff = 1;
            else if (now == "close" || now == "turn off")
                onoff = 2;
            else
                target = now;
        }
        if (onoff == 0 || target == "window")
            return false;
        target = target.Replace(" windows", string.Empty).Replace(" window", string.Empty);
        return onoff == 1 ? open(target) : close(target);
    }
    private bool open(string target)
    {
        if (target == "logistics info" || target == "logistic" || target == "logistics" || target == "logistic info" || target == "tracker" || target == "tracking" || target.Contains("transform") || target.Contains("track"))
        {
            ControlCenter.transformw();
        }
        else if (target.Contains("personal") || target.Contains("order list") || target.Contains("off"))//这里用off是因为有的时候识别会出问题，会识别出off
        {
            ControlCenter.personal();
        }
        else if (target.Contains("cart") || target.Contains("shopping cart"))
        {
            ControlCenter.cart();
        }
        return false;
    }
    private bool close(string target)
    {
        if (target == "logistics info" || target == "logistic" || target == "logistics" || target == "logistic info" || target == "tracker" || target == "tracking" || target.Contains("transform") || target.Contains("track"))
        {
            try { Destroy(GameObject.Find("transform_clone")); } catch { }
        }
        else if (target.Contains("personal") || target.Contains("order list") || target.Contains("off"))//这里用off是因为有的时候识别会出问题，会识别出off
        {
            try { Destroy(GameObject.Find("personal_clone")); } catch { }
        }
        else if (target.Contains("cart") || target.Contains("shopping cart"))
        {
            try { Destroy(GameObject.Find("cart_clone")); } catch { }
        }
        else if (target.Contains("confirm"))
        {
            try { Destroy(GameObject.Find("neworder_clone")); } catch { }
        }
        else if (target.Contains("payment"))
        {
            try { Destroy(GameObject.Find("payment_clone")); } catch { }
        }
        else if (target.Contains("message box") || target.Contains("message"))
        {
            try { Destroy(GameObject.Find("msg_clone")); } catch { }
        }
        else if (target.Contains("good detail") || target.Contains("goods detail"))
        {
            try { Destroy(GameObject.Find("good_clone")); } catch { }
        }
        else if (target.Contains("search"))
        {
            try { Destroy(GameObject.Find("search_clone")); } catch { }
        }
        return false;
    }
}
