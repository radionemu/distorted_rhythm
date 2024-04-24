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
        WWW www = new WWW("http://localhost/sqlconnect/register.php", form);
        yield return www;
        if(www.text == "0"){
            //work!
            Debug.Log("User created successfully");
        }else{
            Debug.Log("TASK FAILED SUCCESSFULLY. Error #"+www.text);
        }
    }

    IEnumerator Login(){
        WWWForm form = new WWWForm();
        form.AddField("username",NameField.text);
        form.AddField("password",passwordField.text);
        WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
        yield return www;
        if(www.text[0] == '0'){
            Debug.Log("Login Successfully");

            DBManager.username = NameField.text;
            DBManager.score = int.Parse(www.text.Split('\t')[1]);
            GamePlayButton.interactable = true;
        }else{
            Debug.Log("TASK FAILED SUCCESSFULLY. Error #"+www.text);
        }

    }




    public void VerifyInputs(){
        RegisterButton.interactable = (NameField.text.Length >= 8 && passwordField.text.Length >=8);
    }
}
