using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRun : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadGameObject("http://192.168.1.107:88/test4.assetbundle",GameObject.Find("test5")));
    }
    public static IEnumerator LoadGameObject(string path, GameObject parent)
    {
        //StartCoroutine(LoadGameObject("URL",GameObject));
        WWW bundle = new WWW(path);
        yield return bundle;
        
        Object[] objs = bundle.assetBundle.LoadAllAssets();
        foreach (Object obj in objs)
        {
            GameObject model = Instantiate(obj) as GameObject;
            model.name = "loadedgood";
            model.transform.parent = parent.transform;//设定子物体
            yield return model;
        }
        bundle.assetBundle.Unload(false);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
