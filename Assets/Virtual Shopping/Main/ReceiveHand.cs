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
    public static bool stillworking;
    private Image image;
    private float duration;

    public static GameObject selected;

    // Use this for initialization
    void Start () {
        TCP_Server.server();
        image = GameObject.Find("Hands/Twohands Canvas/Image").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        #region//点击，允许双手
        if (position.LS == 3 || position.RS == 3)
        {
            DoClick();
        }
        #endregion
        #region//双手向上，唤起菜单
        if (position.LS == 2 && position.RS == 2 && !ControlCenter.Menu.activeInHierarchy)
        {
            duration += 0.015f;
            if (duration <= 1.2f)
                image.fillAmount = duration <= 1 ? duration : 1f;
            else
            {
                image.fillAmount = 0f;
                ControlCenter.Menu.SetActive(true);
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
    }
    
    private RaycastHit objhit;
    private Ray _ray;

    public void DoClick()
    {
        //try
        //{
            NohandClick.gameObj.SendMessageUpwards("Clicked");//用Upwards的原因是，有时碰撞到的物体是按钮的子物体，如果这样出bug可以去掉UpwardRay _ray = new Ray(transform.position, transform.forward);
        //}
        /*//以下是为了移动UI，如果点击不成功，则尝试移动
        catch
        {
            NohandClick.UI.SendMessageUpwards("Clicked");
        }*/
    }
}
public class TCP_Server
{
    private static byte[] result = new byte[1024];
    private static int myProt = 9000;   //端口9001 
    static Socket serverSocket;

    public static int Raction_before, Raction_not_count;
    public static int Laction_before, Laction_not_count;
    public static int ClickColddown;


    public static void server()
    {
        ReceiveHand.stillworking = true;
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
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

        if (info[0] == 0)
        {
            position.hands = 0;
            position.Laction = 0;
            position.Raction = 0;
        }
        else if (info[0] == 1)
        {
            position.hands = 1;
            position.Lx = info[1] * position.Rate + 0.2f;
            position.Ly = info[2] * position.Rate + 0f;
            position.Lz = info[3] * position.Rate - 0.5f;
            position.Laction = (int)info[4];
            position.Raction = 0;
        }
        else if (info[0] == 2)
        {
            position.hands = 2;
            position.Rx = info[1] * position.Rate - 0.2f;
            position.Ry = info[2] * position.Rate + 0f;
            position.Rz = info[3] * position.Rate - 0.5f;
            position.Raction = (int)info[4];
            position.Laction = 0;
        }
        else if (info[0] == 3)
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
