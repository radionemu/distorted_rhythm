using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputmanager : MonoBehaviour
{
    public bool [] isDown = new bool [5];
    public bool [] isStroke = {false, false};
    public bool [] prevStroke = {false, false};

    public Camera PCcam;
    public Camera MBcam;
    Camera maincam => _ = settingManager.BuildSetting == settingManager.PortMode.Desktop ? PCcam : MBcam;

    public Judge mJudge;
    // Start is called before the first frame update
    Ray ray;
    RaycastHit hit;

    public float threshold = 600f;
    public float deltaThreshold = 60f;
    private int touchindex = 0;
    private Vector2 touchBeganPos;
    private Vector2 touchEndPos;
    
    float width;
    float height;
    float prevDelta = 0.0f;
    int prevdeltasign = 0;

    float prevTime = 0.0f;

    void Start()
    {
        width = (float)Screen.width/2.0f;
        height = (float)Screen.height/2.0f;
    }

    private bool checkTag(string str){
        return hit.transform.gameObject.CompareTag("BT-A");
    }

    // Update is called once per frame
    void Update()
    {
        Array.Fill(isDown, false);
        Array.Fill(isStroke, false);
        bool newTouch = false;
        for(int i=0; i<Input.touchCount; i++){
            if(i>=5) break;
            Touch touch = Input.GetTouch(i);
            ray = maincam.ScreenPointToRay(touch.position);
            // Debug.DrawRay(ray.origin, ray.direction*100);
            // Debug.Log(ray.origin.x+" "+ray.origin.y+" "+ray.origin.z);
            // Debug.Log(Physics.Raycast(ray));
            if (Physics.Raycast(ray.origin, ray.direction*10, out hit))
            {
                if (hit.transform.gameObject.CompareTag("BT-A"))
                {
                    isDown[0]= true;
                }else if (hit.transform.gameObject.CompareTag("BT-B"))
                {
                    isDown[1]= true;
                }else if (hit.transform.gameObject.CompareTag("BT-C"))
                {
                    isDown[2]= true;
                }else if (hit.transform.gameObject.CompareTag("BT-D"))
                {
                    isDown[3]= true;
                }
                // Debug.Log(hit.transform.name);
                if(touch.phase == UnityEngine.TouchPhase.Began || touch.phase == UnityEngine.TouchPhase.Ended){
                    if(hit.transform.gameObject.CompareTag("BT-A")||hit.transform.gameObject.CompareTag("BT-B")||hit.transform.gameObject.CompareTag("BT-C")||hit.transform.gameObject.CompareTag("BT-D")){
                        mJudge.ReqCharge();
                    }
                }
            }

            if(ray.origin.x<0){
                float curtime = Time.time;
                    if(touch.deltaPosition.y < -threshold && isStroke[1]==false){
                        isStroke[0]=false;
                        isStroke[1]=true;
                        if(prevdeltasign != Math.Sign(touch.deltaPosition.y) && (curtime - prevTime >= 0.1f)){
                            mJudge.ReqJudge(isDown);
                            prevdeltasign = Math.Sign(touch.deltaPosition.y);
                            Debug.Log(curtime-prevTime);
                            prevTime = curtime;
                        }
                    }
                    else if(touch.deltaPosition.y > threshold && isStroke[0]==false){
                        isStroke[0]=true;
                        isStroke[1]=false;
                        if(prevdeltasign != Math.Sign(touch.deltaPosition.y) && (curtime - prevTime >= 0.1f)){
                            mJudge.ReqJudge(isDown);
                            prevdeltasign = Math.Sign(touch.deltaPosition.y);
                            Debug.Log(curtime-prevTime);
                            prevTime = curtime;
                        }
                    }else{
                        isStroke[0]=false;
                        isStroke[1]=false;
                        prevdeltasign = 0;
                    }
                    if(touch.phase == UnityEngine.TouchPhase.Ended){
                        prevTime = 0f;
                    }
                // if(touch.phase == UnityEngine.TouchPhase.Began){
                //     touchindex = i;
                //     touchBeganPos = touch.position;
                // }
                // if(touchindex == i){
                //     if(prevDelta > deltaThreshold && (Math.Sign(prevDelta) != Math.Sign(prevdeltasign))){
                //         prevdeltasign = Math.Sign(prevDelta);
                //         touchBeganPos = touch.position;
                //     }
                //     touchEndPos = touch.position;
                //     Vector2 diff = touchEndPos - touchBeganPos;
                //     bool check = !(prevStroke[0]==isStroke[0]&&prevStroke[1]==isStroke[1]);
                //     if(diff.y < -threshold){
                //         if(prevStroke[1] == false){
                //             isStroke[0] = false;
                //             isStroke[1] = true;
                //             if(check)
                //                 mJudge.ReqJudge(isDown);
                //         }
                //     }else if(diff.y > threshold){
                //         if(prevStroke[0] == false){
                //             isStroke[0] = true;
                //             isStroke[1] = false;
                //             if(check)
                //                 mJudge.ReqJudge(isDown);
                //         }
                //     }
                //     prevDelta = touch.deltaPosition.y;
                //     prevStroke[0] = isStroke[0]; prevStroke[1] = isStroke[1];
                //    // Debug.Log(isStroke[0]+" "+isStroke[1]);
                //};
                // Debug.Log("STROKE");
                // Debug.Log(touch.deltaPosition.y);
            }
        }

        if(Input.touchCount<=0){
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
                isDown[4] = isStroke[0] = true;
            }else{
                isDown[4] = isStroke[0] = false;
            }

            if(Input.GetKey(KeyCode.Keypad8)){
                isDown[4] = isStroke[1]=true;
            }else{
                isDown[4] = isStroke[1] = false;
            }
        
            if(Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Keypad8)){
                mJudge.ReqJudge(isDown);
            }

            if(Input.GetKeyUp(KeyCode.Alpha1)||Input.GetKeyUp(KeyCode.Alpha2)||Input.GetKeyUp(KeyCode.Alpha3)||Input.GetKeyUp(KeyCode.Alpha4)){
                mJudge.ReqCharge();
            }

            if(Input.GetKeyDown(KeyCode.Alpha1)||Input.GetKeyDown(KeyCode.Alpha2)||Input.GetKeyDown(KeyCode.Alpha3)||Input.GetKeyDown(KeyCode.Alpha4)){
                mJudge.ReqCharge();
            }
        }


    }
}
