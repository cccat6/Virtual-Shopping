using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRun : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //StartCoroutine(LoadGameObject("http://172.16.12.233:8888/AutoTest.assetbundle", GameObject.Find("test5")));
    }
    public static IEnumerator LoadGameObject(string path, GameObject parent)
    {
        //StartCoroutine(LoadGameObject("URL",GameObject));
        WWW bundle = new WWW(path);
        yield return bundle;
        
        Object[] objs = bundle.assetBundle.LoadAllAssets();
        Texture2D texture = new Texture2D(0, 0);
        Material material;
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
        }
        foreach (Object obj in objs)//获取GameObject并加载
        {
            if (obj.GetType() == typeof(GameObject))
            {
                GameObject model = Instantiate(obj) as GameObject;
                model.name = "loadedgood";
                //model.transform.parent = parent.transform;//设定子物体
                yield return model;
            }
        }
        //bundle.assetBundle.Unload(false);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
