using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public GameObject DebugCanvas;
    private static DebugManager instance;
    public static DebugManager GetInstance() => instance;

    public PlayerSetting setting;
    public Sync mSync;

    public TMP_InputField Sync;
    public TMP_InputField Speed;

    public bool isDebugOn = false;

    void Awake()
    {
        instance = this;
        isDebugOn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12)) {
            setDebug();
        }   
    }

    void setDebug() {
        isDebugOn = !isDebugOn;
        DebugCanvas.SetActive(isDebugOn);
        if(DebugCanvas.activeSelf){
            Sync.text = setting.soundOffset.ToString();
            Speed.text = mSync.HiSpeed.ToString();
        }
    }

    public void OnSelectServer(TMP_Dropdown dropdown){
        if (dropdown.value == 0) {
            ServerManager.isChangeServer = false;
        }else if(dropdown.value ==1){
            ServerManager.isChangeServer = true;
        }
    }

    public void OnSetSync() {
        float sync = float.Parse(Sync.text);
        setting.soundOffset = sync;
    }

    public void OnSetSpeed(){
        float speed = float.Parse(Speed.text);
        mSync.HiSpeed = speed;
    }
}
