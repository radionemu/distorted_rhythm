using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public Sync _sync;

    public GameObject NotePrefab;

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
                NoteGenerate();
                isgen = false;
            }
        }

    }

    void NoteGenerate(){
            Instantiate(NotePrefab, new Vector3(400 ,_sync._hiSpeed*(0f)), Quaternion.identity);
            Instantiate(NotePrefab, new Vector3(450 ,_sync._hiSpeed*(1f)), Quaternion.identity);
            Instantiate(NotePrefab, new Vector3(500 ,_sync._hiSpeed*(2f)), Quaternion.identity);
            Instantiate(NotePrefab, new Vector3(550 ,_sync._hiSpeed*(3f)), Quaternion.identity);
    }


}
