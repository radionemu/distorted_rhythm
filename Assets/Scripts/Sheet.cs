using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheet : MonoBehaviour
{
    // [FileInfo]
    public string Version {set; get;}
    public string FileName  {set; get;}
    public string ImageName {set; get;}
    public float BPM    {set; get;}
    public int BeatBar  {set; get;}
    public float Offset {set; get;}

    // [Content Info]
    public string Title {set; get;}
    public string Artist    {set; get;}
    public string Genre {set; get;}
    public int Difficulty   {set; get;}

    // [NoteInfo]
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isStringEpsilon(string str){
        return str.Length == 0;
    }
}
