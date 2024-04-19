using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    //Judge Timing

    //ref Class
    public Sync mAudio;
    public Sheet mSheet;

    List<Queue<Note>> Lanes = new List<Queue<Note>>();
    Queue<Note> laneA = new();
    Queue<Note> laneB = new();
    Queue<Note> laneC = new();
    Queue<Note> laneD = new();

    // Start is called before the first frame update
    void Start()
    {
        mAudio = GetComponent<Sync>();

        //Enqueue Notes each lane
        foreach(Note note in mSheet.Notes){
            switch (note.lane){
                case 1:laneA.Enqueue(note);break;
                case 2:laneA.Enqueue(note);break;
                case 3:laneA.Enqueue(note);break;
                case 4:laneA.Enqueue(note);break;
                default:break;
            }
        }
        Lanes.Add(laneA);
        Lanes.Add(laneB);
        Lanes.Add(laneC);
        Lanes.Add(laneD);

        StartCoroutine(nameof(JudgePoor));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator JudgePoor(){
        int sample = mAudio.music.timeSamples;
        foreach(Queue<Note> lane in Lanes){
            foreach(Note note in lane){
                if(note.beat*mAudio.beatPerSample + 22050 < sample){
                    Debug.Log("MISS");
                    lane.Dequeue();
                }
            }
        }

        yield return null;
    }
}
