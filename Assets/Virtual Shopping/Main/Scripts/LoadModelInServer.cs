using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadModelInServer : MonoBehaviour {

    public void LoadModel(string path, GameObject parent)
    {
        StartCoroutine(LoadALLGameObject(path, parent));
    }

    //读取全部资源
    private IEnumerator LoadALLGameObject(string path, GameObject parent)
    {
        WWW bundle = new WWW(path);

        yield return bundle;

        //通过Prefab的名称把他们都读取出来
        Object[] objs = bundle.assetBundle.LoadAllAssets();
        //Object pic = bundle.assetBundle.LoadAsset("info");
        foreach (Object obj in objs)
        {
            obj.name = "loadedgood";
            GameObject model = Instantiate(obj) as GameObject;
            model.transform.parent = parent.transform;//设定子物体
            yield return model;
        }
        //for (int i = 0; i < a.transform.childCount; i++)
        //    a.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = (pic as Texture2D);
        bundle.assetBundle.Unload(false);
    }
}
