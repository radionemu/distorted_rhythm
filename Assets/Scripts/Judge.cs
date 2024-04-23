using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum JudgeType{
    PGREAT,
    GREAT,
    GOOD,
    OK,
    MISS,
    COK,
    CGOOD,
    UNJUDGE,
}

public enum JudgeTiming{
    FAST,
    SLOW,
    JUST,
}

public class Judge : MonoBehaviour
{
    public NoteManager mNoteMgr;
    public ScoreManager mScoreMgr;

    //Judge Timing
    public float PGREAT = 33f;
    public float GREAT = 57f;
    public float GOOD = 81f;
    public float OK = 105f;
    public float MISS = 129f;

    //ref Class
    public Sync mAudio;
    public Sheet mSheet;
    public UIManager mUIMan;

    List<Queue<Note>> Lanes = new List<Queue<Note>>();
    Queue<Note> laneA = new();
    Queue<Note> laneB = new();
    Queue<Note> laneC = new();
    Queue<Note> laneD = new();
    Queue<Note> laneM = new();

    List<Note> Row = new();

    private int PGREATPCM;
    private int GREATPCM;
    private int GOODPCM;
    private int OKPCM;
    private int MISSPCM;

    private bool isCharge = false;
    private bool [] charge = new bool[4]{false, false, false, false};

    public JudgeType tmpJudge;
    public JudgeTiming tmptiming;

    // Start is called before the first frame update
    void Start()
    {   
        PGREATPCM = (int)(PGREAT * mAudio.music.clip.frequency/1000f);
        GREATPCM = (int)(GREAT * mAudio.music.clip.frequency/1000f);
        GOODPCM = (int)(GOOD * mAudio.music.clip.frequency/1000f);
        OKPCM = (int)(OK * mAudio.music.clip.frequency/1000f);
        MISSPCM = (int)(MISS * mAudio.music.clip.frequency/1000f);

        //InitQueue();
    }

