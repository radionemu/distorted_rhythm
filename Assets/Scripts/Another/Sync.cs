using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour
{

    //ambiguous
    public float HiSpeed = 10f;

    public float musicBPM = 150f; //BPM of MUSIC
    float stdBPM = 60.0f; //one minutes
    
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

    float frequency = 0f;
    float nextSample = 0f;
    
    public float offset;
    public float offsetPCM;


    void Start()
    {
        //component
        playTik = GetComponent<AudioSource>();
        music = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        HiSpeed = 10.0f;
        frequency = music.clip.frequency;//Integer
        offset = 0.0f; //offset of start point
        offsetPCM = frequency * offset;
        oneBeatTime = stdBPM / musicBPM * (beatnom / beatdenom);//delta sec for one beat
        nextSample += offsetPCM; // next sample
        bitPerSec = stdBPM / (8 * musicBPM);
        bitPerSample = bitPerSec * music.clip.frequency;
        barPerSec = oneBeatTime * 4.0f;
        barPerSample = barPerSec * music.clip.frequency;   

        music.Play();  
    }

    void Update() {
        StartCoroutine(PlayTik());    
    }

    IEnumerator PlayTik()
    {   
        // 초당 44100 샘플값 증가 프리퀀시값인 44100나누면 정확히 나눠떨어짐
        if (music.timeSamples >= nextSample)
        {
            playTik.PlayOneShot(tikClip); // 사운드 재생
            beatPerSample = oneBeatTime * frequency;
            nextSample += beatPerSample;
        }
        yield return null;
    }
}