using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmModel : MonoBehaviour {
    private GameObject showpic, goodname, price, quantity, model, mod1, mod2, mod3;
    public string goodid, goodnames, quanlities, prices;//new这个类的时候一定要填写这几个参数，在克隆还没有可用的时候就要填写
    private string[] models = new string[0];
    private bool needLoadModel = true;
    private int page = 0;
    private bool canNextPage = false, canLastPage = false;

    // Use this for initialization
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

        showpic = transform.Find("Panel").Find("Item").Find("Show").gameObject;
        goodname = transform.Find("Panel").Find("Item").Find("Name").gameObject;
        price = transform.Find("Panel").Find("Item").Find("Price").gameObject;
        quantity = transform.Find("Panel").Find("Item").Find("Amount").gameObject;
        model = transform.Find("Panel").Find("Model").gameObject;
        mod1 = transform.Find("Panel").Find("Panel").Find("Model1").gameObject;
        mod2 = transform.Find("Panel").Find("Panel").Find("Model2").gameObject;
        mod3 = transform.Find("Panel").Find("Panel").Find("Model3").gameObject;

        StartCoroutine(ControlCenter.UpdateImg("http://model.holoworld.win/GetModel.ashx?type=0&id=" + goodid, showpic));
        goodname.GetComponent<Text>().text = goodnames;
        price.GetComponent<Text>().text = prices;
        quantity.GetComponent<Text>().text = quanlities;
        model.GetComponent<Text>().text = Language.lang.needselectmodel;
        StartCoroutine(loadModels());
        mod1.SetActive(false);
        mod2.SetActive(false);
        mod3.SetActive(false);
    }
	
	// Update is called once per frame
	void Update (){
        if (needLoadModel)
        {
            try
            {
                mod2.SetActive(false);
                mod3.SetActive(false);
                mod1.transform.GetChild(0).gameObject.GetComponent<Text>().text = models[3 * page];
                mod1.SetActive(true);
                mod2.transform.GetChild(0).gameObject.GetComponent<Text>().text = models[3 * page + 1];
                mod2.SetActive(true);
                mod3.transform.GetChild(0).gameObject.GetComponent<Text>().text = models[3 * page + 2];
                mod3.SetActive(true);
                if (page != 0)
                    canLastPage = true;
            }
            catch { canLastPage = false; }
            needLoadModel = false;
        }
    }

    IEnumerator loadModels()
    {
        WWW www = new WWW("http://central.holoworld.win/GoodModel.ashx?langNum=" + Language.lang.langNum + "&id=" + goodid);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        if (result.Contains(";"))
            models = result.Split(';');
        else
            models = new string[] { result };
        if (models.Length < 3)
            canNextPage = false;
        needLoadModel = true;
    }
    public void go()
    {
        if (model.GetComponent<Text>().text == Language.lang.needselectaddress)
        {
            ControlCenter.ShowMessage(Language.lang.needselectaddress);
            return;
        }
        ControlCenter.buildOrder(goodid, goodnames, quanlities, prices, model.GetComponent<Text>().text);
        Destroy(transform.gameObject);
    }
    public void up()
    {
        if (canLastPage)
        {
            page--;
            needLoadModel = true;
        }
    }
    public void down()
    {
        if (canLastPage)
        {
            page++;
            needLoadModel = true;
        }
    }
}
