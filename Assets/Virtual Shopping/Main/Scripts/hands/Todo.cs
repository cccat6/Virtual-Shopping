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
        var button = selected.GetComponent<Button>();//更改点击事件触发方法，为调用SoftClick
        if (button != null)
         {
             button.onClick.RemoveAllListeners();
             button.onClick.AddListener(SoftClick);
         }
     }
 
     public void SoftClick()
     {
         Debug.Log("Test Click. This is Soft Click");
     }
	
	// Update is called once per frame
	void Update () {
        selected = findGameObject();

        isCatch();
        isRotate();
        isChangeSize();
        isLUP();
        isRUP();
        isBUp();
        UpClear();
        isClick();
	}
    #region
    int beforec4 = -1, beforec3 = -1, beforec2 = -1, beforec1 = -1;//点击除抖
    public void isClick()//点击
    {
        beforec4 = beforec3;
        beforec3 = beforec2;
        beforec2 = beforec1;
        beforec1 = position.RS;
        if (beforec3 == 4 && beforec3 == 3 && beforec2 == 0 && beforec1 == 0)
        {
            SoftClick();
        }
    }
    int beforelc3 = -1, beforelc2 = -1, beforelc1 = -1;//左手拈花指除抖
    int beforerc3 = -1, beforerc2 = -1, beforerc1 = -1;//右手拈花指除抖
    public void isCatch()//拖拽，移动菜单
    {
        if ((beforelc3 == 1 || beforelc2 == 1 || beforelc3 == 1) && (beforerc3 != 1 && beforerc2 != 1 && beforerc1 != 1))
        {

        }
    }
    public void isRotate()//旋转，滑动菜单
    {
        if ((beforelc3 != 1 && beforelc2 != 1 && beforelc3 != 1) && (beforerc3 == 1 || beforerc2 == 1 || beforerc1 == 1))
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
        if ((beforelu3 == 1 || beforelu2 == 1 || beforelu3 == 1) && (beforeru3 != 1 && beforeru2 != 1 && beforeru1 != 1))
        {
            if (lc < 120)
            {
                lc++;
                //rc = 0;
            }
        }
    }
    public void isRUP()//右手向上，切换物理状态
    {
        if ((beforelu3 != 1 && beforelu2 != 1 && beforelu3 != 1) && (beforeru3 == 1 || beforeru2 == 1 || beforeru1 == 1))
        {
            if (rc < 120)
            {
                rc++;
                //lc = 0;
            }
        }
    }
    public void isBUp()//双手向上，开关菜单
    {
        if ((beforelu3 == 1 || beforelu2 == 1 || beforelu3 == 1) && (beforeru3 == 1 || beforeru2 == 1 || beforeru1 == 1))
        {
            if (lc < 120)
            {
                lc++;
            }
            if (rc < 120)
            {
                rc++;
            }
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