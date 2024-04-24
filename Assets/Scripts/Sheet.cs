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
    public float beatNom  {set; get;}
    public float beatDenom  {set; get;}
    public float Offset {set; get;}

    // [Content Info]
    public string Title {set; get;}
    public string Artist    {set; get;}
    public string Genre {set; get;}
    public int Difficulty   {set; get;}

    // [NoteInfo]

    public List<Note> Notes = new();//init

    // [EOF]
    bool isStringEpsilon(string str){
        return str.Length == 0;
    }

    public bool init(){
        Notes = new();
        return true;
    }
}
