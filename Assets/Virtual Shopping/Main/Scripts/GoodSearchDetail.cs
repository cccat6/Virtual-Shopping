using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine.UI;

public class GoodSearchDetail : MonoBehaviour {

    // Use this for initialization
    public static float radius = 2.5f;//屏幕与质心的距离
    public static float movespeed = 0f;//转动速度，正为顺时针，负为逆时针，这里只是初始化，值无影响

    public GameObject Preobject;
    public string keyword;
    public GoodSearchProxy.queryOrderBy query;
    public bool aod;

    ControlCenter.GoodInfo[] g = null;

    List<GameObject> good = new List<GameObject>();
    public  class Good
    {
        public GameObject obj ;
        public float angle ;
    }
    public List<Good> screen = new List<Good>();
    static int goodNow = 0;

    public  void Go(ControlCenter.GoodInfo[] goods)
    {
        if (screen.Count == 1)
            movespeed = -9f;
        else if (screen.Count == 2)
            movespeed = -3f;
        else if (screen.Count == 3)
            movespeed = -0.6f;
        else if (screen.Count == 4)
            movespeed = -0.2f;
        for (int i = 0; i < screen.Count; i++)
        {
            if (screen[i].angle == 360f)
                screen[i].angle = 0f;
            screen[i].angle += movespeed;//每过1帧变换多少度
            screen[i].obj.transform.localPosition = new Vector3(Mathf.Sin((screen[i].angle * Mathf.PI) / 180) * radius, screen[i].obj.transform.localPosition.y, Mathf.Cos((screen[i].angle * Mathf.PI) / 180) * radius);
            screen[i].obj.transform.localRotation = Quaternion.Euler(screen[i].obj.transform.localRotation.eulerAngles.x, screen[i].angle, screen[i].obj.transform.localRotation.eulerAngles.z);
            if ((screen[i].angle > -270.95f) && screen[i].angle < -269.05f)
            {
                screen[i].angle = 90f;
                screen[i].obj.transform.localPosition.Set(Mathf.Sin((90 * Mathf.PI) / 180) * radius, screen[i].obj.transform.localPosition.y, Mathf.Cos((90 * Mathf.PI) / 180) * radius);
                updateGood(screen[i].obj, goods[goodNow++ % goods.Length]);
            }
        }
        if (screen.Count < 10 && (screen.Count == 0 || (screen[screen.Count - 1].angle > 53.95f) && screen[screen.Count - 1].angle < 54.05f))
        {
            float x = Mathf.Sin((90 * Mathf.PI) / 180) * radius;
            float z = Mathf.Cos((90 * Mathf.PI) / 180) * radius;
            Vector3 vec = new Vector3(x, 1, z);
            Quaternion qua = Quaternion.Euler(0, 90f, 0);
            GameObject get = Instantiate(Preobject, vec, qua);
            get.transform.SetParent(transform);
            screen.Add(new Good() { obj = get,angle=90f});

            try
            {
                updateGood(get, goods[goodNow++ % goods.Length]);
            }
            catch
            {
                ControlCenter.ShowMessage(Language.lang.error);
                return;
            }
        }
    }
    public void updateGood(GameObject obj, ControlCenter.GoodInfo info)
    {
        obj.transform.Find("Goodname").GetComponent<Text>().text = info.name;
        obj.transform.Find("Goodprice").GetComponent<Text>().text = "$" + info.price;
        StartCoroutine(ControlCenter.UpdatePic("http://model.holoworld.win/GetModel.ashx?type=0&id=" + info.id, obj.transform.Find("Image").gameObject));
        obj.transform.Find("goodid").GetComponent<Text>().text = info.id;
        obj.transform.Find("isScene").GetComponent<Text>().text = info.isScene;
    }
    void Start (){
        if (ControlCenter.inied)
            return;
        StartCoroutine(search(keyword, true, query, aod, false, 0.0, 0.0, false, 0, 0));//暂时不用价格过滤和销量过滤，在语言理解服务上没有做
        transform.position = new Vector3(0f, 1.5f, 0f);
    }

