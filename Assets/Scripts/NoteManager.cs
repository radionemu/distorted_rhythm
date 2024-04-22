using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NoteManager : MonoBehaviour
{
    public RectTransform JudgeLine;
    public Sync _sync;

    public Sheet mSheet;
    public Judge mJudge;

    public List<RectTransform> mPos;

    public GameObject NotePrefab;
    public GameObject CNotePrefab;
    public GameObject MNotePrefab;
    GameObject CNoteClone;
    GameObject CNoteJudgeChild;

    public Dictionary<int, NoteObj> Notes = new();
    public List<Note> JudgeNote = new();

    bool isgen = true;

    // Start is called before the first frame update
    void Start()
    {
        //temp
        ShortNoteGenerate();
        mJudge.InitQueue();
    }

    // Update is called once per frame
    void Update()
    {
        // if(isgen){
        //     if(_sync.music.timeSamples >= _sync.offsetPCM){
        //         ShortNoteGenerate();
        //         // ChargeNoteGenerate();
        //         isgen = false;
        //     }
        // }

    }

    void ShortNoteGenerate(){
        float fLaneX = JudgeLine.position.x-JudgeLine.sizeDelta.x/2+50;
        GameObject tmp = null;
        Note tmpNote = new();
        foreach(Note note in mSheet.Notes){
            if(note.nType == NoteType.NM){
                tmp = Instantiate(NotePrefab, new Vector3(fLaneX+(note.lane-1)*100, JudgeLine.position.y+_sync.HiSpeed*(note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom))), Quaternion.identity, GameObject.Find("Canvas").transform);   
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
            }
            if(note.nType == NoteType.CS){
                tmpNote = note;
                tmp = Instantiate(CNotePrefab, new Vector3(fLaneX+(note.lane-1)*100, JudgeLine.position.y+_sync.HiSpeed*(note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom))), Quaternion.identity, GameObject.Find("Canvas").transform);
            }
            if(note.nType == NoteType.CE){
                float delta = (note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom)) - (tmpNote.section*4*(mSheet.beatNom/mSheet.beatDenom)+4*(((float)tmpNote.nom)/tmpNote.denom));
                Debug.Log(delta);
                tmp.transform.localScale = new Vector3(CNotePrefab.transform.localScale.x, CNotePrefab.transform.localScale.y*_sync.HiSpeed*delta, CNotePrefab.transform.localScale.z);
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
            }
            if(note.nType == NoteType.MT){
                tmp = Instantiate(MNotePrefab, new Vector3(JudgeLine.position.x, JudgeLine.position.y+_sync.HiSpeed*(note.section*4*(mSheet.beatNom/mSheet.beatDenom) + 4*(((float)note.nom)/note.denom))), Quaternion.identity, GameObject.Find("Canvas").transform);   
                Notes.Add(tmp.GetInstanceID(),tmp.GetComponent<NoteObj>());
            }
            note.setID(tmp.GetInstanceID());
            JudgeNote.Add(note);
        }
    }

    void ChargeNoteGenerate(){
            CNoteClone = Instantiate(CNotePrefab, new Vector3(3,_sync.HiSpeed*(1f)-0.5f), Quaternion.identity);
            CNoteClone.transform.localScale = new Vector3(CNotePrefab.transform.localScale.x, _sync.HiSpeed*(1f)+1.0f, CNotePrefab.transform.localScale.z);
            for(int i = 1; i <= 1*4f; i++){
                CNoteJudgeChild = Instantiate(MNotePrefab, new Vector3(3, CNoteClone.transform.position.y + _sync.HiSpeed*(i/4f)), Quaternion.identity);
                CNoteJudgeChild.transform.parent = CNoteClone.transform;          
           }
    }

    public void ReqDestroy(int id){
        if(Notes.ContainsKey(id)){
            GameObject obj = Notes[id].gameObject;
            Destroy(obj);
        } 
    }
}
