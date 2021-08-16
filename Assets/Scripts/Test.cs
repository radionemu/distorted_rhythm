using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    AudioSource aud;
    
    void Start() {
    	aud = GetComponent<AudioSource>();
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            aud.Stop();
            aud.Play();
        }
        Debug.Log(aud.timeSamples);
    }
}
