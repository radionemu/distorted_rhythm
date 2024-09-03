using System.Collections;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public GameObject TitleCanvas;
    public GameObject EntryCanvas;
    private static TitleManager instance;
    public static TitleManager GetInstance() => instance;

    public GameObject Black;
    bool isInteractive = true;
    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void init() {
        isInteractive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && isInteractive) {
            isInteractive = false;
            // StopAllCoroutines();
            StartCoroutine(LoadManager.GetInstance().Load(setLoad));
        }   
    }

    public void setLoad() {
        EntryManager.GetInstance().init();
        SelectManager.GetInstance().init();
        // HowToPlayManager.GetInstance().init();
        StartManager.GetInstance().init();

        TitleCanvas.SetActive(false);
        Black.SetActive(false);
        EntryCanvas.SetActive(true);
        EntryManager.GetInstance().CallEntry();
    }
}
