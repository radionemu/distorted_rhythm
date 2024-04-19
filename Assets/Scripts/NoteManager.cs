using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class NoteManager : MonoBehaviour
{
    public Sync _sync;

    public Sheet mSheet;

    public GameObject NotePrefab;
    public GameObject CNotePrefab;
    public GameObject CNoteChildP;
    GameObject CNoteClone;
    GameObject CNoteJudgeChild;

    bool isgen = true;

    // Start is called before the first frame update
    void Start()
    {
        //temp
        ShortNoteGenerate();
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
        foreach(Note note in mSheet.Notes){
            if(note.nType == NoteType.NM){
                Instantiate(NotePrefab, new Vector3(note.lane-1 ,_sync.HiSpeed*(note.section * mSheet.beatDenom + note.nom)), Quaternion.identity);   
            }
        }
    }

    void ChargeNoteGenerate(){

            CNoteClone = Instantiate(CNotePrefab, new Vector3(3,_sync.HiSpeed*(1f)-0.5f), Quaternion.identity);
            CNoteClone.transform.localScale = new Vector3(CNotePrefab.transform.localScale.x, _sync.HiSpeed*(1f)+1.0f, CNotePrefab.transform.localScale.z);
        //판정부 1비트 /(기준 비트 = 4) 포지션에 노트 인스턴스 생성 => 로컬스케일 * 기준 비트를 for문에 넣어야 함
            for(int i = 1; i <= 1*4f; i++){
                CNoteJudgeChild = Instantiate(CNoteChildP, new Vector3(3, CNoteClone.transform.position.y + _sync.HiSpeed*(i/4f)), Quaternion.identity);
                CNoteJudgeChild.transform.parent = CNoteClone.transform;          
           }
    }
}
