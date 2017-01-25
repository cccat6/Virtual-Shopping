using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Gvr.Internal;

public class PaymentControl : MonoBehaviour {
    public static string paypassword = null;
    public static bool shifton = false;
    private Image image;

    public static void switchshift()
    {
        shifton = !shifton;
    }
    
    List<GameObject> K = new List<GameObject>();
    void GetKList()
    {
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#1"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#2"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#3"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#4"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#5"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#6"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#7"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#8"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#9"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#10"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#11"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#12"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#13"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#14"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#15"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#16"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#17"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#18"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#19"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#20"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#21"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#22"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#23"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#24"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#25"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#26"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#27"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#28"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#29"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#30"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#31"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#32"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#33"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#34"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#35"));
    K.Add(GameObject.Find("Payment Canvas/Panel/Keyboard/#36"));
    }
    void SetKey(GameObject gameobject,int KeyNum)
    {
        switch (KeyNum)
        {
            case 1:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "1";
                break;
            case 2:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "2";
                break;
            case 3:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "3";
                break;
            case 4:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "4";
                break;
            case 5:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "5";
                break;
            case 6:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "6";
                break;
            case 7:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "7";
                break;
            case 8:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "8";
                break;
            case 9:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "9";
                break;
            case 10:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "0";
                break;
            case 11:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "A";
                break;
            case 12:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "B";
                break;
            case 13:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "C";
                break;
            case 14:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "D";
                break;
            case 15:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "E";
                break;
            case 16:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "F";
                break;
            case 17:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "G";
                break;
            case 18:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "H";
                break;
            case 19:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "I";
                break;
            case 20:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "J";
                break;
            case 21:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "K";
                break;
            case 22:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "L";
                break;
            case 23:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "M";
                break;
            case 24:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "N";
                break;
            case 25:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "O";
                break;
            case 26:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "P";
                break;
            case 27:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "Q";
                break;
            case 28:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "R";
                break;
            case 29:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "S";
                break;
            case 30:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "T";
                break;
            case 31:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "U";
                break;
            case 32:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "V";
                break;
            case 33:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "W";
                break;
            case 34:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "X";
                break;
            case 35:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "Y";
                break;
            case 36:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "Z";
                break;
            default:
                gameobject.transform.Find("Text").GetComponent<Text>().text = "#";
                break;
        }
    }

    // Use this for initialization
    void Start () {
        GetKList();
        //Debug.Log(K);
        List<int> list = new List<int>();
        System.Random rm = new System.Random();
        //int RmNum = 36;
        for (int i = 0; i<36; i++)
        {
            int num = rm.Next(1, 37);
            if (!list.Contains(num))
                list.Add(num);
            else
                i--;
        }
        //以上是为键盘生成随机排序
        for(int i=0;i<=36;i++)
        {
            SetKey(K[i], list[i]);
        }

        image = GetComponent<Image>();
    }
    
    // Update is called once per frame
    void Update () {
        if (shifton)
        {
            GameObject.Find("Payment Canvas/Panel/Keyboard/Shift/Text").GetComponent<Text>().text = "SHIFT";
        }
        else
        {
            GameObject.Find("Payment Canvas/Panel/Keyboard/Shift/Text").GetComponent<Text>().text = "Shift";
        }

        string passshow = null;
        for (int i = 0; i < paypassword.Length; i++)
        {
            passshow += "*";
        }
        GameObject.Find("Payment Canvas/Panel/Password/Text").GetComponent<Text>().text = (paypassword == "" ? "Password" : passshow);
    }
}
