using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public Sync _sync;


    public float _spdAmplifier = 1.1f;
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
        _noteSpeed = _sync._hiSpeed / (60/_sync.musicBPM);
        timer += Time.smoothDeltaTime;
        StartCoroutine(NoteScroll());
    }

    IEnumerator NoteScroll(){
        transform.Translate(new Vector3(0, -(_noteSpeed)*Time.smoothDeltaTime));
        yield return null;
    }

    public void HiSpeed(){
        if(Input.GetKeyDown(KeyCode.E)){
            transform.position = new Vector3(transform.position.x, transform.position.y * _spdAmplifier);
            _sync._hiSpeed *= _spdAmplifier;
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            transform.position = new Vector3(transform.position.x, transform.position.y / _spdAmplifier);
            _sync._hiSpeed /= _spdAmplifier;
        }
    }
}