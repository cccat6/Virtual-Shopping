using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

//TODO replace the GameObject.Find to GameObject.FindGameObjectWithTag....

public class RefreshExpress : MonoBehaviour
{ //中间类，用于内部操作信息刷新，列表更换
    public static RefreshExpress manager;

    // Use this for initialization
    public static List<Express> g;
    public static int ini = 0;

    bool Etem = true;
    public string id = null;

    void Start()
    {

        manager = new RefreshExpress();


        HidePanel();
        id = ControlCenter.GetString("id");
        if (id == "-1" || string.IsNullOrEmpty(id))
        {
            ControlCenter.ShowMessage(Language.lang.unlogin);
            return;
        }
        //g = GetExpressDetail.ExpressManager();
        //List<Express> gi = new List<Express>();
        //gi = GetList(g, 0);
        //UpdatePanel(gi);
        //if (ControlCenter.IsRegister)
        //{
        //    g = new GetExpressDetail().ExpressManager();
        //    List<Express> temp = GetList(g, 0);
        //    UpdatePanel(temp);
        //}
        //else
        //{
        //    new ControlCenter().ShowMessage("用户Re未登陆");
        //    return;
        //}
    }
    void Update()
    {
        if (Etem)
        {
            if (GetExpressDetail.Eresult != null)
            {
                //g = GetExpressDetail.ExpressManager();
                g = GetExpressDetail.GetExpressEntiry(GetExpressDetail.Eresult);
                List<Express> gi = new List<Express>();
                gi = GetList(g, 0);
                UpdatePanel(gi);
                Etem = false;


            }
            else
            {
                //Debug.Log("Express still null");
            }
        }
    }
    public void HidePanel()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("ExpressPanel");
        GameObject objs = GameObject.FindGameObjectWithTag("expressLine");
        
        int count = obj.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(false);
            objs.transform.GetChild(i).gameObject.SetActive(false);
            //  objs.transform.GetChild(i).gameObject.SetActive(false);
            //lineObj.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    // Update is called once per frame

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
            List<Express> temp = GetList(g, ini);
            UpdatePanel(temp);
        }
    }
    public void DownScroll()
    {
        ini++;
        if (ini * 6 > g.Count && !((ini - 1) * 6 < g.Count))
        {
            ini--;
        }
        else
        {
            List<Express> temp = GetList(g, ini);
            UpdatePanel(temp);
        }
        //Test
        // ControlCenter.Menu.SetActive(true);

    }
    private static List<Express> GetList(List<Express> g, int index)
    {
        List<Express> list = new List<Express>();
        int length = (index + 1) * 6 > g.Count ? g.Count : (index + 1) * 6;
        for (int i = (index * 6); i < length; i++)
        {
            if (g[i] != null)
            {
                list.Add(g[i]);
            }
        }
        return list;
    }
    public void UpdatePanel(List<Express> g) //用于更新panel内信息
    {

        if (g.Count == 0)
        {
            return;
        }
        GameObject obj = GameObject.FindGameObjectWithTag("ExpressPanel");
        GameObject objs = GameObject.FindGameObjectWithTag("expressLine");

        HidePanel();
        int count = obj.transform.childCount;
        for (int i = 0; i < g.Count; i++)
        {
            // drawline.transform.GetChild(i).GetComponent<LineRenderer>
            obj.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = g[i].Nu + "   " + g[i].Last;
            obj.transform.GetChild(i).gameObject.SetActive(true);

            DrawLine(objs.transform.GetChild(i), g[i].SLongitude, g[i].SLatitude, g[i].ELongitude, g[i].ELatitude, g[i].FinishRate);
            objs.transform.GetChild(i).gameObject.SetActive(true);            
        }

        int count1 = objs.transform.childCount;
        //for (int i = 0; i < count1; i++)
        //{
        //    DrawLine(objs.transform.GetChild(i), g[i].SLongitude, g[i].SLatitude, g[i].ELongitude, g[i].ELatitude, g[i].FinishRate);
        //    for (int j = 0; j < 4; j++)
        //    {
        //        objs.transform.GetChild(i).gameObject.SetActive(true);

        //    }
        //}
    }

    //经纬度转平面坐标 近似值
    Vector3 GisToLoc(double east, double north)
    {
        return new Vector3((float)(east) * 8.3747f - 512.5f, (float)north * 7.6485f - 183.5f, -200f);
    }
    //计算两点倾斜角，返回角度
    static float KCal(Vector3 first, Vector3 second)
    {
        return (Mathf.Atan2((second.y - first.y), (second.x - first.x)) * 180f) / Mathf.PI;
    }
    //计算两点距离，用于缩放直线
    static float DCal(Vector3 first, Vector3 second)
    {
        return Mathf.Sqrt(Mathf.Pow((first.x - second.x), 2) + Mathf.Pow((first.y - second.y), 2));
    }
    public void DrawLine(Transform drawline, float sEast, float sNorth, float eEast, float eNorth, float finishRate)
    {
        const float len = 200f; //近似单位1直线长度，用于缩放
        //Test Case 
        //Vector3 start1 = GisToLoc(40f, 75f); //(-261.259,237.1675)
        //Vector3 end1 = GisToLoc(16f, 23f);//(458.9652,-7.8845)
        Vector3 start1 = GisToLoc(sEast, sNorth);
        Vector3 end1 = GisToLoc(eEast, eNorth);
        float K = KCal(start1, end1); //计算倾斜角
        float distance = DCal(start1, end1);//计算两点距离，用于直线模型缩放
        Vector3 startRotate = new Vector3(180f, 0f, 180f - K);
        drawline.GetChild(0).localPosition = start1;
        drawline.GetChild(0).localRotation = Quaternion.Euler(startRotate);

        Vector3 endRotate = new Vector3(180f, 0, -K);
        drawline.GetChild(2).localPosition = end1;
        drawline.GetChild(2).localRotation = Quaternion.Euler(endRotate);

        Vector3 lineRotate = new Vector3(0f, 0f, 90f + K);
        Vector3 lineScale = new Vector3(0.5f, distance / len, 0.5f);
        Vector3 lineLoc = new Vector3((start1.x + end1.x) / 2f, (start1.y + end1.y) / 2f, -275f);
        drawline.GetChild(1).localPosition = lineLoc;
        drawline.GetChild(1).localRotation = Quaternion.Euler(lineRotate);
        drawline.GetChild(1).localScale = lineScale;

        drawline.GetChild(3).GetComponent<Text>().text = finishRate.ToString() + "%";
        drawline.GetChild(3).localPosition = lineLoc;
    }
}
