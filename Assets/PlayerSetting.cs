using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    public float musicStartOffset = 0.0f;

    public float soundOffset = 0.0f;
    public float noteOffset = 0.0f;

    public float JudgeOffset { get { return soundOffset + musicStartOffset; } set { } }
    public float DisplayOffset{ get{return noteOffset + musicStartOffset;} set { } }

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
