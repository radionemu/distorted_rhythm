using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoteObj : MonoBehaviour
{
    public Note note;
    public Note CENote;

    public Sync _sync;
    public float _spdAmplifier = 1.0f;
    float _noteSpeed;
    bool ismove = true;
    float timer;
    
    //Coroutine Running Check
    public bool IENoteScaleRunning = false;
    public bool IENoteScrollRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NoteScroll());
    }

    // Update is called once per frame
    void Update()
    {
        HiSpeed();
        _noteSpeed = _sync.HiSpeed * (_sync.musicBPM/60.0f);
        timer += Time.smoothDeltaTime;
        
        
    }

    public void Move(){
        StartCoroutine(NoteScroll());
    }

    public void reqChargeScale(Transform JudgeLine){
        StartCoroutine(NoteScale(JudgeLine));
    }

    public IEnumerator NoteScroll(){
        IENoteScrollRunning = true;
        while(true){
            transform.Translate(new Vector3(0, -_noteSpeed*Time.smoothDeltaTime));
            yield return null;
        }
    }

    public IEnumerator NoteScale(Transform JudgeLine){
        IENoteScaleRunning = true;
        while(true){
            yield return null;
            transform.position = new Vector3(transform.position.x, JudgeLine.position.y);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y-_noteSpeed*Time.smoothDeltaTime, transform.localScale.z);
        }
    }

    public void HiSpeed(){
        if(Input.GetKeyDown(KeyCode.E)){
            transform.position = new Vector3(transform.position.x, transform.position.y * _spdAmplifier);
            _sync.HiSpeed += _spdAmplifier;
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            transform.position = new Vector3(transform.position.x, transform.position.y / _spdAmplifier);
            _sync.HiSpeed -= _spdAmplifier;
        }
    }
}