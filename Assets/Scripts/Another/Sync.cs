using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour
{

    //ambiguous
    public float HiSpeed;

    public float musicBPM = 150f; //BPM of MUSIC
    public float stdBPM = 60.0f; //one minutes
    
    //this mean total beat is 4/4
    float beatnom = 4.0f;
    float beatdenom = 4.0f;

   
   public AudioSource playTik;
   public AudioClip tikClip;
    public AudioSource music;

    public float oneBeatTime = 0f;
    public float beatPerSample = 0f;

    public float bitPerSec = 0f;
    public float bitPerSample = 0f;

    public float barPerSec = 0f;
    public float barPerSample = 0f;

    float frequency = 0.2f;
    float nextSample = 0f;
    
    public float offset = 0.000f;
    public float offsetPCM = 7000f;


    void Start()
    {
        //component
        playTik = GetComponent<AudioSource>();
        music = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        frequency = music.clip.frequency;//Integer
        // offsetPCM = frequency * offset;
        oneBeatTime = stdBPM / musicBPM * (beatnom / beatdenom);//delta sec for one beat
        // nextSample += offsetPCM; // next sample
        bitPerSec = stdBPM / (8 * musicBPM);
        bitPerSample = bitPerSec * music.clip.frequency;
        barPerSec = oneBeatTime * 4.0f;
        barPerSample = barPerSec * music.clip.frequency;   

    }

    void Update() {
        StartCoroutine(PlayTik());    
    }

    public void PlayMusic(){
        music.Play();
    }

    IEnumerator PlayTik()
    {   
        if (music.timeSamples >= nextSample)
        {
            playTik.PlayOneShot(tikClip); // 사운드 재생
            beatPerSample = oneBeatTime * frequency;
            nextSample += beatPerSample;
        }
        yield return null;
    }
}