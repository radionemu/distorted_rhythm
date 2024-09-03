using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    private static ResultManager instance;
    public static ResultManager GetInstance() => instance;

    public GameObject ResultCanvas;
    public GameObject F2PCanvas;
    public Animator animator;
    public ScoreManager mScoreMgr;
    public TextMeshProUGUI rank;

    public GameObject rankCellPrefab;
    public GameObject VertRank;

    public List<GameObject> pool = null;

    public bool isInteractive = false;

    public bool isInteractivetwo = false;

    public Image LV;

    void Awake()
    {
        instance = this;
        isInteractive = false;
        isInteractivetwo = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void init() {
        isInteractive = false;
        isInteractivetwo = false;
        ti = 0f;
        tti = 0f;
        pool = null;
    }

    public float ti = 0f;
    public float tti = 0f;
    // Update is called once per frame
    void Update()
    {
        if (isInteractive) { 
            ti+= Time.deltaTime;
        }
        if (isInteractive && (ti >= 10f || Input.GetKeyDown(KeyCode.Keypad7))) {
            StartCoroutine(ViewRank());
        }

        if (isInteractivetwo) {
            tti += Time.deltaTime;
        }
        if (isInteractivetwo && (tti >= 10f || Input.GetKeyDown(KeyCode.Keypad7))) {
            isInteractivetwo = false;
            StartCoroutine(LoadManager.GetInstance().Load(insert));
        }
    }

    public void insert() {
        StartCoroutine(InsertRank());
        ResultCanvas.SetActive(false);
        F2PCanvas.SetActive(true);
        StartCoroutine(EndManager.GetInstance().Load());
    }

    public IEnumerator InsertRank() { 
        WWWForm form = new WWWForm();
        string text = "";
        form.AddField("lv", SelectManager.GetInstance().SelectCursor);
        form.AddField("score", mScoreMgr.TotalScore.ToString());
        form.AddField("playerhash", DBManager.userhash);
        UnityWebRequest www = UnityWebRequest.Post("http://106.246.242.58:11345/demo/insertrank", form);
        yield return www.SendWebRequest();
        if(www.downloadHandler.text.Length<=0){
            Debug.LogError("Network Disconnected");
            yield return null;
        }else{

        }
        isInteractivetwo = false;

    }

    public IEnumerator ViewRank() {
        animator.SetTrigger("Inter");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("RANK"));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        yield return new WaitForSeconds(1f);
        isInteractivetwo = true;
        isInteractive = false;
    }

    public IEnumerator Load() {
        ti = 0f;
        tti = 0f;
        isInteractive = false;
        isInteractivetwo = false;
        foreach (GameObject obj in pool){
            Destroy(obj);
        }
        pool = new();
        WWWForm form = new WWWForm();
        string text = "";
        form.AddField("lv", SelectManager.GetInstance().SelectCursor);
        UnityWebRequest www = UnityWebRequest.Post("http://106.246.242.58:11345/demo/viewrank", form);
        yield return www.SendWebRequest();
        if(www.downloadHandler.text.Length<=0){
            Debug.LogError("Network Disconnected");
            yield return null;
        }else{
            text = www.downloadHandler.text.Split("@")[1];
            byte[] bytes = Convert.FromBase64String(text);
            text = Encoding.UTF8.GetString(bytes);
        }

        int rrank = 0;
        if (text.Length <= 0) {
            GameObject go = Instantiate(rankCellPrefab, VertRank.transform);
            go.GetComponent<RankCell>().init(1.ToString(), DBManager.username, mScoreMgr.TotalScore.ToString(), true);
            pool.Add(go);
        } else { 
            List<string> scorelist = text.Split(',').ToList();
            int adder = 0;
            int prevscore = -1;
            for (int i = scorelist.Count - 1; i>=0; i--) {
                if (string.IsNullOrWhiteSpace(scorelist[i])) scorelist.RemoveAt(i);
            }
            int s = 1;
            for (int i = 0; i < scorelist.Count(); i++) {
                string[] rns = scorelist[i].Split(' ');
                GameObject go; 
                if (prevscore >= mScoreMgr.TotalScore && mScoreMgr.TotalScore > int.Parse(rns[2]))
                {
                    go = Instantiate(rankCellPrefab, VertRank.transform);
                    go.GetComponent<RankCell>().init((s).ToString(), DBManager.username, mScoreMgr.TotalScore.ToString(), true);
                    pool.Add(go);
                    rrank = s + adder;
                    adder++;
                }
                go = Instantiate(rankCellPrefab, VertRank.transform);
                go.GetComponent<RankCell>().init((s + adder).ToString(), rns[1], rns[2]);
                pool.Add(go);
                prevscore = int.Parse(rns[2]);
                s++;
            }
            if (adder == 0) { 
                GameObject go = Instantiate(rankCellPrefab, VertRank.transform);
                go.GetComponent<RankCell>().init((scorelist.Count() + adder + 1).ToString(), DBManager.username, mScoreMgr.TotalScore.ToString(), true);
                pool.Add(go);
                rrank = scorelist.Count + adder;
            }
        }
        if (pool.Count > 10) { 
            if (rrank <= 4)
            {
                for (int i = 10; i < pool.Count(); i++)
                {
                    pool[i].SetActive(false);
                }
            }
            else if (4 < rrank && rrank <= pool.Count - 6)
            {
                for (int i = 0; i < rrank - 4; i++)
                {
                    pool[i].SetActive(false);
                }
                for (int i = rrank + 6; i < pool.Count - 1; i++)
                {
                    pool[i].SetActive(false);
                }
            }
            else {
                for (int i = 0; i < pool.Count - 10; i++) {
                    pool[i].SetActive(false);
                }
            }
        }


        LV.sprite = StartManager.GetInstance().LV.sprite;

        yield return new WaitForSeconds(2f);
        ResultCanvas.SetActive(true);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("BFD"));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        yield return new WaitForSeconds(1.5f);
        rank.text = GetRank();
        StartManager.GetInstance().GameplayCanvas.SetActive(false);
        animator.SetTrigger("Load");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Load"));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        yield return new WaitForSeconds(1f);
        isInteractive = true;
    }

    public string GetRank() {
        if (mScoreMgr.TotalScore <= 500000) return "D";
        if (mScoreMgr.TotalScore <= 600000) return "C";
        if (mScoreMgr.TotalScore <= 700000) return "B";
        if (mScoreMgr.TotalScore <= 800000) return "A";
        if (mScoreMgr.TotalScore <= 900000) return "S";
        if (mScoreMgr.TotalScore <= 950000) return "SS";
        return "M";
    }
}
