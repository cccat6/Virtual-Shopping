using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class VocalListener : MonoBehaviour {
    public static GameObject ListenIcon;

    private static AudioClip clip;
    private static int maxRecordTime = 10;
    private static int samplingRate = 16000;

    public static double backgrountVolume = 0f;//环境噪音大小
    public const string SpeechKey = "16afa20faac7462c9a64cb4b78a7e203";

    double average = 0f;

    public static string resultAu = null;
    static string kAu = null;

    // Use this for initialiyation
    void Start () {
        ListenIcon = ControlCenter.Voice;
        if (backgrountVolume == 0f)
        {
            if (!ControlCenter.MainCamera.GetComponent<SoundsControl>().playing())
            {
                maxRecordTime = 2;
                StartRecording();
            }
        }
        else
        {
            maxRecordTime = 8;
            StartRecording();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Login.logining)
            return;
        if (resultAu != null && resultAu != "")
        {
            Debug.Log(resultAu);//输出最终结果
            if (resultAu.Contains("Error"))
                ControlCenter.ShowMessage(Language.lang.error);
            string operate = resultAu;
            resultAu = null;
            ControlCenter.MainCamera.GetComponent<LanguageUnderstand>().go(operate);
        }

        if (backgrountVolume == 0 && !Microphone.IsRecording(null))
            Start();
        else if(backgrountVolume == 0 && Microphone.IsRecording(null))
        {
            float length2 = Microphone.GetPosition(null) / samplingRate;
            if (length2 >= 2)
            {
                AudioClip outClip = new AudioClip();
                int outlength = 0;
                float[] source2 = new float[Microphone.GetPosition(null) * clip.channels];
                EndRecording(out outlength, out outClip);
                clip.GetData(source2, 0);
                Array.Copy(source2, source2.Length - 16000, source2, 0, 16000);
                foreach (float now in source2)
                    backgrountVolume += Mathf.Abs(now);
                backgrountVolume /= source2.Length;
                maxRecordTime = 10;
            }
            return;
        }
        if (!Microphone.IsRecording(null))
            return;

        float length = Microphone.GetPosition(null) / samplingRate;
        if (length >= 2)
        {
            float[] sourceI = new float[Microphone.GetPosition(null) * clip.channels];
            float[] source = new float[24000];
            clip.GetData(sourceI, 0);
            Array.Copy(sourceI, sourceI.Length - 24000, source, 0, 24000);
            average = 0f;
            foreach (float now in source)
                average += Mathf.Abs(now);
            average /= source.Length;
            if ((Microphone.IsRecording(null) && !(average > backgrountVolume * 1.2f) && length > 1.5f) || length > 9.9f)
            {
                AudioClip outClip = new AudioClip();
                int outlength = 0;
                EndRecording(out outlength, out outClip);
                if (length > 2.1f)
                {
                    Debug.Log("Start get text");
                    LanguageUnderstand.ListenIcon = ListenIcon;
                    SoundsControl.playAudio(ControlCenter.MainCamera.GetComponent<PrefebCollector>().vocalend);
                    StartCoroutine(GetTextFromAudio(getWAV(outClip)));
                    resultAu = "";
                    //Debug.Log(GetTextFromAudio(getWAV(outClip)));
                }
                else
                    ListenIcon.SetActive(false);
            }
        }
    }
    public void Clicked()
    {
        Debug.Log("clicked button");
        Debug.Log(StartRecording());
    }

    public static bool StartRecording()
    {
        if (backgrountVolume != 0.0)
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
    private void EndRecording(out int length, out AudioClip outClip)
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
    private byte[] GetData16(AudioClip clip)
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
    private byte[] getWAV(AudioClip clip)
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

            //UInt16 two = 2;
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
    private IEnumerator GetTextFromAudio(byte[] data)//调用语音识别
    {
        StartCoroutine(GetAuthorization(SpeechKey));
        while (kAu == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("ContentType", "audio/wav; codec=\"audio/pcm\"; samplerate=16000");
        headers.Add("Authorization", "Bearer " + kAu); kAu = null;
        headers.Add("ContentLength", data.Length.ToString());
        WWW postData = new WWW(@"https://speech.platform.bing.com/recognize?scenarios=smd&appid=D4D52672-91D7-4C74-8AD8-42B1D98141A5&locale="
            + Language.lang.langCode
            + "&device.os=bot&form=BCSSTT&version=3.0&format=json&instanceid=565D69FF-E928-4B7E-87DA-9A750B96D9E3&maxnbest=1&requestid="
            + Guid.NewGuid()
            , data, headers);
        string er = postData.error;
        while (!postData.isDone) yield return new WaitForSeconds(0.1f);
        string result = postData.text;
        Debug.Log("Before analysis: " + result);
        try
        {
            Regex reg = new Regex("name\":\".+?(?=\")");
            Match match = reg.Match(result);
            result = match.Groups[0].Value.Split('\"')[2];
        }
        catch { resultAu = "Error"; }
        Debug.Log("Before translate: " + result);
        WWW english = new WWW(@"http://central.holoworld.win/TransEN.ashx", Encoding.UTF8.GetBytes(result));
        while (!english.isDone) yield return new WaitForSeconds(0.1f);
        result = english.text;
        Debug.Log("After translate: " + result);
        if (string.IsNullOrEmpty(result))
            result = "Error";
        resultAu = result;
    }
    private IEnumerator GetAuthorization(string key)//获取语音识别用的Authorization，有效期10分钟
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("ContentType", "application/x-www-form-urlencoded");
        headers.Add("Ocp-Apim-Subscription-Key", key);
        WWW postData = new WWW(@"https://api.cognitive.microsoft.com/sts/v1.0/issueToken", new byte[] { 0 }, headers);
        string er=postData.error;
        while (!postData.isDone) yield return new WaitForSeconds(0.1f);
        string result = postData.text;
        kAu = result;
    }

    public static void RefreshBackground()
    {
        backgrountVolume = 0f;
    }
}
