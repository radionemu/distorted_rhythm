using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ResultUIManager : MonoBehaviour
{
    public ScoreManager mScoreMgr;

    public TextMeshProUGUI TotalScoreNum;
    public UnityEngine.UI.Image FastGraph;
    public UnityEngine.UI.Image SlowGraph;

    public TextMeshProUGUI PGREATnum;
    public TextMeshProUGUI GREATnum;
    public TextMeshProUGUI GOODnum;
    public TextMeshProUGUI OKnum;
    public TextMeshProUGUI MISSnum;

    public TextMeshProUGUI FASTnum;
    public TextMeshProUGUI SLOWnum;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(){
        PGREATnum.text = mScoreMgr.TotalPGREAT.ToString();
        GREATnum.text = mScoreMgr.TotalGREAT.ToString();
        GOODnum.text = mScoreMgr.TotalGOOD.ToString();
        OKnum.text=mScoreMgr.TotalOK.ToString();
        MISSnum.text=mScoreMgr.TotalMISS.ToString();

        FASTnum.text=mScoreMgr.indFast.ToString();
        SLOWnum.text=mScoreMgr.indSlow.ToString();

        FastGraph.transform.localScale = new Vector2(mScoreMgr.indFast / (float)mScoreMgr.GetTotalInput(),FastGraph.transform.localScale.y);
        SlowGraph.transform.localScale = new Vector2(mScoreMgr.indSlow / (float)mScoreMgr.GetTotalInput(),SlowGraph.transform.localScale.y);
        TotalScoreNum.text = string.Format("{0:D7}", mScoreMgr.TotalScore);
        DBManager.score = (int)mScoreMgr.TotalScore;
    }
}
