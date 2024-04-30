using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NoteManager : MonoBehaviour
{
    public float NoteOffset = -0.1f; //sec
    public Judge mJudge;
    public ScoreManager mScoreMan;
    public PlayerSetting mPsetting;

    public GameObject NotePrefab;
    public GameObject CNotePrefab;
    public GameObject MNotePrefab;
    public GameObject initPreset;

    public Transform JudgeLine;

    public Dictionary<int, NoteObj> Notes = new();
    public List<Note> JudgeNote = new();


    // Start is called before the first frame update
    void Start()
    {
        //temp
        // GenerateNote();
        // mJudge.InitQueue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GenerateNote(Sync mSync, Sheet mSheet, List<Color> noteCol){
        foreach(KeyValuePair<int, NoteObj> n in Notes){
            GameObject obj = Notes[n.Key].gameObject;
            //add Effect if needed
            //Probably Use Coroutine
            NoteObj nobj = obj.GetComponent<NoteObj>();
                nobj.StopAllCoroutines();
            Destroy(obj);
        }
        Notes.Clear();
        JudgeNote.Clear();
        uint NM=0;
        uint CS=0;
        uint CE=0;
        uint MT=0;
        float fLaneX = JudgeLine.position.x-JudgeLine.transform.lossyScale.x*2+NotePrefab.transform.lossyScale.x/2;
        int isGuide = mSheet.DrumIntro ? 1 : 0;
        GameObject tmp = null;
        Note tmpNote = new();
        foreach(Note note in mSheet.Notes){
            if(note.nType == NoteType.NM){
                tmp = Instantiate(NotePrefab, new Vector3(
                                        fLaneX+(note.lane-1)*NotePrefab.transform.localScale.x,
                                        JudgeLine.position.y+mSync.HiSpeed*mSync.oneBeatTime*(4*(mSheet.beatNom/mSheet.beatDenom)*(isGuide+note.section+((float)note.nom)/note.denom))+ mPsetting.DisplayOffset,
                                        0.5f)
                                , Quaternion.identity, initPreset.transform);   
                //obj config
                NoteObj obj = tmp.GetComponent<NoteObj>();
                obj.JudgelinePosition = JudgeLine.position;
                obj.Init(mSync, mSheet, note, noteCol);
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
                NM++;
            }
            if(note.nType == NoteType.CS){
                tmpNote = note;
                tmp = Instantiate(CNotePrefab, new Vector3(
                                        fLaneX+(note.lane-1)*NotePrefab.transform.localScale.x,
                                        JudgeLine.position.y+mSync.HiSpeed*mSync.oneBeatTime*(4*(mSheet.beatNom/mSheet.beatDenom)*(isGuide+note.section+((float)note.nom)/note.denom))+ mPsetting.DisplayOffset,
                                        0.5f)
                                , Quaternion.identity, initPreset.transform);
                CS++;
            }
            if(note.nType == NoteType.CE){
                float delta = mSync.oneBeatTime*(4*(mSheet.beatNom/mSheet.beatDenom)*(isGuide+note.section+((float)note.nom)/note.denom)) - mSync.oneBeatTime*(4*(mSheet.beatNom/mSheet.beatDenom)*(isGuide+tmpNote.section+((float)tmpNote.nom)/tmpNote.denom));
                // Debug.Log(delta);
                tmp.transform.localScale = new Vector3(CNotePrefab.transform.localScale.x, CNotePrefab.transform.localScale.y*mSync.HiSpeed*delta, CNotePrefab.transform.localScale.z);
                NoteObj obj = tmp.GetComponent<NoteObj>();
                obj.JudgelinePosition = JudgeLine.position;
                obj.Init(mSync, mSheet, tmpNote, noteCol);
                obj.SetCE(note);
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
                CE++;
            }
            if(note.nType == NoteType.MT){
                tmp = Instantiate(MNotePrefab, new Vector3(JudgeLine.position.x,
                                        JudgeLine.position.y+mSync.HiSpeed*mSync.oneBeatTime*(4*(mSheet.beatNom/mSheet.beatDenom)*(isGuide+note.section+((float)note.nom)/note.denom))+ mPsetting.DisplayOffset,
                                        0.5f)
                                , Quaternion.identity, initPreset.transform);                   
                NoteObj obj = tmp.GetComponent<NoteObj>();
                obj.JudgelinePosition = JudgeLine.position;
                obj.Init(mSync, mSheet, note, noteCol);
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
                MT++;
            }
            note.setID(tmp.GetInstanceID());
            JudgeNote.Add(note);
        }
        mScoreMan.SetNoteInfo(NM, CS, CE, MT);
        return true;
    }

    public List<Note> getJudgeNote(){
        return JudgeNote;
    }
    public void ReqChargeAdjust(int id){
        if(Notes.ContainsKey(id)){
            GameObject obj = Notes[id].gameObject;
            float deltaY = obj.transform.position.y - JudgeLine.position.y;
            obj.transform.position = new Vector3(obj.transform.position.x, JudgeLine.position.y);
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y+deltaY, obj.transform.localScale.z);
            NoteObj nobj = obj.GetComponent<NoteObj>();
            // if(nobj.IENoteScrollRunning){
            //     nobj.StopCoroutine(nobj.NoteScroll());
            //     nobj.IENoteScrollRunning = false;
            // }
            nobj.StartCoroutine(nobj.NoteScale(JudgeLine));
        }
    }

    public void ReqDestroy(int id){
        if(Notes.ContainsKey(id)){
            GameObject obj = Notes[id].gameObject;
            //add Effect if needed
            //Probably Use Coroutine
            NoteObj nobj = obj.GetComponent<NoteObj>();
            if(nobj.IENoteScaleRunning){
                nobj.StopCoroutine(nobj.NoteScale(JudgeLine));
            }
            Destroy(obj);
        }
        Notes.Remove(id);
    }
}
