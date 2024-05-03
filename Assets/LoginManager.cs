using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField NameField;
    public TMP_InputField passwordField;

    public Button loginButton;
    public Button RegisterButton;

    public GameObject CellPrefabs;
    public GameObject CellListPC;
    public GameObject CellListMB;
    GameObject CellListROOT => _ = settingManager.BuildSetting == settingManager.PortMode.Desktop? CellListPC : CellListMB;
    List<GameObject> Lists=new();

    private void Start() {
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
        }else{
            Debug.Log("TASK FAILED SUCCESSFULLY. Error #"+www.text);
        }
    }

}
