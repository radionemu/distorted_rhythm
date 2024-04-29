using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoteObj : MonoBehaviour
{
    public Note mNote;
    public Note CENote;

    public Sync mSync;
    public Sheet mSheet;
    public Play mPlay;//?
    public PlayerSetting mPSetting;
    float mNoteSpd;
    bool ismove = true;
    float timer;
    
    //Coroutine Running Check
    public bool IENoteScaleRunning = false;
    public bool IENoteScrollRunning = false;

    //positioning
    public Vector3 JudgelinePosition;

    public float mNoteTiming;

    // Start is called before the first frame update
    void Start()
    {
        // IENoteScaleRunning = false;
        // IENoteScrollRunning = false;
        // mPlay = GameObject.Find("PlayManager").GetComponent<Play>();
        // StartCoroutine(NoteScroll());
    }

    public void Init(Sync sync, Sheet sheet, Note note, List<Color> oColor){
        IENoteScaleRunning = false;
        IENoteScrollRunning = false;
        GameObject obj = GameObject.Find("PlayManager");
        mPlay = obj.GetComponent<Play>();
        mPSetting = obj.GetComponent<PlayerSetting>();
        // StartCoroutine(NoteScrollTranslate());

        mSync = sync;
        mSheet = sheet;
        this.mNote = note;
        mNoteTiming = mSync.oneBeatTime;

        try{
            gameObject.GetComponent<SpriteRenderer>().color = oColor[mNote.lane-1];
        }catch{
            gameObject.GetComponentInChildren<SpriteRenderer>().color = oColor[mNote.lane-1];
        }


    }

    public void SetCE(Note CENOTE){
        this.CENote = CENOTE;
    }

    // Update is called once per frame
    void Update()
    {
        mNoteSpd = mSync.HiSpeed * mNoteTiming;
        timer += Time.smoothDeltaTime;
    }

    private void FixedUpdate() {
        IENoteScrollRunning = true;
        if(mPlay.isPlay == true){
            int isGuide = mSheet.DrumIntro ? 1 : 0;
            float NoteTiming = (mSync.oneBeatTime*(4*(mSheet.beatNom/mSheet.beatDenom)*(isGuide+mNote.section+((float)mNote.nom)/mNote.denom))+mPSetting.DisplayOffset)*mSync.music.clip.frequency;
            float curTiming = mSync.music.timeSamples+mSync.guidePCM;

            float x = transform.position.x;
            float z = transform.position.z;
            float freq = mSync.music.clip.frequency;
            float noteY = JudgelinePosition.y + mSync.HiSpeed * NoteTiming / freq;
            float judgeY = JudgelinePosition.y ;

            // Debug.Log(mPSetting.DisplayOffset+" "+mPSetting.musicStartOffset);

            Vector3 noteV3 = new(x,noteY,z);
            Vector3 judgeV3 = new(x,judgeY,z);
            //BELIEVE or DIE
            transform.position = CustomMath.ExtraLerp(noteV3,judgeV3,1.0f-(NoteTiming-curTiming)/NoteTiming);
        }   
    }

    public void Move(){
        StartCoroutine(NoteScroll());
    }

    public void reqChargeScale(Transform JudgeLine){
        StartCoroutine(NoteScale(JudgeLine));
    }

    public IEnumerator NoteScroll(){
        IENoteScrollRunning = true;
        while(true){
            if(mPlay.isPlay == true){
                float NoteTiming = mSync.oneBeatTime*(4*(mSheet.beatNom/mSheet.beatDenom)*(mNote.section+((float)mNote.nom)/mNote.denom))*mSync.music.clip.frequency;
                float curTiming = mSync.music.timeSamples;
                Debug.Log(mSync.oneBeatTime+" "+mSheet.beatNom+" "+mSheet.beatDenom+" "+mNote.section+" "+mNote.nom+" "+mNote.denom+" "+mSync.music.clip.frequency);
                float CurrentTiming = mSync.music.timeSamples;
                Debug.Log(NoteTiming+" "+CurrentTiming);

                float x = transform.position.x;
                float z = transform.position.z;
                float freq = mSync.music.clip.frequency;
                float noteY = JudgelinePosition.y + mSync.HiSpeed * NoteTiming / freq;
                float judgeY = JudgelinePosition.y;

                Vector3 noteV3 = new(x,noteY,z);
                Vector3 judgeV3 = new(x,judgeY,z);
                //BELIEVE or DIE
                transform.position = CustomMath.ExtraLerp(noteV3,judgeV3,1.0f-(NoteTiming-curTiming)/NoteTiming);
            }   
            yield return null;
        }

    }

    public IEnumerator NoteScrollTranslate(){
        IENoteScrollRunning = true;
        while(true){
            if(mPlay.isPlay == true){
                transform.Translate(mNoteSpd * Time.deltaTime * Vector3.down);
            }
            yield return null;
        }
    }

    public IEnumerator NoteScale(Transform JudgeLine){
        IENoteScaleRunning = true;
        while(true){
            yield return null;
            int isGuide = mSheet.DrumIntro ? 1 : 0;
            transform.position = new Vector3(transform.position.x, JudgeLine.position.y);
            float NoteTiming = (mSync.oneBeatTime*(4*(mSheet.beatNom/mSheet.beatDenom)*(isGuide+CENote.section+((float)CENote.nom)/CENote.denom))+mPSetting.DisplayOffset)*mSync.music.clip.frequency;
            float curTiming = mSync.music.timeSamples+mSync.guidePCM;
            float deltaTiming = (NoteTiming - curTiming)/mSync.music.clip.frequency;

            transform.localScale = new Vector3(transform.localScale.x, deltaTiming*mSync.HiSpeed, transform.localScale.z);
        }
    }
}