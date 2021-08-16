using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BPM : MonoBehaviour
{
    private static BPM _BPMInstance;
    public double _bpm;
    private double _beatInterval, _beatTimer, _beatIntervalD, _beatTimerD, _timer;
    public static bool _beatFull, _beatD;
    public static int _beatCountFull, _beatCountD, _beatCountBar;

    public int divide = 8;
    public static int _beatBar = 4;
    
    public static float _bpmforcalc = 144;

    //UI표시
    public Text _beatCountText, _beatTimerText, _beatCountBarText;

    //다른 인스턴스가 있을 경우 최 하단의 오브젝트만을 남기고 모두 삭제
    private void Awake() {
        if (_BPMInstance != null && _BPMInstance != this){
            Destroy(this.gameObject);
        }else{
            _BPMInstance = this;
            DontDestroyOnLoad(this.gameObject);    
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= 1.73)
           StartCoroutine("BeatDetection");
    }

    IEnumerator BeatDetection(){
        //


        //full beat count
        _beatFull = false;
        _beatInterval = 60 / _bpm;
        _beatTimer += Time.deltaTime;
        if(_beatTimer >= _beatInterval){
            _beatTimer -= _beatInterval;
            _beatFull = true;
            _beatCountFull++;
            Debug.Log("Full");
        }

        //비트카운트 나누기
        _beatD = false;
        _beatIntervalD = _beatInterval / divide;
        _beatTimerD += Time.deltaTime;
        if(_beatTimerD >= _beatIntervalD){
            _beatTimerD -= _beatIntervalD;
            _beatD = true;
            _beatCountD++;
            Debug.Log("D"+divide);
        }
        

        //박자 var / 4 기준으로
        if(_beatCountFull >= _beatBar){
            _beatCountFull -= _beatBar;
            _beatCountBar++;
            Debug.Log("Bar");
        }

        _beatCountText.text = _beatCountFull.ToString();
        _beatCountBarText.text = _beatCountBar.ToString();
        _beatTimerText.text = _beatTimer.ToString();

        yield return null;
    }
}
