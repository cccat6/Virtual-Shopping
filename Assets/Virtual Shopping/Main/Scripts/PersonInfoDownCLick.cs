using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PersonInfoDownCLick : MonoBehaviour {
    private void Start()
    {
        
    }
    private void Update()
    {

    }
    public void Clicked()
    {
        //this.transform.GetChild(0).GetComponent<Text>().text = Presult;
        RefreshPersonInfo.refreshPersonalInfo.DownScroll();
        Debug.Log("PersonInfoDOwnClick");
    }

    //static string Presult = null;
    //WWW www = null;
    // static string personalInfoUrl = "http://central.holoworld.win/PersonalInfo.ashx?langNum="+Language.lang.langNum+"&id=";

    //public IEnumerator Getstring(string url, string id, WWW www)
    //{
    //    string temp = url + id;
    //    www = new WWW(temp);
    //    while (!www.isDone)
    //    {
    //        yield return new WaitForSeconds(1);
    //    }
    //    if (www != null && string.IsNullOrEmpty(www.error))
    //    {
    //        Presult = www.text;
    //    }
    //}
    //public static WWW getData = null;
    //public static WWW www = null;

    // Use this for initialization



    //void Start () {

    //   }

    //   // Update is called once per frame
    //   void Update () {
    //   }
    //   public void Clicked()
    //   {
    //       // this.transform.GetChild(0).GetComponent<Text>().text = result;
    //       string a = "http://wx1.sinaimg.cn/large/006qfYfDgy1fe2u39mz8ij30zk0qoqcg.jpg";

    //       Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(1f, 1f));

    //       GameObject.FindGameObjectWithTag("Image").GetComponent<Image>().sprite = sprite;
    //           //sprite = Resources.Load(www.texture, typeof(Sprite)) as Sprite;
    //       www.LoadImageIntoTexture(www.texture);

    //   }
    //       StartCoroutine(Get("https://holoworld.search.windows.net/indexes/goods/docs?api-version=2016-09-01&api-key=0F18C88DE06BE3E22F103CFB0887EAF7&search=*"));

    //  public static string result = null;
    //   public IEnumerator Get(string url)
    //   {
    //        www = new WWW(url);
    //       while (!www.isDone)
    //       {
    //           yield return new WaitForSeconds(1);

    //       }

    //       if (www != null && string.IsNullOrEmpty(www.error))
    //       {
    //            result = www.text;

    //       }

    //   }
}
