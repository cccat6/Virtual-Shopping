using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Login : MonoBehaviour {
    private static AudioClip clip;
    private static int maxRecordTime = 5;
    private static int samplingRate = 16000;
    
    public const string SpeechKey = "16afa20faac7462c9a64cb4b78a7e203";

    public static bool logining = false;
    public static string result = null;
    private static GameObject ListenIcon = null;

    // Use this for initialization
    void Start (){
        if (ControlCenter.inied)
            return;
        ListenIcon = ControlCenter.Voice;
    }

    // Update is called once per frame
	void Update () {
        if (logining && Microphone.IsRecording(null))
        {
            float length = Microphone.GetPosition(null) / samplingRate;
            if (length >= 2)
            {
                double average = 0.0;
                float[] sourceI = new float[Microphone.GetPosition(null) * clip.channels];
                float[] source = new float[24000];
                clip.GetData(sourceI, 0);
                Array.Copy(sourceI, sourceI.Length - 24000, source, 0, 24000);
                average = 0f;
                foreach (float now in source)
                    average += Mathf.Abs(now);
                average /= source.Length;
                if ((Microphone.IsRecording(null) && !(average > VocalListener.backgrountVolume * 1.2f) && length > 1.5f) || length > 9.9f)
                {
                    AudioClip outClip = new AudioClip();
                    int outlength = 0;
                    EndRecording(out outlength, out outClip);
                    if (length > 2.1f)
                    {
                        Debug.Log("Start post to login");
                        //ListenIcon.SetActive(false);
                        SoundsControl.playAudio(ControlCenter.MainCamera.GetComponent<PrefebCollector>().vocalend);
                        byte[] data = getWAV(outClip);
                        StartCoroutine(login(data));
                    }
                    else
                    {
                        ListenIcon.SetActive(false);
                        SoundsControl.playAudio(ControlCenter.MainCamera.GetComponent<PrefebCollector>().vocalend);
                        ControlCenter.ShowMessage(Language.lang.loginfail);
                    }
                }
            }

        }
        if (!string.IsNullOrEmpty(result))
        {
            Debug.Log(result);
            try
            {
                if (int.Parse(result) > 0)
                {
                    ControlCenter.SetString("id", result);
                    result = null;
                    SoundsControl.playAudio(ControlCenter.MainCamera.GetComponent<PrefebCollector>().logfinish);
                }
                else
                {
                    ControlCenter.ShowMessage(Language.lang.loginfail);
                    result = null;
                }
            }
            catch
            {
                ControlCenter.ShowMessage(Language.lang.loginfail);
                result = null;
            }
        }
	}

    public static void startLogin()
    {
        Debug.Log("Start listen vocal print");
        StartRecording();
        SoundsControl.playAudio(ControlCenter.MainCamera.GetComponent<PrefebCollector>().logprocess);
        logining = true;
    }

    private IEnumerator login(byte[] bArr)//提交音频开始登陆，需要用StartCoroutine启动
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("ContentType", "application/octet-stream");
        WWW postData = new WWW(@"http://central.holoworld.win/Login.ashx", bArr, headers);
        string er = postData.error;
        while (!postData.isDone) yield return new WaitForSeconds(0.1f);
        result = postData.text;
        logining = false;
        ListenIcon.SetActive(false);
    }

    private static bool StartRecording()
    {
        ListenIcon.SetActive(true);
        try
        {
            Microphone.End(null);
            clip = Microphone.Start(null, false, maxRecordTime, samplingRate);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }
    private static void EndRecording(out int length, out AudioClip outClip)
    {
        int lastPos = Microphone.GetPosition(null);

        if (Microphone.IsRecording(null))
        {
            length = lastPos / samplingRate;
        }
        else
        {
            length = maxRecordTime;
        }
        Microphone.End(null);

        if (length < 1.0f)
        {
            outClip = null;
            return;
        }
        outClip = clip;
        Debug.Log("end listening");
    }
    private static byte[] GetData16(AudioClip clip)
    {
        var data = new float[clip.samples * clip.channels];

        clip.GetData(data, 0);

        byte[] bytes = new byte[data.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < data.Length; i++)
        {
            short value = (short)(data[i] * rescaleFactor);
            BitConverter.GetBytes(value).CopyTo(bytes, i * 2);
        }

        return bytes;
    }
    private static byte[] getWAV(AudioClip clip)
    {
        byte[] bytes = null;

        using (var memoryStream = new System.IO.MemoryStream())
        {
            memoryStream.Write(new byte[44], 0, 44);//预留44字节头部信息

            byte[] bytesData = GetData16(clip);

            memoryStream.Write(bytesData, 0, bytesData.Length);

            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);

            byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            memoryStream.Write(riff, 0, 4);

            byte[] chunkSize = BitConverter.GetBytes(memoryStream.Length - 8);
            memoryStream.Write(chunkSize, 0, 4);

            byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            memoryStream.Write(wave, 0, 4);

            byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            memoryStream.Write(fmt, 0, 4);

            byte[] subChunk1 = BitConverter.GetBytes(16);
            memoryStream.Write(subChunk1, 0, 4);

            UInt16 two = 2;
            UInt16 one = 1;

            byte[] audioFormat = BitConverter.GetBytes(one);
            memoryStream.Write(audioFormat, 0, 2);

            byte[] numChannels = BitConverter.GetBytes(clip.channels);
            memoryStream.Write(numChannels, 0, 2);

            byte[] sampleRate = BitConverter.GetBytes(clip.frequency);
            memoryStream.Write(sampleRate, 0, 4);

            byte[] byteRate = BitConverter.GetBytes(clip.frequency * clip.channels * 2); // sampleRate * bytesPerSample*number of channels
            memoryStream.Write(byteRate, 0, 4);

            UInt16 blockAlign = (ushort)(clip.channels * 2);
            memoryStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            UInt16 bps = 16;
            byte[] bitsPerSample = BitConverter.GetBytes(bps);
            memoryStream.Write(bitsPerSample, 0, 2);

            byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
            memoryStream.Write(datastring, 0, 4);

            byte[] subChunk2 = BitConverter.GetBytes(clip.samples * clip.channels * 2);
            memoryStream.Write(subChunk2, 0, 4);

            bytes = memoryStream.ToArray();
        }

        return bytes;
    }
    private static byte[] combine(byte[] first, byte[] second)
    {
        byte[] result = new byte[first.Length + second.Length];
        first.CopyTo(result, 0);
        second.CopyTo(result, first.Length);
        return result;
    }
}
