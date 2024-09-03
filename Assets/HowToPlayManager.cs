using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayManager : MonoBehaviour
{
    [Header("Entire Section")]
    public GameObject HowToPlayCanvas;
    public Animator H2PController;

    public GameObject Triangle;

    private static HowToPlayManager instance;
    public static HowToPlayManager GetInstance() => instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HowToPlayCanvas.activeSelf && H2PController.GetCurrentAnimatorStateInfo(0).IsName("SKIP") &&  H2PController.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f && Input.GetKeyDown(KeyCode.Keypad7)) {
            H2PController.SetTrigger("Enter");
            StartCoroutine(uninit());
        }
    }

    public void init(){
        HowToPlayCanvas.SetActive(true);
    }

    IEnumerator uninit() {
        yield return new WaitUntil(() => H2PController.GetCurrentAnimatorStateInfo(0).IsName("End"));
        yield return new WaitWhile(() => H2PController.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        HowToPlayCanvas.SetActive(false);
        StartCoroutine(LoadManager.GetInstance().Load(Load));
    }

    public void Load() {
        Triangle.SetActive(false);
        SelectManager.GetInstance()?.Load();
    }
}

