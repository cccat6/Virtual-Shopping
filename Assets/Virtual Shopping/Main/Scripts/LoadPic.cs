using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //LoadImage test = new LoadImage();
        //test.run(GameObject.Find("Cart Canvas/Grid Panel/Item/Show"), "http://192.168.0.108:88/test.png");
        //Debug.Log("At Load Image Start");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public class LoadImage
    {
        public void run(GameObject gameobject, string url)
        {
            //Debug.Log("At Load Image");
            //WWW www = new WWW("http://192.168.0.108:88/test.png");
            WWW www = new WWW(url);
            while (!www.isDone) ;
            if (www != null && string.IsNullOrEmpty(www.error))
            {
                //获取Texture
                //Texture texture = www.texture;
                //GameObject.Find("Cart Canvas/Grid Panel/Item/Show").GetComponent<Renderer>().material.mainTexture = texture;
                gameobject.GetComponent<Renderer>().material.mainTexture = www.texture;
                //www.assetBundle.Unload(true);
            }
        }
    }
}
