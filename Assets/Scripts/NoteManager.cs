using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public Sync _sync;

    public GameObject NotePrefab;
    public GameObject CNotePrefab;
    public GameObject CNoteChildP;
    GameObject CNoteClone;
    GameObject CNoteJudgeChild;

    bool isgen = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isgen){
            if(_sync.music.timeSamples >= _sync.offsetForSample){
                ShortNoteGenerate();
                ChargeNoteGenerate();
                isgen = false;
            }
        }

    }

    void ShortNoteGenerate(){
            Instantiate(NotePrefab, new Vector3(400 ,_sync._hiSpeed*(0f)), Quaternion.identity);
            Instantiate(NotePrefab, new Vector3(450 ,_sync._hiSpeed*(1f)), Quaternion.identity);
            Instantiate(NotePrefab, new Vector3(500 ,_sync._hiSpeed*(2f)), Quaternion.identity);
            Instantiate(NotePrefab, new Vector3(550 ,_sync._hiSpeed*(3f)), Quaternion.identity);
    }

    void ChargeNoteGenerate(){
        //인스턴스 생성 및 크기 조정 (피봇 형식으로 크기 조정)
            CNoteClone = Instantiate(CNotePrefab, new Vector3(475,_sync._hiSpeed*(1f)), Quaternion.identity);
            CNoteClone.transform.localScale = new Vector3(CNotePrefab.transform.localScale.x, _sync._hiSpeed*(1f), CNotePrefab.transform.localScale.z);
        //판정부 1비트 /(기준 비트 = 4) 포지션에 노트 인스턴스 생성 => 로컬스케일 * 기준 비트를 for문에 넣어야 함
            for(int i = 1; i <= 1*4f; i++){
                CNoteJudgeChild = Instantiate(CNoteChildP, new Vector3(525, CNoteClone.transform.position.y + _sync._hiSpeed*(i/4f)), Quaternion.identity);
                CNoteJudgeChild.transform.parent = CNoteClone.transform;          
           }
    }
}
