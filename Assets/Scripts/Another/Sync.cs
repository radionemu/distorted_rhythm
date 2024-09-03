using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour
{

    //ambiguous
    public float HiSpeed = 15f;

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
    public bool isGuidPlaying = true;
    public float guideTime = 0.000f;
    public float guidePCM = 0000f;

    void Start()
    {
        //component
        // playTik = GetComponent<AudioSource>();
        // music = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        // frequency = music.clip.frequency;//Integer
        // offsetPCM = frequency * offset;
        // oneBeatTime = stdBPM / musicBPM * (beatnom / beatdenom);//delta sec for one beat
        // nextSample += offsetPCM; // next sample
        // bitPerSec = stdBPM / (8 * musicBPM);
        // bitPerSample = bitPerSec * music.clip.frequency;
        // barPerSec = oneBeatTime * 4.0f;
        // barPerSample = barPerSec * music.clip.frequency;   

    }

    public bool init(Sheet mSheet){
        
        //component
        playTik = GetComponent<AudioSource>();
        music = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        //load resource
        music.clip = Resources.Load("Music/"+mSheet.FileName) as AudioClip;
        musicBPM = mSheet.BPM;
        stdBPM = 60f;
        beatnom = mSheet.beatNom;
        beatdenom = mSheet.beatDenom;

        //music Frequency
        frequency = music.clip.frequency;//Integer
        //offset
        offset = mSheet.Offset;
        offsetPCM = frequency * offset;
        oneBeatTime = stdBPM / musicBPM * (beatnom / beatdenom);//delta sec for one beat
        nextSample += 0; // next sample
        bitPerSec = stdBPM / (8 * musicBPM);
        bitPerSample = bitPerSec * music.clip.frequency;
        barPerSec = oneBeatTime * 4.0f;
        barPerSample = barPerSec * music.clip.frequency;

        return true;
    }

    void Update() {
    }

    public void reqPlayMusic(bool guide){
        music.Stop();

        StopAllCoroutines();
        StartCoroutine(PlayMusic(guide));
    }

    public IEnumerator PlayMusic(bool guide){
        float startTime = Time.time;
        music.timeSamples = (int)offsetPCM;
        nextSample = 0;
        int i = 0;
        Debug.Log(music.timeSamples);
        if(guide){
            isGuidPlaying = true;
            while(i<5){
                guideTime = Time.time-startTime; guidePCM = guideTime * frequency;
                if (guidePCM >= nextSample)
                {
                    if(i<4)
                        playTik.PlayOneShot(tikClip); // 사운드 재생
                    beatPerSample = oneBeatTime * frequency;
                    nextSample += beatPerSample;
                    i++;
                }
                yield return null;
            }
        }
        yield return new WaitUntil(()=>i>4);
        Debug.Log("PLAY!");
        music.Play();
        isGuidPlaying = false;
    }

    IEnumerator PlayIntro(int iter)
    {   
        int i = 0;
        while(i>iter){
            if (music.timeSamples >= nextSample)
            {
                playTik.PlayOneShot(tikClip); // 사운드 재생
                beatPerSample = oneBeatTime * frequency;
                nextSample += beatPerSample;
            }
            yield return null;
        }
        yield return true;
    }

    public bool IsPlaying(){
        return isGuidPlaying || music.isPlaying;
    }
}