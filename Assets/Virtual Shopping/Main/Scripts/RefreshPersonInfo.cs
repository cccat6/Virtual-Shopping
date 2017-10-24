using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshPersonInfo : MonoBehaviour
{
    bool Ptem = true;
    bool Gtem = true;
    public GameObject o1, o2, o3, o4;

    //用来跨类进行操作 详见PersonInfoDownClick.cs 和 PersonInfoUpClick.cs
    public static RefreshPersonInfo refreshPersonalInfo = new RefreshPersonInfo();

    GetPersonInfo.PersonInfo pi = new GetPersonInfo.PersonInfo();
    static List<GetPersonInfo.GoodIn> gi = new List<GetPersonInfo.GoodIn>();
   

    public string id = null;
    public static int ini = 0;
    
    // Use this for initialization
    void Start()
    {
        if (ControlCenter.inied)
            return;
        Ptem = true;
        Gtem = true;
        HidePersonInfoPanel();
        id = ControlCenter.GetString("id");
        if (id == "-1" || string.IsNullOrEmpty(id))
        {
            ControlCenter.ShowMessage(Language.lang.unlogin);
            return;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Ptem)
        {
            if (GetPersonInfo.Presult != null)
            {
                pi = GetPersonInfo.GetPersonalInfoEntiry(GetPersonInfo.Presult);
                GameObject img = transform.Find("Grid Panel").Find("Image").gameObject;

                StartCoroutine(ControlCenter.UpdatePic("http://holoworld.azurewebsites.net/morning//upload/icon/icon.jpg"/*pi.Pic//这里需要修改，临时用默认图片替代*/, transform.Find("Grid Panel/Image").gameObject));
                transform.Find("Grid Panel/Username").GetComponent<Text>().text = pi.UserName;
                transform.Find("Grid Panel/Balance").GetComponent<Text>().text = pi.Balance;
                
                Ptem = false;
            }
        }
        if (Gtem)
        {
            if (GetPersonInfo.Gresult != null)
            {
                gi = GetPersonInfo.GetGoodInfoEntiry(GetPersonInfo.Gresult);
                List<GetPersonInfo.GoodIn> g = new List<GetPersonInfo.GoodIn>();
                g = GetList(gi, 0);
                UpdatePanel(g);
                Gtem = false;
            }
        }
    }
    void HidePersonInfoPanel()
    {
        /*o1.SetActive(false);
        o2.SetActive(false);
        o3.SetActive(false);
        o4.SetActive(false);*/
    }


    public void UpScroll()
    {
        ini--;
        if (ini < 0)
        {
            ini = 0;
            return;//超过下界，返回空

        }
        else //更新panel内部数据
        {
            List<GetPersonInfo.GoodIn> temp = GetList(gi, ini);
            UpdatePanel(temp);
        }
    }
    public void DownScroll()
    {
        ini++;
        if (ini * 4 > gi.Count && !((ini - 1) * 4 < gi.Count))
        {
            ini--;
        }
        else
        {
            List<GetPersonInfo.GoodIn> temp = GetList(gi, ini);
            UpdatePanel(temp);
        }


    }
    private static List<GetPersonInfo.GoodIn> GetList(List<GetPersonInfo.GoodIn> g, int index)
    {
        List<GetPersonInfo.GoodIn> list = new List<GetPersonInfo.GoodIn>();
        int length = (index + 1) * 4 > g.Count ? g.Count : (index + 1) * 4;
        for (int i = (index * 4); i < length; i++)
        {
            if (g[i] != null)
            {
                list.Add(g[i]);
            }
        }
        return list;
    }

    public void UpdatePanel(List<GetPersonInfo.GoodIn> g) //用于更新panel内信息
    {
        if (g.Count == 0)
        {
            return;
        }
        HidePersonInfoPanel();

        GameObject obj = transform.Find("Grid Panel/Grid Panel").gameObject;
        int count = obj.transform.childCount;

        for (int i = 0; i < g.Count; i++)
        {
            Transform thisObj = obj.transform.GetChild(i);
            thisObj.gameObject.SetActive(true);

            thisObj.GetChild(4).GetComponent<Text>().text = g[i].orderid;//设置订单id在ui里储存
            StartCoroutine(ControlCenter.UpdateImg(g[i].GoodPic, thisObj.Find("Show").gameObject));
            thisObj.GetChild(1).GetComponent<Text>().text = g[i].GoodName;
            thisObj.GetChild(2).GetComponent<Text>().text = 
                g[i].Status == "0" ? "Terminate" : 
                g[i].Status == "1" ? "Unpayed" : 
                g[i].Status == "2" ? "Ready to delivery" : 
                g[i].Status == "3" ? "In transit" : 
                g[i].Status == "4" ? "Signed" : 
                "Error status";

            thisObj.GetChild(3).GetChild(0).GetComponent<Text>().text =//状态是0或4的时候，订单是不能被操作的
                g[i].Status == "1" ? "Pay" :
                g[i].Status == "2" ? "Cancel" :
                g[i].Status == "3" ? "Sign" :
                "Error";
            if (g[i].Status == "0" || g[i].Status == "4")
                thisObj.GetChild(3).gameObject.SetActive(false);
            else
                thisObj.GetChild(3).gameObject.SetActive(true);


        }

        // yield return new WaitForSecondsRealtime(0.05f);
    }
}
