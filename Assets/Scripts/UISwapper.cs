using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwapper : MonoBehaviour
{
    public GameObject Gameplay;
    public GameObject Result;

    public GameObject Title;

    public Play mPlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapCanvas(GameObject a, GameObject b){
        a.SetActive(false);
        b.SetActive(true);
    }

    public void OnPlayClick(){
        SwapCanvas(Title, Gameplay);
        mPlay.StartCoroutine(mPlay.InitGameplay());
    }

    public void OnReturnClick(){
        StartCoroutine(UpdateScoreDB());
        SwapCanvas(Result, Title);
    }

    IEnumerator UpdateScoreDB(){
        Debug.Log(DBManager.score);
        WWWForm form = new WWWForm();
        form.AddField("username",DBManager.username);
        form.AddField("score",DBManager.score);
        WWW www = new WWW("http://localhost/sqlconnect/score.php", form);
        yield return www;
        if(www.text[0] == '0'){
            string[] txt = www.text.Split('\t');
            Debug.Log("Req Success "+txt[1]);
        }else{
            Debug.Log("TASK FAILED SUCCESSFULLY. Error #"+www.text);
        }
    }
}
