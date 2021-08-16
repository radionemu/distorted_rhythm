using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public Text _text;
    Sheet _sheet;

    // Start is called before the first frame update
    void Start()
    {
        _sheet = GameObject.Find("Sheet").GetComponent<Sheet>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_sheet == null){
            Debug.Log("시발 이게 왜 없노");
        }
        else{
            _text.text = _sheet.Title + _sheet.Artist + _sheet.Genre +  _sheet.Difficulty.ToString();           
        }

    }
}
