using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum JudgeType
{
	PGREAT,
	GREAT,
	GOOD,
	OK,
	MISS,
	COK,
	CGOOD,
	UNJUDGE,
}

public enum JudgeTiming
{
	FAST,
	SLOW,
	JUST,
}

public class Judge : MonoBehaviour
{
	public NoteManager mNoteMgr;
	public ScoreManager mScoreMgr;

	//Judge Timing
	public float PGREAT = 33f;
	public float GREAT = 57f;
	public float GOOD = 81f;
	public float OK = 105f;
	public float MISS = 129f;

	//ref Class
	public Sync mAudio;
	public Sheet mSheet;
	public UIManager mUIMan;
	public PlayerSetting mPsetting;

	List<Queue<Note>> Lanes = new List<Queue<Note>>();
	Queue<Note> laneA = new();
	Queue<Note> laneB = new();
	Queue<Note> laneC = new();
	Queue<Note> laneD = new();
	Queue<Note> laneM = new();

	// List<Note> Row = new();

	private int PGREATPCM;
	private int GREATPCM;
	private int GOODPCM;
	private int OKPCM;
	private int MISSPCM;

	private bool isCharge = false;
	private bool[] charge = new bool[4] { false, false, false, false };

	// Start is called before the first frame update
	class JudgeItem
	{
		public JudgeType judge;
		public float judgeTiming;
		public JudgeTiming judgeTimingEnum = JudgeTiming.JUST;
		public List<Note> judgeRow = new();
		public bool IsChargeStart => judgeRow.Exists(e => e.nType == NoteType.CS);
		public bool IsChargeEnd => judgeRow.Exists(e => e.nType == NoteType.CE);
		public bool IsCorrect => judge != JudgeType.MISS;

		public JudgeItem(JudgeType judge, float judgeTiming, List<Note> judgeRow)
		{
			this.judge = judge;
			this.judgeTiming = judgeTiming;
			this.judgeRow = judgeRow;

			if (this.judgeTiming > 0.001f)
				judgeTimingEnum = JudgeTiming.SLOW;
			else if (this.judgeTiming < -0.001f)
				judgeTimingEnum = JudgeTiming.FAST;
			else
				judgeTimingEnum = JudgeTiming.JUST;
		}
	}

	private ConcurrentQueue<JudgeItem> judgeQueue = new();


	public void Init()
	{
		PGREATPCM = (int)(PGREAT * mAudio.frequency / 1000f);
		GREATPCM = (int)(GREAT * mAudio.frequency / 1000f);
		GOODPCM = (int)(GOOD * mAudio.frequency / 1000f);
		OKPCM = (int)(OK * mAudio.frequency / 1000f);
		MISSPCM = (int)(MISS * mAudio.frequency / 1000f);

	}

	public bool InitQueue()
	{
		Lanes.Clear();
		laneA.Clear();
		laneB.Clear();
		laneC.Clear();
		laneD.Clear();
		laneM.Clear();

		//Enqueue Notes each lane
		foreach (Note note in mNoteMgr.JudgeNote)
		{
			switch (note.lane)
			{
				case 1: laneA.Enqueue(note); break;
				case 2: laneB.Enqueue(note); break;
				case 3: laneC.Enqueue(note); break;
				case 4: laneD.Enqueue(note); break;
				case 5: laneM.Enqueue(note); break;
				default: return false;
			}
		}
		Lanes.Add(laneA);
		Lanes.Add(laneB);
		Lanes.Add(laneC);
		Lanes.Add(laneD);
		Lanes.Add(laneM);

		return true;
	}

	// Update is called once per frame
	void Update()
	{
		StartCoroutine(JudgeMiss());
	}

	void LateUpdate()
	{
		if (judgeQueue.Count <= 0) return;
		while (judgeQueue.TryDequeue(out var item))
		{
			if (!item.IsChargeEnd)
			{
				//After Judge
				foreach (Note note in item.judgeRow)
				{
					if (item.judge == JudgeType.UNJUDGE) break;
					if (note.nType == NoteType.CE) break;
					mScoreMgr.ReqJudge2Score(item.judge, item.judgeTimingEnum);
					if (item.judgeTimingEnum == JudgeTiming.FAST && item.judge == JudgeType.MISS)
					{
						continue;
					}
					if (note.nType == NoteType.CS)
					{
						int ind = note.lane - 1;
						Lanes[ind].Dequeue();
						if (item.IsCorrect)
							mNoteMgr.ReqChargeAdjust(note.objID);
					}
					else if (note.nType == NoteType.NM || note.nType == NoteType.MT)
					{
						int ind = note.lane - 1;
						Lanes[ind].Dequeue();
						mNoteMgr.ReqDestroy(note.objID);
					}

					//Request to UI Manager
					mUIMan.ReqJudge(item.judge, note.lane);
					mUIMan.ReqFastSlow(item.judgeTimingEnum, item.judgeTiming);
				}

			}
			else
			{
				//After Judge
				foreach (Note note in item.judgeRow)
				{
					if (note.nType == NoteType.CE)
					{
						int ind = note.lane - 1;
						Array.Fill(charge, false);
						isCharge = false;
						Lanes[ind].Dequeue();
						mNoteMgr.ReqDestroy(note.objID);
						mScoreMgr.ReqJudge2Score(item.judge, item.judgeTimingEnum);
					}
				}
				mUIMan.ReqJudge(item.judge, -1);
				mUIMan.ReqFastSlow(item.judgeTimingEnum, item.judgeTiming);
			}


		}

	}

