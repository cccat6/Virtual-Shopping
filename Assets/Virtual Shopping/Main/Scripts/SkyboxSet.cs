using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSet : MonoBehaviour {
    public Material sky;//天空盒的材质

    int ini = 0;
	// Use this for initialization
	void Start (){

    }
	
	// Update is called once per frame
	void Update () {
        if (ini > 1)
            return;
        else if (ini == 1)
            RenderSettings.skybox = sky;//把天空盒换成这个材质
        ini++;
    }

    public static void setSkybox(Material skyMaterial)
    {
        RenderSettings.skybox = skyMaterial;//把天空盒换成这个材质
    }
}
