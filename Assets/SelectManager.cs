using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SelectManager : MonoBehaviour
{
    [Header("Entire Section")]
    public GameObject SelectCanvas;

    private static SelectManager instance;
    public static SelectManager GetInstance() => instance;

    public bool isInteractive = false;

    public int SelectCursor = 0;
    public int SelectStack = 0;
    public GameObject LVRoot;
    [SerializeField] private GameObject[] SongList;
    [SerializeField] private GameObject[] LevelList;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Outline[] outlines;

    void Awake()
    {
        instance = this;
        isInteractive = false;
        SelectCursor = 0;
    }

    public void init() {
        isInteractive = false;
        SelectCursor = 0;
        LVRoot.transform.localEulerAngles = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugManager.GetInstance().isDebugOn) return;
        if (!isInteractive) return;
        for (int i = 0; i < 3; i++) {
            if (SelectCursor == i) {
                var scale = SongList[i].transform.localScale;
                SongList[i].transform.localScale = Vector3.Lerp(scale, new(1.1f, 1.1f,1.1f), Time.deltaTime*10);
                // outlines[i].effectDistance = Vector2.Lerp(outlines[i].effectDistance, new(5, -5), Time.deltaTime * 10);
            }else{
                var scale = SongList[i].transform.localScale;
                SongList[i].transform.localScale = Vector3.Lerp(scale, new(1.0f, 1.0f, 1.0f), Time.deltaTime*10);
                // outlines[i].effectDistance = Vector2.Lerp(outlines[i].effectDistance, new(0, -0), Time.deltaTime * 10);
            }
        }
        var qr = Quaternion.Euler(new Vector3(0, 0, -SelectCursor * 10f));
        LVRoot.transform.rotation = Quaternion.Lerp(LVRoot.transform.rotation, qr, Time.deltaTime*10);
        


        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            SelectCursor = SelectCursor <= 0 ? 0 : SelectCursor - 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectCursor = SelectCursor >= 2 ? 2 : SelectCursor + 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7)) {
            isInteractive = false;
            StartCoroutine(StartManager.GetInstance().Load());
        }
    }

    public void UpdateLevel() { 

    }

    public void Load() {
        SelectCanvas.SetActive(true);
        isInteractive = true;
        SelectCursor = 0;
    }
}
