using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour
{
    public float musicBPM = 144f;

    public float _hiSpeed = 10f;

    float stdBPM = 60.0f;
    //float musicBeat = 4.0f;
    //float stdBeat = 4.0f;
   
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
    
    public float offset; // 음악의 시작점(초)
    public float offsetForSample; // 음악의 시작점(샘플)

    public float scrollSpeed; //곡 bpm에 따른 기본 배속
    public float userSpeedRate;

    void Start()
    {
        playTik = GetComponent<AudioSource>();
        music = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        scrollSpeed = 10.0f;
        userSpeedRate = 1f;

        musicBPM = 144f;
        //frequency of current music
        frequency = music.clip.frequency;//Integer
        // start point of music 
        offset = 1.72f;
        // 시작점 초를 샘플로 변환
        offsetForSample = frequency * offset;
        // 한박자 시간값
        oneBeatTime = (stdBPM / musicBPM);// * (musicBeat / stdBeat);
        // 첫박자 샘플값(오프셋)
        nextSample += offsetForSample;
        // 32비트기준 1비트의 시간값
        //bitPerSec = stdBPM / (8 * musicBPM);
        // 32비트기준 1비트의 샘플값
        //bitPerSample = bitPerSec * playMusic.clip.frequency;
        // 1바 시간값
        barPerSec = oneBeatTime * 4.0f;
        // 1바 샘플값
        //barPerSample = barPerSec * playMusic.clip.frequency;   

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