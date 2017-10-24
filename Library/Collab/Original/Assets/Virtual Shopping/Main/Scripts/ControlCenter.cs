using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlCenter : MonoBehaviour {
    #region//变量
    public static GameObject CenterObj;
    public static GameObject Menu,Payment,Msg,Trans,Search,Cart,Info,Good,Voice,NewOrder,MainCamera,Copy, Model, House, HouseView, Star;
    public static Vector3 facePosition;//适合生成新窗口的位置
    public static Quaternion faceRotation;//适合生成新窗口的角度
    public static float faceRadius = 1.8f;//适合生成新窗口的距离，这个是可以设置的，不会自动变化
    public const string version = "V0.4 Beta";//版本号，更新的时候一定要重新写入，这个和生成应用版本号不是一回事，切记！！！
    public static bool canCircle = true;//为了旋转展示商品后避免被点击而设置的变量
    public static int device = 0;//设备类型，1为调试，2为安卓，3为苹果，4为WP
    public static bool inied = true;//是否初始化完成，true为未完成，false为已完成
    #endregion

    public class GoodInfo
    {
        //public double __invalid_name__@search.score { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public int sales { get; set; }
        public string isScene { get; set; }
        public GoodInfo()
        {
            this.id = string.Empty;
            this.name = string.Empty;
            this.price = string.Empty;
            this.sales = 0;
            this.isScene = "0";
        }
        //public float power { get; set; }
        //public DateTimeOffset lastedittime { get; set; }
        //public int belong { get; set; }
        //public string status { get; set; }
    }
    public static void SetString(string key, string value)//缓存设置
    {
        PlayerPrefs.SetString(key, value);
    }
    public static string GetString(string key)//缓存读取
    {
        //if(key=="id")return "3";
        return PlayerPrefs.GetString(key);
    }

    void Start ()
    {
        MainCamera = GameObject.Find("Main Camera");
        CenterObj = MainCamera;
        Menu = GameObject.Find("Menu Canvas");
        Payment = GameObject.Find("Payment Canvas");
        Msg = GameObject.Find("Msg Canvas");
        Trans = GameObject.Find("Trans Canvas");
        Search = GameObject.Find("Search Canvas");
        Cart = GameObject.Find("Cart Canvas");
        Info = GameObject.Find("Info Canvas");
        Good = GameObject.Find("Good Canvas");
        Voice = GameObject.Find("Voice Canvas");
        NewOrder = GameObject.Find("Order Canvas");
        Copy = GameObject.Find("Copy Canvas");
        Model = GameObject.Find("Model Canvas");
        House = GameObject.Find("House Canvas");
        HouseView = GameObject.Find("House View");
        Star = GameObject.Find("Star");

        int langNum;
        if (!string.IsNullOrEmpty(GetString("lang")))
            langNum = int.Parse(GetString("lang"));
        else
        {
            langNum = Application.systemLanguage.ToString() == "Chinese" ? 1 : Application.systemLanguage.ToString() == "English" ? 0 : 0;
            SetString("lang", langNum.ToString());
        }
        Debug.Log(langNum);
        Language.ini(langNum);//语言初始化

        Star.SetActive(false);
        HouseView.SetActive(false);
        House.SetActive(false);
        Model.SetActive(false);
        Copy.SetActive(false);
        NewOrder.SetActive(false);
        Voice.SetActive(false);
        Good.SetActive(false);
        Info.SetActive(false);
        Cart.SetActive(false);
        Search.SetActive(false);
        Trans.SetActive(false);
        Msg.SetActive(false);
        Payment.SetActive(false);
        Menu.SetActive(false);

        facePosition = new Vector3(
            Mathf.Sin((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * faceRadius,
            Mathf.Sin((-CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f > 1.5f ? Mathf.Sin((-CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f : 1.5f,
            Mathf.Cos((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * faceRadius);
        faceRotation = Quaternion.Euler(
            CenterObj.transform.rotation.eulerAngles.x < 0f ? CenterObj.transform.rotation.eulerAngles.x : 0f,
            CenterObj.transform.rotation.eulerAngles.y,
            0f);


#if UNITY_EDITOR
        device = 1;
#endif
#if UNITY_ANDROID
        device = 2;
#endif
#if UNITY_IPHONE
        device = 3;
#endif
#if UNITY_METRO
        device = 4;
#endif
        Debug.Log("device: " + device);
        check();//检查版本
        SetString("id", "-1");//设置初始化id为-1，即未登录

        for (int i = 0; i < 10; i++)
            star();
        inied = false;

        //以上是正式代码，以下是测试代码

        house("1");

    }

    void Update () {
        //更新适合生成新窗口的位置和角度
        //float b = CenterObj.transform.rotation.eulerAngles.y;
        facePosition = new Vector3(
            Mathf.Sin((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * faceRadius,
            Mathf.Sin((-CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f > 1.5f ? Mathf.Sin((-CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f : 1.5f,
            Mathf.Cos((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * faceRadius);
        faceRotation = Quaternion.Euler(
            CenterObj.transform.rotation.eulerAngles.x < 0f ? CenterObj.transform.rotation.eulerAngles.x : 0f, 
            CenterObj.transform.rotation.eulerAngles.y, 
            0f);
    }

    public static void check()//检查新版本和网络连接
    {
        WWW web = new WWW("http://central.holoworld.win/Version.html");//检测版本号的地址
        if (web.error != null)
            ShowMessage(Language.lang.notconnecttoserver);
        while (!web.isDone)
            System.Threading.Thread.Sleep(10);
        string result = web.text;
        Debug.Log("check version: "+result.Contains(version));
        if (result.Contains(version))
        {
            SoundsControl.playText(Language.lang.welcome);
            if (GetString("id") == "-1")
                ShowMessage(Language.lang.needlogin);
        }
        else
        {
            SoundsControl.playText(Language.lang.needup);
            ShowMessage(Language.lang.newvirsonavaliavle);
        }
    }

    public void StartPayment(string name,double money,string model)//启动支付
    {
        Payment.SetActive(true);
        GameObject.Find("Payment Canvas/Panel/Name").GetComponent<Text>().text = name;
        GameObject.Find("Payment Canvas/Panel/Price").GetComponent<Text>().text = Language.lang.total + ": $"+money.ToString();
    }


    public void GetAsset(string BundleURL)//加载动态模型
    {
        Debug.Log(BundleURL);
        StartCoroutine(DownloadAssetAndScene(BundleURL));
    }
    IEnumerator DownloadAssetAndScene(string BundleURL)//加载动态模型附属
    {
        //下载assetbundle，加载GoodModel
        using (WWW asset = new WWW(BundleURL))
        {
            yield return asset;
            AssetBundle bundle = asset.assetBundle;
            Instantiate(bundle.LoadAsset("GoodModel"));
            bundle.Unload(false);
            Caching.CleanCache();
            yield return new WaitForSeconds(0.1f);
        }

    }

    public static IEnumerator UpdateImg(string url, GameObject updateobj)//加载贴图，给cube等用的
    {
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        Texture2D tex = new Texture2D(256, 256, TextureFormat.ETC_RGB4, false);
        tex.LoadImage(www.texture.EncodeToJPG());
        tex.Compress(false);
        try
        {
            updateobj.GetComponent<Renderer>().material.mainTexture = tex;
        }
        catch
        {
            updateobj.GetComponentInChildren<Renderer>().material.mainTexture = tex;
        }
    }

    public static IEnumerator UpdatePic(string url, GameObject updateobj)//加载贴图，给Image用的
    {
        WWW www = new WWW(url);
        yield return www;
        Texture2D tex = new Texture2D(256, 256, TextureFormat.ETC_RGB4, false);
        tex.LoadImage(www.bytes);
        tex.Compress(false);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        try
        {
            updateobj.GetComponent<Image>().overrideSprite = sprite;
        }
        catch
        {
            updateobj.GetComponentInChildren<Image>().overrideSprite = sprite;
        }
    }

    public static GameObject ShowMessage(string msg)
    {
        SoundsControl.playAudio(MainCamera.GetComponent<PrefebCollector>().msg);
        GameObject obj = Instantiate(Msg);
        obj.name = "msg_clone";
        obj.transform.position = new Vector3(facePosition.x, facePosition.y - 0.2f, facePosition.z);
        obj.transform.rotation = faceRotation;
        obj.transform.FindChild("Text").GetComponent<Text>().text = msg;
        obj.SetActive(true);
        Menu.SetActive(false);
        return obj;
    }
    public static void confirmModel(string goodid, string goodnames, string quantities, string prices)//启动确认型号窗口
    {
        GameObject obj = Instantiate(Model);
        obj.name = "confirmmodel_clone";
        obj.transform.position = facePosition;
        obj.transform.rotation = faceRotation;
        obj.GetComponent<ConfirmModel>().goodid = goodid;
        obj.GetComponent<ConfirmModel>().goodnames = goodnames;
        obj.GetComponent<ConfirmModel>().quanlities = quantities;
        obj.GetComponent<ConfirmModel>().prices = prices;
        obj.SetActive(true);
        Menu.SetActive(false);
    }
    public static void buildOrder(string goodid, string goodnames, string quantities, string prices, string model)//启动新建订单窗口
    {
        GameObject obj = Instantiate(NewOrder);
        obj.name = "neworder_clone";
        obj.transform.position = facePosition;
        obj.transform.rotation = faceRotation;
        obj.GetComponent<NewOrder>().goodid = goodid;
        obj.GetComponent<NewOrder>().goodnames = goodnames;
        obj.GetComponent<NewOrder>().quanlities = quantities;
        obj.GetComponent<NewOrder>().prices = prices;
        obj.GetComponent<NewOrder>().model = model;
        obj.SetActive(true);
        Menu.SetActive(false);
    }
    public static void pay(string orderid)//启动支付窗口
    {
        Debug.Log(orderid);
        GameObject obj = Instantiate(Payment);
        obj.name = "payment_clone";
        obj.transform.position = facePosition;
        obj.transform.rotation = faceRotation;
        obj.transform.FindChild("Panel").FindChild("Orderid").gameObject.GetComponent<Text>().text = orderid;
        obj.SetActive(true);
        Menu.SetActive(false);
    }
    public static void cart()//启动购物车窗口
    {
        try { Destroy(GameObject.Find("cart_clone")); } catch { }
        GameObject obj = Instantiate(Cart);
        obj.name = "cart_clone";
        obj.transform.position = new Vector3(facePosition.x, facePosition.y - 0.5f, facePosition.z);
        obj.transform.rotation = faceRotation;
        obj.SetActive(true);
        Menu.SetActive(false);
    }
    public static void detail(string goodid)//启动商品详情窗口
    {
        try { Destroy(GameObject.Find("good_clone")); } catch { }
        GameObject obj = Instantiate(Good);
        obj.name = "good_clone";
        obj.transform.position = new Vector3(facePosition.x, facePosition.y, facePosition.z);
        obj.transform.rotation = faceRotation;
        obj.GetComponent<GoodDetail>().goodid = goodid;
        obj.SetActive(true);
        Menu.SetActive(false);
    }
    public static void personal()//启动个人信息窗口
    {
        try { Destroy(GameObject.Find("personal_clone")); } catch { }
        GameObject obj = Instantiate(Info);
        obj.name = "personal_clone";
        /*obj.transform.position = new Vector3(
            Mathf.Sin((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * faceRadius+1f,
            Mathf.Sin((-CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f > 1.5f ? Mathf.Sin((-CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f : 1.5f,
            Mathf.Cos((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * faceRadius+1f);*/
        obj.transform.position = facePosition;
        obj.transform.rotation = faceRotation;
        obj.SetActive(true);
        Menu.SetActive(false);
    }
    public static void transformw()//启动物流窗口
    {
        try {Destroy(GameObject.Find("transform_clone"));}catch { }
        GameObject obj = Instantiate(Trans);
        obj.name = "transform_clone";
        /*obj.transform.position = new Vector3(
            Mathf.Sin((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * faceRadius+1f,
            Mathf.Sin((-CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f > 1.5f ? Mathf.Sin((-CenterObj.transform.rotation.eulerAngles.x * Mathf.PI) / 180) * 3f : 1.5f,
            Mathf.Cos((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * faceRadius+1f);*/
        obj.transform.position = facePosition;
        obj.transform.rotation = faceRotation;
        obj.SetActive(true);
        Menu.SetActive(false);
    }
    public static void search(string keyword, GoodSearchProxy.queryOrderBy query, bool aod)//启动搜索窗口
    {
        try { Destroy(GameObject.Find("search_clone")); } catch { }
        GameObject obj = Instantiate(Search);
        obj.name = "search_clone";
        obj.transform.position = facePosition;
        obj.transform.rotation = faceRotation;
        obj.GetComponent<GoodSearchDetail>().keyword = keyword;
        obj.GetComponent<GoodSearchDetail>().query = query;
        obj.GetComponent<GoodSearchDetail>().aod = aod;
        obj.SetActive(true);
        Menu.SetActive(false);
    }

    public static void menu()//启动菜单
    {
        try { Destroy(GameObject.Find("menu_clone")); } catch { }
        GameObject obj = Instantiate(Menu);
        obj.name = "menu_clone";
        Vector3 menuPosition=new Vector3(
            Mathf.Sin((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * 1f,
            0.8f,
            Mathf.Cos((CenterObj.transform.rotation.eulerAngles.y * Mathf.PI) / 180) * 1f);
        obj.transform.position = menuPosition;
        obj.transform.rotation = faceRotation;
        obj.SetActive(true);
    }
    public static void houseDetail(string goodid)//启动房子详情窗口
    {
        try { Destroy(GameObject.Find("house_clone")); } catch { }
        GameObject obj = Instantiate(House);
        obj.name = "house_clone";
        obj.transform.position = new Vector3(facePosition.x, facePosition.y, facePosition.z);
        obj.transform.rotation = faceRotation;
        obj.GetComponent<HouseDetail>().goodid = goodid;
        obj.SetActive(true);
        Menu.SetActive(false);
    }
    public static void house(string goodid)//启动房子
    {
        try { Destroy(GameObject.Find("houseView_clone")); } catch { }
        GameObject obj = Instantiate(HouseView);
        obj.name = "houseView_clone";
        Vector3 menuPosition = new Vector3(0f, 1.5f, 0f);
        obj.transform.position = menuPosition;
        obj.transform.GetComponent<HouseView>().goodid = goodid;
        obj.SetActive(true);
    }
    public static void star()//加入星星
    {
        GameObject obj = Instantiate(Star);
        obj.name = "star_clone";
        obj.SetActive(true);
    }

    public static void startListen()
    {
        SoundsControl.playAudio(MainCamera.GetComponent<PrefebCollector>().vocalstart);
        VocalListener.StartRecording();
    }

    public static bool objectExistActive(string path)
    {
        try
        {
            if (GameObject.Find(path).activeInHierarchy)
                return true;
            return false;
        }
        catch
        {
            return false;
        }
    }

}
