using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObj : MonoBehaviour
{
    public Sync _sync;


    public float _spdAmplifier = 1.0f;
    float _noteSpeed;
    bool ismove = true;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HiSpeed();
        _noteSpeed = _sync.HiSpeed * (_sync.musicBPM/60.0f);
        timer += Time.smoothDeltaTime;
        StartCoroutine(NoteScroll());
        
    }

    IEnumerator NoteScroll(){
        transform.Translate(new Vector3(0, -_noteSpeed*Time.smoothDeltaTime));
        yield return null;
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