using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodDetail : MonoBehaviour {
    public string goodid;
    private string name = "null", price = "null", info = "null";
    private bool reflush = false;
    private GameObject model;
    private float rotateRate = 0.3f;//每一帧变化多少角度

    // Use this for initialization
    void Start () {
        if (ControlCenter.inied)
            return;
        StartCoroutine(getDetail(goodid));
        StartCoroutine(LoadGameObject("http://model.holoworld.win/GetModel.ashx?id=" + goodid + "&type=" + ControlCenter.device, transform.gameObject));
	}
	
	// Update is called once per frame
	void Update () {
        if (reflush)
        {
            reflush = false;
            transform.Find("Detail Panel").Find("Goodname").GetComponent<Text>().text = name;
            transform.Find("Detail Panel").Find("Price").GetComponent<Text>().text = price;
            transform.Find("Detail Panel").Find("Goodinfo").GetComponent<Text>().text = info;
        }
	}

    public IEnumerator getDetail(string id)
    {
        WWW www = new WWW("http://central.holoworld.win/GoodDetail.ashx?langNum="+Language.lang.langNum+"&id=" + id);
        while (!www.isDone)
        {
            yield return 1;
        }
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            try
            {
                string[] result = www.text.Split('翐');
                name = result[0];
                price = "$" + result[1];
                info = result[2];
                reflush = true;
            }
            catch { }
        }
    }
    public IEnumerator LoadGameObject(string path, GameObject parent)
    {
        Debug.Log(path);
        //GameObject model = null;
        WWW bundle = new WWW(path);
        while (!bundle.isDone)
        {
            transform.Find("Text").GetComponent<Text>().text = ((int)(bundle.progress * 100)).ToString() + "%";
            yield return new WaitForSeconds(0.25f);
        }
        Destroy(transform.Find("Text").gameObject);
        Debug.Log(bundle.bytesDownloaded);
        Object[] objs = bundle.assetBundle.LoadAllAssets();
        /*Texture2D texture = new Texture2D(0, 0);
        Material material = new Material("");
        foreach (Object obj in objs)//获取贴图
        {
            if (obj.GetType() == typeof(Texture2D))
            {
                texture = obj as Texture2D;
                break;
            }
        }
        foreach (Object obj in objs)//获取材质球并赋予贴图
        {
            if (obj.GetType() == typeof(Material))
            {
                material = obj as Material;
                material.mainTexture = texture;
                break;
            }
        }*/
        foreach (Object obj in objs)//获取GameObject并加载
        {
            if (obj.GetType() == typeof(GameObject))
            {
                model = Instantiate(obj) as GameObject;
                model.name = "loadedgood";
                model.transform.parent = parent.transform;//设定子物体
                yield return model;
            }
        }
        if (model != null)
        {
            
            float x = model.GetComponentInChildren<Renderer>().bounds.size.x;
            float y = model.GetComponentInChildren<Renderer>().bounds.size.y;
            float z = model.GetComponentInChildren<Renderer>().bounds.size.z;
            float largest = x > y ? x > z ? x : z : y;
            float resize = largest / 1f;//要把单轴尺寸最大值变为1
            
            float lx = model.transform.localScale.x;
            float ly = model.transform.localScale.y;
            float lz = model.transform.localScale.z;

            model.transform.localPosition = new Vector3(0f, 0f, 0f);
            Quaternion q = new Quaternion();
            q.eulerAngles.Set(0f, 0f, 0f);
            model.transform.localRotation = q;
            model.transform.localScale = new Vector3(lx / resize, ly / resize, lz / resize);
        }
        bundle.assetBundle.Unload(false);
    }
    float x = 0f, y = 0f, z = 0f;
    public void rotate(int mode)
    {
        ControlCenter.canCircle = false;
        if (model != null)
        {
            if (mode == 1)
            {
                y += 0.7f;
            }
            else if (mode == 2)
            {
                y -= 0.7f;
            }
            else if (mode == 3)
            {
                x -= 0.7f;
            }
            else if (mode == 4)
            {
                x += 0.7f;
            }
            model.transform.localRotation = Quaternion.Euler(x, y, z);
        }
    }
    public void addCart()
    {
        WWW postData = new WWW("http://central.holoworld.win/AddCart.ashx?id=" + ControlCenter.GetString("id") + "&good=" + goodid + "&quantity=1", new byte[] { 0 });
        Destroy(transform.gameObject);
    }
    public void buyThis()
    {
        ControlCenter.confirmModel(goodid, name, "1", price);//这里有点小bug，购买数量不能设置，只能为1
        try { Destroy(GameObject.Find("search_clone")); } catch { }
        Destroy(transform.gameObject);
    }
    public void readInfo()
    {
        SoundsControl.playText(info);
    }
}
