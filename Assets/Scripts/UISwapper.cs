using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwapper : MonoBehaviour
{
    public GameObject PCUI;
    public GameObject MBUI;

    public GameObject Gameplay => _ = settingManager.BuildSetting == settingManager.PortMode.Desktop ? PCUI.transform.GetChild(0).gameObject : MBUI.transform.GetChild(0).gameObject;
    public GameObject Result => _ = settingManager.BuildSetting == settingManager.PortMode.Desktop ? PCUI.transform.GetChild(1).gameObject : MBUI.transform.GetChild(1).gameObject;
    public GameObject Title => _ = settingManager.BuildSetting == settingManager.PortMode.Desktop ? PCUI.transform.GetChild(2).gameObject : MBUI.transform.GetChild(2).gameObject;

    public GameObject PCcam;
    public GameObject MBcam;

    public Play mPlay;
    public RankManager mRankMan;

    // Start is called before the first frame update
    void Start()
    {
        if(settingManager.BuildSetting == settingManager.PortMode.Desktop){
            PCUI.SetActive(true);
            MBUI.SetActive(false);
            PCcam.SetActive(true);
            MBcam.SetActive(false);
        }else{
            PCUI.SetActive(false);
            MBUI.SetActive(true);
            PCcam.SetActive(false);
            MBcam.SetActive(true);
        }
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
        StartCoroutine(ReturnTitle());
    }

    IEnumerator ReturnTitle(){
        if(DBManager.username != null){
            yield return StartCoroutine(UpdateScoreDB());
        }
        SwapCanvas(Result, Title);
        yield return mRankMan.CallViewRank();
    }

    IEnumerator UpdateScoreDB(){
        Debug.Log(DBManager.score);
        WWWForm form = new WWWForm();
        form.AddField("username",DBManager.username);
        form.AddField("score",DBManager.score);
        WWW www = new WWW("http://106.246.242.58:11345/demo/score", form);
        yield return www;
        Debug.Log(www.text);
        if(www.text[0] == '0'){
            string[] txt = www.text.Split('\t');
            Debug.Log("Req Success "+txt[1]);
        }else{
            Debug.Log("TASK FAILED SUCCESSFULLY. Error #"+www.text);
        }
    }
}
