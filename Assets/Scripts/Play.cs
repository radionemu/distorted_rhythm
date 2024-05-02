using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if(isPlay==true&&(!mSync.IsPlaying() || Input.GetKeyDown(KeyCode.Return))){
            mSync.music.Stop();
            isPlay = false;
            mResult.UpdateScore();
            mUISwap.SwapCanvas(mUISwap.Gameplay, mUISwap.Result);
        }
    }

    public IEnumerator CheckMusicEnd(){
        while(true){
            if(!mSync.IsPlaying() && isPlay == true){
                isPlay = false;
                mUISwap.SwapCanvas(mUISwap.Gameplay, mUISwap.Result);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator InitGameplay(){
        isPlay = false;
        //Init Parser
        yield return mNoteParser.ReadFile();

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
}
