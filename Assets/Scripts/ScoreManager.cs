using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    public UIManager mUIMgr;
    public uint TotalScore;

    public uint Combo;
    public uint CB;

    public List<JudgeType> PaceMaker;

    public uint TotalNoteSize = 0; //includes NM CS, CE, MT 
    public uint TotalNMSize = 0; //includes NM
    public uint TotalCSSize = 0; //includes CS
    public uint TotalCESize = 0; //includes CE
    public uint TotalCNSize = 0; //includes CS + CE
    public uint TotalMTSize = 0; //includes MT

    public uint TotalPGREAT = 0; //SCORE*1.0
    public uint TotalGREAT = 0; //SCORE*0.5
    public uint TotalGOOD = 0; //SCORE*0.2
    public uint TotalOK = 0; //SCORE*0.0
    public uint TotalMISS = 0; //SCORE*0.0

    public uint indFast = 0;
    public uint indSlow = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNoteInfo(uint nm, uint cs, uint ce, uint mt){
        TotalNoteSize = nm + cs + ce + mt;
        TotalNMSize = nm;
        TotalCSSize = cs;
        TotalCESize = ce;
        TotalMTSize = mt;
        TotalCNSize = cs + ce;
        TotalPGREAT = 0;
        TotalGREAT = 0;
        TotalGOOD = 0;
        TotalOK = 0;
        TotalMISS = 0;
        indFast = 0;
        indSlow = 0;
        Combo = 0;
        CB = 0;
        TotalScore = 0;
    }

    // TotalScore
    public void ReqJudge2Score(JudgeType jtype, JudgeTiming jTiming){
        switch (jtype){
            case JudgeType.PGREAT: TotalPGREAT++; break;
            case JudgeType.GREAT: TotalGREAT++; break;
            case JudgeType.GOOD: TotalGOOD++; break;
            case JudgeType.OK: TotalOK++; break;
            case JudgeType.MISS: TotalMISS++; break;
            case JudgeType.COK : TotalPGREAT++; break;
            case JudgeType.CGOOD : TotalGREAT++; break;
        }
        if(jtype == JudgeType.OK || jtype== JudgeType.MISS){
            Combo = 0;
            CB++;
        }else if(jtype == JudgeType.PGREAT || jtype == JudgeType.GREAT || jtype == JudgeType.GOOD){
            Combo++;
        }

        if(jTiming == JudgeTiming.FAST && jtype != JudgeType.PGREAT && jtype != JudgeType.COK){
            indFast++;
        }if(jTiming == JudgeTiming.SLOW && jtype != JudgeType.PGREAT && jtype != JudgeType.COK){
            indSlow++;
        }

        TotalScore = (uint)(1000000 * (TotalPGREAT+ TotalGREAT*0.5f + TotalGOOD*0.2f) / TotalNoteSize);
        mUIMgr.ReqScore(TotalScore);
        mUIMgr.ReqCombo(Combo);
    }

    public bool isFC(){
        return CB == 0;
    }

    public uint GetTotalInput(){
        return TotalPGREAT + TotalGREAT + TotalGOOD + TotalOK + TotalMISS;
    }

}