	IEnumerator JudgeMiss()
	{
		int sample = mAudio.music.timeSamples;
		// Debug.Log("sample : "+sample);
		foreach (Queue<Note> lane in Lanes)
		{
			if (lane.Count <= 0) continue;
			Note note = lane.Peek();
			// Debug.Log(""+GetPCM(note));
			int qsample = sample - (int)GetPCM(note) - (int)(mAudio.music.clip.frequency * mPsetting.JudgeOffset);
			if (note.nType == NoteType.CE && qsample >= 0)
			{
				ReqCharge();
			}
			if (qsample > OKPCM)
			{
				mUIMan.ReqJudge(JudgeType.MISS, note.lane);
				mScoreMgr.ReqJudge2Score(JudgeType.MISS, JudgeTiming.SLOW);
				if (lane.Peek().nType != NoteType.CS)
				{
					mNoteMgr.ReqDestroy(note.objID);
				}
				lane.Dequeue();
			}
		}

		yield return null;
	}

	public float GetPCM(Note note)
	{
		return mAudio.oneBeatTime * mAudio.frequency * (4 * (mSheet.beatNom / mSheet.beatDenom) * (note.section + ((float)note.nom) / note.denom));
	}

	private bool InRange(int timing, int pmRange)
	{
		return timing < pmRange && timing > -pmRange;
	}

	public void ReqJudgeThread(bool[] inputButtons)
	{
		List<Note> judgeRow = new();
		int cSample = (int)mAudio.GetCurrentSampleCountThreadSafe();
		//get Notes in Current Row

		bool[] reqButtons = new bool[5] { false, false, false, false, false };//BT-A, BT-B, BT-C, BT-D, STROKE
		int timing = int.MaxValue;
		int rowPCM = int.MaxValue;
		foreach (Queue<Note> lane in Lanes)
		{
			if (lane.Count <= 0) continue;
			var note = lane.Peek();
			int ntiming = (int)(64 * (mSheet.beatNom / mSheet.beatDenom) * (note.section + ((float)note.nom) / note.denom));
			if (timing > ntiming)
			{
				rowPCM = (int)GetPCM(note);
				timing = ntiming;
				judgeRow.Clear();
				Array.Fill(reqButtons, false);
				judgeRow.Add(note);
				reqButtons[note.lane - 1] = true;
			}
			else if (timing == ntiming)
			{
				rowPCM = (int)GetPCM(note);
				reqButtons[note.lane - 1] = true;
				judgeRow.Add(note);
			}
		}

		if (judgeRow.Count <= 0) return;

		bool isCorrect = true;

		if (judgeRow[0].nType == NoteType.NM || judgeRow[0].nType == NoteType.CS)
		{
			for (int i = 0; i < 4; i++)
			{
				if (reqButtons[i] != inputButtons[i])
				{
					isCorrect = false; break;
				}
			}
		}
		else if (judgeRow[0].nType == NoteType.MT)
		{
			for (int i = 0; i < 4; i++)
			{
				if (inputButtons[i] == true)
				{
					isCorrect = false; break;
				}
			}
		}

		int jSample = cSample - rowPCM - (int)(mAudio.frequency * mPsetting.JudgeOffset);
		float judgeInterval = (float)jSample / mAudio.frequency;

		JudgeType judge = JudgeType.UNJUDGE;
		if (InRange(jSample, PGREATPCM))
			judge = JudgeType.PGREAT;
		else if (InRange(jSample, GREATPCM))
			judge = JudgeType.GREAT;
		else if (InRange(jSample, GOODPCM))
			judge = JudgeType.GOOD;
		else if (InRange(jSample, OKPCM))
			judge = JudgeType.OK;
		else if (InRange(jSample, MISSPCM))
			judge = JudgeType.MISS;
		else if (jSample < -MISSPCM)
			judge = JudgeType.UNJUDGE;

		if (!isCorrect)
			judge = JudgeType.MISS;

		//TODO isCharge update
		if (judgeRow.Exists(e => e.nType == NoteType.CS))
			isCharge = true;
		else
			isCharge = false;

		judgeQueue.Enqueue(new(judge, judgeInterval, judgeRow));
	}

	public void ReqCharge()
	{
		if (!isCharge) return;

		List<Note> judgeRow = new();
		int cSample = (int)mAudio.GetCurrentSampleCountThreadSafe();
		int timing = int.MaxValue;
		int rowPCM = int.MaxValue;
		foreach (Queue<Note> lane in Lanes)
		{
			if (lane.Count <= 0) continue;
			var note = lane.Peek();
			if (note.nType != NoteType.CE) continue;
			int ntiming = (int)(note.section * 4 * (mSheet.beatNom / mSheet.beatDenom) + 4 * (((float)note.nom) / note.denom));
			if (timing > ntiming)
			{
				rowPCM = (int)GetPCM(note);
				timing = ntiming;
				judgeRow.Clear();
				judgeRow.Add(note);
			}
			else if (timing == ntiming)
			{
				rowPCM = (int)GetPCM(note);
				judgeRow.Add(note);
			}
		}
		int jSample = cSample - rowPCM - (int)(mAudio.music.clip.frequency * mPsetting.JudgeOffset);
		float judgeTiming = (float)jSample / mAudio.music.clip.frequency;

		JudgeType judge = JudgeType.UNJUDGE;
		if (jSample < GREATPCM && jSample > -GREATPCM)
			judge = JudgeType.COK;
		else
			judge = JudgeType.CGOOD;

		isCharge = false;

		judgeQueue.Enqueue(new(judge, judgeTiming, judgeRow));
	}
}

