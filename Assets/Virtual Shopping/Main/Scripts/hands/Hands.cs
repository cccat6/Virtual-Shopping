using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hands
{
    /*public class position
    {
        public static int hands = 0;
        public static float[] Lpositions = new float[7];
        public static float[,] Lthumb = new float[4, 7];
        public static float[,] Lindex = new float[4, 7];
        public static float[,] Lmiddle = new float[4, 7];
        public static float[,] Lring = new float[4, 7];
        public static float[,] Lpinky = new float[4, 7];
        public static float[] Rpositions = new float[7];
        public static float[,] Rthumb = new float[4, 7];
        public static float[,] Rindex = new float[4, 7];
        public static float[,] Rmiddle = new float[4, 7];
        public static float[,] Rring = new float[4, 7];
        public static float[,] Rpinky = new float[4, 7];

        public static bool usingfinger = false;
        public static int LUR = 100;//Leap到Unity坐标系转换倍率
        public static float x_shift = (float)0, y_shift = (float)1.5, z_shift = (float)-3;//三轴手坐标偏移修正

        public static Quaternion calculate_angle(float x, float y, float z)
        {
            return (Quaternion.Euler((float)System.Math.Sqrt(x * y), (float)System.Math.Sqrt(y * z), (float)System.Math.Sqrt(z * x));
        }

    }*/
    public class position
    {
        public static int hands = 0;
        public static float Lx, Ly, Lz, Rx, Ry, Rz;
        public static int Laction = 0, Raction = 0;
        public static int LS = 0, RS = 0;
        public static float Rate = 0.3f;//修正位置的比例
    }
    public class Hands : MonoBehaviour
    {
        public static GameObject L;
        public static GameObject R;
        // Use this for initialization
        void Start()
        {
            L = GameObject.Find("Hands/Lhand");
            R = GameObject.Find("Hands/Rhand");
            //transform.position = new Vector3(90.0f, 180.0f, 0.0f);
        }
        //public float MoveSpeed = 1.0f;

        void Update()
        {
            L.SetActive(position.hands == 1 || position.hands == 3);
            R.SetActive(position.hands == 2 || position.hands == 3);
            NohandClick.NohandMode = position.hands == 0 && GvrViewer.Instance.VRModeEnabled;//如果没有检测到手的数据，则启用无手势模式，同时判断是否为全景模式
        }
    }
}