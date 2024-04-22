using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Image> LaneButton;
    public List<Image> LaneEffect;
    public List<Color> oColor;
    public List<Color> tColor;

    public List<Image> StrokeButton;

    private Inputmanager mIManager;

    public TextMeshProUGUI Judge;
    public TextMeshProUGUI combo;
    public TextMeshProUGUI Fastslow;

    // Start is called before the first frame update
    void Start()
    {
        mIManager = GameObject.Find("InputManager").GetComponent<Inputmanager>();   
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<mIManager.isDown.Count(); i++){
            if(mIManager.isDown[i]){
                LaneButton[i].color = oColor[i];
                LaneEffect[i].enabled = true;
            }else{
                LaneButton[i].color = tColor[i];
                LaneEffect[i].enabled = false;
            }
        }
        for(int i=0; i<mIManager.isStroke.Count(); i++){
            if(mIManager.isStroke[i]){
                StrokeButton[i].color = Color.white;
            }else{
                StrokeButton[i].color = Color.gray;
            }
        }
    }

    public void reqJudge(JudgeType pJudge){
        if(pJudge == JudgeType.PGREAT){
            Judge.text = "PGREAT";
        }else if(pJudge == JudgeType.GREAT){
            Judge.text = "GREAT";
        }
    }

    public void reqFastSlow(JudgeTiming pTiming, float milli){
        if(pTiming == JudgeTiming.FAST){
            Fastslow.text = "FAST +"+string.Format("{0:f3}",milli);
        }else if(pTiming == JudgeTiming.SLOW){
            Fastslow.text = "SLOW +"+string.Format("{0:f3}",milli);
        }
    }
}
