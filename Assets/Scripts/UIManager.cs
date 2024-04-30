using System;
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
    public List<SpriteRenderer> LaneLine;
    public List<Color> oColor;
    public List<Color> tColor;

    public List<SpriteRenderer> StrokeButton;

    private Inputmanager mIManager;

    public TextMeshProUGUI Judge;
    public TextMeshProUGUI Combo;
    public TextMeshProUGUI Fastslow;

    public TextMeshProUGUI Score;

    public List<Material> JudgeMaterials;
    public Animator JudgeAnimator;
    public ParticleSystem JudgeEffect;
        public List<ParticleSystem> JudgeFXs;
        public List<GameObject> LaneFXs;
    public List<Color> JFXCol;

    public List<GameObject> ComboCounter;

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
                LaneEffect[i].color = new Color(oColor[i].r, oColor[i].g, oColor[i].b, 0.8f);
                LaneLine[i].enabled = true;
                LaneLine[i].color = oColor[i];
            }else{
                LaneButton[i].color = tColor[i];
                LaneEffect[i].enabled = false;
                LaneEffect[i].color = Color.white;
                LaneLine[i].enabled = false;
                LaneLine[i].color = Color.white;
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

    public void ReqJudge(JudgeType pJudge, int lane){
        Judge.text = pJudge switch
        {
            JudgeType.PGREAT => "Perfect",
            JudgeType.GREAT => "Great",
            JudgeType.GOOD => "Good",
            JudgeType.OK => "OK",
            JudgeType.MISS => "Miss",
            JudgeType.COK => "Full Charge",
            JudgeType.CGOOD => "Half Charge",
            JudgeType.UNJUDGE => "",
            _ => "INVALID"
        };
        Judge.fontMaterial = pJudge switch
        {
            JudgeType.PGREAT => JudgeMaterials[0],
            JudgeType.GREAT => JudgeMaterials[1],
            JudgeType.GOOD => JudgeMaterials[2],
            JudgeType.OK => JudgeMaterials[3],
            JudgeType.MISS => JudgeMaterials[4],
            JudgeType.COK => JudgeMaterials[0],
            JudgeType.CGOOD => JudgeMaterials[1],
            JudgeType.UNJUDGE => JudgeMaterials[4],
            _ => throw new System.NotImplementedException()
        };
        var c = JudgeFXs[2].main;
        var a = JudgeFXs[0].main;
        var b = JudgeFXs[1].main;
        bool laneFX = false;
        switch(pJudge){
                case JudgeType.PGREAT:
                a.startColor = new ParticleSystem.MinMaxGradient(JFXCol[0]);
                b.startColor = new ParticleSystem.MinMaxGradient(JFXCol[0]);
                c.startColor = new ParticleSystem.MinMaxGradient(JFXCol[0]);
                JudgeEffect.Play();
                laneFX = true;
            break;
            case JudgeType.GREAT:
                a.startColor = new ParticleSystem.MinMaxGradient(JFXCol[1]);
                b.startColor = new ParticleSystem.MinMaxGradient(JFXCol[1]);
                c.startColor = new ParticleSystem.MinMaxGradient(JFXCol[1]);
                JudgeEffect.Play();
                laneFX = true;
            break;
            case JudgeType.GOOD:
                a.startColor = new ParticleSystem.MinMaxGradient(JFXCol[2]);
                b.startColor = new ParticleSystem.MinMaxGradient(JFXCol[2]);
                c.startColor = new ParticleSystem.MinMaxGradient(JFXCol[2]);
                JudgeEffect.Play();
                laneFX = true;
            break;
            case JudgeType.COK:
                a.startColor = new ParticleSystem.MinMaxGradient(JFXCol[0]);
                b.startColor = new ParticleSystem.MinMaxGradient(JFXCol[0]);
                c.startColor = new ParticleSystem.MinMaxGradient(JFXCol[0]);
                JudgeEffect.Play();
            break;
            case JudgeType.CGOOD:
                a.startColor = new ParticleSystem.MinMaxGradient(JFXCol[1]);
                b.startColor = new ParticleSystem.MinMaxGradient(JFXCol[1]);
                c.startColor = new ParticleSystem.MinMaxGradient(JFXCol[1]);
                JudgeEffect.Play();
            break;
            default:break;
        }

        //lane particle
        if(lane > 0 && laneFX == true){
            ParticleSystem lfxa = LaneFXs[lane-1].transform.GetChild(0).GetComponent<ParticleSystem>();
            ParticleSystem lfxb = LaneFXs[lane-1].transform.GetChild(1).GetComponent<ParticleSystem>();
            var lfxam = lfxa.main; var lfxbm = lfxb.main;
            lfxam.startColor = oColor[lane-1];
            lfxbm.startColor = oColor[lane-1];
            lfxa.Play(); lfxb.Play();
        }

        JudgeAnimator.Play("JudgeEffect", -1, 0f);
    }

    public void ReqFastSlow(JudgeTiming pTiming, float milli){
        if(pTiming == JudgeTiming.FAST){
            Fastslow.text = "FAST +"+string.Format("{0:f3}",milli);
            Fastslow.color = Color.blue;
        }else if(pTiming == JudgeTiming.SLOW){
            Fastslow.text = "SLOW +"+string.Format("{0:f3}",milli);
            Fastslow.color = Color.red;
        }
    }

    public void ReqScore(uint score){
        Score.text = string.Format("{0:D7}", score);
    }

    public void ReqCombo(uint combo, uint prevCombo){
        int [] comb = {(int)combo/1000, (int)(combo%1000)/100, (int)(combo%100)/10, (int)combo%10};
        int [] precomb = {(int)prevCombo/1000, (int)(prevCombo%1000)/100, (int)(prevCombo%100)/10,(int) prevCombo%10};

        //compare
        for(int i=0; i<4; i++){
            if(comb[i] == 0){
                int sum = 0;
                for(int j=0; j<i;j++)
                    sum += comb[j];
                if(sum == 0){
                    ComboCounter[i].SetActive(false);
                    ComboCounter[4].SetActive(false);
                    continue;
                }else{
                    ComboCounter[i].SetActive(true);
                    ComboCounter[4].SetActive(true);
                    ComboCounter[i].GetComponent<TextMeshProUGUI>().text=comb[i].ToString();
                    Animator animator = ComboCounter[i].GetComponent<Animator>();
                    animator.Play("ComboCount", -1, 0f);
                }
            }else if(comb[i] - precomb[i] > 0){
                ComboCounter[i].SetActive(true);
                ComboCounter[4].SetActive(true);
                ComboCounter[i].GetComponent<TextMeshProUGUI>().text=comb[i].ToString();
                Animator animator = ComboCounter[i].GetComponent<Animator>();
                animator.Play("ComboCount", -1, 0f);
            }else if(precomb[i]- comb[i] == 9){
                ComboCounter[i].SetActive(true);
                ComboCounter[4].SetActive(true);
                ComboCounter[i].GetComponent<TextMeshProUGUI>().text=comb[i].ToString();
                Animator animator = ComboCounter[i].GetComponent<Animator>();
                animator.Play("ComboCount", -1, 0f);
            }
        }

        Combo.text = combo.ToString();
    }
}
