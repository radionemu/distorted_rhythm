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

    public Sync _sync;

    public Sheet mSheet;
    public Judge mJudge;
    public ScoreManager mScoreMan;

    public GameObject NotePrefab;
    public GameObject CNotePrefab;
    public GameObject MNotePrefab;

    public Transform JudgeLine;

    public Dictionary<int, NoteObj> Notes = new();
    public List<Note> JudgeNote = new();


    // Start is called before the first frame update
    void Start()
    {
        //temp
        GenerateNote();
        mJudge.InitQueue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateNote(){
        uint NM=0, CS =0, CE =0, MT = 0;
        float fLaneX = JudgeLine.position.x-JudgeLine.transform.lossyScale.x/2+NotePrefab.transform.lossyScale.x/2;
        GameObject tmp = null;
        Note tmpNote = new();
        foreach(Note note in mSheet.Notes){
            if(note.nType == NoteType.NM){
                tmp = Instantiate(NotePrefab, new Vector3(fLaneX+(note.lane-1)*NotePrefab.transform.localScale.x, JudgeLine.position.y+_sync.HiSpeed*(note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom)+ NoteOffset)), Quaternion.identity);   
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
                NM++;
            }
            if(note.nType == NoteType.CS){
                tmpNote = note;
                tmp = Instantiate(CNotePrefab, new Vector3(fLaneX+(note.lane-1)*NotePrefab.transform.localScale.x, JudgeLine.position.y+_sync.HiSpeed*(note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom) + NoteOffset)), Quaternion.identity);
                CS++;
            }
            if(note.nType == NoteType.CE){
                float delta = (note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom)) - (tmpNote.section*4*(mSheet.beatNom/mSheet.beatDenom)+4*(((float)tmpNote.nom)/tmpNote.denom));
                // Debug.Log(delta);
                tmp.transform.localScale = new Vector3(CNotePrefab.transform.localScale.x, CNotePrefab.transform.localScale.y*_sync.HiSpeed*delta, CNotePrefab.transform.localScale.z);
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
                CE++;
            }
            if(note.nType == NoteType.MT){
                tmp = Instantiate(MNotePrefab, new Vector3(JudgeLine.position.x, JudgeLine.position.y+_sync.HiSpeed*(note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom) + NoteOffset)), Quaternion.identity);   
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
                MT++;
            }
            note.setID(tmp.GetInstanceID());
            JudgeNote.Add(note);
        }
        mScoreMan.SetNoteInfo(NM, CS, CE, MT);
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
    }
}
