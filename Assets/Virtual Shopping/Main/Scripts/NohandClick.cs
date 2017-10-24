using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NohandClick : MonoBehaviour
{
    private Image image;
    private float duration;

    private Camera mainCamera;
    private RaycastHit objhit, objhit2;
    private Ray _ray;
    public static GameObject gameObj,UI;//保存射线打到的物体

    public static bool NohandMode = false;//是否启用无手势模式

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        //image.type = Image.Type.Filled;
        image.fillAmount = 0f;
        duration = 0;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Control(true);
        MobilePick();
    }
    
    void MobilePick()
    {
        Ray _ray = new Ray(transform.position, transform.forward);
        //_ray = mainCamera.ScreenPointToRay();//从摄像机发出一条射线,到点击的坐标
        Debug.DrawLine(_ray.origin, objhit.point, Color.red, 2);//显示一条射线，只有在scene视图中才能看到
        int Mask = LayerMask.GetMask("Button");
        if (Physics.Raycast(_ray, out objhit, 10000f, Mask))
        {
            gameObj = objhit.collider.gameObject;
            Control(true);
            //Debug.Log("Hit objname:" + gameObj.name + "Hit objlayer:" + gameObj.layer);
        }
        else
        {
            Ray _ray2 = new Ray(transform.position, transform.forward);
            //_ray = mainCamera.ScreenPointToRay();//从摄像机发出一条射线,到点击的坐标
            Debug.DrawLine(_ray.origin, objhit2.point, Color.red, 2);//显示一条射线，只有在scene视图中才能看到
            int Mask2 = LayerMask.GetMask("CanMoveUI");
            if (Physics.Raycast(_ray2, out objhit2, 10000f, Mask2))
            {
                gameObj = objhit2.collider.gameObject;
            }
            else
            {
                gameObj = null;
            }
            //gameObj = null;
            Control(false);
        }

        
    }

    void Control(bool addorno)
    {
        try
        {
            ControlCenter.canCircle = true;
            gameObj.SendMessageUpwards("Select");//用来持续发送选中信息的
            if (!ControlCenter.canCircle)
            {
                duration = 0f;
                image.fillAmount = duration;
                ControlCenter.canCircle = true;
                return;
            }
        }
        catch { }
        if (addorno && NohandMode)
        {
            duration += 0.015f;
            if (image.fillAmount < 1f)
            {
                image.color = Color.white;
                image.fillAmount = duration >= 0 ? duration : 0f;
            }
            else if (image.fillAmount == 1f)
            {
                image.color = Color.green;
                if (duration >= 1.5f)
                {
                    duration = -0.6f;
                    image.fillAmount = 0f;
                }
            }
            if (image.fillAmount == 0f && NohandMode)
            {
                try
                {
                    gameObj.SendMessageUpwards("Clicked");//用Upwards的原因是，有时碰撞到的物体是按钮的子物体，如果这样出bug可以去掉Upwards
                }
                catch { }
                duration = 0f;
                //Debug.Log("Hited " + gameObj);
            }
        }
        else
        {
            duration = 0f;
            image.fillAmount = duration;
        }
    }

    public static bool DoClick()
    {
        if (!NohandMode)
        {
            gameObj.SendMessageUpwards("Clicked");//用Upwards的原因是，有时碰撞到的物体是按钮的子物体，如果这样出bug可以去掉Upward
            return true;
        }
        else
            return false;
    }
}
