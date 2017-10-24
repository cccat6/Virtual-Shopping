using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour {

    /// <summary>
    /// 0是英文，1是中文
    /// </summary>
    public static Lang lang = new Lang();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void ini(int LangNum)
    {
        if (LangNum == 0)
            lang = new Lang_EN().toLang();
        else if (LangNum == 1)
            lang = new Lang_CN().toLang();

        ControlCenter.Payment.SetActive(true);
        ControlCenter.Msg.SetActive(true);
        ControlCenter.Trans.SetActive(true);
        ControlCenter.Cart.SetActive(true);
        ControlCenter.Info.SetActive(true);
        ControlCenter.Good.SetActive(true);
        ControlCenter.NewOrder.SetActive(true);
        ControlCenter.Model.SetActive(true);

        ControlCenter.Payment.transform.Find("Head").Find("Text").GetComponent<Text>().text = lang.P;
        ControlCenter.Payment.transform.Find("Panel").Find("Continue").Find("Text").GetComponent<Text>().text = lang.continue_;
        ControlCenter.Payment.transform.Find("Panel").Find("Cancel").Find("Text").GetComponent<Text>().text = lang.cancel;
        ControlCenter.Msg.transform.Find("Continue").Find("Text").GetComponent<Text>().text = lang.continue_;
        ControlCenter.Trans.transform.Find("Head").Find("Text").GetComponent<Text>().text = lang.LI;
        ControlCenter.Trans.transform.Find("Panel").Find("Head").Find("Text").GetComponent<Text>().text = lang.details;
        ControlCenter.Trans.transform.Find("Panel").Find("Up").Find("Text").GetComponent<Text>().text = lang.up;
        ControlCenter.Trans.transform.Find("Panel").Find("Down").Find("Text").GetComponent<Text>().text = lang.down;
        ControlCenter.Trans.transform.Find("Close Button").Find("Text").GetComponent<Text>().text = lang.close;
        ControlCenter.Cart.transform.Find("Buy").Find("Text").GetComponent<Text>().text = lang.buy;
        ControlCenter.Cart.transform.Find("Close").Find("Text").GetComponent<Text>().text = lang.close;
        ControlCenter.Info.transform.Find("Head").Find("Text").GetComponent<Text>().text = lang.PI;
        ControlCenter.Info.transform.Find("Grid Panel").Find("Order").Find("Text").GetComponent<Text>().text = lang.order;
        ControlCenter.Info.transform.Find("Up").Find("Text").GetComponent<Text>().text = lang.up;
        ControlCenter.Info.transform.Find("Down").Find("Text").GetComponent<Text>().text = lang.down;
        ControlCenter.Info.transform.Find("Close Button").Find("Text").GetComponent<Text>().text = lang.close;
        ControlCenter.Good.transform.Find("Head").Find("Text").GetComponent<Text>().text = lang.GD;
        ControlCenter.Good.transform.Find("Add Cart").Find("Text").GetComponent<Text>().text = lang.addcart;
        ControlCenter.Good.transform.Find("Buynow Button").Find("Text").GetComponent<Text>().text = lang.buy;
        ControlCenter.Good.transform.Find("Read Button").Find("Text").GetComponent<Text>().text = lang.read;
        ControlCenter.NewOrder.transform.Find("Head").Find("Text").GetComponent<Text>().text = lang.CO;
        ControlCenter.NewOrder.transform.Find("Panel").Find("Address").GetComponent<Text>().text = lang.needselectaddress;
        ControlCenter.NewOrder.transform.Find("Panel").Find("Continue").Find("Text").GetComponent<Text>().text = lang.continue_;
        ControlCenter.NewOrder.transform.Find("Panel").Find("Cancel").Find("Text").GetComponent<Text>().text = lang.cancel;
        ControlCenter.NewOrder.transform.Find("Panel").Find("Panel").Find("Up").Find("Text").GetComponent<Text>().text = lang.up;
        ControlCenter.NewOrder.transform.Find("Panel").Find("Panel").Find("Down").Find("Text").GetComponent<Text>().text = lang.down;
        ControlCenter.Model.transform.Find("Head").Find("Text").GetComponent<Text>().text = lang.CM;
        ControlCenter.Model.transform.Find("Panel").Find("Model").GetComponent<Text>().text = lang.needselectmodel;
        ControlCenter.Model.transform.Find("Panel").Find("Continue").Find("Text").GetComponent<Text>().text = lang.continue_;
        ControlCenter.Model.transform.Find("Panel").Find("Cancel").Find("Text").GetComponent<Text>().text = lang.cancel;
        ControlCenter.Model.transform.Find("Panel").Find("Panel").Find("Up").Find("Text").GetComponent<Text>().text = lang.up;
        ControlCenter.Model.transform.Find("Panel").Find("Panel").Find("Down").Find("Text").GetComponent<Text>().text = lang.down;

        ControlCenter.Payment.SetActive(false);
        ControlCenter.Msg.SetActive(false);
        ControlCenter.Trans.SetActive(false);
        ControlCenter.Cart.SetActive(false);
        ControlCenter.Info.SetActive(false);
        ControlCenter.Good.SetActive(false);
        ControlCenter.NewOrder.SetActive(false);
        ControlCenter.Model.SetActive(false);
    }

    public class Lang
    {
        public int langNum { get; set; }
        public string voice { get; set; }
        public string langCode { get; set; }

        public string welcome { get; set; }
        public string needup { get; set; }
        public string total { get; set; }
        public string buy { get; set; }
        public string addcart { get; set; }
        public string close { get; set; }
        public string cancel { get; set; }
        public string continue_ { get; set; }
        public string read { get; set; }
        public string up { get; set; }
        public string down { get; set; }
        public string details { get; set; }
        public string order { get; set; }
        public string CO { get; set; }
        public string P { get; set; }
        public string LI { get; set; }
        public string GD { get; set; }
        public string PI { get; set; }
        public string CM { get; set; }

        public string emptycart { get; set; }
        public string notconnecttoserver { get; set; }
        public string needlogin { get; set; }
        public string newvirsonavaliavle { get; set; }
        public string failloaddata { get; set; }
        public string illegaloperate { get; set; }
        public string errororder { get; set; }
        public string notfindgood { get; set; }
        public string responseerror { get; set; }
        public string loginfail { get; set; }
        public string needselectaddress { get; set; }
        public string needselectmodel { get; set; }
        public string creatingorder { get; set; }
        public string usernotexist { get; set; }
        public string havenotaddress { get; set; }
        public string paying { get; set; }
        public string unlogin { get; set; }
        public string error { get; set; }
    }
    private class Lang_EN
    {
        int langNum = 0;
        string voice = "<speak version='1.0' xml:lang='en-US'><voice xml:lang='en-US' xml:gender='Female' name='Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)'>";
        string langCode = "en-US";

        string welcome = "welcome to use virtual shopping, it was made by team holo-world";
        string needup = "virtual shopping needs to update, please download the new virson on our website";
        string total = "Total";
        string buy = "Buy";
        string addcart = "Add Cart";
        string close = "Close";
        string cancel = "Cancel";
        string continue_ = "Continue";
        string read = "Read";
        string up = "UP";
        string down = "DOWN";
        string details = "Details";
        string order = "Order";
        string CO = "Confirm Order";
        string P = "Payment";
        string LI = "Logistic Info";
        string GD = "Good Detail";
        string PI = "Personal Info";
        string CM = "Confirm Model";

        string emptycart = "Your cart is empty";
        string notconnecttoserver = "Could not connect to the server";
        string needlogin = "Please login to your account";
        string newvirsonavaliavle = "New virson is avaliable";
        string failloaddata = "Fail load data";
        string illegaloperate = "Illegal operate!";
        string errororder = "Error order!";
        string notfindgood = "Sorry, we could not find the good that you search.\r\nPlease restatement your search request.";
        string responseerror = "Response error";
        string loginfail = "Login fail";
        string needselectaddress = "Please select a receive address";
        string needselectmodel = "Please select a model";
        string creatingorder = "Creating order...";
        string usernotexist = "User not exist";
        string havenotaddress = "You have not any receive address\r\nCould not creat order";
        string paying = "Paying...";
        string unlogin = "User not login";
        string error = "Error";

        public Lang toLang()
        {
            return new Lang { needselectmodel=needselectmodel, CM=CM, langCode = langCode, voice =voice,addcart = addcart, cancel = cancel, notfindgood = notfindgood, langNum = langNum, welcome = welcome, needup = needup, total = total, buy = buy, close = close, continue_ = continue_, read = read, up = up, down = down, details = details, order = order, CO = CO, P = P, LI = LI, GD = GD, PI = PI, emptycart = emptycart, notconnecttoserver = notconnecttoserver, needlogin = needlogin, newvirsonavaliavle = newvirsonavaliavle, failloaddata = failloaddata, illegaloperate = illegaloperate, errororder = errororder, responseerror = responseerror, loginfail = loginfail, needselectaddress = needselectaddress, creatingorder = creatingorder, usernotexist = usernotexist, havenotaddress = havenotaddress, paying = paying, unlogin = unlogin, error = error };
        }
    }
    private class Lang_CN
    {
        int langNum = 1;
        string voice = "<speak version='1.0' xml:lang='zh-CN'><voice xml:lang='zh-CN' xml:gender='Female' name='Microsoft Server Speech Text to Speech Voice (zh-CN, HuihuiRUS)'>";
        string langCode = "zh-CN";

        string welcome = "欢迎使用虚拟购物，本程序由HoloWorld团队制作";
        string needup = "应用需要更新，请前往官网下载最新版本";
        string total = "合计";
        string buy = "立即购买";
        string addcart = "加入购物车";
        string close = "关闭";
        string cancel = "取消";
        string continue_ = "继续";
        string read = "朗读";
        string up = "向上";
        string down = "向下";
        string details = "详细信息";
        string order = "订单信息";
        string CO = "确认订单";
        string P = "支付";
        string LI = "物流信息";
        string GD = "商品信息";
        string PI = "个人信息";
        string CM = "选择型号";

        string emptycart = "您的购物车是空的";
        string notconnecttoserver = "无法连接到服务器";
        string needlogin = "请登录您的帐户";
        string newvirsonavaliavle = "应用有新版本";
        string failloaddata = "加载数据失败";
        string illegaloperate = "非法操作！";
        string errororder = "错误的订单！";
        string notfindgood = "未找到商品，请重新尝试";
        string responseerror = "返回了错误";
        string loginfail = "登录失败";
        string needselectaddress = "请选择一个收货地址";
        string needselectmodel = "请选择一个型号";
        string creatingorder = "创建订单中。。。";
        string usernotexist = "用户不存在";
        string havenotaddress = "您没有收货地址，无法创建订单";
        string paying = "支付中。。。";
        string unlogin = "用户未登录";
        string error = "错误";

        public Lang toLang()
        {
            return new Lang { needselectmodel=needselectmodel, CM=CM, langCode = langCode, voice = voice, addcart = addcart, cancel = cancel, notfindgood = notfindgood, langNum = langNum, welcome = welcome, needup = needup, total = total, buy = buy, close = close, continue_ = continue_, read = read, up = up, down = down, details = details, order = order, CO = CO, P = P, LI = LI, GD = GD, PI = PI, emptycart = emptycart, notconnecttoserver = notconnecttoserver, needlogin = needlogin, newvirsonavaliavle = newvirsonavaliavle, failloaddata = failloaddata, illegaloperate = illegaloperate, errororder = errororder, responseerror = responseerror, loginfail = loginfail, needselectaddress = needselectaddress, creatingorder = creatingorder, usernotexist = usernotexist, havenotaddress = havenotaddress, paying = paying, unlogin = unlogin, error = error };
        }
    }
    private class Lang_BACK
    {
        int langNum = -1;
        string voice = "";
        string langCode = "";

        string welcome = "";
        string needup = "";
        string total = "";
        string buy = "";
        string addcart = "";
        string close = "";
        string cancel = "";
        string continue_ = "";
        string read = "";
        string up = "";
        string down = "";
        string details = "";
        string order = "";
        string CO = "";
        string P = "";
        string LI = "";
        string GD = "";
        string PI = "";
        string CM = "";

        string emptycart = "";
        string notconnecttoserver = "";
        string needlogin = "";
        string newvirsonavaliavle = "";
        string failloaddata = "";
        string illegaloperate = "";
        string errororder = "";
        string notfindgood = "";
        string responseerror = "";
        string loginfail = "";
        string needselectaddress = "";
        string needselectmodel = "";
        string creatingorder = "";
        string usernotexist = "";
        string havenotaddress = "";
        string paying = "";
        string unlogin = "";
        string error = "";

        public Lang toLang()
        {
            return new Lang { needselectmodel=needselectmodel, CM = CM, langCode = langCode, voice = voice, addcart =addcart,cancel=cancel,notfindgood = notfindgood, langNum = langNum, welcome = welcome, needup = needup, total = total, buy = buy, close = close, continue_ = continue_, read = read, up = up, down = down, details = details, order = order, CO = CO, P = P, LI = LI, GD = GD, PI = PI, emptycart = emptycart, notconnecttoserver = notconnecttoserver, needlogin = needlogin, newvirsonavaliavle = newvirsonavaliavle, failloaddata = failloaddata, illegaloperate = illegaloperate, errororder = errororder, responseerror = responseerror, loginfail = loginfail, needselectaddress = needselectaddress, creatingorder = creatingorder, usernotexist = usernotexist, havenotaddress = havenotaddress, paying = paying, unlogin = unlogin, error = error };
        }
    }
}
