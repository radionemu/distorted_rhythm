using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum NoteType{
    INVALID,
    NM, 
    CS,
    CE,
    MT
}

public class Note : MonoBehaviour {

    public int lane = 0;
    // 1~4 normal lane, 5 Mute Lane
    public NoteType nType;
    //1 Short Note, 2 Long Note start 3 Long Note End
    public int section;
    public int nom;
    public int denom;

    public float beat; //previous section PCM
    //frequency is too hard

    public Note(string[] tokens){
        try{
            switch (tokens[0]){
                case "NM":
                    nType = NoteType.NM;
                    break;
                case "CS":
                    nType = NoteType.CS;
                    break;
                case "CE":
                    nType = NoteType.CE;
                    break;
                case "MT":
                    nType = NoteType.MT;
                    break;
                default:
                    Debug.LogError("Type ERROR! check the type ["+tokens[0]+"]");
                    break;
            }
            lane = int.Parse(tokens[1]);
            section = int.Parse(tokens[2]);
            nom = int.Parse(tokens[3]);
            denom = int.Parse(tokens[4]); 
        }catch(IndexOutOfRangeException){
            Debug.LogError("INDEX OUT!!!");
        }
    }
    
    public void setBeat(float a){
        beat = a;
    }

}