using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    public Sync mSync;

    public UISwapper mUISwap;

    public ResultUIManager mResult;

    public NoteParse mNoteParser;
    public NoteManager mNoteManager;

    public Judge mJudge;
    public UIManager mUIMan;

    public bool isPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(mSync.music.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlay==true&&(!mSync.music.isPlaying || Input.GetKeyDown(KeyCode.Return))){
            mSync.music.Stop();
            isPlay = false;
            mResult.UpdateScore();
            mUISwap.SwapCanvas(mUISwap.Gameplay, mUISwap.Result);
        }
    }

    public IEnumerator CheckMusicEnd(){
        while(true){
            if(!mSync.music.isPlaying && isPlay == true){
                isPlay = false;
                mUISwap.SwapCanvas(mUISwap.Gameplay, mUISwap.Result);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator InitGameplay(){
        //Init Parser
        yield return mNoteParser.ReadFile();
        yield return mNoteManager.GenerateNote();
        yield return mUIMan.Init();
        yield return mJudge.InitQueue();
        // yield return new WaitForSeconds(3f);
        Debug.Log("isPlay");
        isPlay = true;
        mSync.PlayMusic();
        StartCoroutine(CheckMusicEnd());
        yield return null;
    }
}
