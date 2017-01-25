using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlCenter : MonoBehaviour {
    #region//变量
    public static GameObject Menu,Payment,Msg,Trans,Search,Cart,Info,Good,Zoom,Rotate,Voice;
    #endregion

    void Start () {
        Menu = GameObject.Find("Menu Canvas");
        Menu.SetActive(false);
        Payment = GameObject.Find("Payment Canvas");
        Payment.SetActive(false);
        Msg = GameObject.Find("Msg Canvas");
        Msg.SetActive(false);
        Trans = GameObject.Find("Trans Canvas");
        Trans.SetActive(false);
        Search = GameObject.Find("Search Canvas");
        Search.SetActive(false);
        Cart = GameObject.Find("Cart Canvas");
        Cart.SetActive(false);
        Info = GameObject.Find("Info Canvas");
        Info.SetActive(false);
        Good = GameObject.Find("Good Canvas");
        Good.SetActive(false);
        Zoom = GameObject.Find("Zoom Canvas");
        Zoom.SetActive(false);
        Rotate = GameObject.Find("Rotate Canvas");
        Rotate.SetActive(false);
        Voice = GameObject.Find("Voice Canvas");
        Voice.SetActive(false);
    }
	
	void Update () {
		
	}

    public void StartPayment(string name,double money,string model)//启动支付
    {
        Payment.SetActive(true);
        GameObject.Find("Payment Canvas/Panel/Name").GetComponent<Text>().text = name;
        GameObject.Find("Payment Canvas/Panel/Price").GetComponent<Text>().text = "Total: $"+money.ToString();
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
            yield return new WaitForSeconds(5);
        }

    }

    public IEnumerator UpdateImg(string url, GameObject updateobj)//加载贴图
    {
        WWW www = new WWW(url);
        yield return www;
        Texture2D txt2d = new Texture2D(4, 4, TextureFormat.DXT1, false);
        www.LoadImageIntoTexture(txt2d);
        //updateobj.GetComponent().mainTexture = txt2d;
        updateobj.GetComponent<Renderer>().material.mainTexture = txt2d;
    }

    public void ShowMessage(string msg)
    {
        Msg.SetActive(true);
        GameObject.Find("Msg Canvas/Text").GetComponent<Text>().text = msg;
    }

    public void ShowTrans()
    {
        Trans.SetActive(true);
        Menu.SetActive(false);
    }

    public void ShowVoice()
    {
        Voice.SetActive(true);
        Menu.SetActive(false);
    }

    public void ShowSearch(string searchwords)
    {
        Trans.SetActive(true);
    }

    public void ShowCart()
    {
        Cart.SetActive(true);
        Menu.SetActive(false);
    }

    public void ShowInfo()
    {
        Info.SetActive(true);
        Menu.SetActive(false);
    }

    public void ShowGood(string goodID)
    {
        Good.SetActive(true);
    }

    public void ShowZoom(GameObject gameobject)
    {
        Zoom.SetActive(true);
    }

    public void ShowRotate(GameObject gameobject)
    {
        Rotate.SetActive(true);
    }

}
