using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Hands;
using UnityEngine.UI;

public class ReceiveHand : MonoBehaviour {
    public static bool stillworking, showingL;//showingL是是否已经开始展示
    public static Image image, imageL;
    public static float duration, shration, stdistance = 0f;
    public static GameObject showL = null;//左手上的模型
    public static Vector3 stsize,positionR=new Vector3();

    public static GameObject selected;

    // Use this for initialization
    void Start () {
        TCP_Server.server();
        image = GameObject.Find("Hands/Twohands Canvas/Image").GetComponent<Image>();//唤起菜单的进度条
        imageL = GameObject.Find("Hands/Twohands Canvas/Image2").GetComponent<Image>();//左手观察商品的进度条，在左手上，需要修改，暂时用这个
    }
	
	// Update is called once per frame
	void Update () {
        #region//点击，允许双手
        if (position.LS == 3 || position.RS == 3)
        {
            NohandClick.DoClick();
        }
        #endregion
        #region//双手向上，唤起菜单
        if (position.LS == 2 && position.RS == 2)
        {
            duration += 0.015f;
            if (duration <= 1.2f)
                image.fillAmount = duration <= 1 ? duration : 1f;
            else
            {
                image.fillAmount = 0f;
                ControlCenter.menu();
                duration = 0f;
            }
        }
        else
        {
            duration = 0f;
            image.fillAmount = duration;
        }
        #endregion
        #region//左手抓住，拖动菜单
        //Debug.Log(position.LS+" "+ position.RS+ selected.name);
        if (position.LS == 1 && position.RS == 0 && selected != null)
        {
            Vector3 UIposition = new Vector3(
                Hands.Hands.L.transform.position.x * 3f,
                Hands.Hands.L.transform.position.y * 5f - 4f > 1.3f ? Hands.Hands.L.transform.position.y * 5f - 4f : 1.3f,
                Hands.Hands.L.transform.position.z * 4f - 1.5f);
            MoveUI.SetTo(UIposition, Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y);
        }
        #endregion
        #region//左手向上，展示商品，旋转
        if (position.LS == 2 && !showingL && ControlCenter.objectExistActive("good_clone/loadedgood"))
        {
            shration += 0.015f;
            if (shration <= 1.2f)
                imageL.fillAmount = shration <= 1 ? shration : 1f;
            else
            {
                imageL.fillAmount = 0f;

                if (showL == null)
                {
                    //复制模型到左手上，注意要给showL赋值
                    showL = Instantiate(GameObject.Find("good_clone/loadedgood"));
                    showL.name = "loadedgood";

                    float x = showL.GetComponentInChildren<Renderer>().bounds.size.x;
                    float y = showL.GetComponentInChildren<Renderer>().bounds.size.y;
                    float z = showL.GetComponentInChildren<Renderer>().bounds.size.z;
                    float largest = x > y ? x > z ? x : z : y;
                    float resize = largest / 0.1f;//要把单轴尺寸最大值变为0.1

                    float lx = showL.transform.localScale.x;
                    float ly = showL.transform.localScale.y;
                    float lz = showL.transform.localScale.z;

                    showL.transform.localPosition = new Vector3(0f, 0f, 0f);
                    Quaternion q = new Quaternion();
                    q.eulerAngles.Set(0f, 0f, 0f);
                    showL.transform.localRotation = q;
                    showL.transform.localScale = new Vector3(lx / resize, ly / resize, lz / resize);

                    Vector3 positionL = new Vector3(
                        Hands.Hands.L.transform.position.x,
                        Hands.Hands.L.transform.position.y + 0.2f,
                        Hands.Hands.L.transform.position.z);
                    showL.transform.position = positionL;//设置复制的模型位置
                }
                if (position.RS == 1)
                {
                    if (positionR.Equals(new Vector3()))
                        positionR = Hands.Hands.R.transform.position;
                    Vector3 nowpositionR = Hands.Hands.R.transform.position;
                    Vector3 nowrotation = showL.transform.localRotation.eulerAngles;
                    nowrotation.x += nowpositionR.x - positionR.x;//每帧都会按照变化旋转，可能需要常数来调整速度，可能需要修改
                    nowrotation.y += nowpositionR.z - positionR.z;
                    nowrotation.z += nowpositionR.y - positionR.y;
                    showL.transform.localRotation = Quaternion.Euler(nowrotation);//设置旋转
                }
                else
                {
                    positionR = new Vector3();
                }
            }
        }
        else
        {
            shration = 0f;
            imageL.fillAmount = shration;

            if (showL != null)
            {
                Destroy(showL);
            }
        }
        #endregion
        #region//双手抓住，缩放
        if (position.LS == 1 && position.RS == 1 && ControlCenter.objectExistActive("good_clone/loadedgood"))
        {
            if (stdistance == 0f)
            {
                stdistance = Mathf.Sqrt(
                    Mathf.Pow(Hands.Hands.L.transform.position.x - Hands.Hands.R.transform.position.x, 2f) +
                    Mathf.Pow(Hands.Hands.L.transform.position.y - Hands.Hands.R.transform.position.y, 2f) +
                    Mathf.Pow(Hands.Hands.L.transform.position.z - Hands.Hands.R.transform.position.z, 2f));
                stsize = GameObject.Find("good_clone/loadedgood").transform.localScale;
            }
            float nowdistance = Mathf.Sqrt(
                    Mathf.Pow(Hands.Hands.L.transform.position.x - Hands.Hands.R.transform.position.x, 2f) +
                    Mathf.Pow(Hands.Hands.L.transform.position.y - Hands.Hands.R.transform.position.y, 2f) +
                    Mathf.Pow(Hands.Hands.L.transform.position.z - Hands.Hands.R.transform.position.z, 2f));
            float rate = nowdistance / stdistance * 0.5f;//变化相对原大小的比例，可能需要修改
            Vector3 newsize = new Vector3(stsize.x * rate, stsize.y * rate, stsize.z * rate);
            GameObject.Find("good_clone/loadedgood").transform.localScale = newsize;
        }
        else
        {
            stdistance = 0f;
        }
        #endregion
    }

