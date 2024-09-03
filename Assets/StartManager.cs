using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    private static StartManager instance;
    public static StartManager GetInstance() => instance;

    public GameObject StartCanvas;
    public GameObject SelectCanvas;
    public Play PlayManager;
    public GameObject GameplayCanvas;
    public GameObject Background;
    public Light2D Light;

    public Animator animator;

    public Image LV;
    public Sprite lv3;
    public Sprite lv5;
    public Sprite lv8;

    Color oricol;
    Vector3 backgrondpos;

    public Dictionary<int, string> Charts = new();
    public string targetChart = "";

    void Awake()
    {
        instance = this;
        oricol = Light.color;
        backgrondpos = Background.transform.localPosition;
        Charts = new();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(){
        Light.color = oricol;
        Background.transform.localPosition = backgrondpos;
        targetChart = "";
    }

    public IEnumerator Load() {
        StartCanvas.SetActive(true);
        if (SelectManager.GetInstance().SelectCursor == 0)
            LV.sprite = lv3;
        if (SelectManager.GetInstance().SelectCursor == 1)
            LV.sprite = lv5;
        if (SelectManager.GetInstance().SelectCursor == 2)
            LV.sprite = lv8;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("FDIN"));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        string text = "";
        if (!Charts.ContainsKey(SelectManager.GetInstance().SelectCursor))
        {
            WWWForm form = new WWWForm();
            form.AddField("lv", SelectManager.GetInstance().SelectCursor);
            UnityWebRequest www = UnityWebRequest.Post("http://"+ServerManager.GetServer()+"/demo/getlevel", form);
            yield return www.SendWebRequest();
            if (www.downloadHandler.text.Length <= 0)
            {
                Debug.LogError("Network Disconnected");
                yield return null;
            }
            else
            {
                text = www.downloadHandler.text;
                byte[] bytes = Convert.FromBase64String(text);
                text = Encoding.UTF8.GetString(bytes);
            }

            Charts.Add(SelectManager.GetInstance().SelectCursor, text);
        }
        else {
            text = Charts[SelectManager.GetInstance().SelectCursor];
        }
        targetChart = text;

        //Load
        SelectCanvas.SetActive(false);
        GameplayCanvas.SetActive(true);
        Background.transform.localPosition = new Vector3(Background.transform.localPosition.x, Background.transform.localPosition.y, 5);
        Light.color = new Color(1, 1, 1, 1);
        animator.SetTrigger("Load");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("FDOUT"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
        StartCanvas.SetActive(false);
        yield return new WaitForSeconds(1f);

        PlayManager.StartGame();
    }
}
