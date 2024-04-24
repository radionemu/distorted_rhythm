using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class NoteParse : MonoBehaviour
{
    enum ParseArea{
        FileInfo,
        ContentInfo,
        NoteInfo,
        EOF
    };

    Judge mJudge;
    ParseArea mAreaState;
    Sheet mSheet;
    public int spawnIndex;
    public bool spawnEnd;

    string line;
    string[] token;

    TextAsset txtFile;
    StringReader stringReader;

    // Start is called before the first frame update
    void Start()
    {
        mJudge = GameObject.Find("JudgeManager").GetComponent<Judge>();
        mSheet = GameObject.Find("Sheet").GetComponent<Sheet>();
        txtFile = Resources.Load("File 1") as TextAsset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ReadFile(){
        mSheet.Notes.Clear();
        int i = 0;
        int PCM = 0;
        stringReader = new StringReader(txtFile.text);
        while(true){
            i++;
            line = stringReader.ReadLine();
            
            //skippable
            if(line.Length == 0) continue;
            if(line[0] == '#'){
                continue; //this is Comment
            }
            
            token = line.Split(':');
            //indicate mode
            if(line == "[FileInfo]"){
                mAreaState = ParseArea.FileInfo;continue;
            }else if(line == "[ContentInfo]"){
                mAreaState = ParseArea.ContentInfo;continue;
            }else if(line == "[NoteInfo]"){
                mAreaState = ParseArea.NoteInfo;continue;
            }else if(line == "[EOF]"){
                mAreaState = ParseArea.EOF;
            }
            
            if(mAreaState == ParseArea.FileInfo){
                switch(token[0]){
                    case "Version":
                        mSheet.Version = token[1];
                        break;
                    case "FileName": 
                        mSheet.FileName = token[1];
                        break;
                    case "ImageName": 
                        mSheet.ImageName = token[1];
                        break;
                    case "BPM":
                        mSheet.BPM = float.Parse(token[1]);
                        break;
                    case "beatNom":
                        mSheet.beatNom = float.Parse(token[1]);
                        break;
                    case "beatDenom":
                        mSheet.beatDenom = float.Parse(token[1]);
                        break;
                    case "Offset":
                        mSheet.Offset = float.Parse(token[1]);
                        break;
                    default:
                        break;
                }
            }else if(mAreaState == ParseArea.ContentInfo){
                switch(token[0]){
                    case "Title":
                        mSheet.Title = token[1];
                        break;
                    case "Artist":
                        mSheet.Artist = token[1];
                        break;
                    case "Genre":
                        mSheet.Genre = token[1];
                        break;
                    case "Difficulty":
                        mSheet.Difficulty = int.Parse(token[1]);
                        break;
                    default:
                        break;
                }
            }else if(mAreaState == ParseArea.NoteInfo){
                if(line[0..4]=="Note"){
                    token = line[(line.IndexOf('(')+1)..line.IndexOf(')')].Split(',');
                    mSheet.Notes.Add(new Note(token));//Check it Later
                    //Have to Check Strictly 
                }else{
                    Debug.LogError("Note info is not Correct in Line ["+i+"]!");
                    continue;
                }


            }else if(mAreaState == ParseArea.EOF){
                break; //break the loop
            }
        }
        return true;
        //test code
        // mJudge.InitQueue();
    }
}
