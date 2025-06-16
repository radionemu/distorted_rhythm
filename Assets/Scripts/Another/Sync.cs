using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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

	public int frequency = 0;
	float nextSample = 0f;

	public float offset = 0.000f;
	public float offsetPCM = 7000f;
	public bool isGuidPlaying = true;
	public double guideTime = 0.000f;
	public float guidePCM = 0000f;

	void Start()
	{

	}

	public bool init(Sheet mSheet)
	{

		//component
		playTik = GetComponent<AudioSource>();
		music = GameObject.Find("Audio Source").GetComponent<AudioSource>();

		//load resource
		music.clip = Resources.Load("Music/" + mSheet.FileName) as AudioClip;
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

	public void reqPlayMusic(bool guide)
	{
		music.Stop();
		timeThread?.Join();
		StopAllCoroutines();
		StartCoroutine(PlayMusic(guide));
	}

	public IEnumerator PlayMusic(bool guide)
	{
		double startTime = AudioSettings.dspTime;
		music.timeSamples = (int)offsetPCM;
		nextSample = 0;
		int i = 0;
		// Debug.Log(music.timeSamples);
		if (guide)
		{
			isGuidPlaying = true;
			while (i < 5)
			{
				guideTime = AudioSettings.dspTime - startTime; guidePCM = (float)(guideTime * frequency);
				if (guidePCM >= nextSample)
				{
					if (i < 4)
						playTik.PlayOneShot(tikClip); // 사운드 재생
					beatPerSample = oneBeatTime * frequency;
					nextSample += beatPerSample;
					i++;
				}
				yield return null;
			}
		}
		yield return new WaitUntil(() => i > 4);
		// Debug.Log("PLAY!");
		music.Play();
		timeThread = new Thread(TimeThread);
		timeThread.Priority = System.Threading.ThreadPriority.Highest;
		timeThread.IsBackground = true;
		timeThread.Start();

		startDsp = AudioSettings.dspTime;
		isGuidPlaying = false;
	}

	public bool IsPlaying()
	{
		return isGuidPlaying || music.isPlaying;
	}

	//MultiThreading
	private long currentSampleCounter = 0;
	private int i = 1;
	public double startDsp = 0f;

	void Update()
	{
		currentSampleCounter = music.timeSamples;
	}

	// void OnAudioFilterRead(float[] data, int channels)
	// {
	// 	long samplesInThisBuffer = data.Length / channels;
	// 	Interlocked.Add(ref currentSampleCounter, samplesInThisBuffer);
	// }
	Thread timeThread;
	float rest = 0f;
	public void TimeThread()
	{
		currentSampleCounter += (long)(frequency / 1000f);
		rest += frequency % 1000;
		if (rest > 1f)
		{
			currentSampleCounter++;
			rest -= 1f;
		}
		delayUs(1000);
	}

	void delayUs(long us)
	{
		//Stopwatch 초기화 후 시간 측정 시작
		Stopwatch startNew = Stopwatch.StartNew();
		//설정한 us를 비교에 쓰일 Tick값으로 변환
		long usDelayTick = (us * Stopwatch.Frequency) / 1000000;
		//변환된 Tick값보다 클때까지 대기 
		while (startNew.ElapsedTicks < usDelayTick);
	}

	public long GetCurrentSampleCountThreadSafe()
	{
		return currentSampleCounter;
	}
}