    public float pauseAngle = 0f, pauseHight = 1f;
    List<float> returnAngle = new List<float>();
    List<float> returnHight = new List<float>();
    // Update is called once per frame
    void Update() {
        if (!ControlCenter.objectExistActive("good_clone") && !ControlCenter.objectExistActive("houseView_clone") && !ControlCenter.objectExistActive("house_clone"))
        {
            if (g != null)
            {
                if (pauseAngle != 0f)
                {
                    pauseAngle -= 4f;
                    pauseHight += 0.04f;
                    for (int i = 0; i < screen.Count; i++)
                    {
                        screen[i].obj.transform.localRotation = Quaternion.Euler(new Vector3(pauseAngle, screen[i].obj.transform.localRotation.eulerAngles.y, screen[i].obj.transform.localRotation.eulerAngles.z));
                        screen[i].obj.transform.position = new Vector3(screen[i].obj.transform.position.x, pauseHight, screen[i].obj.transform.position.z);
                    }
                    return;
                }
                else if (pauseAngle == 0f && returnAngle.Count > 0)
                {
                    for (int i = 0; i < screen.Count; i++)
                    {
                        screen[i].obj.transform.localRotation = Quaternion.Euler(new Vector3(returnAngle[i], screen[i].obj.transform.localRotation.eulerAngles.y, screen[i].obj.transform.localRotation.eulerAngles.z));
                        screen[i].obj.transform.position = new Vector3(screen[i].obj.transform.position.x, returnHight[i], screen[i].obj.transform.position.z);
                    }
                    returnAngle.Clear();
                    returnHight.Clear();
                }
                //if (screen.Count < 2)
                //    Debug.Log(g.Length);//调试输出获取到的商品信息
                if (g.Length == 0)
                 {
                    ControlCenter.ShowMessage(Language.lang.notfindgood);
                    Destroy(transform.gameObject);
                 }//如果搜索到的是空，就销毁这个搜索，并弹窗提示
                Go(g);
            }
        }
        else if (pauseAngle != 80f)
        {
            if (pauseAngle == 0)
            {
                for (int i = 0; i < screen.Count; i++)
                {
                    returnAngle.Add(screen[i].obj.transform.localRotation.x);
                    returnHight.Add(screen[i].obj.transform.position.y);
                }
            }
            pauseAngle += 4f;
            pauseHight -= 0.04f;
            for (int i = 0; i < screen.Count; i++)
            {
                screen[i].obj.transform.localRotation = Quaternion.Euler(new Vector3(pauseAngle, screen[i].obj.transform.localRotation.eulerAngles.y, screen[i].obj.transform.localRotation.eulerAngles.z));
                screen[i].obj.transform.position = new Vector3(screen[i].obj.transform.position.x, pauseHight, screen[i].obj.transform.position.z);
            }
        }
    }

    public IEnumerator search(string searchKeyWord, bool orderOrNotOrder, GoodSearchProxy.queryOrderBy queryorderby, bool ascOrDesc, bool filterPrice, double priceUpper, double priceLower, bool filterSales, int salesUpper, int salesLower)
    {
        StartCoroutine(GoodSearchProxy.GetSearchResult(searchKeyWord, orderOrNotOrder, queryorderby, ascOrDesc, filterPrice, priceUpper, priceLower, filterSales, salesUpper, salesLower));
        while (GoodSearchProxy.reresult == null)
            yield return 1;
        g = GoodSearchProxy.reresult.ToArray();
        GoodSearchProxy.reresult = null;
    }

    public void selected(string goodid, bool isScene)
    {
        Debug.Log(goodid);
        if (!isScene)
            ControlCenter.detail(goodid);
        else
            ControlCenter.houseDetail(goodid);
        Destroy(transform);
    }
}
