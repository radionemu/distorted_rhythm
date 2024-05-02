using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servletTEst : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void callReqServlet(){
        StartCoroutine(reqServlet());
    }

    IEnumerator reqServlet(){
        WWW www = new WWW("http://106.246.242.58:11345/demo/svl");
        yield return www;
        if(www.text[0] == '0'){
            Debug.Log(www.text);
        }
    }
}
