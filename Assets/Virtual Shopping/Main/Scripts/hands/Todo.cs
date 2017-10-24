using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hands;


public class Todo : MonoBehaviour {

    GameObject selected;

    // Use this for initialization
    void Start()
     {
        //var button = /*transform.gameObject.*/GetComponent<Button>();//更改点击事件触发方法，为调用SoftClick
        /*var button = selected.GetComponent<Button>();//更改点击事件触发方法，为调用SoftClick
        if (button != null)
         {
             button.onClick.RemoveAllListeners();
             button.onClick.AddListener(SoftClick);
         }*/
     }
	
	// Update is called once per frame
	void Update () {
        selected = findGameObject();

        /*isCatch();
        isRotate();
        isChangeSize();
        isLUP();
        isBUp();
        UpClear();
        isClick();*/
	}
    /*
    #region
    int beforec5 = -1, beforec4 = -1, beforec3 = -1, beforec2 = -1, beforec1 = -1;//点击除抖
    public void isClick()//点击
    {
        beforec5 = beforec4;
        beforec4 = beforec3;
        beforec3 = beforec2;
        beforec2 = beforec1;
        beforec1 = position.RS;
        if (beforec4 == 3 && beforec3 == 3 && beforec3 == 3 && beforec2 == 0 && beforec1 == 0)
        {
            //SoftClick();
            NohandClick.DoClick();
        }
    }
    int beforelc3 = -1, beforelc2 = -1, beforelc1 = -1;//左手拈花指除抖
    int beforerc3 = -1, beforerc2 = -1, beforerc1 = -1;//右手拈花指除抖
    public void isCatch()//拖拽，移动菜单
    {
        if ((beforelc3 == 1 || beforelc2 == 1 || beforelc1 == 1) && (beforerc3 != 1 && beforerc2 != 1 && beforerc1 != 1))
        {

        }
    }
    public void isRotate()//旋转，滑动菜单
    {
        if ((beforelc3 != 1 && beforelc2 != 1 && beforelc1 != 1) && (beforerc3 == 1 || beforerc2 == 1 || beforerc1 == 1))
        {

        }
    }
    public void isChangeSize()//缩放
    {
        if ((beforelc3 == 1 || beforelc2 == 1 || beforelc3 == 1) && (beforerc3 == 1 || beforerc2 == 1 || beforerc1 == 1))
        {

        }
    }
    int beforelu3 = -1, beforelu2 = -1, beforelu1 = -1;//左手向上除抖
    int beforeru3 = -1, beforeru2 = -1, beforeru1 = -1;//右手向上除抖
    public int lc = 0, rc = 0;//左右手帧数计数器
    public void isLUP()//左手向上，物体手上
    {
        if ((beforelu3 == 1 || beforelu2 == 1 || beforelu1 == 1) && (beforeru3 != 1 && beforeru2 != 1 && beforeru1 != 1))
        {
            ReceiveHand.shration += 0.015f;
            if (ReceiveHand.shration <= 1.2f)
                ReceiveHand.imageL.fillAmount = ReceiveHand.shration <= 1 ? ReceiveHand.shration : 1f;
            else
            {
                ReceiveHand.imageL.fillAmount = 0f;

                if (ReceiveHand.showL == null)
                {
                    //复制模型到左手上，注意要给showL赋值
                    ReceiveHand.showL = Instantiate(GameObject.Find("good_clone/loadedgood"));
                    ReceiveHand.showL.name = "loadedgood";

                    float x = ReceiveHand.showL.GetComponentInChildren<Renderer>().bounds.size.x;
                    float y = ReceiveHand.showL.GetComponentInChildren<Renderer>().bounds.size.y;
                    float z = ReceiveHand.showL.GetComponentInChildren<Renderer>().bounds.size.z;
                    float largest = x > y ? x > z ? x : z : y;
                    float resize = largest / 0.1f;//要把单轴尺寸最大值变为0.1

                    float lx = ReceiveHand.showL.transform.localScale.x;
                    float ly = ReceiveHand.showL.transform.localScale.y;
                    float lz = ReceiveHand.showL.transform.localScale.z;

                    ReceiveHand.showL.transform.localPosition = new Vector3(0f, 0f, 0f);
                    Quaternion q = new Quaternion();
                    q.eulerAngles.Set(0f, 0f, 0f);
                    ReceiveHand.showL.transform.localRotation = q;
                    ReceiveHand.showL.transform.localScale = new Vector3(lx / resize, ly / resize, lz / resize);

                    Vector3 positionL = new Vector3(
                        Hands.Hands.L.transform.position.x,
                        Hands.Hands.L.transform.position.y + 0.2f,
                        Hands.Hands.L.transform.position.z);
                    ReceiveHand.showL.transform.position = positionL;//设置复制的模型位置
                }
                if (beforeru3 != 1 && beforeru2 != 1 && beforeru1 != 1)
                {
                    if (ReceiveHand.positionR.Equals(new Vector3()))
                        ReceiveHand.positionR = Hands.Hands.R.transform.position;
                    Vector3 nowpositionR = Hands.Hands.R.transform.position;
                    Vector3 nowrotation = ReceiveHand.showL.transform.localRotation.eulerAngles;
                    nowrotation.x += nowpositionR.x - ReceiveHand.positionR.x;//每帧都会按照变化旋转，可能需要常数来调整速度，可能需要修改
                    nowrotation.y += nowpositionR.z - ReceiveHand.positionR.z;
                    nowrotation.z += nowpositionR.y - ReceiveHand.positionR.y;
                    ReceiveHand.showL.transform.localRotation = Quaternion.Euler(nowrotation);//设置旋转
                }
                else
                {
                    ReceiveHand.positionR = new Vector3();
                }
            }
        }
        else
        {
            ReceiveHand.shration = 0f;
            ReceiveHand.imageL.fillAmount = ReceiveHand.shration;

            if (ReceiveHand.showL != null)
            {
                Destroy(ReceiveHand.showL);
            }
        }
    }
    public void isBUp()//双手向上，唤起菜单
    {
        if ((beforelu3 == 1 || beforelu2 == 1 || beforelu3 == 1) && (beforeru3 == 1 || beforeru2 == 1 || beforeru1 == 1))
        {
            ReceiveHand.duration += 0.015f;
            if (ReceiveHand.duration <= 1.2f)
                ReceiveHand.image.fillAmount = ReceiveHand.duration <= 1 ? ReceiveHand.duration : 1f;
            else
            {
                ReceiveHand.image.fillAmount = 0f;
                ControlCenter.menu();
                ReceiveHand.duration = 0f;
            }
        }
        else
        {
            ReceiveHand.duration = 0f;
            ReceiveHand.image.fillAmount = ReceiveHand.duration;
        }
    }
    public void UpClear()//检测并复位双手向上状态
    {
        if(beforelu3 != 1 && beforelu2 != 1 && beforelu3 != 1)
        {
            lc = 0;
        }
        if(beforeru3 != 1 && beforeru2 != 1 && beforeru1 != 1)
        {
            rc = 0;
        }
    }
    #endregion
    */
    public GameObject findGameObject()//查找射线所触碰的游戏物体
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        if (Physics.Raycast(ray, out hit))
        {
            return (hit.transform.gameObject);
            //Debug.Log(hit.transform.gameObject);
            //Debug.Log(hit.transform.tag);
        }
        return (null);
    }

}