using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private GameObject[] SongList;
    [SerializeField] private GameObject[] LevelList;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Outline[] outlines;

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
        if (!isInteractive) return;
        for (int i = 0; i < 3; i++) {
            if (SelectCursor == i) {
                var scale = SongList[i].transform.localScale;
                SongList[i].transform.localScale = Vector3.Lerp(scale, new(1.6f, 1.6f,1.6f), Time.deltaTime*10);
                outlines[i].effectDistance = Vector2.Lerp(outlines[i].effectDistance, new(5, -5), Time.deltaTime * 10);
            }else{
                var scale = SongList[i].transform.localScale;
                SongList[i].transform.localScale = Vector3.Lerp(scale, new(1.5f, 1.5f, 1.5f), Time.deltaTime*10);
                outlines[i].effectDistance = Vector2.Lerp(outlines[i].effectDistance, new(0, -0), Time.deltaTime * 10);
            }
        }
        if (SelectStack > 0) { 
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            SelectCursor = SelectCursor <= 0 ? 0 : SelectCursor - 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectCursor = SelectCursor >= 2 ? 2 : SelectCursor + 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SelectStack = SelectStack >= 1 ? 1 : SelectStack + 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SelectStack = SelectStack <= 0 ? 0 : SelectStack - 1;
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