    public void InitQueue(){
        //Enqueue Notes each lane
        foreach(Note note in mNoteMgr.JudgeNote){
            switch (note.lane){
                case 1:laneA.Enqueue(note);break;
                case 2:laneB.Enqueue(note);break;
                case 3:laneC.Enqueue(note);break;
                case 4:laneD.Enqueue(note);break;
                case 5:laneM.Enqueue(note);break;
                default:break;
            }
        }
        Lanes.Add(laneA);
        Lanes.Add(laneB);
        Lanes.Add(laneC);
        Lanes.Add(laneD);
        Lanes.Add(laneM);
    }
 
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(JudgeMiss());
    }

    IEnumerator JudgeMiss(){
        int sample = mAudio.music.timeSamples;
        // Debug.Log("sample : "+sample);
        foreach(Queue<Note> lane in Lanes){
            if(lane.Count <= 0) continue;
            Note note = lane.Peek();
            // Debug.Log(""+GetPCM(note));
            int qsample = sample - (int)GetPCM(note) - (int)mAudio.offsetPCM;
            if(note.nType == NoteType.CE && qsample>=0){
                ReqCharge();
            }else if(qsample>OKPCM){
                mUIMan.ReqJudge(JudgeType.MISS);
                lane.Dequeue();
                mNoteMgr.ReqDestroy(note.objID);
            }
        }

        yield return null;
    }

    public float GetPCM(Note note){
        return mAudio.oneBeatTime * mAudio.music.clip.frequency * (note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom));
    }

    private bool InRange(int timing, int pmRange){
        return timing < pmRange && timing > -pmRange;
    }

    public void reqJudge(bool [] inputButtons){
        int cSample = mAudio.music.timeSamples;
        //get Notes in Current Row
        
        bool [] reqButtons = new bool[5] {false, false, false, false, false};//BT-A, BT-B, BT-C, BT-D, STROKE
        int timing = int.MaxValue;
        int rowPCM = int.MaxValue;
        foreach(Queue<Note> lane in Lanes){
            if(lane.Count <= 0) continue;
            var note = lane.Peek();
            int ntiming = (int)(note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom));
            if(timing > ntiming){
                rowPCM = (int)GetPCM(note);
                timing = ntiming;
                Row.Clear();
                Array.Fill(reqButtons, false);
                Row.Add(note);
                reqButtons[note.lane-1] = true;
            }else if(timing == ntiming){
                rowPCM = (int)GetPCM(note);
                reqButtons[note.lane-1] = true;
                Row.Add(note);
            }
        }
        if(Row.Count<=0)return;

        //check if button is mapped correctly
        bool isCorrect = true;

        if(Row[0].nType == NoteType.NM || Row[0].nType == NoteType.CS){
            for(int i=0; i<4; i++){
                if(reqButtons[i] != inputButtons[i]){
                    isCorrect = false; break;
                }
            }
        }else if(Row[0].nType == NoteType.MT){
            for(int i=0; i<4; i++){
                if(inputButtons[i]==true){
                    isCorrect = false; break;
                }
            }
        }

        int jSample = cSample-rowPCM - (int)mAudio.offsetPCM;
        float jMilli = (float)jSample/mAudio.music.clip.frequency;


        if(InRange(jSample, PGREATPCM)){
            tmpJudge = JudgeType.PGREAT;
        }
        else if(InRange(jSample, GREATPCM)){
            tmpJudge = JudgeType.GREAT;
        }else if(InRange(jSample, GOODPCM)){
            tmpJudge = JudgeType.GOOD;
        }else if(InRange(jSample, OKPCM)){
            tmpJudge = JudgeType.OK;
        }else if(InRange(jSample, MISSPCM)){
            tmpJudge = JudgeType.MISS;
        }else if(jSample < -MISSPCM){
            tmpJudge = JudgeType.UNJUDGE;
        }

        if(jMilli>0.001f){
            tmptiming = JudgeTiming.SLOW;
        }else if (jMilli<-0.001f){
            tmptiming = JudgeTiming.FAST;
        }else{
            tmptiming = JudgeTiming.JUST;
        }

        if(!isCorrect){
            Debug.Log("WRONG BUTTON");
            tmpJudge = JudgeType.MISS;
        }


        //After Judge
        foreach(Note note in Row){
            if(tmpJudge == JudgeType.UNJUDGE) continue;
            mScoreMgr.ReqJudge2Score(tmpJudge, tmptiming);
            if(tmptiming == JudgeTiming.FAST && tmpJudge == JudgeType.MISS){
                continue;
            }
            if(note.nType==NoteType.CS){
                int ind = note.lane-1;
                charge[ind] = true;
                isCharge = true;
                Lanes[ind].Dequeue();
                mNoteMgr.ReqChargeAdjust(note.objID);
            }else{
                int ind = note.lane-1;
                Lanes[ind].Dequeue();
                Debug.Log(note.objID);
                mNoteMgr.ReqDestroy(note.objID);
            }
        }
        //Request to UI Manager
        mUIMan.ReqJudge(tmpJudge);
        mUIMan.ReqFastSlow(tmptiming, jMilli);
    }

    public void ReqCharge(){
        if(!isCharge)return;

        int cSample = mAudio.music.timeSamples;
        int timing = int.MaxValue;
        int rowPCM = int.MaxValue;
        foreach(Queue<Note> lane in Lanes){
            if(lane.Count <= 0) continue;
            var note = lane.Peek();
            if(note.nType != NoteType.CE) continue;
            int ntiming = (int)(note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom));
            if(timing > ntiming){
                rowPCM = (int)GetPCM(note);
                timing = ntiming;
                Row.Clear();
                Row.Add(note);
            }else if(timing == ntiming){
                rowPCM = (int)GetPCM(note);
                Row.Add(note);
            }
        }
        int jSample = cSample-rowPCM - (int)mAudio.offsetPCM;
        float jMilli = (float)jSample/mAudio.music.clip.frequency;
        if(jSample < GREATPCM && jSample > -GREATPCM){
            tmpJudge = JudgeType.COK;
        }else{
            tmpJudge = JudgeType.CGOOD;
        }
        if(jMilli>0.001f){
            tmptiming = JudgeTiming.SLOW;
        }else if (jMilli<-0.001f){
            tmptiming = JudgeTiming.FAST;
        }else{
            tmptiming = JudgeTiming.JUST;
        }


        //After Judge
        foreach(Note note in Row){
            if(note.nType==NoteType.CE){
                int ind = note.lane-1;
                Array.Fill(charge,false);
                isCharge = false;
                Lanes[ind].Dequeue();
                mNoteMgr.ReqDestroy(note.objID);
                mScoreMgr.ReqJudge2Score(tmpJudge, tmptiming);
                //UI req OK?
            }
        }
        mUIMan.ReqJudge(tmpJudge);
        mUIMan.ReqFastSlow(tmptiming, jMilli);
    }
}
