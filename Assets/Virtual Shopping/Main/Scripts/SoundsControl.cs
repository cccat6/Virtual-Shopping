using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SoundsControl : MonoBehaviour {//声音控制类，用来播放音频的

    public AudioSource voice;
    public float volume = 0.8f;
    private static AudioClip loadClip = null;
    private static string playtext = null;

    public const string SpeechKey = "16afa20faac7462c9a64cb4b78a7e203";

    // Use this for initialization
    void Start () {
        voice.volume = volume;
	}
	
	// Update is called once per frame
	void Update () {
        if (loadClip != null)
        {
            stop();
            voice.clip = loadClip;
            loadClip = null;
            play();
        }
        if (playtext != null)
        {
            StartCoroutine(GetSound(playtext));
            playtext = null;
        }
	}

    /*private IEnumerator loadAudio(string path)
    {
        WWW www = new WWW(path);
        while (!www.isDone)
            yield return 0;
        loadClip = www.GetAudioClip();
    }

    public void playFile(string path)
    {
        StartCoroutine(loadAudio(path));
    }*/
    public static void playAudio(AudioClip audio)
    {
        loadClip = audio;
    }
    public void play()//播放
    {
        if (!voice.isPlaying)
            voice.Play();
    }
    public void pause()//暂停
    {
        if (voice.isPlaying)
            voice.Pause();
    }
    public void stop()//停止
    {
        if (voice.isPlaying)
            voice.Stop();
    }
    public bool playing()
    {
        return voice.isPlaying;
    }

    static string kAu = null;
    public static void playText(string text)//文字转语言后播放
    {
        playtext = text;
    }
    private IEnumerator GetAuthorization(string key)//获取语音识别用的Authorization，有效期10分钟
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("ContentType", "application/x-www-form-urlencoded");
        headers.Add("Ocp-Apim-Subscription-Key", key);
        WWW postData = new WWW(@"https://api.cognitive.microsoft.com/sts/v1.0/issueToken", new byte[] { 0 }, headers);
        string er = postData.error;
        while (!postData.isDone) yield return new WaitForSeconds(0.1f);
        string result = postData.text;
        kAu = result;
    }
    private IEnumerator GetSound(string text)
    {
        StartCoroutine(GetAuthorization(SpeechKey));
        while (kAu == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("ContentType", "application/ssml+xml");
        headers.Add("X-Microsoft-OutputFormat", "riff-16khz-16bit-mono-pcm");
        headers.Add("User-Agent", "VirtualShopping");
        headers.Add("Authorization", "Bearer " + kAu); kAu = null;
        /*<speak version='1.0' xml:lang='en-US'><voice xml:lang='en-US' xml:gender='Female' name='Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)'>Microsoft Bing Voice Output API</voice></speak>*/
        //https://docs.microsoft.com/zh-cn/azure/cognitive-services/Speech/api-reference-rest/bingvoiceoutput 参考资料
        string postdata = Language.lang.voice + text + "</voice></speak>";
        WWW getSound = new WWW(@"https://speech.platform.bing.com/synthesize", Encoding.UTF8.GetBytes(postdata), headers);
        string er = getSound.error;
        while (!getSound.isDone) yield return new WaitForSeconds(0.1f);
        loadClip = getSound.GetAudioClip(true,false,AudioType.WAV);
    }
}