    private RaycastHit objhit;
    private Ray _ray;
}
public class TCP_Server
{
    private static byte[] result = new byte[1024];
    private static int myProt = 9000;   //端口9000
    static Socket serverSocket;

    public static int Raction_before, Raction_not_count;
    public static int Laction_before, Laction_not_count;
    public static int ClickColddown;
    public static int handExiseColddown;


    public static void server()
    {
        ReceiveHand.stillworking = true;
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, myProt));  //绑定IP地址：端口  
        serverSocket.Listen(2);    //设定最多2个排队连接请求  
                                    //通过Clientsoket发送数据  
        Thread myThread = new Thread(ListenClientConnect);
        myThread.Start();
    }

    private static void ListenClientConnect()
    {
        while (true)
        {
            Socket clientSocket = serverSocket.Accept();
            clientSocket.Send(Encoding.ASCII.GetBytes("connected"));
            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start(clientSocket);
            try { if (!ReceiveHand.stillworking) break; }
            catch { break; }
        }
    }

    private static void ReceiveMessage(object clientSocket)
    {
        Socket myClientSocket = (Socket)clientSocket;
        while (true)
        {
            try
            {
                //myClientSocket.Send(result);
                //通过clientSocket接收数据  
                int receiveNumber = myClientSocket.Receive(result);
                string data = Encoding.ASCII.GetString(result, 0, receiveNumber);
                //if (data=="quit")//收到退出线程命令
                //{
                //    Thread.ResetAbort();
                //}
                update(data);
                //myClientSocket.Send(result);
            }
            catch (Exception ex)
            {
                myClientSocket.Send(Encoding.ASCII.GetBytes(ex.ToString()));
                myClientSocket.Send(result);
                myClientSocket.Shutdown(SocketShutdown.Both);
                myClientSocket.Close();
                Debug.Log(ex.Message);
                break;
            }
            try { if (!ReceiveHand.stillworking) break; }
            catch { break; }
        }
    }
    private static void update(string data)
    {//2,0.3348409,2.465194,2.516141,0,
        string[] split = data.Split(',');
        List<float> info = new List<float>();
        for (int i = 0; i < split.Length-1; i++)
        {
            try { info.Add(float.Parse(split[i])); }
            catch { }
        }

        if (info[0] == 0 || handExiseColddown==0)
        {
            position.hands = 0;
            position.Laction = 0;
            position.Raction = 0;
        }
        else if (info[0] == 1 || handExiseColddown ==1)
        {
            position.hands = 1;
            position.Lx = info[1] * position.Rate + 0.2f;
            position.Ly = info[2] * position.Rate + 0f;
            position.Lz = info[3] * position.Rate - 0.5f;
            position.Laction = (int)info[4];
            position.Raction = 0;
        }
        else if (info[0] == 2 || handExiseColddown ==2)
        {
            position.hands = 2;
            position.Rx = info[1] * position.Rate - 0.2f;
            position.Ry = info[2] * position.Rate + 0f;
            position.Rz = info[3] * position.Rate - 0.5f;
            position.Raction = (int)info[4];
            position.Laction = 0;
        }
        else if (info[0] == 3 || handExiseColddown ==3)
        {
            position.hands = 3;
            position.Lx = info[1] * position.Rate + 0.2f;
            position.Ly = info[2] * position.Rate + 0f;
            position.Lz = info[3] * position.Rate - 0.5f;
            position.Laction = (int)info[4];
            position.Rx = info[5] * position.Rate - 0.2f;
            position.Ry = info[6] * position.Rate + 0f;
            position.Rz = info[7] * position.Rate - 0.5f;
            position.Raction = (int)info[8];
        }
        handExiseColddown = (int)info[0];

        if (Laction_before != position.Laction)
            Laction_not_count++;
        else
            Laction_not_count = 0;

        if (Raction_before != position.Raction)
            Raction_not_count++;
        else
            Raction_not_count = 0;
        
        if(Laction_not_count>3)//有个允许样本误差修正数
        {
            Laction_before = position.Laction;
            Laction_not_count = 0;
        }

        if (Raction_not_count > 3)//有个允许样本误差修正数
        {
            Raction_before = position.Raction;
            Raction_not_count = 0;
        }
        ClickColddown--;//冷却减少
        if (position.LS == 3 || ClickColddown > 0)
        {
            position.LS = 0;
            return;
        }
        if (position.LS == 3 && ClickColddown == 0)
        {
            ClickColddown = 3;//冷却3个样本
        }
        if (position.RS == 3 || ClickColddown > 0)
        {
            position.RS = 0;
            return;
        }
        if (position.RS == 3 && ClickColddown == 0)
        {
            ClickColddown = 3;//冷却3个样本
        }
        position.LS = Laction_before;
        position.RS = Raction_before;

    }
}
