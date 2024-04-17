using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class NoteParse : MonoBehaviour
{
    //UI TEST
    

    Sheet _sheet;
    int i;
    public int spawnIndex;
    public bool spawnEnd;

    string line;
    string[] token;

    TextAsset txtFile;
    StringReader stringReader;

    // Start is called before the first frame update
    void Start()
    {
        _sheet = GameObject.Find("Sheet").GetComponent<Sheet>();
        txtFile = Resources.Load("File 1") as TextAsset;
        stringReader = new StringReader(txtFile.text);
        ReadFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //파일 리딩
    //리딩 후 한 줄씩 파싱
    //인식 후 배열에 추가

    void ReadFile(){
        //변수 초기화

        //파일 읽기


        //라인 단위로 문자열로 저장
        while(true){
            line = stringReader.ReadLine();
            //Debug.Log(line);
            token = line.Split(':');
            
            if(token[0] == "Version")   _sheet.Version = token[1];
            else if(token[0] == "FileName") _sheet.FileName = token[1];
            else if(token[0] == "ImageName")_sheet.ImageName = token[1];
            else if(token[0] == "BPM")_sheet.BPM = float.Parse(token[1]);
            else if(token[0] == "BeatBar") _sheet.BeatBar = int.Parse(token[1]);
            else if(token[0] == "Offset") _sheet.Offset = float.Parse(token[1]);
            else if(token[0] == "Title") _sheet.Title = token[1];
            else if(token[0] == "Artist") _sheet.Artist = token[1];
            else if(token[0] == "Genre") _sheet.Genre = token[1];
            else if(token[0] == "Difficulty") _sheet.Difficulty = int.Parse(token[1]);
            else if(line == "[NoteInfo]"){
                //노트 해석
                
            }
            if(line == "[EOF]"){
                break;
            }
        }
    }
}
