using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class EntryManager : MonoBehaviour
{
    //enums
    public enum NETStatus{
        BeforeConnect,
        TryToConnect,
        Connected,
        TryToLogReg,
        Login,
        Register,
        TimeOut,
        ConExcpt
    };

    public enum ConExcpt{
        NoConExcpt = 0,
        ConnectionFailed = 1,
        NameAlreadyExist = 3,
        RegisterInsertFail = 4,
        UserNotExist = 5,
        PWNotCorrect = 6,
        ScoreUpdateFail = 7
    };
    public NETStatus mStatus = NETStatus.BeforeConnect;
    public ConExcpt mConExpt = ConExcpt.NoConExcpt;

    [Header("Entire Section")]
    public GameObject NetTestObj;
    public GameObject LoginObj;
    public GameObject EntryCanvas;

    [Header("Network Test Section")]
    public GameObject NetAnima;
    public TextMeshProUGUI NetText;
    
    [Header("Login Section")]
    public GameObject Fields;
    public TextMeshProUGUI LoginText;
    public TextMeshProUGUI ExcptText;

    public TMP_InputField NameField;
    public TMP_InputField passwordField;

    public Button loginButton;
    public Button RegisterButton;

    private static EntryManager instance;
    public static EntryManager GetInstance() => instance;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallRegister(){
        StartCoroutine(Register());
    }

    public void CallLogin(){
        StartCoroutine(Login());
    }

    public void CallEntry() {
        StartCoroutine(Entry());
    }

    public IEnumerator Entry(){
        yield return StartCoroutine(NetTestProcess());
        yield return StartCoroutine(LoginProcess());
        HowToPlayManager.GetInstance()?.init();
    }

    public void init() {
        DBManager.score = 0;
        DBManager.userhash = "";
        DBManager.username = "";
        mStatus = NETStatus.BeforeConnect;
        mConExpt = ConExcpt.NoConExcpt;
        NameField.text = "";
        LoginText.text = "라이브하우스에 오신 걸 환영합니다!\n당신의 닉네임을 알려주세요";
    }

    IEnumerator FadeInOutNetTest(bool inout){
        int netCC = NetAnima.transform.childCount;
        float dtime = 0.0f; float duration = 2.0f;
        while(dtime <= 1f){
            for(int i=0; i<netCC; i++){
                Image img = NetAnima.transform.GetChild(i).gameObject.GetComponent<Image>();
                img.color = Color.Lerp(new Color(img.color.r, img.color.g, img.color.b, inout?0f:1f),new Color(img.color.r, img.color.g, img.color.b, inout?1f:0f), dtime);
            }
            NetText.color = Color.Lerp(new Color(NetText.color.r, NetText.color.g, NetText.color.b, inout?0f:1f),new Color(NetText.color.r, NetText.color.g, NetText.color.b, inout?1f:0f), dtime);
            dtime += duration * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeInOutLogin(bool inout){
        Image[] imgs = Fields.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] tmps = Fields.GetComponentsInChildren<TextMeshProUGUI>();
        float dtime = 0.0f; float duration = 2.0f;
        while(dtime <= 1f){
            for(int i=0; i< imgs.Length; i++){
                imgs[i].color = Color.Lerp(new Color(imgs[i].color.r, imgs[i].color.g, imgs[i].color.b, inout?0f:1f),new Color(imgs[i].color.r, imgs[i].color.g, imgs[i].color.b, inout?1f:0f), dtime);
            }
            for(int i=0; i<tmps.Length; i++){
                tmps[i].color = Color.Lerp(new Color(tmps[i].color.r, tmps[i].color.g, tmps[i].color.b, inout?0f:1f),new Color(tmps[i].color.r, tmps[i].color.g, tmps[i].color.b, inout?1f:0f), dtime);
            }
            LoginText.color = Color.Lerp(new Color(LoginText.color.r, LoginText.color.g, LoginText.color.b, inout?0f:1f),new Color(LoginText.color.r, LoginText.color.g, LoginText.color.b, inout?1f:0f), dtime);
            ExcptText.color = Color.Lerp(new Color(ExcptText.color.r, ExcptText.color.g, ExcptText.color.b, inout?0f:1f),new Color(ExcptText.color.r, ExcptText.color.g, ExcptText.color.b, inout?1f:0f), dtime);
            dtime += duration * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator NetTestProcess(){
        //Initial Fade in
        NetTestObj.SetActive(true);
        Animator NetTest = NetAnima.GetComponent<Animator>();
        yield return StartCoroutine(FadeInOutNetTest(true));
        //loop three times
        for(int loop = 0; loop < 3; loop++){
            NetText.text = "네트워크에 연결중입니다...";
            //NetWork Test
            yield return StartCoroutine(CheckNETStatus());
                //check 
            Debug.Log(mStatus);
            //Change NetAnimation
            if(mStatus == NETStatus.Connected){
                NetTest.SetTrigger("Test Success");
                NetText.text = "네트워크에 연결되었습니다!";
                yield return new WaitForSeconds(3f);
                break;
            }else{
                NetTest.SetTrigger("Test Failed");
                NetText.text = "네트워크 연결에 실패하였습니다.";
                yield return new WaitForSeconds(3f);
            }
        }
        if(mStatus != NETStatus.Connected){
            NetText.text = "타이틀 화면으로 돌아갑니다.";
            yield return new WaitForSeconds(3f);
        }
        yield return StartCoroutine(FadeInOutNetTest(false));
        NetTestObj.SetActive(false);
    }

    IEnumerator LoginProcess(){
        LoginObj.SetActive(true);
        yield return StartCoroutine(FadeInOutLogin(true));
        mStatus = NETStatus.TryToLogReg;
        ExcptText.text = "";
        while(true){
            yield return new WaitUntil(()=>mStatus!=NETStatus.TryToLogReg);
            if(mStatus == NETStatus.Login){
                //this includes
                LoginText.text = "반갑습니다. " + DBManager.username+"님!";
                ExcptText.text = "";
                mConExpt = ConExcpt.NoConExcpt;//reset exception
                yield return new WaitForSeconds(1.5f);
                break;
            }else if(mStatus == NETStatus.ConExcpt){
                if(mConExpt == ConExcpt.NameAlreadyExist){
                    ExcptText.text = "동일한 아이디의 유저가 이미 존재합니다.";
                }else if(mConExpt == ConExcpt.UserNotExist){
                    ExcptText.text = "가입되어있지 않은 아이디입니다.";
                }else if(mConExpt == ConExcpt.PWNotCorrect){
                    ExcptText.text = "패스워드가 일치하지 않습니다.";
                }
                mStatus = NETStatus.TryToLogReg;
            }
        }
        yield return StartCoroutine(FadeInOutLogin(false));
        LoginObj.SetActive(false);
        EntryCanvas.SetActive(false);
    }


    IEnumerator CheckNETStatus(){
        WWWForm form = new WWWForm();
        WWW www = new WWW("http://"+ServerManager.GetServer()+"/demo/networkConnection", form);
        yield return www;
        if(www.text.Length <= 0){
            Debug.LogError("Connection Time out");
            mStatus = NETStatus.TimeOut;
        }else if(www.text[0] == '0'){
            Debug.Log("Network Test Successful");
            mStatus = NETStatus.Connected;
        }else{
            Debug.LogError("Exception #"+www.text);
            mStatus = NETStatus.ConExcpt;
            mConExpt = (ConExcpt)int.Parse(www.text[0]+"");
        }
        yield return null;
    }

    IEnumerator Register(){
        WWWForm form = new WWWForm();
        form.AddField("player_name", NameField.text);
        WWW www = new WWW("http://"+ServerManager.GetServer()+"/demo/register", form);
        yield return www;
        if(www.text.Length<=0){
            Debug.LogError("Network Disconnected on Register phase");
            mStatus = NETStatus.TimeOut;
        }else if(www.text[0] == '0'){
            Debug.Log("User created successfully");
            yield return StartCoroutine(Login());
        }else{
            Debug.LogError("TASK FAILED SUCCESSFULLY. Error #"+www.text);
            mStatus = NETStatus.ConExcpt;
            mConExpt = (ConExcpt)int.Parse(www.text[0]+"");
        }
    }

    IEnumerator Login(){
        WWWForm form = new WWWForm();
        form.AddField("player_name",NameField.text);
        WWW www = new WWW("http://"+ServerManager.GetServer()+"/demo/register", form);
        yield return www;
        if(www.text.Length <= 0){
            Debug.LogError("Network Disconnected on Login Phase.");
            mStatus = NETStatus.TimeOut;
        }
        else if(www.text[0] == '0'){
            Debug.Log("Login Successfully");
            DBManager.userhash = www.text.Split(" ")[1];
            DBManager.username = NameField.text;
            DBManager.score = 0;
            mStatus = NETStatus.Login;
        }else{
            Debug.LogError("TASK FAILED SUCCESSFULLY. Error #"+www.text);
            mStatus = NETStatus.ConExcpt;
            mConExpt = (ConExcpt)int.Parse(www.text[0]+"");
        }
    }

    public void VerifyInputs(){
        loginButton.interactable = NameField.text.Length >= 2 && NameField.text.Length <= 8;
    }

}
