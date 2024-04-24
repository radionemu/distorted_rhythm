using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<SpriteRenderer> LaneButton;
    public List<SpriteRenderer> LaneEffect;
    public List<Color> oColor;
    public List<Color> tColor;

    public List<SpriteRenderer> StrokeButton;

    private Inputmanager mIManager;

    public TextMeshProUGUI Judge;
    public TextMeshProUGUI Combo;
    public TextMeshProUGUI Fastslow;

    public TextMeshProUGUI Score;

    // Start is called before the first frame update
    void Start()
    {
        mIManager = GameObject.Find("InputManager").GetComponent<Inputmanager>();   
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<mIManager.isDown.Count()-1; i++){
            if(mIManager.isDown[i]){
                LaneButton[i].color = oColor[i];
                LaneEffect[i].enabled = true;
                LaneEffect[i].color = oColor[i];
            }else{
                LaneButton[i].color = tColor[i];
                LaneEffect[i].enabled = false;
                LaneEffect[i].color = Color.white;
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

    public bool Init(){
        Judge.text = "";
        Combo.text = "";
        Fastslow.text ="";
        return true;
    }

    public void ReqJudge(JudgeType pJudge){
        Judge.text = pJudge switch
        {
            JudgeType.PGREAT => "PGREAT",
            JudgeType.GREAT => "GREAT",
            JudgeType.GOOD => "GOOD",
            JudgeType.OK => "OK",
            JudgeType.MISS => "MISS",
            JudgeType.COK => "FULL CHARGE",
            JudgeType.CGOOD => "HALF CHARGE",
            JudgeType.UNJUDGE => "",
            _ => "INVALID"
        };
    }

    public void ReqFastSlow(JudgeTiming pTiming, float milli){
        if(pTiming == JudgeTiming.FAST){
            Fastslow.text = "FAST +"+string.Format("{0:f3}",milli);
        }else if(pTiming == JudgeTiming.SLOW){
            Fastslow.text = "SLOW +"+string.Format("{0:f3}",milli);
        }
    }

    public void ReqScore(uint score){
        Score.text = string.Format("{0:D7}", score);
    }

    public void ReqCombo(uint combo){
        Combo.text = combo.ToString();
    }
}
