using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum JudgeType{
    PGREAT,
    GREAT,
}

public enum JudgeTiming{
    FAST,
    SLOW,
    JUST,
}

public class Judge : MonoBehaviour
{
    public NoteManager mNoteMgr;

    //Judge Timing
    public float PGREAT = 33.34f;
    public float GREAT = 66.68f;

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

    private bool isCharge = false;
    private bool [] charge = new bool[4]{false, false, false, false};

    // Start is called before the first frame update
    void Start()
    {   
        PGREATPCM = (int)(PGREAT * mAudio.music.clip.frequency/1000f);
        GREATPCM = (int)(GREAT * mAudio.music.clip.frequency/1000f);
        Debug.Log(mAudio.music.clip.frequency);
        Debug.Log(PGREATPCM);
        Debug.Log(GREATPCM);

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
    }
 
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(JudgePoor());
    }

    IEnumerator JudgePoor(){
        int sample = mAudio.music.timeSamples;
        // Debug.Log("sample : "+sample);
        foreach(Queue<Note> lane in Lanes){
            if(lane.Count <= 0) continue;
            Note note = lane.Peek();
            // Debug.Log(""+GetPCM(note));
            if(note.nType == NoteType.CE && GetPCM(note) < sample){
                ReqCharge();
            }
        }

        yield return null;
    }

    public float GetPCM(Note note){
        return mAudio.oneBeatTime * mAudio.music.clip.frequency * (note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom));
    }

    public void reqJudge(bool [] inputButtons){
        int cSample = mAudio.music.timeSamples;
        //get Notes in Current Row
        
        bool [] reqButtons = new bool[4] {false, false, false, false};
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
        if(!isCorrect){
            Debug.Log("WRONG BUTTON");
            return;
        }

        int jSample = cSample-rowPCM - (int)mAudio.offsetPCM;
        float jMilli = (float)jSample/mAudio.music.clip.frequency;
        string str = "";
        Debug.Log(jSample);
        if(jSample < PGREATPCM && jSample > -PGREATPCM){
            str += "PGREAT";
            mUIMan.reqJudge(JudgeType.PGREAT);
        }
        else if(jSample < GREATPCM && jSample > -GREATPCM){
            str += "GREAT";
            mUIMan.reqJudge(JudgeType.GREAT);
        }
        if(jMilli>0.001f){
            str += " SLOW -"+(int)(jMilli*1000f);
            mUIMan.reqFastSlow(JudgeTiming.SLOW, jMilli);
        }else if (jMilli<-0.001f){
            str += " FAST +"+(int)(-jMilli*1000f);
            mUIMan.reqFastSlow(JudgeTiming.FAST, jMilli);
        }else{
            str += " JUST ";
        }
        Debug.Log(str);

        //After Judge
        foreach(Note note in Row){
            if(note.nType==NoteType.CS){
                int ind = note.lane-1;
                charge[ind] = true;
                isCharge = true;
                Lanes[ind].Dequeue();
            }else{
                int ind = note.lane-1;
                Lanes[ind].Dequeue();
                Debug.Log(note.objID);
                mNoteMgr.ReqDestroy(note.objID);
            }
        }
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
        string str = "";
        Debug.Log(jSample);
        if(jSample < GREATPCM && jSample > -GREATPCM){
            str += "OK!";
        }
        Debug.Log(str);

        //After Judge
        foreach(Note note in Row){
            if(note.nType==NoteType.CE){
                int ind = note.lane-1;
                Array.Fill(charge,false);
                isCharge = false;
                Lanes[ind].Dequeue();
                mNoteMgr.ReqDestroy(note.objID);
            }
        }
    }
}
