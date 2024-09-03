using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Play : MonoBehaviour
{
    public Sync mSync;
    public Sheet mSheet;

    public UISwapper mUISwap;

    public ResultUIManager mResult;

    public NoteParse mNoteParser;
    public NoteManager mNoteManager;

    public Judge mJudge;
    public UIManager mUIMan;

    public PlayerSetting mPsetting;

    public bool isPlay = false;

    private bool sss = true;

    // Start is called before the first frame update
    void Start()
    {
        settingManager.BuildSetting = settingManager.PortMode.Desktop;
        Application.targetFrameRate = -1;
        // Debug.Log(mSync.music.clip.length);
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(sss){
        //     mSync.music.Stop();
        //     StopAllCoroutines();
        //     Debug.Log("TEST START");
        //     StartCoroutine(InitGameplay());
        //     sss = false;
        // }
        if(Input.GetKeyDown(KeyCode.Slash)){
            mSync.music.Stop();
            StopAllCoroutines();
            Debug.Log("TEST START");
            StartCoroutine(InitGameplay());
        }

        if(isPlay==true&&mSync.IsPlaying() && Input.GetKeyDown(KeyCode.Return)){
            mSync.music.Stop();
            isPlay = false;
            mResult.UpdateScore();
            StopCoroutine(CheckMusicEnd());
            StartCoroutine(ResultManager.GetInstance().Load());
            // mUISwap.SwapCanvas(mUISwap.Gameplay, mUISwap.Result);
        }
    }

    public IEnumerator CheckMusicEnd(){
        while(true){
            if (!mSync.IsPlaying() && isPlay == true)
            {
                isPlay = false;
                mResult.UpdateScore();
                StartCoroutine(ResultManager.GetInstance().Load());
                // mUISwap.SwapCanvas(mUISwap.Gameplay, mUISwap.Result);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StartGame() { 
        mSync.music.Stop();
        StopAllCoroutines();
        Debug.Log("TEST START");
        StartCoroutine(InitGameplay());
    }

    public IEnumerator InitGameplay(){
        isPlay = false;
        //Init Parser


        yield return mNoteParser.ReadFile(StartManager.GetInstance().targetChart);

        //init Sync & audio manager
        mSheet = mNoteParser.GetSheet();
        mPsetting.musicStartOffset = mSheet.Offset;
        yield return mSync.init(mNoteParser.GetSheet());
        mSync.reqPlayMusic(mSheet.DrumIntro);

        //Generate Note
        yield return mNoteManager.GenerateNote(mSync, mSheet, mUIMan.oColor);

        yield return mUIMan.Init(mSheet);
        yield return mJudge.InitQueue();
        // yield return new WaitForSeconds(3f);
        Debug.Log("isPlay");
        isPlay = true;
        StartCoroutine(CheckMusicEnd());
        yield return null;
    }

    public string GetUnicodeString(string input)
    {

        Encoding encoding = Encoding.GetEncoding(51949);//중국어 간체 
        var bytest = encoding.GetBytes(input); 
        var output = encoding.GetString(bytest); 
        //Console.WriteLine(input); 
        //Console.WriteLine(output); 

        List<string> unicodes = new List<string>(); 

        string result = String.Empty; 

        if (input != output) 
        { 

            for (int i = 0; i < input.Length; i += char.IsSurrogatePair(input, i) ? 2 : 1) 
            { 
                int codepoint = char.ConvertToUtf32(input, i); 
                unicodes.Add(String.Format("&#{0}", codepoint)); 
            } 
             
            for (int i = 0; i < input.Length; i++) 
            { 

                if (input[i].ToString() != output[i].ToString()) 
                { 
                    result += unicodes[i]; 
                } 

                else 
                { 
                    result += input[i]; 
                } 
            } 
        } 

        else result = input; 

        return result; 
    }
}
