using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField NameField;
    public TMP_InputField passwordField;

    public Button loginButton;
    public Button RegisterButton;
    public Button GamePlayButton;

    public GameObject CellPrefabs;
    public GameObject CellListPC;
    public GameObject CellListMB;
    GameObject CellListROOT => _ = settingManager.BuildSetting == settingManager.PortMode.Desktop? CellListPC : CellListMB;
    List<GameObject> Lists=new();

    private void Start() {
        // StartCoroutine(ViewRank());
    }

    public void CallRegister(){
        StartCoroutine(Register());
    }

    public void CallLogin(){
        StartCoroutine(Login());
    }

    IEnumerator Register(){
        WWWForm form = new WWWForm();
        form.AddField("username", NameField.text);
        form.AddField("password", passwordField.text);
        WWW www = new WWW("http://106.246.242.58:11345/demo/register", form);
        yield return www;
        if(www.text[0] == '0'){
            //work!
            Debug.Log("User created successfully");
        }else{
            Debug.Log("TASK FAILED SUCCESSFULLY. Error #"+www.text);
        }
        yield return StartCoroutine(Login());
    }

    IEnumerator Login(){
        WWWForm form = new WWWForm();
        form.AddField("username",NameField.text);
        form.AddField("password",passwordField.text);
        WWW www = new WWW("http://106.246.242.58:11345/demo/login", form);
        yield return www;
        if(www.text[0] == '0'){
            Debug.Log("Login Successfully");
            DBManager.username = NameField.text;
            DBManager.score = int.Parse(www.text.Split('\t')[1]);
            Debug.Log(DBManager.score);
            GamePlayButton.interactable = true;
        }else{
            Debug.Log("TASK FAILED SUCCESSFULLY. Error #"+www.text);
        }
        yield return StartCoroutine(ViewRank());
    }

    IEnumerator ViewRank(){
        GameObject CellList = CellListROOT.transform.GetChild(2).GetChild(9).GetChild(2).gameObject;
        foreach(GameObject obj in Lists){
            Destroy(obj);
        }
        Lists.Clear();
        WWW www = new WWW("http://106.246.242.58:11345/demo/rank");
        yield return www;
        if(www.text[0]=='0'){
            // Debug.Log(www.text);
            string [] str = www.text.Split('\t')[1..];
            foreach (string rankrow in str){
                GameObject tmp = Instantiate(CellPrefabs,CellList.transform);
                TextMeshProUGUI rnknum = tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI rnkname = tmp.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI rnkscore = tmp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                string[] rankcells = rankrow.Split(',');
                rnknum.text = string.Format("{0:D3}",int.Parse(rankcells[0]));
                rnkname.text = rankcells[1];
                rnkscore.text = string.Format("{0:D7}",int.Parse(rankcells[2]));
                Lists.Add(tmp);
            }
        }
    }


    public void VerifyInputs(){
        RegisterButton.interactable = (NameField.text.Length >= 8 && passwordField.text.Length >=8);
    }
}
