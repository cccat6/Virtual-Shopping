using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartControl : MonoBehaviour {
    
    public static float radius = 0.55f;//屏幕与质心的距离
    public static float movespeed = -1f;//转动速度，正为顺时针，负为逆时针，这里只是初始化，值无影响

    public GameObject Preobject;
    private class Good
    {
        public string id { get; set; }
        public string quantity { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public Texture2D tex { get; set; }
    }
    private List<Good> goods = new List<Good>();
    private int page = 0;
    private int roll = 0;
    private bool ini = false;
    private int complete = 0;
    private GameObject msg;

    // Use this for initialiyation
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

        StartCoroutine(loadGoods());
	}
	
	// Update is called once per frame
	void Update () {
        load();
        if (ini)
            loadItems();
        if (roll != 0f)//上下切换
        {
            try
            {
                for (int i = 0; i < screen.Count; i++)
                {
                    if (screen[i].angle == 360f)
                        screen[i].angle = 0f;
                    screen[i].angle += roll > 0f ? -6f : 6f;
                    screen[i].obj.transform.localPosition = new Vector3(screen[i].obj.transform.localPosition.x, Mathf.Cos((screen[i].angle * Mathf.PI) / 180) * radius, Mathf.Sin((screen[i].angle * Mathf.PI) / 180) * radius);
                    screen[i].obj.transform.localRotation = Quaternion.Euler(screen[i].angle + 90f, 0f, screen[i].obj.transform.localRotation.eulerAngles.z);
                }
            }
            catch { }
            roll += roll > 0 ? -6 : 6;
        }
    }

    public class Item
    {
        public GameObject obj;
        public float angle;
    }
    public List<Item> screen = new List<Item>();
    private bool finishLoadScreen = false;
    public void load()
    {
        if (screen.Count == 6 && finishLoadScreen)
            return;
        else if(screen.Count == 6 && !finishLoadScreen)
        {
            for (int i = 0; i < screen.Count; i++)
            {
                if (screen[i].angle == 360f)
                    screen[i].angle = 0f;
                screen[i].angle += 60;//每过1帧变换多少度
                screen[i].obj.transform.localPosition = new Vector3(screen[i].obj.transform.localPosition.x, Mathf.Cos((screen[i].angle * Mathf.PI) / 180) * radius, Mathf.Sin((screen[i].angle * Mathf.PI) / 180) * radius);
                screen[i].obj.transform.localRotation = Quaternion.Euler(screen[i].angle + 90f, 0f, 0f);
            }
            finishLoadScreen = true;
            msg = ControlCenter.ShowMessage("Login...");
        }
        if (screen.Count == 0)
            movespeed = -60f;
        else if (screen.Count == 1)
            movespeed = -30f;
        else if (screen.Count == 2)
            movespeed = -20f;
        else if (screen.Count == 3)
            movespeed = -15f;
        else if (screen.Count == 4)
            movespeed = -12f;
        else if (screen.Count == 5)
            movespeed = -6f;
        for (int i = 0; i < screen.Count; i++)
        {
            if (screen[i].angle == 360f)
                screen[i].angle = 0f;
            screen[i].angle += movespeed;//每过1帧变换多少度
            screen[i].obj.transform.localPosition = new Vector3(screen[i].obj.transform.localPosition.x, Mathf.Cos((screen[i].angle * Mathf.PI) / 180) * radius, Mathf.Sin((screen[i].angle * Mathf.PI) / 180) * radius);
            screen[i].obj.transform.localRotation = Quaternion.Euler(screen[i].angle + 90f, 0f, 0f);
        }
        if (screen.Count < 6 && (screen.Count == 0 || (screen[screen.Count - 1].angle > 29.95f) && screen[screen.Count - 1].angle < 30.05f))
        {
            float z = Mathf.Sin((90 * Mathf.PI) / 180) * radius;
            float y = Mathf.Cos((90 * Mathf.PI) / 180) * radius;
            Vector3 vec = new Vector3(0, y, z);
            Quaternion qua = Quaternion.Euler(0, 0, 0);
            GameObject get = Instantiate(Preobject, vec, qua);
            get.transform.SetParent(transform);
            get.transform.localPosition = new Vector3(get.transform.localPosition.x, Mathf.Cos((90 * Mathf.PI) / 180) * radius, Mathf.Sin((90 * Mathf.PI) / 180) * radius);
            get.transform.localRotation = Quaternion.Euler(180f, 0f, 180f);
            screen.Add(new Item() { obj = get, angle = 90f });
        }
    }

    public void addQ(int amount)
    {
        if (amount != 0)
            goods[page].quantity += (int.Parse(goods[page].quantity) + amount).ToString();
        else
            goods[page].quantity = "0";
        WWW postData = new WWW("http://central.holoworld.win/AddCart.ashx?id=" + ControlCenter.GetString("id") + "&good=" + goods[goods.Count - page - 1].id + "&quantity=" + amount, new byte[] { 0 });
    }

    public void up()
    {
        if (page < goods.Count - 1)
        {
            page++;
            if (page < goods.Count - 6)
            {
                screen[page > 5 ? page % 6 : page].obj.transform.Find("Name").gameObject.GetComponent<Text>().text = goods[page + 6].name;
                screen[page > 5 ? page % 6 : page].obj.transform.Find("Price").gameObject.GetComponent<Text>().text = "$" + goods[page + 6].price;
                screen[page > 5 ? page % 6 : page].obj.GetComponent<Renderer>().material.mainTexture = goods[page + 6].tex;
            }
            roll = -60;
        }
    }
    public void down()
    {
        if (page > 0)
        {
            page--;
            if (goods.Count > 6)
            {
                screen[page > 5 ? page % 6 : page].obj.transform.Find("Name").gameObject.GetComponent<Text>().text = goods[page + 6].name;
                screen[page > 5 ? page % 6 : page].obj.transform.Find("Price").gameObject.GetComponent<Text>().text = "$" + goods[page + 6].price;
                screen[page > 5 ? page % 6 : page].obj.GetComponent<Renderer>().material.mainTexture = goods[page + 6].tex;
            }
            roll = 60;
        }
    }
    private void loadItems()
    {
        Destroy(msg);
        Debug.Log("load items");
        ini = false;
        try
        {
            screen[0].obj.transform.Find("Name").gameObject.GetComponent<Text>().text = goods[page].name;
            screen[0].obj.transform.Find("Price").gameObject.GetComponent<Text>().text = "$" + goods[page].price;
            screen[0].obj.transform.Find("Amount").gameObject.GetComponent<Text>().text = goods[page].quantity;
            screen[0].obj.transform.Find("Show").GetComponent<Renderer>().material.mainTexture = goods[page].tex;
            try
            {
                screen[1].obj.transform.Find("Name").gameObject.GetComponent<Text>().text = goods[page + 1].name;
                screen[1].obj.transform.Find("Price").gameObject.GetComponent<Text>().text = "$" + goods[page + 1].price;
                screen[1].obj.transform.Find("Amount").gameObject.GetComponent<Text>().text = goods[page + 1].quantity;
                screen[1].obj.transform.Find("Show").GetComponent<Renderer>().material.mainTexture = goods[page + 1].tex;
            }
            catch { Destroy(screen[1].obj); }
            try
            {
                screen[2].obj.transform.Find("Name").gameObject.GetComponent<Text>().text = goods[page + 2].name;
                screen[2].obj.transform.Find("Price").gameObject.GetComponent<Text>().text = "$" + goods[page + 2].price;
                screen[2].obj.transform.Find("Amount").gameObject.GetComponent<Text>().text = goods[page + 2].quantity;
                screen[2].obj.transform.Find("Show").GetComponent<Renderer>().material.mainTexture = goods[page + 2].tex;
            }
            catch { Destroy(screen[2].obj); }
            try
            {
                screen[3].obj.transform.Find("Name").gameObject.GetComponent<Text>().text = goods[page + 3].name;
                screen[3].obj.transform.Find("Price").gameObject.GetComponent<Text>().text = "$" + goods[page + 3].price;
                screen[3].obj.transform.Find("Amount").gameObject.GetComponent<Text>().text = goods[page + 3].quantity;
                screen[3].obj.transform.Find("Show").GetComponent<Renderer>().material.mainTexture = goods[page + 3].tex;
            }
            catch { Destroy(screen[3].obj); }
            try
            {
                screen[4].obj.transform.Find("Name").gameObject.GetComponent<Text>().text = goods[page + 4].name;
                screen[4].obj.transform.Find("Price").gameObject.GetComponent<Text>().text = "$" + goods[page + 4].price;
                screen[4].obj.transform.Find("Amount").gameObject.GetComponent<Text>().text = goods[page + 4].quantity;
                screen[4].obj.transform.Find("Show").GetComponent<Renderer>().material.mainTexture = goods[page + 4].tex;
            }
            catch { Destroy(screen[4].obj); }
            try
            {
                screen[5].obj.transform.Find("Name").gameObject.GetComponent<Text>().text = goods[page + 5].name;
                screen[5].obj.transform.Find("Price").gameObject.GetComponent<Text>().text = "$" + goods[page + 5].price;
                screen[5].obj.transform.Find("Amount").gameObject.GetComponent<Text>().text = goods[page + 5].quantity;
                screen[5].obj.transform.Find("Show").GetComponent<Renderer>().material.mainTexture = goods[page + 5].tex;
            }
            catch { Destroy(screen[5].obj); }
        }
        catch(Exception ex) { Debug.Log(ex.Message); }
    }
    IEnumerator loadGoods()
    {
        WWW www = new WWW("http://central.holoworld.win/CartList.ashx?id=" + ControlCenter.GetString("id"));
        while (!www.isDone)
            yield return new WaitForSeconds(0.1f);
        string result = www.text;
        Debug.Log(result);
        if (!result.Contains("["))
        {
            ControlCenter.ShowMessage(result);
            Destroy(transform.gameObject);
            yield break;
        }
        if (result == "{\"goodId\":[],\"quantity\":[]}" || string.IsNullOrEmpty(result))
        {
            ControlCenter.ShowMessage(Language.lang.emptycart);
            Destroy(transform.gameObject);
            yield break;
        }
        string[] ids = null, qus = null;
        try
        {
            string[] a = result.Split('[');
            string[] b = new string[] { a[1].Split(']')[0].Replace("\"", ""), a[2].Split(']')[0].Replace("\"", "") };
            ids = b[0].Split(',');
            qus = b[1].Split(',');
        }
        catch (System.Exception ex)
        {
            ControlCenter.ShowMessage(Language.lang.error + "Cart: " + ex.Message);
            Destroy(transform.gameObject);
        }
        for (int i = 0; i < ids.Length; i++)
        {
            StartCoroutine(loadDetail(ids[i], qus[i]));
        }
        while (complete < ids.Length)
            yield return new WaitForSeconds(0.1f);
        ini = true;
        Debug.Log("finish load cart");
    }
    IEnumerator loadDetail(string id, string quantity)
    {
        WWW www2 = new WWW("http://central.holoworld.win/NameAndPrice.ashx?langNum=" + Language.lang.langNum + "&id=" + id);
        WWW www3 = new WWW("http://model.holoworld.win/GetModel.ashx?type=0&id=" + id);
        yield return www2;
        string nap = www2.text;
        yield return www3;
        Texture2D tex = new Texture2D(256, 256, TextureFormat.ETC_RGB4, false);
        tex.LoadImage(www3.bytes);
        tex.Compress(false);
        goods.Add(new Good { id = id, quantity = quantity, name = nap.Split(':')[0], price = nap.Split(':')[1], tex = tex });
        complete++;
    }
    public void buyThis()
    {
        ControlCenter.confirmModel(goods[page].id, goods[page].name, goods[page].quantity, goods[page].price);
        Destroy(transform.gameObject);
    }
}
