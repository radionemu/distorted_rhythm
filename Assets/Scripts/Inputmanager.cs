using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputmanager : MonoBehaviour
{
    public bool [] isDown = new bool [4];
    public bool [] isStroke = new bool[2];

    public Judge mJudge;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1)){
            isDown[0] = true;
        }else{
            isDown[0] = false;
        }
        
        if(Input.GetKey(KeyCode.Alpha2)){
            isDown[1] = true;
        }else{
            isDown[1] = false;
        }
        
        if(Input.GetKey(KeyCode.Alpha3)){
            isDown[2] = true;
        }else{
            isDown[2] = false;
        }

        if(Input.GetKey(KeyCode.Alpha4)){
            isDown[3] = true;
        }else{
            isDown[3] = false; 
        }

        if(Input.GetKey(KeyCode.Keypad7)){
            isStroke[0]=true; 
        }else{
            isStroke[0]=false;
        }

        if(Input.GetKey(KeyCode.Keypad8)){
            isStroke[1]=true;
        }else{
            isStroke[1] = false;
        }
    
        if(Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Keypad8)){
            mJudge.reqJudge(isDown);
        }

        if(Input.GetKeyUp(KeyCode.Alpha1)||Input.GetKeyUp(KeyCode.Alpha2)||Input.GetKeyUp(KeyCode.Alpha3)||Input.GetKeyUp(KeyCode.Alpha4)){
            mJudge.ReqCharge();
        }
    }
}
